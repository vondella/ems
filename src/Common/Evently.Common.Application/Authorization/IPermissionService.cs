using Evently.Common.Domain;

namespace Evently.Common.Application.Authorization;

public interface IPermissionService
{
    Task<ResponseWrapper<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
