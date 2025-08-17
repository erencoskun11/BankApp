using BankAppDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Domain.Entities;

public class Card : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(16)]
    public string CardNumber { get; set; }

    [Required]
    public int ExpiryMonth { get; set; }

    [Required]
    public int ExpiryYear { get; set; }

    [Required, MaxLength(3)]
    public string CCV { get; set; }

    public bool IsActive { get; set; } = true;

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public int CardTypeId { get; set; }
    public virtual CardType? CardType { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}


