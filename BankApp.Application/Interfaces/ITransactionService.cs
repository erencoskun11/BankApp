using BankApp.Application.DTOs.TransactionDtos;
using BankApp.Domain.Entities;

namespace BankApp.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetAllAsync();
        Task<TransactionDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(TransactionCreateDto dto);
        Task<bool> UpdateAsync(TransactionUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TransactionViewDto>> GetTransactionsFromViewAsync();
        Task<IEnumerable<TransactionDto>> GetAllTransactionsWithDetails();
    }
}

