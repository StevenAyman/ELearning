using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Users;

namespace ELearning.Application.Users.UpdateUserProfile;
internal sealed class UpdateUserProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<User> userRepository) : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly IUserRepository<User> _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var spec = new BaseSpecifications<User>(u => u.IdentityId == request.IdentityId);
        var user = await _userRepository.GetWithSpecAsync(spec, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        var date = DateOnly.Parse(request.BirthDate, new CultureInfo("zh-CN"));

        user.UpdateDateOfBirth(Date.Create(date));
        user.UpdateFirstName(request.FirstName);
        user.UpdateLastName(request.LastName);
        user.UpdateCity(request.City);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result < 0)
        {
            return Result.Failure(new Error("User.Update", "Something went wrong while trying to update user"));
        }

        return Result.Success();
    }
}
