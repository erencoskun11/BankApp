
using BankApp.Domain.Entities;

namespace BankAppDomain.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        Task UpdateAsync(Card entity);
        Task DeleteAsync(Card entity);
        Task<List<Card>> GetExpiredCardsAsync();

    }
}