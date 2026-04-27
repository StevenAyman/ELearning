using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ELearning.Infastructure.Services.KeycloakService.DTOs;

namespace ELearning.Infastructure.Services.KeycloakService;
public sealed class KeycloakAttributesConverter : JsonConverter<KeycloakUserAttirbutes>
{
    public override KeycloakUserAttirbutes? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string[]>>(ref reader, options);
        return new KeycloakUserAttirbutes
        {
            City = dict?.GetValueOrDefault("city")?[0] ?? "",
            DateOfBirth = dict?.GetValueOrDefault("date_of_birth")?[0] ?? ""
        };
    }

    public override void Write(Utf8JsonWriter writer, KeycloakUserAttirbutes value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
