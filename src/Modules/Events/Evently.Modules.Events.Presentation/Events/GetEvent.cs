using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.GetEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal  static class GetEvent
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id}", async (Guid id,ISender sender) =>
        {
            EventResponse @event = await sender.Send(new GetEventQuery(id));
            //EventResponse? @event = await context.Events.Where(e => e.Id == id)
            //    .Select(e => new EventResponse(e.Id,
            //        e.Title, e.Description,e.Location,e.StartsAtUtc,e.EndsAtUtc)).SingleOrDefaultAsync();
            return @event is null ? Results.NotFound() : Results.Ok(@event);
        }).WithTags(Tags.Events);
    }
}
