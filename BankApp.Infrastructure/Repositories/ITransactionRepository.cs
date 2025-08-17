using BankApp.Domain.Entities;
using BankAppDomain;

namespace BankApp.Infrastructure.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsWithDetailsAsync();
        Task<List<Transaction>> GetTransactionsOlderThanAsync(DateTime cutoffDate);


    }
}