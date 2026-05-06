using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;

namespace ELearning.Application.Instructors.RemoveAssignedInstructor;
internal sealed class RemoveAssignedInstructorCommandHandler(
    IInstructorReadService instructorReadService,
    IInstructorSubjectRepository instructorSubjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveAssignedInstructorCommand>
{
    private readonly IInstructorReadService _instructorReadService = instructorReadService;
    private readonly IInstructorSubjectRepository _instructorSubjectRepository = instructorSubjectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveAssignedInstructorCommand request, CancellationToken cancellationToken)
    {
        var IsFound = await _instructorReadService.IsInstructorAssignedAsync(
            request.InstructorId, request.ClassId, request.SubjectId, cancellationToken);

        if (!IsFound)
        {
            return Result.Failure(InstructorErrors.AssignmentNotExist);
        }

        _instructorSubjectRepository.Delete(request.InstructorId, request.ClassId, request.SubjectId);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
