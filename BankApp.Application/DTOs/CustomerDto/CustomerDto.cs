using BankApp.Application.DTOs.AccountDtos;

namespace BankApp.Application.DTOs.CustomerDto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal RiskLimit { get; set; }

    }
}

