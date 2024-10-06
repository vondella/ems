using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public class UpdateCategory:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPut("/api/v{version:apiVersion}/categories/{id}", async (Guid id, UpdateCategoryRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateCategoryCommand(id, request.Name));
            return Results.Ok(result);
        })
        .WithName("Update Category")
        .WithSummary("Update Category")
        .WithDescription("Update Category")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
    public sealed class UpdateCategoryRequest
    {
        public string Name { get; init; }
    }
}