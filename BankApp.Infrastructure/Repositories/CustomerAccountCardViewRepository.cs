using BankAppDomain.Views;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Infrastructure.Repositories
{
    public class CustomerAccountCardViewRepository : ICustomerAccountCardViewRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;


        public CustomerAccountCardViewRepository(IConfiguration configuration, IDbConnection dbConnection)
        {
            _configuration = configuration;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<CustomerAccountCardView>> GetAllAsync()
        {
            var query = "SELECT * FROM vw_CustomerAccountCard"; // ← View adını buna göre güncelle
            var result = await _dbConnection.QueryAsync<CustomerAccountCardView>(query);
            return result;
        }
        public async Task<CustomerAccountCardView?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM vw_CustomerAccountCard WHERE CustomerId = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<CustomerAccountCardView>(query, new { Id = id });
    
        }
    }
}
