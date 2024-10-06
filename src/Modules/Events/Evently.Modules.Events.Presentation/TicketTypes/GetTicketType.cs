using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public class GetTicketType:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapGet("/api/v{version:apiVersion}/ticket-types/{id}", async (Guid id,ISender sender) =>
        {
            var result = await sender.Send(new GetTicketTypeQuery(id));
            return Results.Ok(result);
        })
        .WithName("Get TicketType")
        .WithSummary("Get TicketType")
        .WithDescription("Get TicketType")
        .Produces<ResponseWrapper<TicketTypeResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}