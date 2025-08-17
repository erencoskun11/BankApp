namespace BankApp.Application.DTOs.AccountDtos
{
    public class AccountUpdateDto
    {
        public int Id{ get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public bool IsActive { get; set; }
    }

}
