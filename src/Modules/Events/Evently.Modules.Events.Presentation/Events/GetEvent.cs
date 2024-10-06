using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.GetEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public  class GetEvent:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapGet("/api/v{version:apiVersion}/events/{id}", async (Guid id, ISender sender) =>
        {
            var results = await sender.Send(new GetEventQuery(id));
            return Results.Ok(results);
        }).WithName("Get Event")
        .WithSummary("Get Event")
        .WithDescription("Get Event")
        .Produces<ResponseWrapper<EventResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}
