using BankApp.Application.Etos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankApp.Application.EventHandlers
{
    public class AccountDeleteEventHandler : IEventHandler<AccountDeleteEto>
    {
        private readonly ILogger<AccountDeleteEventHandler> _logger;

        public AccountDeleteEventHandler(ILogger<AccountDeleteEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(AccountDeleteEto @event)
        {
            _logger.LogInformation("AccountDeleteEto received. Processing without ElasticSearch...");

            // Örnek log; AccountDeleteEto içindeki uygun property’leri kullan
            _logger.LogInformation(
                "AccountDeleteEto processed successfully. AccountNumber: {AccountNumber}, CustomerId: {CustomerId}",
                @event.AccountNumber,
                @event.CustomerId
            );

            await Task.CompletedTask;
        }
    }
}
