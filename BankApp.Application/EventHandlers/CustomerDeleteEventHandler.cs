using System.Threading.Tasks;
using BankApp.Application.Events;
using BankApp.Application.Interfaces;
using BankAppDomain.Constants;
using Cqrs.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BankApp.Application.EventHandlers
{
    public class CustomerDeleteEventHandler : IEventHandler<CustomerDeleteEto>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CustomerDeleteEventHandler> _logger;

        public CustomerDeleteEventHandler(IServiceScopeFactory scopeFactory, ILogger<CustomerDeleteEventHandler> logger)
        {
            _scopeFactory= scopeFactory;
            _logger = logger;
        }

        public async Task HandleAsync(CustomerDeleteEto @event)
        {
            _logger.LogInformation("CustomerDeletedEto received. Indexing to ElasticSearch...");

            using var scope = _scopeFactory.CreateScope();
            var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

            await elasticSearchService.IndexAsync(@event, ElasticSearchConstants.Elastic.CustomerDeletedLogsIndex);

            _logger.LogInformation("CustomerDeletedEto indexed.");
        }
    }
}

