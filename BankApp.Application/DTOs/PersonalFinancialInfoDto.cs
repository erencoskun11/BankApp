using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.DTOs
{
    public class PersonalFinancialInfoDto
    {
        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? MaskedNationalId { get; set; }
        public int? AccountCount { get; set; }
        public string? MaskedAccountNumbers { get; set; }
    }
}
