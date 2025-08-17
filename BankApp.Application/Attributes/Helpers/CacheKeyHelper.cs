using BankAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAppDomain.Constants;

using static BankAppDomain.Constants.ElasticSearchConstants;
using BankApp.Domain.Entities;
using Card = BankApp.Domain.Entities.Card;
using Customer = BankApp.Domain.Entities.Customer;

namespace BankApp.Application.Attributes.Helpers
{
    public static class CacheKeyHelper
    {
        public static string GetCacheKey(Type entityType)
        {
            if (entityType == typeof(Customer))
                return CacheContants.CustomerList;
            else if (entityType == typeof(Account))
                return CacheContants.AccountList;
            else if (entityType == typeof(Card))
                return CacheContants.CardList;
            else
                return CacheContants.DefaultKey;
        }
    }
}
