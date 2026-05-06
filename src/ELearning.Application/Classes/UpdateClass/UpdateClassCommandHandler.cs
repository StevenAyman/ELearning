using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;

namespace ELearning.Application.Classes.UpdateClass;
internal sealed class UpdateClassCommandHandler(
    ILearningClassRepository learningClassRepository,
    IUnitOfWork unitOfWork,
    IClassReadService classReadService) : ICommandHandler<UpdateClassCommand>
{
    private readonly ILearningClassRepository _learningClassRepository = learningClassRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IClassReadService _classReadService = classReadService;

    public async Task<Result> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        var learningClass = await _learningClassRepository.GetByIdAsync(request.Id, cancellationToken);

        if (learningClass is null)
        {
            return Result.Failure(ClassErrors.NotFound);
        }

        if (learningClass.Type.Value == request.NewName)
        {
            return Result.Success();
        }

        var isExist = await _classReadService.GetByNameAsync(request.NewName, cancellationToken);

        if (isExist is not null)
        {
            return Result.Failure(ClassErrors.IsExist);
        }

        learningClass.UpdateType(new TypeName(request.NewName));
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
