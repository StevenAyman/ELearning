using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakUserDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; init; }

    [JsonPropertyName("lastName")]
    public string LastName { get; init; }
    [JsonPropertyName("username")]
    public string Username { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; }

    [JsonConverter(typeof(KeycloakAttributesConverter))]
    [JsonPropertyName("attributes")]
    public KeycloakUserAttirbutes Attirbutes { get; init; }
}
