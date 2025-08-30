using BankApp.Application.Interfaces;
using BankApp.Application.Mapping;
using BankApp.Application.Services;
using BankApp.Infrastructure.Data;
using BankApp.Infrastructure.Repositories;
using BankAppDomain.Managers;
using System;
using System.Data;
using System.Text.Json.Serialization;
using BankApp.Application.Etos;
using BankApp.Application.EventHandlers;
using BankApp.Application.Events;
using BankApp.Application.Validations.Transaction;
using BankAppDomain.Constants;
using BankAppDomain.Events;
using BankAppDomain.Models.RabbitModels;
using BankAppDomain.Repositories;
using BankApp.Infrastructure.Messaging;
using BankApp.Workers.Consumers;
using BankApp.Workers.Workers;
using Dapper;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using Npgsql;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// DATABASE (PostgreSQL)
var pgConn = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseNpgsql(
        pgConn,
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("BankApp.Infrastructure");
            sqlOptions.EnableRetryOnFailure();
        }));

// REDIS
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!);

// RABBITMQ
builder.Services.AddSingleton(sp =>
{
    var cfg = builder.Configuration.GetSection("RabbitMQ").Get<RabbitSettings>()!;
    return new ConnectionFactory
    {
        HostName = cfg.HostName!,
        Port = cfg.Port,
        UserName = cfg.UserName!,
        Password = cfg.Password!,
        VirtualHost = cfg.VirtualHost,
        DispatchConsumersAsync = true,
        AutomaticRecoveryEnabled = true,
        NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
        RequestedHeartbeat = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = sp.GetRequiredService<ConnectionFactory>();
    return factory.CreateConnection();
});

builder.Services.AddScoped(typeof(IEventPublisher<>), typeof(RabbitMqEventPublisher<>));
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddHostedService<OutboxMessageDispatcher>();

// EVENT HANDLERS
builder.Services.AddScoped<IEventHandler<CustomerCreateEto>, CustomerCreateEventHandler>();
builder.Services.AddScoped<IEventHandler<CustomerDeleteEto>, CustomerDeleteEventHandler>();
builder.Services.AddScoped<IEventHandler<AccountCreateEto>, AccountCreateEventHandler>();
builder.Services.AddScoped<IEventHandler<CardCreateEto>, CardCreateEventHandler>();
builder.Services.AddScoped<AccountDeleteEventHandler>();

// ELASTICSEARCH
var elasticUri = new Uri(builder.Configuration["ElasticsearchSettings:Uri"]!);
builder.Services.AddSingleton<IElasticClient>(
    new ElasticClient(new ConnectionSettings(elasticUri)
        .DefaultIndex(ElasticSearchConstants.DefaultIndex)));
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

// EVENT CONSUMERS
builder.Services.AddHostedService<CustomerCreatedEventConsumer>();
builder.Services.AddHostedService<CustomerDeletedEventConsumer>();
builder.Services.AddHostedService<AccountCreatedEventConsumer>();
builder.Services.AddHostedService<CardCreateEventConsumer>();
builder.Services.AddHostedService<AccountDeleteEventConsumer>();

// AUTOMAPPER
builder.Services.AddAutoMapper(typeof(MappingProfile));

// HANGFIRE (PostgreSQL storage)
builder.Services.AddHangfire(cfg => cfg.UsePostgreSqlStorage(pgConn));
builder.Services.AddHangfireServer();

// REPOSITORIES
builder.Services.AddScoped(typeof(BankAppDomain.IRepository<>), typeof(EfCoreRepository<>));
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardTypeRepository, CardTypeRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
builder.Services.AddScoped<IPersonalFinancialInfoViewRepository, PersonalFinancialInfoViewRepository>();
builder.Services.AddScoped<ICustomerAccountCardViewRepository, CustomerAccountCardViewRepository>();
builder.Services.AddScoped<ICardAccountTransactionViewRepository, CardAccountTransactionViewRepository>();

// DOMAIN SERVICES
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardTypeService, CardTypeService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<IPersonalFinancialInfoViewService, PersonalFinancialInfoViewService>();
builder.Services.AddScoped<ICustomerAccountCardViewService, CustomerAccountCardViewService>();
builder.Services.AddScoped<ICardAccountTransactionViewService, CardAccountTransactionViewService>();

// MANAGERS
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<TransactionManager>();

// WORKERS
builder.Services.AddHostedService<EmailSenderWorker>();
builder.Services.AddHostedService<TransactionWorker>();
builder.Services.AddHostedService<CardActivityWorker>();

// CACHE
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// HTTP CONTEXT
builder.Services.AddHttpContextAccessor();

// CONTROLLERS
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.WriteIndented = true;
    })
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<TransactionUpdateDtoValidator>());

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BankApp API", Version = "v1" }));

// CORS
builder.Services.AddCors(o =>
    o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// APP
var app = builder.Build();

// Ensure database exists & apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<BankDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration failed.");
        throw;
    }
}

app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

RecurringJob.AddOrUpdate<ICardService>(
    "CheckAndUpdateCardsJob",
    svc => svc.CheckAndUpdateCardsAsync(),
    Cron.Minutely);

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
