using BankApp.Application.Etos;
using BankApp.Application.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Workers.Consumers
{
    public class AccountDeleteEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AccountDeleteEventConsumer> _logger;
        private readonly IConnectionProvider _connectionProvider;

        private IConnection? _connection;
        private IModel? _channel;

        private const string QueueName = "account-delete-queue";

        public AccountDeleteEventConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<AccountDeleteEventConsumer> logger,
            IConnectionProvider connectionProvider)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = _connectionProvider.GetConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"[AccountDeleteConsumer] Received message: {message}");

                var accountDeleteEvent = JsonSerializer.Deserialize<AccountDeleteEto>(message);

                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<AccountDeleteEventHandler>();

                await handler.HandleAsync(accountDeleteEvent);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();
            }
            catch { }
            base.Dispose();
        }
    }
}
