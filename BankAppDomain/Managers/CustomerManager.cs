using BankApp.Domain.Entities;
using BankAppDomain.Models.ManagersModels;
using BankAppDomain.Models.ViewModels;

namespace BankAppDomain.Managers
{
    public class CustomerManager
    {
        public CustomerManager() { }

        public Customer Create(CustomerCreateModel model)
        {
            Customer customer = new Customer
            {
                FullName = model.FullName,
                NationalId = model.NationalId,
                BirthPlace = model.BirthPlace,
                BirthDate = model.BirthDate,
                RiskLimit = model.RiskLimit,
            };
            return customer;
        }

        public void Update(Customer customer, CustomerUpdateModel updateModel)
        {
            customer.FullName = updateModel.FullName;
            customer.NationalId = updateModel.NationalId;
            customer.BirthPlace = updateModel.BirthPlace;
            customer.BirthDate = updateModel.BirthDate;
            customer.RiskLimit = updateModel.RiskLimit;
        }
    }
}

