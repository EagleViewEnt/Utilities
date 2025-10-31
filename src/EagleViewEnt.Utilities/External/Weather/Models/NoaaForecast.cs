//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace MgbUtilties.External.Weather.Models
{

    public record NoaaForecast
    {
        [JsonPropertyName("properties")]
        public Properties Properties { get; init; } = new();
    }

    public record Period

    {
        [JsonPropertyName("detailedForecast")]
        public string DetailedForecast { get; init; } = string.Empty;

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; init; }

        [JsonPropertyName("isDaytime")]
        public bool IsDaytime { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; } = string.Empty;

        [JsonPropertyName("number")]
        public int Number { get; init; }

        [JsonPropertyName("probabilityOfPrecipitation")]

        public ProbabilityOfPrecipitation ProbabilityOfPrecipitation { get; init; } = new();

        [JsonPropertyName("relativeHumidity")]
        public RelativeHumidity RelativeHumidity { get; init; } = new();

        [JsonPropertyName("shortForecast")]
        public string ShortForecast { get; init; } = string.Empty;

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; init; }

        [JsonPropertyName("temperature")]
        public int? Temperature { get; init; }

        [JsonPropertyName("temperatureTrend")]
        public string? TemperatureTrend { get; init; } = string.Empty;

        [JsonPropertyName("temperatureUnit")]
        public string? TemperatureUnit { get; init; } = string.Empty;

        [JsonPropertyName("windDirection")]
        public string? WindDirection { get; init; } = string.Empty;

        [JsonPropertyName("windSpeed")]
        public string? WindSpeed { get; init; } = string.Empty;
    }

    public record ProbabilityOfPrecipitation

    {
        [JsonPropertyName("unitCode")]

        public string? UnitCode { get; init; } = string.Empty;

        [JsonPropertyName("value")]

        public int? Value { get; init; }
    }

    public record RelativeHumidity
    {
        [JsonPropertyName("unitCode")]
        public string? UnitCode { get; init; } = string.Empty;

        [JsonPropertyName("value")]
        public int? Value { get; init; }
    }

    public record Properties
    {
        [JsonPropertyName("periods")]
        public Period[] Periods { get; init; } = [];
    }

}
