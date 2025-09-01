using AutoMapper;
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
    public TransactionService(ITransactionRepository transactionRepository,IMapper mapper, TransactionManager transactionManager,IConfiguration configuration)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _transactionManager = transactionManager;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<List<TransactionDto>> GetAllAsync()
    {     
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
        return transactionDtos;
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        return transaction != null ? _mapper.Map<TransactionDto>( transaction ) : null;
    }
    public async Task<bool> CreateAsync(TransactionCreateDto dto)
    {
        var createModel = _mapper.Map<TransactionCreateModel>(dto);

        var entity = _transactionManager.Create(createModel);

        await _transactionRepository.AddAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _transactionRepository.GetByIdAsync(id);
        if (existing == null) return false;

        var deleted = await _transactionRepository.DeleteAsync(id);
        return deleted;
    }
    public async Task<IEnumerable<TransactionViewDto>> GetTransactionsFromViewAsync()
    {
       
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
        var entity = await _transactionRepository.GetByIdAsync(dto.Id);

        if (entity == null)
            return false;
        _mapper.Map(dto, entity);
        await _transactionRepository.UpdateAsync(entity);
        return true;
    }

    async Task<IEnumerable<TransactionDto>> ITransactionService.GetAllTransactionsWithDetails()
    {
        var transactions = await _transactionRepository.GetTransactionsWithDetailsAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    } 
}