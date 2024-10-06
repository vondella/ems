using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public class CreateTicketType:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPost("/api/v{version:apiVersion}/ticket-types", async (CreateTicketTypeRequest request,ISender sender) =>
            {
                var command = request.Adapt<CreateTicketTypeCommand>();
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
        .WithName("Create TicketType")
        .WithSummary("Create TicketType")
        .WithDescription("Create TicketType")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public record CreateTicketTypeRequest(Guid EventId, string Name, decimal Price, string Currency, decimal Quantity);

}