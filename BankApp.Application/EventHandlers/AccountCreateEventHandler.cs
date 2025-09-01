using BankApp.Application.Etos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankApp.Application.EventHandlers
{
    public class AccountCreateEventHandler : IEventHandler<AccountCreateEto>
    {
        private readonly ILogger<AccountCreateEventHandler> _logger;

        public AccountCreateEventHandler(ILogger<AccountCreateEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(AccountCreateEto @event)
        {
            _logger.LogInformation("AccountCreatedEto received. Processing without ElasticSearch...");

            _logger.LogInformation(
                "AccountCreatedEto processed successfully. AccountNumber: {AccountNumber}, CustomerId: {CustomerId}",
                @event.AccountNumber,
                @event.CustomerId
            );

            await Task.CompletedTask;
        }
    }
}
