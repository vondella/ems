using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.RescheduleEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public class RescheduleEvent:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapPut("/api/v{version:apiVersion}/events/{id}", async (Guid id, RescheduleCommandRequest request,ISender sender) =>
        {
            var  result = await sender.Send(
                new RescheduleEventCommand(id, request.StartsAtUtc, request.EndsAtUtc));
            return Results.Ok(result);
        })
        .WithName("Reschedule Event")
        .WithSummary("Reschedule Event")
        .WithDescription("Reschedule Event")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public record RescheduleCommandRequest(DateTime StartsAtUtc, DateTime? EndsAtUtc);
 
}