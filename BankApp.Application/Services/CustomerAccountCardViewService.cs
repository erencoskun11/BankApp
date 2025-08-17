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
    public class CustomerAccountCardViewService :ICustomerAccountCardViewService
    {
        private readonly ICustomerAccountCardViewRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CustomerAccountCardViewService(ICustomerAccountCardViewRepository repository, IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<IEnumerable<CustomerAccountCardViewDto>> GetAllAsync()
        {
            string cacheKey = "c_a_s_list";

            var cachedData = await _cacheService.GetAsync<List<CustomerAccountCardViewDto>>(cacheKey);
            if (cachedData != null) { return cachedData; }

            var data = await _repository.GetAllAsync();
            var mapped = _mapper.Map<List<CustomerAccountCardViewDto>>(data);

            await _cacheService.SetAsync(cacheKey, mapped,TimeSpan.FromMinutes(10));

            return mapped;
        }
        public async Task<CustomerAccountCardViewDto?> GetByIdAsync(int id)
        {
            var data =await _repository.GetByIdAsync(id);
            return data != null ? _mapper.Map<CustomerAccountCardViewDto>(data) : null;
        }
    }
}
