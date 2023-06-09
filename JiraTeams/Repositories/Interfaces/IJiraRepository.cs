﻿using Atlassian.Jira;
using Microsoft.AspNetCore.Mvc;

namespace JiraTeams.Repositories.Interfaces
{
    public interface IJiraRepository
    {
        public void CreateIssueDuplicateAsync(Atlassian.Jira.Issue issue);
        public void ReadIssues();
        public void GetLinkedIssues(Atlassian.Jira.Issue issue);
        public void CreateIssuesLinkingAsync(string inwardKey, string outwardKey);
        public List<Issue> GetAllIssues();
    }
}
