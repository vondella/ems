using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CreateEvent
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request,ISender sender ) =>
        {
            var command = new CreateEventCommand(request.Title,request.Description,request.Location,request.StartsAtUtc,request.EndsAtUtc);
            Guid @eventId = await sender.Send(command);
            return Results.Ok(@eventId);
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
