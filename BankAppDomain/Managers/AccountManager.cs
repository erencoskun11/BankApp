using BankApp.Domain.Entities;
using BankAppDomain.Models;
using BankAppDomain.Models.ManagersModels;

namespace BankAppDomain.Managers
{
    public class AccountManager
    {
        public AccountManager() { }
        public Account Create(AccountCreateModel accountCreateModel)
        {
            return new Account(DateTime.UtcNow, true)
            {
                AccountName = accountCreateModel.AccountName,
                AccountNumber = accountCreateModel.AccountNumber,
                IBAN = accountCreateModel.IBAN,
                CustomerId = accountCreateModel.CustomerId
            };
        }
        public void Update(Account account, AccountUpdateModel updateModel)
        {
            account.AccountName = updateModel.AccountName;
            account.AccountNumber = updateModel.AccountNumber;
            account.IBAN = updateModel.IBAN;
            account.IsActive = updateModel.IsActive;
        }
    }
}
