using BankAppDomain.Events;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher<T> : IEventPublisher<T>
        where T : class
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqEventPublisher(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync(T @event, string queueName)
        {
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            // Kuyruğu yoksa oluşturur, varsa aynen bırakır
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<T> events, string queueName)
        {
            foreach (var e in events)
            {
                PublishAsync(e, queueName);
            }

            return Task.CompletedTask;
        }
    }
}
