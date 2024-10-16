using Carter;
using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Api.OpenTelemetry;
using Evently.Common.Application;
using Evently.Common.Infrastracture;
using Evently.Modules.Attendance.Infrastracture;
using Evently.Modules.Events.Infrastracture;
using Evently.Modules.Events.Infrastracture.Database;
using Evently.Modules.Ticketing.Infrastracture;
using Evently.Modules.Users.Infrastracture;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;
MongoConfig mongoConfig = builder.Configuration.GetSection(nameof(MongoConfig)).Get<MongoConfig>();

// Add services to the container.
builder.Services.AddApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly,
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
    Evently.Modules.Attendance.Application.AssemblyReference.Assembly]);


builder.Services.AddInfrastracture(DiagnosticsConfig.ServiceName, 
[EventModule.ConfigureConsumers(new MongoConfig
    {
         Database = mongoConfig!.Database,
         Collection = mongoConfig.Collection,
         ConnectionString = mongoConfig.ConnectionString
    }),TicketingModule.ConfigureConsumers,AttendanceModule.ConfigureConsumers
 ],databaseConnectionString!,redisConnectionString);

builder.Configuration.AddModuleConfiguration(["events","users","ticketing", "attendance"]);
builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);
builder.Services.AddAttendanceModule(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString);

WebApplication app = builder.Build();
app.UseLogContextTraceLogging();
app.UseSerilogRequestLogging();
app.UseApplication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}
app.MapCarter();
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();

public partial class Program;


