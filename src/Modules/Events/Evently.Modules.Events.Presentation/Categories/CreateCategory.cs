using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Events.Application.Categories.CreateCategory;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public class CreateCategory:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPost("/api/v{version:apiVersion}/categories", async (CreateCategoryRequest request,ISender sender) =>
        {
            var command = request.Adapt<CreateCategoryCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("Create Category")
        .WithSummary("Create Category")
        .WithDescription("Create Category")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //.RequireAuthorization()
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public record CreateCategoryRequest(string Name);
  
}