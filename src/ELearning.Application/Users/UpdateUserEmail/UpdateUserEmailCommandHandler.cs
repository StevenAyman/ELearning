using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Users;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Users.UpdateUserEmail;
internal sealed class UpdateUserEmailCommandHandler(
    IUserRepository<User> userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserEmailCommand>
{
    private readonly IUserRepository<User> _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
    {
        var userSpec = new BaseSpecifications<User>(u => u.Email == new Email(request.OldEmail));

        var user = await _userRepository.GetWithSpecAsync(userSpec, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        userSpec = new BaseSpecifications<User>(u => u.Email == new Email(request.NewEmail));
        var anotherUser = await _userRepository.GetWithSpecAsync(userSpec, cancellationToken);
        if (anotherUser is not null)
        {
            return Result.Failure(UserErrors.EmailExists);
        }

        user.UpdateEmail(new Email(request.NewEmail));
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return Result.Success();
        }

        return Result.Failure(new Error("Error", "Something went wrong while trying to update user email"));
    }
}
