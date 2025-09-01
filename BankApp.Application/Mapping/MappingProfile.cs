using AutoMapper;
using BankApp.Application.DTOs;
using BankApp.Application.DTOs.AccountDtos;
using BankApp.Application.DTOs.CardDtos;
using BankApp.Application.DTOs.CustomerDto;
using BankApp.Application.DTOs.TransactionDtos;
using BankApp.Domain.Entities;
using BankAppDomain.Models.CacheModels;
using BankAppDomain.Models.ManagersModels;
using BankAppDomain.Models.ViewModels;
using BankAppDomain.Views;

namespace BankApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            CreateMap<CustomerCreateDto, CustomerCreateModel>();

            CreateMap<AccountCreateDto, Account>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<AccountCreateDto, AccountCreateModel>();
            CreateMap<Account, AccountCacheModel>();

            CreateMap<CardCreateDto, Card>().ReverseMap();
            CreateMap<Card, CardGetDto>().ReverseMap();
            CreateMap<CardUpdateDto, Card>().ReverseMap();

            CreateMap<TransactionCreateDto, TransactionCreateModel>();
            CreateMap<TransactionCreateDto, Transaction>().ReverseMap();
            CreateMap<TransactionUpdateDto, Transaction>().ReverseMap();
            CreateMap<TransactionDto, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionViewDto>().ReverseMap();

            CreateMap<AccountType, AccountTypeDto>().ReverseMap();
            CreateMap<CardType, CardTypeDto>().ReverseMap();
            CreateMap<TransactionType, TransactionTypeDto>().ReverseMap();

            CreateMap<PersonalFinancialInfoView, PersonalFinancialInfoDto>().ReverseMap();
            CreateMap<CustomerAccountCardView, CustomerAccountCardViewDto>().ReverseMap();
            CreateMap<CardAccountTransactionView, CardAccountTransactionViewDto>().ReverseMap();
        }
    }
}

