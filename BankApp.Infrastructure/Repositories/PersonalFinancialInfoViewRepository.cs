using BankApp.Infrastructure.Data;
using BankApp.Infrastructure.Repositories;
using BankAppDomain.Views;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

public class PersonalFinancialInfoViewRepository : IPersonalFinancialInfoViewRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;
    public PersonalFinancialInfoViewRepository(IConfiguration configuration, IDbConnection connection)
    {
        _configuration = configuration;
        _connection = connection;
    }

    public async Task<IEnumerable<PersonalFinancialInfoView>> GetAllAsync()
    {
        string query = "SELECT * FROM PersonalFinancialInfoView"; // View ve dto daki isimlendirmeler farklıysa alias formatıyla maplenebilir.
        var result = await _connection.QueryAsync<PersonalFinancialInfoView>(query);
        return result;
    }

    public async Task<PersonalFinancialInfoView?> GetByIdAsync(int id)
    {
        string query = "SELECT * FROM PersonalFinancialInfoView WHERE CustomerId = @Id";
        var result = await _connection.QueryFirstOrDefaultAsync<PersonalFinancialInfoView>(query, new { Id = id });
        return result;
    }
}
