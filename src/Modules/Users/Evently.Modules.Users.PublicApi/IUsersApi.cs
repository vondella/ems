using Evently.Common.Domain;

namespace Evently.Modules.Users.PublicApi;

public interface IUsersApi
{
    Task<ResponseWrapper<UserResponse?>> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}