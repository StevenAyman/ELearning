using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakUserAttirbutes
{
    public string City { get; init; }

    public string DateOfBirth { get; init; }
}
