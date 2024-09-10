using Evently.Modules.Events.Application;
using Evently.Modules.Events.Application.Abstractions.Clock;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastracture.Categories;
using Evently.Modules.Events.Infrastracture.Clock;
using Evently.Modules.Events.Infrastracture.Database;
using Evently.Modules.Events.Infrastracture.Events;
using Evently.Modules.Events.Infrastracture.TicketTypes;
using Evently.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Modules.Events.Infrastracture;
public static class EventModule
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        EventEndPoints.MapEndPoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly,includeInternalTypes:true);
        services.AddInfrastracture(configuration);
        return services;
    }

    private static void AddInfrastracture(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database")!;

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                    npgOptions => npgOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                .UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<EventsDbContext>());
    }
    
}
