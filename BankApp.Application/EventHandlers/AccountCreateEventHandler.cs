using BankApp.Application.Etos;
using BankApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAppDomain.Constants;
namespace BankApp.Application.EventHandlers
{
    public class AccountCreateEventHandler : IEventHandler<AccountCreateEto>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AccountCreateEventHandler> _logger;


        public AccountCreateEventHandler(IServiceScopeFactory scopeFactory, ILogger<AccountCreateEventHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(AccountCreateEto @event)
        {
            _logger.LogInformation("AccountCreatedEto received. Indexing to ElasticSearch...");

            using var scope = _scopeFactory.CreateScope();
            var elasticSearchService = scope.ServiceProvider.GetRequiredService<ElasticSearchService>();

            await elasticSearchService.IndexAsync(@event, ElasticSearchConstants.Elastic.AccountCreatedLogsIndex);

            _logger.LogInformation("CustomerCreatedEto indexed successfully.");

        }





    }
}
