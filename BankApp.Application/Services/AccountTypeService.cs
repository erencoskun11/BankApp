using AutoMapper;
using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankAppDomain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankApp.Application.Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IRepository<AccountType> _accountTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public AccountTypeService(IRepository<AccountType> accountTypeRepository, IMapper mapper, ICacheService cacheService)
        {
            _accountTypeRepository = accountTypeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            
        }

        public async Task<List<AccountTypeDto>> GetAllAsync()
        {
            string cacheKey = "account_type_list";

            var cachedData = await _cacheService.GetAsync<List<AccountTypeDto>>(cacheKey);
            if (cachedData != null) { return cachedData; }

            var entities = await _accountTypeRepository.GetAllAsync();
            var mapped = _mapper.Map<List<AccountTypeDto>>(entities);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;

        }



        public async Task<AccountTypeDto?> GetByIdAsync(int id)
        {
            var entity=  await _accountTypeRepository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<AccountTypeDto>(entity) : null;
        }

        public async Task<bool> CreateAsync(AccountTypeDto dto)
        {
            var entity = _mapper.Map<AccountType>(dto); 

            await _accountTypeRepository.AddAsync(entity);
            await _accountTypeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(AccountTypeDto dto)
        {
            var existing = await _accountTypeRepository.GetByIdAsync(dto.Id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            await _accountTypeRepository.UpdateAsync(existing);
            await _accountTypeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _accountTypeRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _accountTypeRepository.DeleteAsync(id);
            await _accountTypeRepository.SaveChangesAsync();
            return true;
        }
     
    }
}



/*dbcontext hali eski 
 using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Application.Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly BankDbContext _context;

        public AccountTypeService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountType>> GetAllAsync()
        {
            return await _context.AccountTypes.ToListAsync();
        }

        public async Task<AccountType?> GetByIdAsync(int id)
        {
            return await _context.AccountTypes.FindAsync(id);
        }

        public async Task<AccountType> CreateAsync(AccountType accountType)
        {
            _context.AccountTypes.Add(accountType);
            await _context.SaveChangesAsync();
            return accountType;
        }

        public async Task<bool> UpdateAsync(AccountType accountType)
        {
            var existing = await _context.AccountTypes.FindAsync(accountType.Id);
            if (existing == null) return false;

            existing.Name = accountType.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.AccountTypes.FindAsync(id);
            if (existing == null) return false;

            _context.AccountTypes.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
*/