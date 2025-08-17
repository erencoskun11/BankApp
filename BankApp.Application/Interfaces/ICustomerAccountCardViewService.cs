using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Application.DTOs;
namespace BankApp.Application.Interfaces
{
    public interface ICustomerAccountCardViewService 
    {
        Task<IEnumerable<CustomerAccountCardViewDto>> GetAllAsync();
        Task<CustomerAccountCardViewDto?> GetByIdAsync(int id);
    }
}
