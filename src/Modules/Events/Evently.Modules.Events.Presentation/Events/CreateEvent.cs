using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.CreateEvent;
using Evently.Modules.Events.Domain.Categories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public  class CreateEvent:ICarterModule
{

    public record CreateEventRequest(
        Guid CategoryId,
        string Title,
        string Description,
        string Location,
        DateTime StartsAtUtc,
        DateTime EndsAtUtc
    );

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPost("/api/v{version:apiVersion}/events", CreateEvents)
            .WithName("create Event")
            .WithSummary("create Event")
            .WithDescription("create Event")
            .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            //.RequireAuthorization()
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
    }
    public static async Task<Results<Ok<ResponseWrapper<Guid>>, BadRequest<string>>> CreateEvents(CreateEventRequest request, ISender sender)
    {
        var command = request.Adapt<CreateEventCommand>();
        var result = await sender.Send(command);
        return TypedResults.Ok(result);
    }
}
