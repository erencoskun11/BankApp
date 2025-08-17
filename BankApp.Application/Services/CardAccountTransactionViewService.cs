using AutoMapper;
using BankApp.Application.DTOs;
using BankApp.Application.Interfaces;
using BankApp.Infrastructure.Repositories;
using BankAppDomain.Views;

public class CardAccountTransactionViewService : ICardAccountTransactionViewService
{
    private readonly ICardAccountTransactionViewRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    public CardAccountTransactionViewService(ICardAccountTransactionViewRepository repository,IMapper mapper, ICacheService cecheService)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheService = cecheService;
    }

    public async Task<IEnumerable<CardAccountTransactionViewDto>> GetAllAsync()
    {
        string cacheKey = "card_account_transaction_view_list";

        var cachedData = await _cacheService.GetAsync<List<CardAccountTransactionViewDto>>(cacheKey);
        
        if(cachedData != null)
        {
            return cachedData;
        }

        var view = await _repository.GetAllAsync();
        var mappedViews = _mapper.Map<List<CardAccountTransactionViewDto>>(view);

        await _cacheService.SetAsync(cacheKey, mappedViews, TimeSpan.FromMinutes(10));
        return mappedViews;
    
    }

    public async Task<CardAccountTransactionViewDto?> GetByIdAsync(int customerId)
    {
        var view =await _repository.GetByIdAsync(customerId);
        return _mapper.Map<CardAccountTransactionViewDto?>(view);
    }
}
