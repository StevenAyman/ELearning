using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Services.KeycloakService;
public sealed class KeycloakOptions
{
    public string Realm { get; init; }
    public string ApplicationClientId { get; init; }
    public string RedirectUri { get; init; }
}
