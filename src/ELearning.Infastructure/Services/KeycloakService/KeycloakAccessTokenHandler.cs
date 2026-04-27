using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infastructure.Services.KeycloakService.DTOs;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace ELearning.Infastructure.Services.KeycloakService;
public sealed class KeycloakAccessTokenHandler(IKeycloakAuthApi authApi, IOptions<KeycloakTokenRequest> options, IFusionCache cache) : DelegatingHandler
{
    private readonly KeycloakTokenRequest _keycloakOptions = options.Value;
    private readonly IKeycloakAuthApi _keycloakAuthApi = authApi;
    private readonly IFusionCache _cache = cache;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tokenResponse = await _cache.GetOrSetAsync("keycloak:admin:access-token", async ct =>
        {
            var response = await _keycloakAuthApi.GetAccessToken(_keycloakOptions);
            var token = response.Content?.AccessToken;
            return token;
        }, new FusionCacheEntryOptions(TimeSpan.FromSeconds(59)), cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse);

        return await base.SendAsync(request, cancellationToken);
    }
}
