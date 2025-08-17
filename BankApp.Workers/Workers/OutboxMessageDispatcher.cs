using BankApp.Infrastructure.Repositories;
using BankAppDomain.Entities;
using BankAppDomain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BankApp.Workers.Workers
{
    public class OutboxMessageDispatcher : BackgroundService
    {
        private readonly ILogger<OutboxMessageDispatcher> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OutboxMessageDispatcher(
            ILogger<OutboxMessageDispatcher> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            // RabbitMQ bağlantısı oluştur
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox dispatcher başlatıldı");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                        var messages = await outboxRepository.GetUnprocessedMessagesAsync();

                        foreach (var message in messages)
                        {
                            try
                            {
                                var body = Encoding.UTF8.GetBytes(message.Content);

                                _channel.BasicPublish(
                                    exchange: "",
                                    routingKey: message.EventType,
                                    basicProperties: null,
                                    body: body
                                );
                                _logger.LogInformation($"✅ Outbox event published: {message.EventType}");

                                await outboxRepository.MarkAsProcessedAsync(message.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"❌ Outbox mesaj gönderilemedi: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"❌ Outbox dispatcher hatası: {ex.Message}");
                }

                await Task.Delay(5000, stoppingToken); // 5 saniyede bir kontrol et
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

