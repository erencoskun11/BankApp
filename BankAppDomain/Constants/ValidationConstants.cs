using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDomain.Constants
{
    public static class ValidationConstants
    {
        public static class Account
        {
            public const string IbanNotEmpty = "Iban bos olamaz";
            public const string IbanLength = "26 hane olmali";
            public const string AccountNameNotEmpty = "Accountname bos olamaz";
            public const string AccountNameLength = "50 den az karakter icermeli";
        }
        public static class Types
        {
            public const string NameNotEmpty = "Bos olamaz";
            public const string NameLength = "20 karakterden fazla olamaz";
        }
        public static class Card
        {
            public const string CardNumberNotEmpty = "bos olamaz";
            public const string CardNumberLength = "16 basamakli olmali";
            public const string CCVNotEmpty = "bos olamaz";
            public const string CCVLength = "3 haneli olmali";

        }
        public static class Customer
        {
        public const string BirthDateNotEmpty = "bos olamaz";
        public const string BirthPlaceNotEmpty = "bos olamaz";
        public const string NationalIdNotEmpty = "bos olamaz";
        public const string NationalIdLength = "11 hane olmali";
        public const string FullNameNotEmpty = "bos olamaz";
        public const string FullNameLength = "3 harften uzun olmali";
        }

        public static class Transaction
        {
            public const string AmountNotNull = "bos olamaz";
            public const string AmountGreaterThan = "O dan buyuk olmali";
            public const string TransactionDateNotNull = "bos olamaz";
            public const string DescriptionNotEmpty = "aciklama bos olamaz";
            public const string DescriptionMax100 = "100 karakterden az olmali";
        }



    }
}
