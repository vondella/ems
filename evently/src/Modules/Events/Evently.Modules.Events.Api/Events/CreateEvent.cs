using Evently.Modules.Events.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Api.Events;

public static class CreateEvent
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request,EventsDbContext context) =>
        {
            var @event = new Event
            {
                Id=Guid.NewGuid(),
                Title=request.Title,
                Description=request.Description,
                Location=request.Location,
                StartsAtUtc=request.StartsAtUtc,
                EndsAtUtc=request.EndsAtUtc
            };
            context.Events.Add(@event);
            await context.SaveChangesAsync();
            return Results.Ok(@event.Id);
        }).WithTags(Tags.Events);
    }
    internal sealed class Request
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartsAtUtc { get; private set; }
        public DateTime EndsAtUtc { get; private set; }
    }
}
