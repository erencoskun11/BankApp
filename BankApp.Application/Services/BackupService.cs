using BankApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Services
{
    public class BackupService
    {

        private readonly ITransactionRepository _transactionRepository;

        public BackupService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task BackupOldTransactionsAsync()
        {
            // 24 saatten eski işlemleri al
            var cutoffDate = DateTime.UtcNow.AddHours(-24);
            var oldTransactions = await _transactionRepository.GetTransactionsOlderThanAsync(cutoffDate);

            // Burada istediğin şekilde yedekleme yapabilirsin,
            // örneğin JSON olarak dosyaya yazabilir veya başka bir yere gönderebilirsin.

            // Örnek olarak konsola yazdırma:
            Console.WriteLine($"Yedeklenecek {oldTransactions.Count} adet eski işlem bulundu.");

            // TODO: Dosyaya yazma veya başka yedekleme işlemleri
        }
    }
}

