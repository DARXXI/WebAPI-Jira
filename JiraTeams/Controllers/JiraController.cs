using Microsoft.AspNetCore.Mvc;
using Atlassian.Jira;
using JiraTeams.Repositories;
using JiraTeams.Repositories.Interfaces;

namespace JiraTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraController : ControllerBase
    {
        private readonly IJiraRepository _jiraRepository;
        

        private readonly ILogger<JiraController> _logger;

        public JiraController(ILogger<JiraController> logger, IJiraRepository jiraRepository)
        {
            _jiraRepository = jiraRepository;
            _logger = logger;
        }

        [HttpGet]
        public void Getdd()
        {
            _jiraRepository.ReadIssueAsync();
            _jiraRepository.CreateIssueAsync();
        }
    }
}