using AutoMapper;
using BankApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using BankApp.Application.DTOs.CardDtos;
using BankApp.Application.Interfaces;
using BankApp.Application.Attributes;
using BankApp.Application.Enums;

namespace BankApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly IMapper _mapper;

    public CardController(ICardService cardService, IMapper mapper)
    {
        _cardService = cardService;
        _mapper = mapper;
    }

    [HttpGet]
    [CacheManagement(typeof(Card),CacheOperationType.Read)]
    public async Task<List<CardGetDto>> GetAll()
    {
        return await _cardService.GetAllAsync();
    }

    [HttpGet("expired")]
    [CacheManagement(typeof(Card),CacheOperationType.Read)]
    public async Task<IActionResult>GetExpiredCards()
    {
        var expiredCards = await _cardService.GetExpiredCardsAsync();
        return Ok(expiredCards);
    }



    [HttpGet("{id}")]
    [CacheManagement(typeof(Card),CacheOperationType.Read)]
    public async Task<CardGetDto?> GetById(int id)
    {
        var card = await _cardService.GetByIdAsync(id);

        return card is null? null : _mapper.Map<CardGetDto>(card);
    }

    [HttpPost]
    public async Task<bool> Create([FromBody] CardCreateDto dto)
    {
        return await _cardService.CreateAsync(dto);
       }

    [HttpPut("{id}")]
    public async Task<bool> Update(int id, [FromBody] CardUpdateDto dto)
    {
        if (id != dto.Id) return false;

        return await _cardService.UpdateAsync(id, dto);

    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(int id)
    {
        return await _cardService.DeleteAsync(id);
    }
}


