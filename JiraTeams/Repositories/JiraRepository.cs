using JiraTeams.Repositories.Interfaces;
using Atlassian.Jira;
using JiraTeams.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace JiraTeams.Repositories
{
    public class JiraRepository : BaseRepository,IJiraRepository 
    {
        readonly Jira _jira;
        private readonly IConfiguration _configuration;
        public JiraRepository(DataBaseContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
            _jira = Jira.CreateRestClient(
                _configuration.GetValue<string>("AuthorizationJira:ServerUrl"),
                _configuration.GetValue<string>("AuthorizationJira:Username"),
                _configuration.GetValue<string>("AuthorizationJira:Password")
            );
        }
        
        public async void CreateIssueAsync(string issueJSON)
        {
            var issue = _jira.CreateIssue("STUD");
            issue.Type = "Task";
            issue.Priority = "Major";
            issue.Summary = "Issue Summary";

            await issue.SaveChangesAsync();
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
            var username = "Arthur.Artyushenkov";
            var password = "Vag99296";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

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
                    CreateIssueAsync(json);
                }
            }
            else
            {
                Console.WriteLine(response.ReasonPhrase);
            }
        }
    }
}
