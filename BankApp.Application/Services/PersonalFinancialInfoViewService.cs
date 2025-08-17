using AutoMapper;
using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Services
{
    public class PersonalFinancialInfoViewService : IPersonalFinancialInfoViewService
    {
        private readonly IPersonalFinancialInfoViewRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
    

        public PersonalFinancialInfoViewService(IPersonalFinancialInfoViewRepository repository, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _cacheService = cacheService;  
        }

        public async Task<IEnumerable<PersonalFinancialInfoDto>> GetAllAsync()
        {
            string cacheKey = "personal_financial_info_list";

            // Cache'den veriyi almaya çalış
            var cachedData = await _cacheService.GetAsync<List<PersonalFinancialInfoDto>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            // Cache yoksa veritabanından çek
            var views = await _repository.GetAllAsync();
            var mapped = _mapper.Map<List<PersonalFinancialInfoDto>>(views);

            // Cache'e ekle, örneğin 10 dakika için
            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }



        public async Task<PersonalFinancialInfoDto?> GetByIdAsync(int id)
        {
            var view = await _repository.GetByIdAsync(id);
            return view != null ?_mapper.Map<PersonalFinancialInfoDto>(view): null;
        }

        public async Task<List<PersonalFinancialInfoDto?>> ToListAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<PersonalFinancialInfoDto>>(entities);

        }
    }
}
