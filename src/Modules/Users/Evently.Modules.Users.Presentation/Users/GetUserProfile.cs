using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Evently.Common.Infrastracture.Authentication;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Application.Users.GetUser;

namespace Evently.Modules.Users.Presentation.Users;

public class GetUserProfile:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapGet("/api/v{version:apiVersion}/users/profile", async (ClaimsPrincipal claims, ISender sender) =>
        {
            var result = await sender.Send(new GetUserQuery(claims.GetUserId()));
            return Results.Ok(result);
        })
        .WithName("user profile")
        .WithSummary("user profile")
        .WithDescription("user profile")
        .Produces<ResponseWrapper<UserResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(Permissions.GetUser)
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }
}