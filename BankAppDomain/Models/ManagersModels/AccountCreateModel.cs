namespace BankAppDomain.Models.ManagersModels
{
    public class AccountCreateModel
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }

    }
}
