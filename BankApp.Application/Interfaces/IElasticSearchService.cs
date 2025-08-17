using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Interfaces
{
    public interface IElasticSearchService
    {
        Task IndexAsync<T>(T document, string indexName) where T : class;
        Task<IEnumerable<T>> SearchAsync<T>(string indexName, string searchText) where T : class;
        Task DeleteAsync<T>(string indexName, string id) where T : class;
    }
}
