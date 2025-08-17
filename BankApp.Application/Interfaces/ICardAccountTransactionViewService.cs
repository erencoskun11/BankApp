using BankApp.Application.DTOs;
using BankAppDomain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Interfaces
{
    public interface ICardAccountTransactionViewService
    {
        Task<IEnumerable<CardAccountTransactionViewDto>>GetAllAsync();
        Task<CardAccountTransactionViewDto?> GetByIdAsync(int customerId);
    }
}
