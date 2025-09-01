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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOutboxRepository _outboxRepository;

        public AccountService(AccountManager accountManager, IAccountRepository accountRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,IOutboxRepository outboxRepository)
        {
            _accountManager = accountManager;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _outboxRepository = outboxRepository;
        }

        public async Task<List<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<List<AccountDto>>(accounts);
           
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

                await _accountRepository.AddAsync(account);
                await _accountRepository.SaveChangesAsync();

                // Outbox message
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
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var existing = await _accountRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _accountRepository.DeleteAsync(id);

            return true;
        }
    }
}
