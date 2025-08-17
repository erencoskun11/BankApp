using BankApp.Application.DTOs.AccountDtos;
using BankAppDomain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Validations.Account
{
    public class AccountUpdateDtoValidator:AbstractValidator<AccountUpdateDto>
    {
        public AccountUpdateDtoValidator() 
        {
            RuleFor(x => x.IBAN)
                .NotEmpty()
                .WithMessage(ValidationConstants.Account.IbanNotEmpty)
                .Length(26)
                .WithMessage(ValidationConstants.Account.IbanLength);

            RuleFor(x => x.AccountName)
                .NotEmpty()
                .WithMessage(ValidationConstants.Account.AccountNameNotEmpty)
                .MaximumLength(50)
                .WithMessage(ValidationConstants.Account.AccountNameLength);
        }
    }
}
