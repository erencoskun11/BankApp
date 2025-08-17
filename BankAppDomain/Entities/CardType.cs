using BankAppDomain.Entities;

namespace BankApp.Domain.Entities;

public class CardType :IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Card> Cards { get; set; } = new List<Card>();

}


