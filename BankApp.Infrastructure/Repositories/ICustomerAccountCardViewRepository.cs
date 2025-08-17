using BankAppDomain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Repositories
{
    public  interface ICustomerAccountCardViewRepository
    {
        Task<IEnumerable<CustomerAccountCardView>> GetAllAsync();
        Task<CustomerAccountCardView?> GetByIdAsync(int id);
    }
}
