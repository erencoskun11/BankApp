using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using BankApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankAppDomain.Repositories
{
    public class CardRepository : EfCoreRepository<Card, BankDbContext>, ICardRepository
    {
        private readonly BankDbContext _context;
        public CardRepository(BankDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Card>> GetExpiredCardsAsync()
        {
            var yesterday = DateTime.Now.AddDays(-1);

            var allCards = await _context.Cards.ToListAsync();

            var expiredCards = allCards.Where(c =>
            {
                var expiryDate = new DateTime(c.ExpiryYear, c.ExpiryMonth, 1).AddMonths(1).AddDays(-1);
                return expiryDate <= yesterday;
            })
                .ToList();

            return expiredCards;
        }
    }
}