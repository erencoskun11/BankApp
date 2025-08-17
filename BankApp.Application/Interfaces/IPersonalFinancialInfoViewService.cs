using BankApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Interfaces
{
    public interface IPersonalFinancialInfoViewService
    {
        Task<IEnumerable<PersonalFinancialInfoDto>> GetAllAsync();
        Task<PersonalFinancialInfoDto> GetByIdAsync(int id);
        Task<List<PersonalFinancialInfoDto>> ToListAsync();
        
    }
}
