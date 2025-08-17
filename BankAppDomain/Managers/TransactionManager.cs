using BankApp.Domain.Entities;
using BankAppDomain.Models.ManagersModels;

namespace BankAppDomain.Managers
{
    public class TransactionManager
    {
        public TransactionManager() { }

        public Transaction Create(TransactionCreateModel model)
        {
            return new Transaction
            {
                Amount = model.Amount,
                Description = model.Description,
                TransactionDate = model.TransactionDate,
                AccountId = model.AccountId,
                CardId = model.CardId,
                TransactionTypeId = model.TransactionTypeId
            };
        }

        public void Update(Transaction existing, TransactionUpdateModel updateModel)
        {
            existing.Amount = updateModel.Amount;
            existing.Description = updateModel.Description;
            existing.TransactionDate = updateModel.TransactionDate;
            existing.AccountId = updateModel.AccountId;
            existing.CardId = updateModel.CardId;
            existing.TransactionTypeId = updateModel.TransactionTypeId;
        }
    }
}
