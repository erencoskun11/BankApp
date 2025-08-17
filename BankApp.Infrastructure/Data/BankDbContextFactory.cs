using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BankApp.Infrastructure.Data
{
    public class BankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BankDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // PostgreSQL yerine SQL Server kullanıyoruz:
            optionsBuilder.UseSqlServer(connectionString);

            return new BankDbContext(optionsBuilder.Options);
        }
    }
}

