using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Subjects;

namespace ELearning.Application.Subjects.UpdateSubject;
internal sealed class UpdateSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISubjectRepository subjectRepository) : ICommandHandler<UpdateSubjectCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISubjectRepository _subjectRepository = subjectRepository;

    public async Task<Result> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subject is null)
        {
            return Result.Failure(SubjectErrors.NotFound);
        }

        if (subject.Name == new TypeName(request.NewName))
        {
            return Result.Success();
        }

        var newName = request.NewName.Trim();

        var spec = new BaseSpecifications<Subject>(s => s.Name == new TypeName(newName));
        var NewSubjectFound = await _subjectRepository.GetWithSpecAsync(spec, cancellationToken);
        if (NewSubjectFound is not null)
        {
            return Result.Failure(SubjectErrors.Duplicate);
        }

        subject.UpdateName(new TypeName(newName));

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result <= 0)
        {
            return Result.Failure(new Error("Error", "Something went wrong while trying to update subject"));
        }

        return Result.Success();
    }
}
