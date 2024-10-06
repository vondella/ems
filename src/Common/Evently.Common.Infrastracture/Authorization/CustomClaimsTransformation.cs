using System.Security.Claims;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain;
using Evently.Common.Infrastracture.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastracture.Authorization;

internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
{
    public async  Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
        {
            return principal;
        }
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
        string identityId = principal.GetIdentityId();

        ResponseWrapper<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);

        if (!result.IsSuccessful)
        {
            throw new EventlyException(nameof(IPermissionService.GetUserPermissionsAsync));
        }
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.ResponseData.UserId.ToString()));
        foreach (string permission in result.ResponseData.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }
        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}