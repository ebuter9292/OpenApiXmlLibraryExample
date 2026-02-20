using System.Text.Json.Serialization;

namespace OpenApiXmlLibraryExample;

[JsonSerializable(typeof(WeatherForecast))]
//[JsonSerializable(typeof(IEnumerable<WeatherForecast>))]
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    WriteIndented = false,
    UseStringEnumConverter = true)]
public sealed partial class SerializerContext : JsonSerializerContext;
