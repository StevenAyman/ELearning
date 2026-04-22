using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Data.Authorization;
public sealed class Permission
{
    private Permission() { }
    public Permission(string permission)
    {
        PermissionType = permission;
    }
    public int Id { get; init; }
    public string PermissionType { get; init; }
    public ICollection<Role> Roles { get; init; } = new HashSet<Role>();
}
