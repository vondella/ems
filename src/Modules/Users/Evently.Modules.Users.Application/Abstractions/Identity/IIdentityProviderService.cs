using Evently.Common.Domain;

namespace Evently.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<ResponseWrapper<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}