using System;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class Root
{
    public string name { get; set; } = "my first webhook via rest"
    public string url { get; set; } = "https://9eb7-31-148-138-233.eu.ngrok.io"
    public List<string> events { get; set; } = ["jira:issue_created","jira:issue_updated"],
    public string filters { get; set; } = "Project = JRA AND resolution = Fixed"
    public bool excludeBody { get; set; } = false
}
