using BankApp.Application.DTOs.TransactionDtos;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController :ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<List<TransactionDto>> GetAll()
        {
            return await _transactionService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<TransactionDto> GetById(int id)
        {
            return await _transactionService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] TransactionCreateDto dto)
        {
            return await _transactionService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task<bool> Update(int id, [FromBody] TransactionUpdateDto dto)
        {
            if (id != dto.Id) return false;

            return await _transactionService.UpdateAsync(dto);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _transactionService.DeleteAsync(id);
        }

        [HttpGet("with-details")]
        public async Task<IEnumerable<TransactionDto>> GetAllWithDetails()
        {
            return await _transactionService.GetAllTransactionsWithDetails();
        }

        [HttpGet("from-view")]
        public async Task<IEnumerable<TransactionViewDto>> GetFromView()
        {
            return await _transactionService.GetTransactionsFromViewAsync();
        }
    }
}
