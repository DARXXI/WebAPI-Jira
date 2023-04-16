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
        public async Task<IActionResult> Get()
        {
            while (true)
            {
                //_jiraRepository.GetLinkedIssues();
                _jiraRepository.ReadIssuesAsync();
                await Task.Delay(10000);
            }
        }
    }
}