using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastracture;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEventsModule(builder.Configuration);
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}
EventModule.MapEndPoints(app);

app.Run();


