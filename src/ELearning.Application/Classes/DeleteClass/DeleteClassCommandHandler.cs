using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;

namespace ELearning.Application.Classes.DeleteClass;
internal sealed class DeleteClassCommandHandler(
    ILearningClassRepository learningClassRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteClassCommand>
{
    private readonly ILearningClassRepository _learningClassRepository = learningClassRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var learningClass = await _learningClassRepository.GetByIdAsync(request.Id, cancellationToken);

        if (learningClass == null)
        {
            return Result.Failure(ClassErrors.NotFound);
        }

        _learningClassRepository.Delete(learningClass);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
