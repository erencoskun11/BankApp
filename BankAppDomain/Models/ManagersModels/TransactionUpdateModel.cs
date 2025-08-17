namespace BankAppDomain.Models.ManagersModels
{
    public class TransactionUpdateModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public int? CardId { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
