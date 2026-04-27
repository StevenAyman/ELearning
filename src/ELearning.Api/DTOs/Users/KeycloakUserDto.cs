namespace ELearning.Api.DTOs.Users;

public sealed class KeycloakUserDto
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string BirthDate { get; init; }
    public string City { get; init; }
}
