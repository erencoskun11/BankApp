using BankApp.Application.Etos;
using BankApp.Application.Interfaces;
using BankApp.Application.Services;
using Chinchilla.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.EventHandlers
{
    public class CardCreateEventHandler : IEventHandler<CardCreateEto>
    {

        private readonly ILogger<CardCreateEventHandler> _logger;

        private readonly IElasticSearchService _elasticsearchService;

        public CardCreateEventHandler(ILogger<CardCreateEventHandler> logger, IElasticSearchService elasticSearchService)
        {
            _logger = logger;
            _elasticsearchService = elasticSearchService;
        }

        public async Task HandleAsync(CardCreateEto @event)
        {
            _logger.LogInformation("CardCreateEto received. Indexing to ElasticSearch...");
            await _elasticsearchService.IndexAsync(@event, "card-create-logs");

            _logger.LogInformation("CardCreateEto indexed successfully. Card: {Card}", @event.MaskedCardNumber);
 
        }
    }
}
