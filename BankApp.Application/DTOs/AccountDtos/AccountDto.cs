namespace BankApp.Application.DTOs.AccountDtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public bool IsActive { get; set; } = true;
        public int CustomerId { get; set; }
    }
}

