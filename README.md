# BankApp

BankApp — a layered, event-driven banking application built with .NET 6.

## Architecture
4-layer structure (Infrastructure / Application / Domain / Core) with a separate Workers project for background services.

## Event-driven flow
Domain events (e.g. `CustomerCreatedEvent`, `CustomerDeletedEvent`) are published via `RabbitMqEventPublisher<T>` to RabbitMQ; BackgroundService consumers receive messages and forward them to the appropriate `IEventHandler`.

## Elasticsearch integration
Event handlers index incoming event data into indices like `customer-created-logs` through an `IElasticSearchService`, making events searchable and analyzable.

## DI & lifetime handling
Consumers/handlers run as singletons while `IElasticSearchService` is scoped — solved by creating scopes with `IServiceScopeFactory` inside handlers to properly resolve scoped services.

## Other technologies
- EF Core (SQL Server) for persistence  
- Redis for distributed caching  
- Hangfire for scheduled/background jobs  
- AutoMapper  
- FluentValidation  

## Development & testing
- RabbitMQ Management UI and API test endpoints used to publish events  
- Validated Elasticsearch with curl  
- Infrastructure can be run with **docker-compose** (Elasticsearch, RabbitMQ, Redis, SQL)  

## Goal
Build a loosely-coupled, scalable and observable system where important domain events are processed asynchronously and stored in Elasticsearch for retrospective queries and analytics.
