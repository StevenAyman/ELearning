using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using ELearning.Domain.Users;

namespace ELearning.Application.Students.UpdateStudent;
internal sealed class UpdateStudentCommandHandler(
    IUserRepository<Student> studentRepository,
    IUnitOfWork unitOfWork,
    IClassReadService classReadService) : ICommandHandler<UpdateStudentCommand>
{
    private readonly IUserRepository<Student> _studentRepository = studentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IClassReadService _classReadService = classReadService;

    public async Task<Result> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        var classToUpdate = await _classReadService.GetByIdAsync(request.ClassId, cancellationToken);

        if (classToUpdate is null)
        {
            return Result.Failure(ClassErrors.NotFound);
        }

        if (student.ClassId == request.ClassId)
        {
            return Result.Success();
        }

        student.UpdateClassId(request.ClassId);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
