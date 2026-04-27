using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Instructors.CreateInstructor;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using ELearning.Domain.Users;

namespace ELearning.Application.Students.RegisterStudent;
internal sealed class CreateInstructorCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<User> userRepository,
    IUserRepository<Student> studentRespository,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<RegisterStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository<User> _userRepository = userRepository;
    private readonly IUserRepository<Student> _studentRepository = studentRespository;
    private readonly IDateTimeProvider _datetimeProvider = dateTimeProvider;
    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var id = $"st_{Guid.CreateVersion7()}";
        var date = DateOnly.Parse(request.DateOfBirth, new CultureInfo("zh-CN"));
        var studentAsUser = User.Register(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            Date.Create(date),
            request.City,
            _datetimeProvider.UtcNow,
            request.IdentityId);

        var student = new Student(id);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _userRepository.Add(studentAsUser);
            _studentRepository.Add(student);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
        }
        catch
        {
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);
            return Result.Failure(StudentErrors.RegisterError);
        }

        return Result.Success();
        
    }
}
