using AutoMapper;
using BankApp.Application.DTOs.AccountDtos;
using BankApp.Application.Interfaces;
using BankApp.Infrastructure.Repositories;
using BankAppDomain.Constants;
using BankAppDomain.Entities;
using BankAppDomain.Managers;
using BankAppDomain.Models;
using BankAppDomain.Models.CacheModels;
using BankAppDomain.Models.ManagersModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BankApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountManager _accountManager;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOutboxRepository _outboxRepository;

        public AccountService(AccountManager accountManager, IAccountRepository accountRepository, IMapper mapper, ICacheService cacheService, IHttpContextAccessor httpContextAccessor,IOutboxRepository outboxRepository)
        {
            _accountManager = accountManager;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _httpContextAccessor = httpContextAccessor;
            _outboxRepository = outboxRepository;
        }

        public async Task<List<AccountDto>> GetAllAccountsAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                 ?? "anonymous";

            string cacheKey = $"{CacheContants.AccountList}_{userId}";

            var cachedAccounts = await _cacheService.GetAsync<List<AccountDto>>(cacheKey);
            if (cachedAccounts != null)
            {
                Console.WriteLine($"📦 Cache'ten getirildi ({CacheContants.AccountList})");
                return cachedAccounts;
            }

            Console.WriteLine($"🛢 Veritabanından getirildi ({CacheContants.AccountList})");
            var accounts = await _accountRepository.GetAllAsync();
            var accountDtos = _mapper.Map<List<AccountDto>>(accounts);
            await _cacheService.SetAsync(cacheKey, accountDtos, TimeSpan.FromMinutes(10));

            return accountDtos;
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account == null ? null : _mapper.Map<AccountDto?>(account);
        }

        public async Task<bool> CreateAccountAsync(AccountCreateDto accountCreateDto)
        {
            try
            {
                var accountCreateModel = _mapper.Map<AccountCreateDto, AccountCreateModel>(accountCreateDto);
                var account = _accountManager.Create(accountCreateModel);
                var accountCacheModel = _mapper.Map<AccountCacheModel>(account);

                await _accountRepository.AddAsync(account);
                await _accountRepository.SaveChangesAsync();

                // 👉 OutboxMessage üret ve kaydet
                var outboxMessage = new OutboxMessage
                {
                    EventType = "AccountCreated",
                    Content = JsonConvert.SerializeObject(new
                    {
                        account.Id,
                        account.AccountName,
                        account.AccountNumber,
                        account.IBAN,
                        account.CustomerId
                    })
                };
                await _outboxRepository.AddAsync(outboxMessage); 





                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "anonymous";
                var cacheKey = $"{CacheContants.AccountList}_{userId}";

                // Cache temizleme
                await _cacheService.RemoveAsync(cacheKey);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAccountAsync(int id, AccountUpdateDto updateDto)
        {
            var existing = await _accountRepository.GetByIdAsync(id);
            if (existing == null) return false;

            var updateModel = _mapper.Map<AccountUpdateDto, AccountUpdateModel>(updateDto);
            _accountManager.Update(existing, updateModel);

            await _accountRepository.UpdateAsync(existing);

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "anonymous";
            var cacheKey = $"{CacheContants.AccountList}_{userId}";

            // Cache temizleme
            await _cacheService.RemoveAsync(cacheKey);

            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var existing = await _accountRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _accountRepository.DeleteAsync(id);

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "anonymous";
            var cacheKey = $"{CacheContants.AccountList}_{userId}";

            // Cache temizleme
            await _cacheService.RemoveAsync(cacheKey);

            return true;
        }
    }
}

/* dbcontext kismi 
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankDbContext _context;

        public AccountService(BankDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Cards)
                .Include(a => a.Transactions)
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Cards)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.OpenedAt = DateTime.UtcNow;
            account.IsActive = true;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            var existing = await _context.Accounts.FindAsync(account.Id);
            if (existing == null) return false;

            existing.AccountName = account.AccountName;
            existing.AccountNumber = account.AccountNumber;
            existing.IBAN = account.IBAN;
            existing.IsActive = account.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var existing = await _context.Accounts.FindAsync(id);
            if (existing == null) return false;

            _context.Accounts.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
*/