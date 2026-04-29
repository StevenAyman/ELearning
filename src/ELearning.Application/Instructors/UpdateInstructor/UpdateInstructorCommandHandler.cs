using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Instructors.UpdateInstructor;
internal sealed class UpdateInstructorCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<Instructor> instructorRepository) : ICommandHandler<UpdateInstructorCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository<Instructor> _instructorRepository = instructorRepository;

    public async Task<Result> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorRepository.GetByIdAsync(request.Id, cancellationToken);

        if (instructor is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        instructor.UpdateBio(new Bio(request.Bio));

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(new Error("Error", "Something went wrong while trying to update instructor"));
        }

        return Result.Success();
    }
}
