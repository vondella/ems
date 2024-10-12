using Evently.Modules.Attendance.Infrastracture.Database;
using Evently.Modules.Events.Infrastracture.Database;
using Evently.Modules.Ticketing.Infrastracture.Database;
using Evently.Modules.Users.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Evently.Api.Extensions;

internal static  class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigration<EventsDbContext>(scope);
        ApplyMigration<UsersDbContext>(scope);
        ApplyMigration<TicketingDbContext>(scope);
        ApplyMigration<AttendanceDbContext>(scope);
    }

    private static void ApplyMigration<TDBContext>(IServiceScope scope)
    where TDBContext:DbContext
    {
      
        using TDBContext context = scope.ServiceProvider.GetRequiredService<TDBContext>();
        context.Database.Migrate();
    }
}
