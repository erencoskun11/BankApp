using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BankApp.Infrastructure.Data
{
    public class BankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder();

            var possibleFiles = new[]
            {
                Path.Combine(basePath, "appsettings.json"),
                Path.Combine(basePath, "..", "BankApp.API2", "appsettings.json"),
                Path.Combine(basePath, "..", "..", "BankApp.API2", "appsettings.json"),
                Path.Combine(basePath, "BankApp.API2", "appsettings.json")
            };

            bool added = false;
            foreach (var file in possibleFiles)
            {
                if (File.Exists(file))
                {
                    configBuilder.SetBasePath(Path.GetDirectoryName(file)!)
                                 .AddJsonFile(Path.GetFileName(file), optional: false, reloadOnChange: true);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                configBuilder.SetBasePath(basePath)
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            }

            configBuilder.AddEnvironmentVariables();
            var configuration = configBuilder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? Environment.GetEnvironmentVariable("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = "Host=localhost;Port=5432;Database=BankDb_Postgres;Username=postgres;Password=guest";
            }

            var optionsBuilder = new DbContextOptionsBuilder<BankDbContext>();
            optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly("BankApp.Infrastructure");
            });

            return new BankDbContext(optionsBuilder.Options);
        }
    }
}

