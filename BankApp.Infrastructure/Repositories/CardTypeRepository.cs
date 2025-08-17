using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;

namespace BankApp.Infrastructure.Repositories
{
    public class CardTypeRepository : EfCoreRepository<CardType,BankDbContext>, ICardTypeRepository
    {
        public CardTypeRepository(BankDbContext context) : base(context)
        {
        }
    }
}
