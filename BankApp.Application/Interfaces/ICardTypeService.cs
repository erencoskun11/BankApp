using BankApp.Application.DTOs;
using BankApp.Domain.Entities;

namespace BankApp.Application.Interfaces
{
    public interface ICardTypeService
    {
        Task<List<CardTypeDto>> GetAllAsync();
        Task<CardTypeDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CardTypeDto dto);
        Task<bool> UpdateAsync(CardTypeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

