
using ELearning.Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ELearning.Api.Extensions;

public static class PermissionExtensions
{
    public static void RequirePermission(this AuthorizationPolicyBuilder builder, params string[] allowedPermissions)
    {
        builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
    }
}
