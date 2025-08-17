using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using BankApp.Application.Etos;
using BankApp.Application.EventHandlers;

public class AccountCreatedEventConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection _connection;
    private IModel _channel;

    public AccountCreatedEventConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "AccountCreateEto",
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
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<AccountCreateEto>(message);

            Console.WriteLine($"[AccountCreatedEventConsumer] Received: {message}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<AccountCreateEto>>();
                await handler.HandleAsync(@event);
            }

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: "AccountCreateEto",
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
