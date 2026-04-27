using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Assistants;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Assistants.CreateAssistant;
internal sealed class CreateAssistantCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository<User> userRepository,
    IUserRepository<Assistant> assistantRepository,
    IDateTimeProvider dateTimeProvider,
    ILogger<CreateAssistantCommandHandler> logger) : ICommandHandler<CreateAssistantCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository<User> _userRepository = userRepository;
    private readonly IUserRepository<Assistant> _instructorRepository = assistantRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILogger<CreateAssistantCommandHandler> _logger = logger;
    public async Task<Result> Handle(CreateAssistantCommand request, CancellationToken cancellationToken)
    {
        var id = $"in_{Guid.CreateVersion7()}";
        var isParsed = DateOnly.TryParse(request.DateOfBirth, new CultureInfo("zh-CN"), out var date);
        if (!isParsed)
        {
            return Result.Failure(new Error("Date.InvalidFormat", "The input data is in invalid format"));
        }
        _logger.LogInformation("Starting to create assistant with keycloak id {UserId} in database", request.IdentityId);
        var assistantAsUser = User.Register(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            Date.Create(date),
            request.City,
            _dateTimeProvider.UtcNow,
            request.IdentityId);

        var assistant = new Assistant(id, "");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _userRepository.Add(assistantAsUser);
            _instructorRepository.Add(assistant);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);
            _logger.LogError(ex, "Can't create assistant with keycloak id {UserId} in database", request.IdentityId);
        }

        _logger.LogInformation("Creating assistant with keycloak id {UserId} in database done successfully.", request.IdentityId);

        return Result.Success();
    }
}
