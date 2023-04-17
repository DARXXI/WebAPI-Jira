namespace JiraTeams.Repositories.Interfaces
{
    public interface IJiraRepository
    {
        public void CreateIssuesLinkingAsync(string inwardKey, string outwardKey);
        public void ReadIssuesAsync();

        public void GetLinkedIssues(string key);
        //public Issue UpdateIssue(Issue issue);
        //public Issue DeleteIssue(int id);
    }
}
