using BankApp.Application.Etos;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BankApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountDeletePublisherController : ControllerBase
    {
        private readonly IConnection _connection;

        public AccountDeletePublisherController(IConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public IActionResult Publish([FromBody] AccountDeleteEto model)
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: "account-delete-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: "",
                routingKey: "account-delete-queue",
                basicProperties: null,
                body: body
            );

            return Ok("AccountDeleteEto mesajı kuyruğa gönderildi.");
        }
    }
}

