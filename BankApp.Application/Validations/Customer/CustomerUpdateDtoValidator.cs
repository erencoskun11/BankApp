using BankApp.Application.DTOs.CustomerDto;
using BankAppDomain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Validations.Customer
{
    public class CustomerUpdateDtoValidator:AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty()
                .WithMessage(ValidationConstants.Customer.NationalIdNotEmpty)
                .Length(11).WithMessage(ValidationConstants.Customer.NationalIdLength);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(ValidationConstants.Customer.FullNameNotEmpty)
                .MinimumLength(3)
                .WithMessage(ValidationConstants.Customer.FullNameLength);
            RuleFor(x => x.BirthDate)
               .NotEmpty()
               .WithMessage(ValidationConstants.Customer.BirthDateNotEmpty);


            RuleFor(x => x.BirthPlace)
                .NotEmpty()
                .WithMessage(ValidationConstants.Customer.BirthPlaceNotEmpty);


        }
    }
}
