using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakUserRegisterationDto
{
    public KeycloakUserRegisterationDto(string username, string email, string firstName, string lastName)
    {
        Enabled = true;
        Attributes = new();
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Credentials = new();
    }
    public string Username { get; init; }
    public string FirstName { get; init;}
    public string LastName { get; init;}
    public string Email { get; init; }
    public bool Enabled { get; init; }
    public Dictionary<string, string[]> Attributes { get; init; }
    public List<KeycloakCredentialsDto> Credentials { get; init; }
}
