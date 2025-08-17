using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.DTOs
{
    public class CustomerAccountCardViewDto
    {
        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public int? AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public int? CardId { get; set; }
        public string? CardNumber { get; set; }
    }
}
