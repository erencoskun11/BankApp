using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardTypeController
    {
        private readonly ICardTypeService _cardTypeService;

        public CardTypeController(ICardTypeService cardTypeService)
        {
            _cardTypeService = cardTypeService;
        }

        [HttpGet]
        public async Task<List<CardTypeDto>> GetAll()
        {
            return await _cardTypeService.GetAllAsync();
            
        }

        [HttpGet("{id}")]
        public async Task<CardTypeDto?> GetById(int id)
        {
            return await _cardTypeService.GetByIdAsync(id);
           
        }

        [HttpPost]
        public async Task<bool> Create(CardTypeDto dto)
        {
            var result = await _cardTypeService.CreateAsync(dto);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateAsync(int id ,CardTypeDto dto)
        {
            
            return await _cardTypeService.UpdateAsync(dto);
           

        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _cardTypeService.DeleteAsync(id);

        }
    }
}










