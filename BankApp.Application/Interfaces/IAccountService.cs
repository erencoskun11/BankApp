using BankApp.Application.DTOs.AccountDtos;
using BankApp.Domain.Entities;

namespace BankApp.Application.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<bool> CreateAccountAsync(AccountCreateDto accountCreateDto);
        Task<bool> UpdateAccountAsync(int id, AccountUpdateDto updateDto);
        Task<bool> DeleteAccountAsync(int id);
    }
}

