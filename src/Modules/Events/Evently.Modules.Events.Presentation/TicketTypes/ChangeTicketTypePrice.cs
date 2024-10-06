using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public class ChangeTicketTypePrice:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapPut("/api/v{version:apiVersion}/ticket-types/{id}/price", async (Guid id, ChangeTicketTypePriceRequest request,ISender sender) =>
        {
            var result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));
            return Results.Ok(result);
        })
        .WithName("Change TicketType Price")
        .WithSummary("Change TicketType Price")
        .WithDescription("Change TicketType Price")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public record ChangeTicketTypePriceRequest(decimal Price);

}