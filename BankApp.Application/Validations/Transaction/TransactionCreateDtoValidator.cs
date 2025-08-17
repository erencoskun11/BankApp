using BankApp.Application.DTOs.TransactionDtos;
using BankAppDomain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Validations.Transaction
{
    public class TransactionCreateDtoValidator:AbstractValidator<TransactionCreateDto>
    {
        public TransactionCreateDtoValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage(ValidationConstants.Transaction.AmountNotNull)
                .GreaterThan(0)
                .WithMessage(ValidationConstants.Transaction.AmountGreaterThan);

            RuleFor(x => x.TransactionDate)
                    .NotEmpty()
                    .WithMessage(ValidationConstants.Transaction.TransactionDateNotNull);

            RuleFor(x => x.Description)
               .NotEmpty()
               .WithMessage(ValidationConstants.Transaction.AmountNotNull)
               .MaximumLength(100)
               .WithMessage(ValidationConstants.Transaction.DescriptionMax100);

        }
    }
}
