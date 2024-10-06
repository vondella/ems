using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.CancelEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public class CancelEvent:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapDelete("/api/v{version:apiVersion}/events/{id}", async (Guid id,ISender sender) =>
        {
            var command = new CancelEventCommand(id);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("Delete Event")
        .WithSummary("Delete Event")
        .WithDescription("Delete Event")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}