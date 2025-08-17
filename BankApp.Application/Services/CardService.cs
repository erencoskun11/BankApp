using BankApp.Application.DTOs.CardDtos;
using BankApp.Domain.Entities;
using AutoMapper;
using BankApp.Application.Interfaces;
using BankAppDomain.Repositories;
using Cqrs.Commands;
using BankApp.Application.Etos;
using BankAppDomain.Events;

namespace BankApp.Application.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IEventPublisher<CardCreateEto> _publisher;
        public CardService(ICardRepository cardRepository, IMapper mapper, ICacheService cacheService,IEventPublisher<CardCreateEto> publisher )
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _publisher = publisher;
        }

        public async Task<List<CardGetDto>> GetAllAsync()
        {
            var cacheKey = "cards_all";
            var cachedData = await _cacheService.GetAsync<List<CardGetDto>>(cacheKey);

            if (cachedData != null) {
                return cachedData;
            }
            var cards =await _cardRepository.GetAllAsync();
            var result =_mapper.Map<List<CardGetDto>>(cards);

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }

        public async Task<CardGetDto?> GetByIdAsync(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            return card != null  ?_mapper.Map<CardGetDto>(card) : null;
        }

        public async Task<bool> CreateAsync(CardCreateDto dto)
        {
            var card = _mapper.Map<Card>(dto);
            await _cardRepository.AddAsync(card);
            await _cardRepository.SaveChangesAsync();

            await _cacheService.RemoveAsync("cards_all"); // <-- ✅ CACHE temizle

            // ✅ EVENT: CardCreateEto oluştur
            var eto = new CardCreateEto
            {
                MaskedCardNumber = card.CardNumber,
                ExpiryMonth = card.ExpiryMonth,
                ExpiryYear = card.ExpiryYear,
                IsActive = card.IsActive,
                AccountId = card.AccountId,
                CardTypeId = card.CardTypeId,
                CreatedAt = DateTime.UtcNow
            };

            // ✅ EVENT: RabbitMQ'ya publish et
            await _publisher.PublishAsync(eto,"CardCreateEto");
            return true;
        }
        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return "****";
            return $"**** **** **** {cardNumber[^4..]}";
        }

        public async Task<bool> UpdateAsync(int id, CardUpdateDto dto)
        {
            var existingCard = await _cardRepository.GetByIdAsync(id);
            if (existingCard == null)
                return false;

            _mapper.Map(dto, existingCard);
            await _cardRepository.UpdateAsync(existingCard);
            await _cardRepository.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
                return false;

            await _cardRepository.DeleteAsync(card);
            await _cardRepository.SaveChangesAsync(); 
            return true;
        }
        public async Task CheckAndUpdateCardsAsync()
        {
            var allCards = await GetAllAsync();

            Console.WriteLine($"Kart sayısı: {allCards.Count} - Güncelleme zamanı: {DateTime.Now}");

        }
        public async Task<List<CardGetDto>> GetExpiredCardsAsync()
        {
            var expiredCards = await _cardRepository.GetExpiredCardsAsync();
            return _mapper.Map<List<CardGetDto>>(expiredCards);
        }

        public async Task<List<CardGetDto>> GetCardsExceptLastWeekAsync()
        {
            // Bu tür sorgular queryable yapılmalı.
            // Queryable vs Enumerable farkı araştırılmalı.
            var allCards = await _cardRepository.GetAllAsync();
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            var filteredCards = allCards
                .Where(c => new DateTime(c.ExpiryYear, c.ExpiryMonth, 1) < oneWeekAgo)
                .ToList();

            return _mapper.Map<List<CardGetDto>>(filteredCards);
        }
    }
}

