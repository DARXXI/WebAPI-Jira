using JiraTeams.Controllers;
using JiraTeams.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace JiraTeams.Services
{
    public class MyHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        //private readonly IJiraRepository _jiraRepository;
        private readonly ILogger<JiraController> _logger;
        public MyHostedService(ILogger<JiraController> logger, IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var jiraRepository = scope.ServiceProvider.GetRequiredService<IJiraRepository>();
                    jiraRepository.ReadIssues();
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
