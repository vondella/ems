
using Carter;
using Evently.Common.Application.EventBus;
using Evently.Common.Application.Extensions;
using Evently.Common.Application.Messaging;
using Evently.Common.Infrastracture.Interceptors;
using Evently.Common.Infrastracture.Outbox;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastracture.Categories;
using Evently.Modules.Events.Infrastracture.Database;
using Evently.Modules.Events.Infrastracture.Events;
using Evently.Modules.Events.Infrastracture.Inbox;
using Evently.Modules.Events.Infrastracture.Outbox;
using Evently.Modules.Events.Infrastracture.PublicApi;
using Evently.Modules.Events.Infrastracture.TicketTypes;
using Evently.Modules.Events.Presentation.Events;
using Evently.Modules.Events.Presentation.Events.CancelEventSaga;
using Evently.Modules.Events.PublicApi;
using MassTransit;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Evently.Modules.Events.Infrastracture;
public static class EventModule
{
  
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        services.AddCarterWithAssemblies([Evently.Modules.Events.Presentation.AssemblyReference.Assembly]);
        services.AddInfrastracture(configuration);
        return services;
    }

    private static void AddInfrastracture(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventsApi, EventsApi>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Events:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
    }
    public static Action<IRegistrationConfigurator> ConfigureConsumers(MongoConfig config)
    {
        return registrationConfigurator => registrationConfigurator
            .AddSagaStateMachine<CancelEventSaga, CancelEventState>(sagaConfigurator =>
            {
                sagaConfigurator.UseInMemoryOutbox();
            })
            .MongoDbRepository(m =>
            {
                m.Connection = config.ConnectionString;
                m.DatabaseName = config.Database;
                m.CollectionName = config.Collection;
            });
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }

}
