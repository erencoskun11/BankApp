using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;

namespace BankApp.Infrastructure.Repositories
{
    public class TransactionTypeRepository : EfCoreRepository<TransactionType,BankDbContext>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(BankDbContext context) : base(context)
        {
        }
    }
}
