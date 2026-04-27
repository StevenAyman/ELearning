using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Users.GetUserWithIdentity;
internal sealed class GetUserWithIdentityQueryHandler(IUserReadService userReadService) : IQueryHandler<GetUserWithIdentityQuery, UserDto>
{
    private readonly IUserReadService _userReadService = userReadService;
    public async Task<Result<UserDto>> Handle(GetUserWithIdentityQuery request, CancellationToken cancellationToken)
    {
        var user = await _userReadService.GetUserWithIdentityIdAsync(request.IdentityId, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(UserErrors.UserNotExist);
        }

        return user;
    }
}
