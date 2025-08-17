using BankAppDomain.Entities;

namespace BankApp.Domain.Entities;

public class AccountType : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}

