using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Users.GetUserWithIdentity;

namespace ELearning.Application.Abstractions.Data;
public interface IUserReadService
{
    Task<UserDto?> GetUserWithIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
}
