using BankApp.Application.DTOs.CardDtos;

namespace BankApp.Application.Interfaces
{
    public interface ICardService 
    {
        Task<List<CardGetDto>> GetAllAsync();
        Task<CardGetDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CardCreateDto dto);
        Task<bool> UpdateAsync(int id, CardUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task CheckAndUpdateCardsAsync();
        Task<List<CardGetDto>> GetExpiredCardsAsync();
        Task<List<CardGetDto>> GetCardsExceptLastWeekAsync();
    }
}


