using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Categories.ArchiveCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public class ArchiveCategory:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPut("/api/v{version:apiVersion}/{id}/categories/archive", async (Guid id,ISender sender) =>
        {
            var result = await sender.Send(new ArchiveCategoryCommand(id));
            return Results.Ok(result);
        })
        .WithName("Archive Category")
        .WithSummary("Archive Category")
        .WithDescription("Archive Category")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}