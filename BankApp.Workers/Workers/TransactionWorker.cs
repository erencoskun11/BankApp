using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using BankApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using BankApp.Application.DTOs.CardDtos;
using BankAppDomain.Constants;

namespace BankApp.Workers.Workers
{
    public class TransactionWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TransactionWorker> _logger;

        public TransactionWorker(IServiceProvider serviceProvider, ILogger<TransactionWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("TransactionWorker calisti: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var cardService = scope.ServiceProvider.GetRequiredService<ICardService>();
                    var elasticService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

                    var cardsToIndex = await cardService.GetCardsExceptLastWeekAsync();

                    foreach (var cardDto in cardsToIndex)
                    {
                        await elasticService.IndexAsync<CardGetDto>(cardDto, ElasticSearchConstants.Card.IndexName);
                        _logger.LogInformation($"Kart {cardDto.Id} elastic'e gönderildi.");
                    }
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }


    }
}
