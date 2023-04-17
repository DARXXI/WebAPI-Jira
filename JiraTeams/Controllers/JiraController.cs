using Microsoft.AspNetCore.Mvc;
using Atlassian.Jira;
using JiraTeams.Repositories;
using JiraTeams.Repositories.Interfaces;
using SimpleJson;
using System.Web;

namespace JiraTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraController : BaseController
    {
        private readonly IJiraRepository _jiraRepository;
        

        private readonly ILogger<JiraController> _logger;

        public JiraController(ILogger<JiraController> logger, IJiraRepository jiraRepository)
        {
            _jiraRepository = jiraRepository;
            _logger = logger;
        }

        //TODO all issues

        //TODO move to program.cs https://habr.com/ru/articles/658847/
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_jiraRepository.GetAllIssues().ToList());
        }
    }
}