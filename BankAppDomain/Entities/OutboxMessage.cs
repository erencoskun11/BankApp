using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDomain.Entities
{
    public class OutboxMessage
    {
        public int Id { get; set; }
        public string EventType { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsProcessed { get; set; } = false;
    }
}
