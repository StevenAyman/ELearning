using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;

namespace ELearning.Application.Classes.AddSubjectToClass;
internal sealed class AddSubjectToClassCommandHandler(
    ILearningClassRepository learningClassRepository,
    IClassReadService classReadService,
    ISubjectReadService subjectReadService,
    IUnitOfWork unitOfWork) : ICommandHandler<AddSubjectToClassCommand>
{
    private readonly ILearningClassRepository _learningClassRepository = learningClassRepository;
    private readonly IClassReadService _classReadService = classReadService;
    private readonly ISubjectReadService _subjectReadService = subjectReadService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddSubjectToClassCommand request, CancellationToken cancellationToken)
    {
        var requiredClass = await _learningClassRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (requiredClass is null)
        {
            return Result.Failure(ClassErrors.NotFound);
        }

        var subject = await _subjectReadService.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
        {
            return Result.Failure(SubjectErrors.NotFound);
        }

        var isFound = await _classReadService.IsSubjectExistInClassAsync(request.ClassId, request.SubjectId, cancellationToken);

        if (isFound)
        {
            return Result.Success();
        }

        _learningClassRepository.AddSubjectToClass(request.ClassId, request.SubjectId);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
