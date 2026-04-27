namespace ELearning.Api.DTOs.Users;

public class FullUserDto
{
    public string Id { get; init; }
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string City { get; init; }
    public DateOnly BirthDate { get; init; }
    public string IdentityId { get; init; }
}
