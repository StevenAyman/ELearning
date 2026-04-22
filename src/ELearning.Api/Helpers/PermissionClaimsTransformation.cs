using System.Security.Claims;
using ELearning.Infastructure.Data.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace ELearning.Api.Helpers;

public sealed class PermissionClaimsTransformation(IPermissionService permissionService) : IClaimsTransformation
{
    private readonly IPermissionService _permissionService = permissionService;
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        var userRoles = principal.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
        if (userRoles is null)
        {
            return principal;
        }

        var allRoles = await _permissionService.GetAllRolesAsync();
        var requiredRole = allRoles.FirstOrDefault(r => userRoles.Contains(r.RoleType, StringComparison.OrdinalIgnoreCase));
        if (requiredRole is null)
        {
            return principal;
        }

        var roleWithPermissions = await _permissionService.GetRoleWithPermissionsAsync(requiredRole.Id);
        var permissions = roleWithPermissions.Permissions;
        var identity = (ClaimsIdentity) principal.Identity;
        foreach( var permission in permissions)
        {
            identity.AddClaim(new Claim(CustomClaims.Permission, permission.PermissionType));
        }

        return principal;
    }
}
