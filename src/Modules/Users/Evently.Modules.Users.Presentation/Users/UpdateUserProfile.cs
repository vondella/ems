using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Modules.Users.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Evently.Common.Domain;
using Evently.Common.Infrastracture.Authentication;
using Microsoft.AspNetCore.Http;

namespace Evently.Modules.Users.Presentation.Users;

public class UpdateUserProfile:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();
        app.MapPut("/api/v{version:apiVersion}/users/profile", async (UpdateUserRequest request,ClaimsPrincipal claims, ISender sender) =>
        {
            var result = await sender.Send(new UpdateUserCommand(
                claims.GetUserId(),
                request.FirstName,
                request.LastName));
            return Results.Ok(result);
        })
        .WithName("update user profile")
        .WithSummary("update user profile")
        .WithDescription("update user profile")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(Permissions.ModifyUser)
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1);
    }

    public sealed record UpdateUserRequest(string FirstName, string LastName);
}