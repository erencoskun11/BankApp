using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using AutoMapper;
using BankAppDomain;
using BankApp.Application.DTOs;

namespace BankApp.Application.Services
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public TransactionTypeService(IRepository<TransactionType> transactionTypeRepository, IMapper mapper, ICacheService cacheService)
        {
            _transactionTypeRepository = transactionTypeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<TransactionTypeDto>> GetAllAsync()
        {
            string cacheKey = "transaction_type_list";

            // Cache'den veri almaya çalış
            var cachedData = await _cacheService.GetAsync<List<TransactionTypeDto>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            // Cache yoksa veritabanından çek
            var entities = await _transactionTypeRepository.GetAllAsync();
            var mapped = _mapper.Map<List<TransactionTypeDto>>(entities);

            // Cache'e ekle
            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }


        public async Task<TransactionTypeDto?> GetByIdAsync(int id)
        {
            var entity=  await _transactionTypeRepository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<TransactionTypeDto>(entity) : null;
        }

        public async Task<bool> CreateAsync(TransactionTypeDto dto)
        {
            var entity = _mapper.Map<TransactionType>(dto);
            await _transactionTypeRepository.AddAsync(entity);
            await _transactionTypeRepository.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _transactionTypeRepository.GetByIdAsync(id);
            if (existing != null) return false;

            await _transactionTypeRepository.DeleteAsync(id);
            await _transactionTypeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TransactionTypeDto dto)
        {
            var existing = await _transactionTypeRepository.GetByIdAsync(dto.Id);
            if (existing != null) return false;
            _mapper.Map(dto, existing);
            await _transactionTypeRepository.UpdateAsync(existing);
            await _transactionTypeRepository.SaveChangesAsync();
            return true;
        }
    }
}
