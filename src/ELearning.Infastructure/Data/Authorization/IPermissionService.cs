using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Data.Authorization;
public interface IPermissionService
{

    Task<IReadOnlyList<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<Role> GetRoleWithPermissionsAsync(int id, CancellationToken cancellationToken = default);
}
