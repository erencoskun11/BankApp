using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Attributes.Interfaces
{
    public interface ICacheKeyProvider
    {
        string GetCacheKey();
        string GetSingleKey(object id);
        Type CacheValueType { get; }
    }
}
