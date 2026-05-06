using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Application.Classes.CreateClass;
internal sealed class CreateClassCommandHandler(
    ILearningClassRepository learningClassRepository,
    IUnitOfWork unitOfWork,
    IClassReadService classReadService) : ICommandHandler<CreateClassCommand, string>
{
    private readonly ILearningClassRepository _learningClassRepository = learningClassRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IClassReadService _classReadService = classReadService;

    public async Task<Result<string>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        var isFound = await _classReadService.GetByNameAsync(request.ClassName, cancellationToken);
        if (isFound is not null)
        {
            return Result<string>.Failure(ClassErrors.IsExist);
        }
        var id = $"lc_{Guid.CreateVersion7()}";
        var newClass = new LearningClass(id, new TypeName(request.ClassName));

        _learningClassRepository.Add(newClass);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result<string>.Failure(Errors.DatabaseError);
        }

        return Result<string>.Succuss(id);
    }
}
