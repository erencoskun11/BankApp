namespace BankApp.Application.DTOs.TransactionDtos
{
    public class TransactionUpdateDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public int? CardId { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
