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

namespace BankApp.Workers.Base
{
    public abstract class BaseConsumer<TEvent> : BackgroundService where TEvent : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        protected BaseConsumer(IServiceScopeFactory serviceScopeFactory, string queueName)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _queueName = queueName;

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

                    var @event = JsonSerializer.Deserialize<TEvent>(message);

                    if (@event == null)
                    {
                        Console.WriteLine($"❌ [{_queueName}] Deserialize işlemi başarısız. Mesaj: {message}");
                        return;
                    }

                    Console.WriteLine($"✅ [{_queueName}] Event alındı: {typeof(TEvent).Name}");

                    using var scope = _serviceScopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                    await handler.HandleAsync(@event);

                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [{_queueName}] HATA: {ex.Message}");
                    // ACK gönderilmiyor ki tekrar denensin (DLQ yoksa dikkat!)
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
