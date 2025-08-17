using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

public class RabbitSetupHostedService : IHostedService
{
    private readonly IConnectionProvider _cp;

    public RabbitSetupHostedService(IConnectionProvider cp) => _cp = cp;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var conn = _cp.GetConnection();
            using var ch = conn.CreateModel();

            // LOGLARDA GÖRDÜĞÜNÜZ ADLA BİRE BİR EŞLEŞTİRİN (typo kontrolü!)
            ch.ExchangeDeclare("BankManagementExchang", ExchangeType.Direct, durable: true, autoDelete: false);
            ch.QueueDeclare("BankManagementQueue", durable: true, exclusive: false, autoDelete: false);
            ch.QueueBind("BankManagementQueue", "BankManagementExchang", routingKey: "some-key");

            Console.WriteLine("[RabbitSetup] Exchange & Queue oluşturuldu veya mevcut olan doğrulandı.");
        }
        catch (Exception ex)
        {
            // Startup'ı çökertmemek için loglayın
            Console.WriteLine("[RabbitSetup] Hata: " + ex);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
