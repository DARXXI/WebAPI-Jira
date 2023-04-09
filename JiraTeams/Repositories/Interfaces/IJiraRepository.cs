using Atlassian.Jira;

namespace JiraTeams.Repositories.Interfaces
{
    public interface IJiraRepository
    {
        public void CreateIssueAsync();
        public void ReadIssueAsync();
        //public Issue UpdateIssue(Issue issue);
        //public Issue DeleteIssue(int id);
    }
}
