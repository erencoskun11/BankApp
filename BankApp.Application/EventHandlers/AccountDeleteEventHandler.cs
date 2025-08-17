using BankApp.Application.Etos;
using BankApp.Application.Interfaces;
using BankAppDomain.Constants;
using Chinchilla.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.EventHandlers
{
    public class AccountDeleteEventHandler : IEventHandler<AccountDeleteEto>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AccountDeleteEventHandler> _logger;
    
        public AccountDeleteEventHandler(IServiceScopeFactory scopeFactory,ILogger<AccountDeleteEventHandler> logger )

        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }    

        public async Task HandleAsync(AccountDeleteEto @event)
        {
            _logger.LogInformation("AccountDeleteEto received. Indexing to ElasticSearch...");

            using var scope = _scopeFactory.CreateScope();
            var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

            await elasticSearchService.IndexAsync(@event, ElasticSearchConstants.Elastic.AccountDeletedLogsIndex);


        }
    
    }

}
