using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Instructors.AssignToSubject;
internal sealed class AssignToSubjectCommandHandler(
    IInstructorSubjectRepository instructorSubjectRepository,
    IUnitOfWork unitOfWork,
    IClassReadService classReadService,
    IInstructorReadService instructorReadService) : ICommandHandler<AssignToSubjectCommand>
{
    private readonly IInstructorSubjectRepository _instructorSubjectRepository = instructorSubjectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IClassReadService _classReadService = classReadService;
    private readonly IInstructorReadService _instructorReadService = instructorReadService;

    public async Task<Result> Handle(AssignToSubjectCommand request, CancellationToken cancellationToken)
    {
        var isSubjectFoundInClass = await _classReadService.IsSubjectExistInClassAsync(request.ClassId, request.SubjectId, cancellationToken);

        if (!isSubjectFoundInClass)
        {
            return Result.Failure(ClassErrors.SubjectNotInClass);
        }

        var isInstructorAssigned = await _instructorReadService.IsInstructorAssignedAsync(
            request.InstructorId, request.ClassId, request.SubjectId, cancellationToken);

        if (isInstructorAssigned)
        {
            return Result.Success();
        }

        var instructor = await _instructorReadService.GetByIdAsync(request.InstructorId, cancellationToken);
        if (instructor is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        _instructorSubjectRepository.Add(request.InstructorId, request.ClassId, request.SubjectId);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
