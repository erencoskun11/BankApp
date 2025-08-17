using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BankApp.Application.EventHandlers;
using BankApp.Application.Events;
using BankAppDomain.Constants;

namespace BankApp.Workers.Consumers
{
    public class CustomerCreatedEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _queueName = QueueNameConstant.CustomerCreated;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public CustomerCreatedEventConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var @event = JsonSerializer.Deserialize<CustomerCreateEto>(message);

                    if (@event == null)
                    {
                        Console.WriteLine($"❌ [{_queueName}] Deserialize hatası. Mesaj: {message}");
                        return;
                    }

                    Console.WriteLine($"✅ [{_queueName}] Event alındı: {nameof(CustomerCreateEto)}");

                    using var scope = _serviceScopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<CustomerCreateEto>>();
                    await handler.HandleAsync(@event);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [{_queueName}] Hata: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

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
