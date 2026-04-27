using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace ELearning.Infastructure.Services.KeycloakService.DTOs;
public sealed class KeycloakTokenRequest
{
    [AliasAs("client_id")]
    public string ClientId { get; init; }

    [AliasAs("grant_type")]
    public string GrantType { get; init; }

    [AliasAs("username")]
    public string Username { get; init; }

    [AliasAs("password")]
    public string Password { get; init; }

}
