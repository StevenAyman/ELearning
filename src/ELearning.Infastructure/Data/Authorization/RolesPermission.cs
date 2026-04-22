using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Data.Authorization;
public sealed class RolesPermission
{
    public int RoleId { get; init; }
    public int PermissionId { get; init; }
}
