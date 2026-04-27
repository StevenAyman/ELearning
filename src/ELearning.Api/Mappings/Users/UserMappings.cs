using ELearning.Api.DTOs.Users;
using ELearning.Application.Users.GetUserWithIdentity;
using ELearning.Infastructure.Services.KeycloakService.DTOs;

namespace ELearning.Api.Mappings.Users;

public static class UserMappings
{
    public static KeycloakUserProfileDto ToKeycloakProfile(this UserProfileDto userDto)
    {
        var userKeycloak = new KeycloakUserProfileDto
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
        };

        userKeycloak.Attributes["city"] = [userDto.City];
        userKeycloak.Attributes["date_of_birth"] = [userDto.BirthDate];

        return userKeycloak;
    }

    public static FullUserDto ToDto(this UserDto dbUser, string username)
    {
        return new FullUserDto
        {
            Id = dbUser.Id,
            FirstName = dbUser.FirstName,
            LastName = dbUser.LastName,
            Email = dbUser.Email,
            BirthDate = dbUser.BirthDate,
            City = dbUser.City,
            UserName = username,
            IdentityId = dbUser.IdentityId,
        };
    }
}
