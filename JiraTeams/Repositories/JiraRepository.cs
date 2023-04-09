using JiraTeams.Repositories.Interfaces;
using Atlassian.Jira;

namespace JiraTeams.Repositories
{
    public class JiraRepository : IJiraRepository
    {
        readonly Jira _jira;
        public JiraRepository() {
            _jira = Jira.CreateRestClient("https://jira.arsis.ru/", "Arthur.Artyushenkov", "Vag99296");
        }
        
        public async void CreateIssueAsync()
        {
            var issue = _jira.CreateIssue("STUD");
            issue.Type = "Task";
            issue.Priority = "Major";
            issue.Summary = "Issue Summary";

            await issue.SaveChangesAsync();
        }

        public async void ReadIssueAsync() 
        {
            var issues = _jira.Issues.Queryable.ToString(); //converts to JQL


            var issue = await _jira.Issues.GetIssueAsync("STUD-11");
            //var issues = from i in _jira.Issues.Queryable
            //             where i.Key == new LiteralMatch("STUD-11")
            //             select i;

            Console.WriteLine(issue.Priority.Name);      // returns the string of the priority field, for example "Critical"
            Console.WriteLine(issue.Type.Name);          // returns the string of the issue type field, for example "Bug"
        }

        //public async void ListenIssueAsync()
        //{
        //    var events = issue.GetChangeLogsAsync().Result;

        //}
    }
    
}
