using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
 
            private readonly ITransactionTypeService _transactionTypeService;


            public TransactionTypeController(ITransactionTypeService transationTypeService)
            {
                _transactionTypeService = transationTypeService;
            }

            [HttpGet]
            public async Task<List<TransactionTypeDto>> GetAll()
            {
                return await _transactionTypeService.GetAllAsync();

            }
            [HttpGet("{id}")]
            public async Task<TransactionTypeDto?> GetById(int id)
            {
                return await _transactionTypeService.GetByIdAsync(id);

            }

            [HttpPost]
            public async Task<bool> CreateAsync(TransactionTypeDto dto)
            {
                var result = await _transactionTypeService.CreateAsync(dto);
                return result;
            }

            [HttpPut("{id}")]
            public async Task<bool> UpdateAsync(int id, TransactionTypeDto dto)
            {

                return await _transactionTypeService.UpdateAsync(dto);


            }

            [HttpDelete("{id}")]
            public async Task<bool> Delete(int id)
            {
                return await _transactionTypeService.DeleteAsync(id);

            }






        }
    }


