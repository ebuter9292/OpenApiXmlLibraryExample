using MartinCostello.OpenApi;

namespace OpenApiXmlLibraryExample
{
    /// <summary>
    /// A Weather forecast including date, temperature, and a summary
    /// </summary>
    [OpenApiExample<WeatherForecast>]
    public class WeatherForecast : IExampleProvider<WeatherForecast>
    {
        /// <summary>
        /// Date of the forecast
        /// </summary>
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        public static WeatherForecast GenerateExample()
        {
            return new WeatherForecast
            {
                Date = new DateOnly(2026, 1, 1),
                TemperatureC = 30,
                Summary = "Summary of weather forecast"
            };
        }
    }
}
