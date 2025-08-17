using AutoMapper;
using BankApp.Application.DTOs.CardDtos;
using BankApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Workers.Workers
{
    public class CardActivityWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CardActivityWorker> _logger;

        public CardActivityWorker(IServiceProvider serviceProvider, ILogger<CardActivityWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("CardActivityWorker başladı: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var cardService = scope.ServiceProvider.GetRequiredService<ICardService>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                    var expiredCards = await cardService.GetExpiredCardsAsync(); 

                    foreach (var card in expiredCards)
                    {
                        if (card.IsActive)
                        {
                            var updateDto = mapper.Map<CardUpdateDto>(card);
                            updateDto.IsActive = false;
                            await cardService.UpdateAsync(card.Id, updateDto);
                            _logger.LogInformation($"Kart pasif yapıldı: ID {card.Id}");
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); 
            }
        }
    }
}
