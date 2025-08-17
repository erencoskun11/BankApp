using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;

namespace BankApp.Infrastructure.Repositories
{
    public class AccountTypeRepository : EfCoreRepository<AccountType, BankDbContext>, IAccountTypeRepository
    {
        public AccountTypeRepository(BankDbContext context) : base(context)
        {
        }
    }
}
