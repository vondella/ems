using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Events.SearchEvents;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public class SearchEvents:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapGet("/api/v{version:apiVersion}/events/search", async ([AsParameters] SearchEventRequest request,ISender sender) =>
        {
            var query = request.Adapt<SearchEventsQuery>();
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("Search Event")
        .WithSummary("Search Event")
        .WithDescription("Search Event")
        .Produces<ResponseWrapper<SearchEventsResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public record SearchEventRequest(
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page,
        int PageSize);


}