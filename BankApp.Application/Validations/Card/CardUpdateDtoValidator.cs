using BankApp.Application.DTOs.CardDtos;
using BankAppDomain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Application.Validations.Card
{
    public class CardUpdateDtoValidator:AbstractValidator<CardUpdateDto>
    {
        public CardUpdateDtoValidator() 
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage(ValidationConstants.Card.CardNumberNotEmpty)
                .Length(11)
                .WithMessage(ValidationConstants.Card.CardNumberLength);

            RuleFor(x => x.CCV)
                .NotEmpty()
                .WithMessage(ValidationConstants.Card.CCVNotEmpty)
                .Length(3)
                .WithMessage(ValidationConstants.Card.CCVLength);
        
        }
    }
}
