using AutoMapper;
using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using BankApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Application.Services
{
    public class CardTypeService : ICardTypeService
    {
        private readonly    ICardTypeRepository _cardTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public CardTypeService(ICardTypeRepository cardTypeRepository, IMapper mapper,ICacheService cacheService)
        {
            _cardTypeRepository = cardTypeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }


        public async Task<List<CardTypeDto>> GetAllAsync()
        {
            string cacheKey = "card_type_list";

            var cachedData = await _cacheService.GetAsync<List<CardTypeDto>>(cacheKey);
            if (cachedData != null) {return cachedData; }

            var cardtypes = await _cardTypeRepository.GetAllAsync();    
            var mapped = _mapper.Map<List<CardTypeDto>>(cardtypes);

            await _cacheService.SetAsync(cacheKey, mapped,TimeSpan.FromMinutes(10));

            return mapped;
        }

       

       

        async Task<CardTypeDto?> ICardTypeService.GetByIdAsync(int id)
        {
            var entity = await _cardTypeRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return _mapper.Map<CardTypeDto>(entity);

        }

        public async Task<bool> CreateAsync(CardTypeDto dto)
        {
            var entity = _mapper.Map<CardType>(dto);
            await _cardTypeRepository.AddAsync(entity);
            await _cardTypeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(CardTypeDto dto)
        {
            var entity = await _cardTypeRepository.GetByIdAsync(dto.Id);
           if(entity == null) return false; 

           _mapper.Map(dto, entity);

            await _cardTypeRepository.UpdateAsync(entity);
            await _cardTypeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _cardTypeRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _cardTypeRepository.DeleteAsync(id);
            await _cardTypeRepository.SaveChangesAsync();
            return true;
        }
    }
}

