using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Refit;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpireAt { get; init; }
}
