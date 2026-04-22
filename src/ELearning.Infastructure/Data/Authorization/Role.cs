using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Data.Authorization;
public sealed class Role
{
    private Role() { }
    public Role(string role)
    {
        RoleType = role;
    }

    public int Id { get; init; }
    public string RoleType { get; init; }
    public ICollection<Permission> Permissions { get; init; } = new HashSet<Permission>();
}
