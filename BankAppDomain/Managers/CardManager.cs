using BankApp.Domain.Entities;
using BankAppDomain.Models.ManagersModels;

namespace BankAppDomain.Managers
{
    public class CardManager
    {
        public CardManager() { }

        public Card Create(CardCreateModel model)
        {
            var card = new Card
            {
                CardNumber = model.CardNumber,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                CCV = model.CCV,
                CardTypeId = model.CardTypeId,
                IsActive = true
            };
            return card;
        }

        public void Update(Card card, CardUpdateModel updateModel)
        {
            card.CardNumber = updateModel.CardNumber;
            card.ExpiryMonth = updateModel.ExpiryMonth;
            card.ExpiryYear = updateModel.ExpiryYear;
            card.CCV = updateModel.CCV;
            card.CardTypeId = updateModel.CardTypeId;
            card.IsActive = updateModel.IsActive;
        }
    }
}
