using BankAppDomain.Views;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Repositories
{
    public class CardAccountTransactionViewRepository : ICardAccountTransactionViewRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public CardAccountTransactionViewRepository(IConfiguration configuration, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<CardAccountTransactionView>> GetAllAsync()
        {
            var query = "SELECT * FROM vw_CardAccountTransaction";
            var result = await _dbConnection.QueryAsync<CardAccountTransactionView>(query);
            return result;
        }

        public async Task<CardAccountTransactionView?> GetByIdAsync(int customerId)
        {
            var query = "SELECT * FROM vw_CardAccountTransaction WHERE CustomerId = @CustomerId";
            return await _dbConnection.QueryFirstOrDefaultAsync<CardAccountTransactionView>(query, new { CustomerId = customerId });
        }
    }
}
