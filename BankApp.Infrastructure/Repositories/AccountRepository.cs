using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;

namespace BankApp.Infrastructure.Repositories
{
    public class AccountRepository : EfCoreRepository<Account, BankDbContext>, IAccountRepository
    {
        public AccountRepository(BankDbContext context) : base(context)
        {
        }
        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }




        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<Account>().FindAsync(id);
            if (entity != null)
            {
                context.Set<Account>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

    }
}