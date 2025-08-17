namespace BankAppDomain.Models.ManagersModels
{
    public class TransactionCreateModel
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int? AccountId { get; set; }
        public int? CardId { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
