using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.Repositories
{
    public class TransactionRepository : EfCoreRepository<Transaction, BankDbContext>, ITransactionRepository
    {
        private readonly BankDbContext _context;

        public TransactionRepository(BankDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsWithDetailsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Card)
                .Include(t => t.TransactionType)
                .ToListAsync();
        }
        public async Task<List<Transaction>> GetTransactionsOlderThanAsync(DateTime cutoffDate)
        {
            return await _context.Transactions
                .Where(t => t.TransactionDate < cutoffDate)
                .ToListAsync();
        }

        public override async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                                 .Where(t => t.Id == id)
                                 .FirstOrDefaultAsync();
        }



    }
}
