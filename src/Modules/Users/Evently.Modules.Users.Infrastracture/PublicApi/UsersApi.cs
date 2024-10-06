using Evently.Common.Domain;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.PublicApi;
using MediatR;
using UserResponse = Evently.Modules.Users.PublicApi.UserResponse;

namespace Evently.Modules.Users.Infrastracture.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<ResponseWrapper<UserResponse?>> GetAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        ResponseWrapper<Application.Users.GetUser.UserResponse> result =
            await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (!result.IsSuccessful)
        {
            return null;
        }

        return ResponseWrapper<UserResponse>.Success(new UserResponse(
            result.ResponseData.Id,
            result.ResponseData.Email,
            result.ResponseData.FirstName,
            result.ResponseData.LastName));
}
}