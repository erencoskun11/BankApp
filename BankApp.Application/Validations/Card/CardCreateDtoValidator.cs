using System;
using System.Collections.Generic;
using BankAppDomain.Constants;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Application.DTOs.CardDtos;
using FluentValidation;

namespace BankApp.Application.Validations.Card
{
    public class CardCreateDtoValidator:AbstractValidator<CardUpdateDto>
    {
        public CardCreateDtoValidator() 
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage(ValidationConstants.Card.CardNumberNotEmpty)
                .Length(16)
                .WithMessage(ValidationConstants.Card.CardNumberLength);

            RuleFor(x => x.CCV)
                .NotEmpty()
                .WithMessage(ValidationConstants.Card.CCVNotEmpty)
                .Length(3)
                .WithMessage(ValidationConstants.Card.CCVLength);


        }
    }
}
