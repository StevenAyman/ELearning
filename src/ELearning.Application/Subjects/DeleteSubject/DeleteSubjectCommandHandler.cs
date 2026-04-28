using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;

namespace ELearning.Application.Subjects.DeleteSubject;
internal sealed class DeleteSubjectCommandHandler(
    IUnitOfWork unitOfWork,
    ISubjectRepository subjectRepository) : ICommandHandler<DeleteSubjectCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISubjectRepository _subjectRepository = subjectRepository;

    public async Task<Result> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (subject is null)
        {
            return Result.Failure(SubjectErrors.NotFound);
        }

        _subjectRepository.Delete(subject);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(new Error("Error", "An Error has been occurred while trying to delete this subject"));
        }

        return Result.Success();
    }
}
