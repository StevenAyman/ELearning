using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using ELearning.Domain.Users;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Instructors.CreateInstructor;
internal sealed class CreateInstructorCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<User> userRepository,
    IUserRepository<Domain.Instructors.Instructor> instructorRepository,
    IDateTimeProvider dateTimeProvider,
    ILogger<CreateInstructorCommandHandler> logger) : ICommandHandler<CreateInstructorCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository<User> _userRepository = userRepository;
    private readonly IUserRepository<Domain.Instructors.Instructor> _instructorRepository = instructorRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILogger<CreateInstructorCommandHandler> _logger = logger;

    public async Task<Result> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var id = $"in_{Guid.CreateVersion7()}";
        var isParsed = DateOnly.TryParse(request.DateOfBirth, new CultureInfo("zh-CN"), out var date);
        if(!isParsed)
        {
            return Result.Failure(new Error("Date.InvalidFormat", "The input data is in invalid format"));
        }
        _logger.LogInformation("Starting to create instructor with keycloak id {UserId} in database", request.IdentityId);
        var instructorAsUser = User.Register(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            Date.Create(date),
            request.City,
            _dateTimeProvider.UtcNow,
            request.IdentityId);

        var instructor = new Domain.Instructors.Instructor(id, null);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _userRepository.Add(instructorAsUser);
            _instructorRepository.Add(instructor);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);
            _logger.LogError(ex, "Can't create instructor with keycloak id {UserId} in database", request.IdentityId);
        }

        _logger.LogInformation("Creating instructor with keycloak id {UserId} in database done successfully.", request.IdentityId);

        return Result.Success();

    }
}
