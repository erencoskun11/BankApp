using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using BankAppDomain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BankApp.Infrastructure.Repositories
{
    public class CustomerRepository : EfCoreRepository<Customer, BankDbContext>, ICustomerRepository
    {
        public CustomerRepository(BankDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNationalIdAsync(string nationalId)
        {
            return await context.Customers.AnyAsync(c => c.NationalId == nationalId);
        }

        public async Task<bool> ExistsAccountNumberAsync(string accountNumber)
        {
            return await context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<bool> ExistsIbanAsync(string iban)
        {
            return await context.Accounts.AnyAsync(a => a.IBAN == iban);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await context.Customers
                                .Include(c => c.Accounts)
                                    .ThenInclude(a => a.Cards)
                                .Include(c => c.Accounts)
                                    .ThenInclude(a => a.Transactions)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task UpdateAsync(Customer customer)
        {
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Customers.FindAsync(id);
            if (entity != null)
            {
                context.Customers.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
