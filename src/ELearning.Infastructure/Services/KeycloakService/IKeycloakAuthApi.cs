using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infastructure.Services.KeycloakService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace ELearning.Infastructure.Services.KeycloakService;
public interface IKeycloakAuthApi
{
    [Post("/realms/master/protocol/openid-connect/token")]
    Task<ApiResponse<KeycloakTokenResponse>> GetAccessToken([Body(BodySerializationMethod.UrlEncoded)]KeycloakTokenRequest keycloakTokenRequest);
}
