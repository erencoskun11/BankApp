namespace BankAppDomain.Models.ManagersModels
{
    public class CardCreateModel
    {
        public int AccountId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CCV { get; set; }
        public int CardTypeId { get; set; }
        public bool IsActive { get; set; }  // Yeni eklenen
    }
    public class CardUpdateModel
    {
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CCV { get; set; }
        public int CardTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
