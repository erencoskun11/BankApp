using BankApp.Domain.Entities;
using BankAppDomain;

namespace BankApp.Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetByIdAsync(int id);

        Task DeleteAsync(int id);

    }
}
