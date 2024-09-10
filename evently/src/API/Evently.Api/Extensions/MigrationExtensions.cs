﻿using Evently.Modules.Events.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

internal static  class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigration<EventsDbContext>(scope);
    }

    private static void ApplyMigration<TDBContext>(IServiceScope scope)
    where TDBContext:DbContext
    {
        using TDBContext context = scope.ServiceProvider.GetRequiredService<TDBContext>();
        context.Database.Migrate();
    }
}
