using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Subjects;

namespace ELearning.Application.Subjects.CreateSubject;
internal sealed class CreateSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISubjectRepository subjectRepository) : ICommandHandler<CreateSubjectCommand, string>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISubjectRepository _subjectRepository = subjectRepository;

    public async Task<Result<string>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subjectSpec = new BaseSpecifications<Subject>(s => s.Name == new TypeName(request.Name));
        var subjectFound = await _subjectRepository.GetWithSpecAsync(subjectSpec, cancellationToken);
        if (subjectFound is not null)
        {
            return Result<string>.Failure(SubjectErrors.Duplicate);
        }

        var name = request.Name.Trim();

        var subject = new Subject($"sb_{Guid.CreateVersion7()}", new TypeName(name));

        _subjectRepository.Add(subject);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result <= 0)
        {
            return Result<string>.Failure(new Error("Error", "Something went wrong while trying to create subject"));
        }

        return subject.Id;
    }
}
