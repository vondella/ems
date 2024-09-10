using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Api;

public static class EventModule
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndPoint(app);
        GetEvent.MapEndPoint(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database")!;
        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                npgOptions => npgOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                .UseSnakeCaseNamingConvention();
        });
        return services;
    }
    
}
