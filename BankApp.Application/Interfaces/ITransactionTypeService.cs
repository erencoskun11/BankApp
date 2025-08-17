using BankApp.Application.DTOs;
using BankApp.Domain.Entities;

namespace BankApp.Application.Interfaces
{
    public interface ITransactionTypeService
    {
        Task<List<TransactionTypeDto>> GetAllAsync();
        Task<TransactionTypeDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(TransactionTypeDto dto);
        Task<bool> CreateAsync(TransactionTypeDto dto);
    }
}

