namespace JiraTeams.Repositories.Interfaces
{
    public interface IJiraRepository
    {
        public void CreateIssueAsync(string key);
        public void ReadIssuesAsync();

        public void GetLinkedIssues(string key);
        //public Issue UpdateIssue(Issue issue);
        //public Issue DeleteIssue(int id);
    }
}
