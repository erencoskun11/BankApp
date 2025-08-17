using BankAppDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Domain.Entities;

public class Transaction : IEntity
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }

    public int? CardId { get; set; }
    public Card? Card { get; set; }


    public int TransactionTypeId { get; set; }
    public TransactionType? TransactionType { get; set; }
}

