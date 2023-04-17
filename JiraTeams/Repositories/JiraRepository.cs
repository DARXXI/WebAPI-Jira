using JiraTeams.Repositories.Interfaces;
using Atlassian.Jira;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JiraTeams.Repositories
{
    public class JiraRepository : BaseRepository, IJiraRepository 
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

        public async void CreateIssueDuplicateAsync(Atlassian.Jira.Issue issue)
        {
            var targetIssue = _jira.CreateIssue(_configuration.GetValue<string>("Projects:TargetProjectName"));

            targetIssue.Type = issue.Type;
            targetIssue.Priority = issue.Priority;
            targetIssue.Summary = issue.Summary;

            await targetIssue.SaveChangesAsync();
            CreateIssuesLinkingAsync(issue.Key.Value, targetIssue.Key.Value); 
        }

        //TODO object mapping
        public async void CreateIssuesLinkingAsync(string inwardKey, string outwardKey) 
        {
            var jsonContent = "{\"inwardIssue\":{\"key\":\""+inwardKey+"\"},\"outwardIssue\":{\"key\":\""+outwardKey+"\"},\"type\":{\"name\":\"Cloners\"}}";
            var jsonStringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetValue<string>
            ("AuthorizationJira:Username")}:{_configuration.GetValue<string>("AuthorizationJira:Password")}")));

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            using HttpResponseMessage response = await client.PostAsync("https://jira.arsis.ru/rest/api/2/issueLink", jsonStringContent);

            Console.WriteLine(response.IsSuccessStatusCode);
        }

        public void ReadIssues()
        {
            var issues = _jira.Issues.Queryable.Where(t => t.Status == _configuration.GetValue<string>("TriggerStatus"))
                .OrderByDescending(t => t.Created);

            if (issues != null)
            {
                foreach (Atlassian.Jira.Issue issue in issues)
                {
                    GetLinkedIssues(issue);
                }
            }
        }

        //TODO object mapping
        public async void GetLinkedIssues(Atlassian.Jira.Issue issue)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetValue<string>
            ("AuthorizationJira:Username")}:{_configuration.GetValue<string>("AuthorizationJira:Password")}")));

            var response = await client.GetAsync($"https://jira.arsis.ru/rest/api/2/issue/{issue.Key.Value}");

            if (response.IsSuccessStatusCode)
            {
                var links = JObject.Parse(await response.Content.ReadAsStringAsync())["fields"]["issuelinks"].ToList();//json to classes c#
                if (links.Count != 0)
                {
                    var linkedKey = "";
                    foreach (JToken link in links)
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
                            //some logic here
                        }
                    }
                }  
                else
                {
                    CreateIssueDuplicateAsync(issue); //token
                }
            }
            else
            {
                Console.WriteLine(response.ReasonPhrase);
            }
        }

        public List<Issue> GetAllIssues()
        {
            return _jira.Issues.Queryable.OrderByDescending(t => t.Created).ToList();
        }
    }
}
