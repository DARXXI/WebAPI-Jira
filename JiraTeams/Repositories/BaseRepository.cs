namespace JiraTeams.Repositories
{ 
    public class BaseRepository
    {
        public DateTime CurrentDate = DateTime.Now.ToLocalTime();
        public HttpClient client = new HttpClient();
    }
}