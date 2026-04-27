using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService;
public sealed class KeycloakWebhookPayload
{
    public string Type { get; init; }    // "REGISTER"
    public string UserId { get; init; }  // The UUID of the user
    public Dictionary<string, string> Details { get; init; } = new Dictionary<string, string>();
}
