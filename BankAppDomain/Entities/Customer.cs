using BankAppDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Domain.Entities;

public class Customer : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, MaxLength(11)]
    public string NationalId { get; set; }

    [Required]
    public string BirthPlace { get; set; }

    public DateTime BirthDate { get; set; }

    public decimal RiskLimit { get; set; } = 10000;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

