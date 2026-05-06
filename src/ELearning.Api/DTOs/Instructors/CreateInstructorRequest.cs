namespace ELearning.Api.DTOs.Instructors;

public class CreateInstructorRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string BirthDate { get; init; }
    public string City { get; init; }
}
