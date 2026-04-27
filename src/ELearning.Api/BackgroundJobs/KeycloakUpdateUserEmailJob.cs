using ELearning.Application.Users.UpdateUserEmail;
using MediatR;

namespace ELearning.Api.BackgroundJobs;

public sealed class KeycloakUpdateUserEmailJob(ISender sender, ILogger<KeycloakUpdateUserEmailJob> logger)
{
    private readonly ISender _sender = sender;
    private readonly ILogger<KeycloakUpdateUserEmailJob> _logger = logger;
    public async Task ProcessAsync(string oldEmail, string newEmail)
    {
        var command = new UpdateUserEmailCommand(oldEmail, newEmail);
        var result = await _sender.Send(command);

        if (result.IsSuccuss)
        {
            _logger.LogInformation("Processing update email job has ended successfully.");
            return;
        }

        _logger.LogInformation("Processing update email job has ended with error {Error}", result?.Error?.Message);

    }
}
