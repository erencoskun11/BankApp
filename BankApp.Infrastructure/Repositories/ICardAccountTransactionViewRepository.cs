using BankAppDomain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Repositories
{
    public interface ICardAccountTransactionViewRepository
    {
        Task<IEnumerable<CardAccountTransactionView>> GetAllAsync();
        Task<CardAccountTransactionView?> GetByIdAsync(int customerId);

    }
}
