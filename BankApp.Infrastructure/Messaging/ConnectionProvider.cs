using RabbitMQ.Client;
using System;
using System.Threading;

public class ConnectionProvider : IConnectionProvider, IDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private readonly object _lock = new();

    public ConnectionProvider(ConnectionFactory factory) => _factory = factory;

    public IConnection GetConnection()
    {
        if (_connection != null && _connection.IsOpen) return _connection;

        lock (_lock)
        {
            if (_connection != null && _connection.IsOpen) return _connection;

            int tries = 0;
            while (tries < 5)
            {
                try
                {
                    _connection = _factory.CreateConnection();
                    Console.WriteLine("[ConnectionProvider] RabbitMQ bağlantısı kuruldu.");
                    return _connection;
                }
                catch (Exception ex)
                {
                    tries++;
                    Console.WriteLine($"[ConnectionProvider] RabbitMQ bağlanma denemesi {tries} başarısız: {ex.Message}");
                    Thread.Sleep(TimeSpan.FromSeconds(2 * tries));
                }
            }

            throw new Exception("RabbitMQ bağlantısı kurulamadı (ConnectionProvider).");
        }
    }

    public void Dispose()
    {
        try
        {
            if (_connection != null)
            {
                if (_connection.IsOpen) _connection.Close();
                _connection.Dispose();
            }
        }
        catch { }
    }
}
