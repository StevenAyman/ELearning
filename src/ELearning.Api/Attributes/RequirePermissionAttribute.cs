using Microsoft.AspNetCore.Authorization;

namespace ELearning.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(params string[] allowedPermissions)
        :base(string.Join(",", allowedPermissions))
    {
    }
}
