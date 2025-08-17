using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Etos
{
    public class AccountCreateEto
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }

    }
}
