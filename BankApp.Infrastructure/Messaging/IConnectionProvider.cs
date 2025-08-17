using RabbitMQ.Client;

public interface IConnectionProvider
{
    /// <summary>
    /// Açık ve kullanılabilir bir IConnection döndürür. Gerekirse bağlantı oluşturur.
    /// </summary>
    IConnection GetConnection();
}
