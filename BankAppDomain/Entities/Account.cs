using BankAppDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Domain.Entities;

public class Account : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string AccountName { get; set; }

    [Required]
    public string AccountNumber { get; set; }

    [Required, MaxLength(26)]
    public string IBAN { get; set; }

    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int CardId { get; set; }
    public  virtual ICollection<Card> Cards { get; set; }  

    public ICollection<Transaction> Transactions { get; set; }
    public Account() { }

    public Account(DateTime openedAt,bool isActive)
    {
        OpenedAt = openedAt;
        IsActive = isActive;
    }
}

