using AutoMapper;
using BankApp.Application.Attributes;
using BankApp.Application.DTOs;
using BankApp.Application.DTOs.CustomerDto;
using BankApp.Application.Enums;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankAppDomain.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly ICardAccountTransactionViewService _cardAccountTransactionService;

        public CustomerController(ICustomerService customerService, IMapper mapper, ICardAccountTransactionViewService cardAccountTransactionService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _cardAccountTransactionService = cardAccountTransactionService;

        }


        [HttpGet]
        [CacheManagement(typeof(Customer),CacheOperationType.Read)]
        public async Task<List<CustomerDto>> GetAll()
        {
            return await _customerService.GetAllCustomersAsync();
        }


       

        // Burda data çekme ve mapleme işlemleri servisin içinde olsun.
        [HttpGet("{id}")]
        [CacheManagement(typeof(Customer), CacheOperationType.Read)]
        public async Task<CustomerDto?> GetById(int id)
        {
            //return await _customerService.GetCustomerByIdAsync(id);
            //Bu şekilde tek satırda sonucu dönecek şekilde servis güncellenmeli.

            return await _customerService.GetCustomerByIdAsync(id);
        }

        [HttpPost]
        [CacheManagement(typeof(Customer), CacheOperationType.Refresh)]
        public async Task<bool> Create([FromBody] CustomerCreateDto dto)
        {
            return await _customerService.CreateCustomerAsync(dto);
        }

        [HttpPut("{id}")]
        [CacheManagement(typeof(Customer), CacheOperationType.Refresh)]
        public async Task<bool> Update(int id, [FromBody] CustomerUpdateDto dto)
        {
            if (id != dto.Id) return false;

            return await _customerService.UpdateCustomerAsync(dto);
        }

        [CacheManagement(typeof(Customer), CacheOperationType.Refresh)]
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _customerService.DeleteCustomerAsync(id);
        }

        [HttpGet("transactions")]
        public async Task<IEnumerable<CardAccountTransactionViewDto>> GetCardAccountTransactions()
        {
            return await _cardAccountTransactionService.GetAllAsync();
        }


    }
}

