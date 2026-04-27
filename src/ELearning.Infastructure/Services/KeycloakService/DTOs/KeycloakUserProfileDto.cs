using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakUserProfileDto
{
    [AliasAs("firstName")]
    public string FirstName { get; init; }
    [AliasAs("lastName")]
    public string LastName { get; init; }
    [AliasAs("email")]
    public string Email { get; set; }
    [AliasAs("attributes")]
    public Dictionary<string, string[]> Attributes { get; init; } = new();
}
