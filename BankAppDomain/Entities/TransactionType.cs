using BankAppDomain.Entities;

namespace BankApp.Domain.Entities;

public class TransactionType :IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Transaction> Transactions { get; set; }

}

