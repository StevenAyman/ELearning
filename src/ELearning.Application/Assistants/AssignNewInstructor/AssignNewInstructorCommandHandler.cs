using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Assistants;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Assistants.AssignNewInstructor;
internal sealed class AssignNewInstructorCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<Assistant> assistantRepository,
    IUserRepository<Instructor> instructorRepository) : ICommandHandler<AssignNewInstructorCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository<Assistant> _assistantRepository = assistantRepository;
    private readonly IUserRepository<Instructor> _instructorRepository = instructorRepository;

    public async Task<Result> Handle(AssignNewInstructorCommand request, CancellationToken cancellationToken)
    {
        var assistant = await _assistantRepository.GetByIdAsync(request.Id, cancellationToken);

        if (assistant is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        var instructor = await _instructorRepository.GetByIdAsync(request.InstructorId, cancellationToken);

        if (instructor is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        if (assistant.InstructorId ==  instructor.Id)
        {
            return Result.Success();
        }

        assistant.UpdateInstructor(instructor.Id);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(new Error("Error", "Something went wrong while trying to assign new instructor"));
        }

        return Result.Success();
    }
}
