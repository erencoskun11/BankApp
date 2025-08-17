using AutoMapper;
using BankApp.Application.DTOs.CustomerDto;
using BankApp.Application.DTOs.TransactionDtos;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure.Repositories;
using BankAppDomain.Managers;
using BankAppDomain.Models.ManagersModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly TransactionManager _transactionManager;
    private readonly string _connectionString;
    private readonly ICacheService _cacheService;

    public TransactionService(ITransactionRepository transactionRepository,IMapper mapper, TransactionManager transactionManager,IConfiguration configuration,ICacheService cacheService )
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _transactionManager = transactionManager;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _cacheService = cacheService;
    }
    public async Task<List<TransactionDto>> GetAllAsync()
    {
        string cacheKey = "transaction_list";

        // 1️⃣ Önce cache'den dene
        var cachedTransactions = await _cacheService.GetAsync<List<TransactionDto>>(cacheKey);
        if (cachedTransactions != null)
        {
            return cachedTransactions;
        }

        // 2️⃣ Cache yoksa veritabanından çek
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

        await _cacheService.SetAsync(cacheKey, transactionDtos, TimeSpan.FromSeconds(10));

        return transactionDtos;
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        return transaction != null ? _mapper.Map<TransactionDto>( transaction ) : null;
    }
    public async Task<bool> CreateAsync(TransactionCreateDto dto)
    {
        // DTO → ManagerModel dönüşümü
        var createModel = _mapper.Map<TransactionCreateModel>(dto);

        // Manager ile entity oluşturulması
        var entity = _transactionManager.Create(createModel);

        await _transactionRepository.AddAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _transactionRepository.GetByIdAsync(id);
        if (existing == null) return false;

        var deleted = await _transactionRepository.DeleteAsync(id);
        if(deleted)
        {
            await _cacheService.RemoveAsync("transaction_list");
        }

        return deleted;
    }
    public async Task<IEnumerable<TransactionViewDto>> GetTransactionsFromViewAsync()
    {
        // Dapper kullanarak view'den veri çekilecek kısım
        // Şimdilik örnek boş dönüş:
        using IDbConnection db =new SqlConnection(_connectionString);
        string sql = "SELECT * FROM vw_Transactions";

        var result = await db.QueryAsync<TransactionViewDto>(sql);
        return result; 
    }
    public async Task<IEnumerable<Transaction>> GetAllTransactionsWithDetails()
    {
        return await _transactionRepository.GetTransactionsWithDetailsAsync();
    }

    public async Task<bool> UpdateAsync(TransactionUpdateDto dto)
    {
        // Önce veritabanından entity'yi çek
        var entity = await _transactionRepository.GetByIdAsync(dto.Id);

        if (entity == null)
            return false;

        // DTO'daki verileri entity'ye kopyala (AutoMapper kullanıyorsan kolay)
        _mapper.Map(dto, entity);

        // Güncelle
        await _transactionRepository.UpdateAsync(entity);

        return true;

    }

    async Task<IEnumerable<TransactionDto>> ITransactionService.GetAllTransactionsWithDetails()
    {
        var transactions = await _transactionRepository.GetTransactionsWithDetailsAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

  
}


