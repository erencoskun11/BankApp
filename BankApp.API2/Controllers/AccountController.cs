using AutoMapper;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BankApp.Application.DTOs.AccountDtos;
using Microsoft.EntityFrameworkCore;
using BankApp.Application.Attributes;
using BankApp.Application.Enums;

namespace BankApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        [CacheManagement(typeof(Account), CacheOperationType.Read)]

        public async Task<List<AccountDto>> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return accounts;
        }

        [HttpGet("{id}")]
        [CacheManagement(typeof(Account),CacheOperationType.Read)]
        public async Task<AccountDto> GetById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);

            var accountDto = _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        [HttpPost]
        [CacheRefresh(typeof(Account))]
        public async Task<bool> Create([FromBody] AccountCreateDto accountCreateDto)
        {
            return await _accountService.CreateAccountAsync(accountCreateDto);
        }

        [HttpPut("{id}")]
        [CacheRefresh(typeof(Account))]
        public async Task<bool> Update(int id, [FromBody] AccountUpdateDto dto)
        {
            if (id != dto.Id) return false;

            var update = await _accountService.UpdateAccountAsync(id, dto);
            return update;
        }

        [HttpDelete("{id}")]
        [CacheRefresh(typeof(Account))]
        public async Task<bool> Delete(int id)
        {
           
                var deleted = await _accountService.DeleteAccountAsync(id);
                if (!deleted) return false;
            return true;
        }
    }
}

