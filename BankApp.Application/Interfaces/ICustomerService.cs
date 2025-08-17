using BankApp.Application.DTOs.CustomerDto;
using BankApp.Domain.Entities;

namespace BankApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<bool> CreateCustomerAsync(CustomerCreateDto dto);
        Task<bool> UpdateCustomerAsync(CustomerUpdateDto dto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> ExistsByNationalIdAsync(string nationalId); 
        Task<List<CustomerDto>> SearchCustomerAsync(string searchText);


    }
}
