using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.DTOs
{
    public class CardAccountTransactionViewDto
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CCV { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public DateTime OpenedAt { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal RiskLimit { get; set; }
    }
}
