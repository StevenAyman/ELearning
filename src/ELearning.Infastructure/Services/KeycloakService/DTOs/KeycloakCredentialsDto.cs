using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakCredentialsDto
{
    public KeycloakCredentialsDto(string value)
    {
        Value = value;
        Type = "password";
        Temporary = false;
    }
    public string Type { get; init; }
    public string Value { get; init; }
    public bool Temporary { get; init; }
}
