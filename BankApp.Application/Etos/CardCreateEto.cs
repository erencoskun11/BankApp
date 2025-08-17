using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Etos
{
    public class CardCreateEto
    {
        public string MaskedCardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public bool IsActive { get; set; }

        public int AccountId { get; set; }
        public int CardTypeId { get; set; }
        public DateTime CreatedAt { get; set; }



    }
}
