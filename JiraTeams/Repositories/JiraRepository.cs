using JiraTeams.Repositories.Interfaces;
using Atlassian.Jira;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Net.Http;

namespace JiraTeams.Repositories
{
    public class JiraRepository : BaseRepository,IJiraRepository 
    {
        readonly Jira _jira;
        private readonly IConfiguration _configuration;
        public JiraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _jira = Jira.CreateRestClient(
                _configuration.GetValue<string>("AuthorizationJira:ServerUrl"),
                _configuration.GetValue<string>("AuthorizationJira:Username"),
                _configuration.GetValue<string>("AuthorizationJira:Password")
            );
        }
        
        public async void CreateIssuesLinkingAsync(string inwardKey, string outwardKey) //linking 2 issues, needed to be separeted with the issue creation
        {
            var jsonBodyData = "{\"inwardIssue\":{\"key\":\"STUD-37\"},\"outwardIssue\":{\"key\":\"STUD-9\"},\"type\":{\"name\":\"Cloners\"}}";
            var jsonContent = new StringContent(jsonBodyData, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetValue<string>
            ("AuthorizationJira:Username")}:{_configuration.GetValue<string>("AuthorizationJira:Password")}")));

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            using HttpResponseMessage response = await client.PostAsync(
            "https://jira.arsis.ru/rest/api/2/issueLink",
            jsonContent);

            Console.WriteLine(response.IsSuccessStatusCode);
        }

        public async void ReadIssuesAsync() 
        {       
            var issues = _jira.Issues.Queryable.Where(t => t.Status == "TO DO").OrderByDescending(t => t.Created);

            if (issues != null)
            {
                foreach (Atlassian.Jira.Issue issue in issues)
                {
                    GetLinkedIssues(issue.Key.Value);
                }
            }   
        }

        public async void GetLinkedIssues(string issueKey)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetValue<string>
            ("AuthorizationJira:Username")}:{_configuration.GetValue<string>("AuthorizationJira:Password")}")));

            var response = await client.GetAsync($"https://jira.arsis.ru/rest/api/2/issue/{issueKey}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var linkedIssues = JObject.Parse(json)["fields"]["issuelinks"].ToList();

                if (linkedIssues.Count != 0)
                {
                    var linkedKey = "";
                    foreach (JToken link in linkedIssues)
                    {
                        try
                        {
                            linkedKey = (string)link["inwardIssue"]["key"];
                        } 
                        catch
                        {
                            linkedKey = (string)link["outwardIssue"]["key"];
                        }
                        finally
                        {
                            Console.WriteLine(linkedKey);
                        }
                    }
                }  
                else
                {
                    CreateIssuesLinkingAsync("","");
                }
            }
            else
            {
                Console.WriteLine(response.ReasonPhrase);
            }
        }
    }
}
