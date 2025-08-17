using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Events
{
    public class CustomerDeleteEto
    {
        public int CustomerId { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

    }
}
