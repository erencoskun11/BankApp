using BankApp.Application.Etos;
using BankApp.Application.EventHandlers;
using Chinchilla.Logging;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using RabbitConnection = RabbitMQ.Client.IConnection;

namespace BankApp.Workers.Consumers
{
    public class AccountDeleteEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AccountDeleteEventConsumer> _logger;
        private readonly RabbitConnection _connection;
        private RabbitMQ.Client.IModel _channel;
        private const string QueueName = "account-delete-queue";

        public AccountDeleteEventConsumer(IServiceScopeFactory scopeFactory, ILogger<AccountDeleteEventConsumer> logger, RabbitConnection connection)

        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            _connection = connection;

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"Received message from queue {QueueName}");

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
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }






    }
}
