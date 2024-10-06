using Asp.Versioning.Builder;
using Carter;
using Evently.Common.Application.Extensions;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Users.Presentation.Users;

public sealed record RegisterUserRequest(string Email, string Password, string FirstName, string LastName);

public class RegisterUser:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.VersionSets();

        app.MapPost("/api/v{version:apiVersion}/users/register", async (RegisterUserRequest request,ISender sender) =>
        {
            ResponseWrapper<Guid> result = await sender.Send(new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName));
            return Results.Ok(result);
        }).WithName("register user")
        .WithSummary("register user")
        .WithDescription("register user")
        .Produces<ResponseWrapper<Guid>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(1); ;
    }
}