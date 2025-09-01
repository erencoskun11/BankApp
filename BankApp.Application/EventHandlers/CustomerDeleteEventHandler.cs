using System.Threading.Tasks;
using BankApp.Application.Events;
using Cqrs.Events;
using Microsoft.Extensions.Logging;

namespace BankApp.Application.EventHandlers
{
    public class CustomerDeleteEventHandler : IEventHandler<CustomerDeleteEto>
    {
        private readonly ILogger<CustomerDeleteEventHandler> _logger;

        public CustomerDeleteEventHandler(ILogger<CustomerDeleteEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(CustomerDeleteEto @event)
        {
            _logger.LogInformation("CustomerDeleteEto received. Processing without ElasticSearch.");

            _logger.LogInformation(
                "Customer deleted successfully: Id={Id}, FullName={FullName}, NationalId={NationalId}",
                @event.CustomerId
            );

            return Task.CompletedTask;
        }
    }
}
