using BankApp.Application.DTOs;

namespace BankApp.Application.Interfaces
{
    public interface IAccountTypeService
    {
        Task<List<AccountTypeDto>> GetAllAsync();
        Task<AccountTypeDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(AccountTypeDto dto);
        Task<bool> UpdateAsync(AccountTypeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

