using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;

namespace ELearning.Application.Classes.RemoveSubjectFromClass;
internal sealed class RemoveSubjectFromClassCommandHandler(
    ILearningClassRepository learningClassRepository,
    IClassReadService classReadService,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveSubjectFromClassCommand>
{
    private readonly ILearningClassRepository _learningClassRepository = learningClassRepository;
    private readonly IClassReadService _classReadService = classReadService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveSubjectFromClassCommand request, CancellationToken cancellationToken)
    {
        var isFound = await _classReadService.IsSubjectExistInClassAsync(request.ClassId, request.SubjectId, cancellationToken);

        if (!isFound)
        {
            return Result.Failure(ClassErrors.SubjectNotInClass);
        }

        _learningClassRepository.RemoveSubjectFromClass(request.ClassId, request.SubjectId);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
