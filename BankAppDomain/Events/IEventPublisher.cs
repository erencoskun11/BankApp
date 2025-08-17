using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDomain.Events
{
    public interface IEventPublisher<T>
    {
        Task PublishAsync(T @event, string queueName);
        Task PublishAsync(IEnumerable<T> @events, string queueName);

    }
}
