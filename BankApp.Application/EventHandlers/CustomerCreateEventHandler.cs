using System.Threading.Tasks;
using BankApp.Application.Events;

namespace BankApp.Application.EventHandlers
{
    public class CustomerCreateEventHandler : IEventHandler<CustomerCreateEto>
    {
        public Task HandleAsync(CustomerCreateEto @event)
        {
            // Event işlensin
            Console.WriteLine($"🎉 Müşteri oluşturuldu: {@event.FullName}");
            return Task.CompletedTask;
        }
    }
}
