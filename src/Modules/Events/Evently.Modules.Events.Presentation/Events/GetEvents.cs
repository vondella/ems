using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public class GetEvents:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapGet("/api/v{version:apiVersion}/events", async (ISender sender) =>
        {
            var  result = await sender.Send(new GetEventsQuery());
            return Results.Ok(result);
        })
        .WithName("Get Events")
        .WithSummary("Get Events")
        .WithDescription("Get Events")
        .Produces<ResponseWrapper<IReadOnlyCollection<EventResponse>>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}