using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BankApp.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeService _accountTypeService;


        public AccountTypeController (IAccountTypeService accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        public async Task<List<AccountTypeDto>> GetAll()
        {
            return await _accountTypeService.GetAllAsync();

        }
        [HttpGet("{id}")]
        public async Task<AccountTypeDto?> GetById(int id)
        {
            return await _accountTypeService.GetByIdAsync(id);

        }

        [HttpPost]
        public async Task<bool> CreateAsync(AccountTypeDto dto)
        {
            var result = await _accountTypeService.CreateAsync(dto);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateAsync(int id, AccountTypeDto dto)
        {

            return await _accountTypeService.UpdateAsync(dto);


        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _accountTypeService.DeleteAsync(id);

        }






    }
}
