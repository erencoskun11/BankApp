using BankApp.Application.Etos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankApp.Application.EventHandlers
{
    public class CardCreateEventHandler : IEventHandler<CardCreateEto>
    {
        private readonly ILogger<CardCreateEventHandler> _logger;

        public CardCreateEventHandler(ILogger<CardCreateEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(CardCreateEto @event)
        {
            // Elasticsearch kaldırıldı, sadece log bırakıldı
            _logger.LogInformation("CardCreateEto received. Card: {Card}", @event.MaskedCardNumber);

            return Task.CompletedTask;
        }
    }
}

