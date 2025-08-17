using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDomain.Constants
{
    public static class ElasticSearchConstants
    {
        public const string DefaultIndex = "default-index";
        public static class Customer
        {
            public const string IndexName = "customers";
        }

        public static class Transaction
        {
            public const string IndexName =  "transactions";
        }
        public static class Card
        {
            public const string IndexName = "cards";
        }
        public static class Elastic
        {
            public const string CustomerCreatedLogsIndex = "customer-created-logs";
            public const string CustomerDeletedLogsIndex = "customer-deleted-logs";
            public const string AccountCreatedLogsIndex = "account-created-logs";
            public const string AccountDeletedLogsIndex = "account-deleted-logs";
        }
    }
}
