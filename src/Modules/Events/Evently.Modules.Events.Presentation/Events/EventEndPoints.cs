using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public static class EventEndPoints
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndPoint(app);
        GetEvent.MapEndPoint(app);
    }
}