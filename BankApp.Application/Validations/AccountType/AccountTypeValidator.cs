using BankApp.Application.DTOs;
using BankAppDomain.Constants;

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Validations.AccountType
{
    public class AccountTypeValidator : AbstractValidator<AccountTypeDto>
    {
        public AccountTypeValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationConstants.Types.NameNotEmpty)
                .MaximumLength(20)
                .WithMessage(ValidationConstants.Types.NameLength);
        }
    }
}
