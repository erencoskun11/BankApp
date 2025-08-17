using BankApp.Domain.Entities;
using BankAppDomain;

namespace BankApp.Application.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> ExistsByNationalIdAsync(string nationalId);
        Task<bool> ExistsAccountNumberAsync(string accountNumber);
        Task<bool> ExistsIbanAsync(string iban);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
