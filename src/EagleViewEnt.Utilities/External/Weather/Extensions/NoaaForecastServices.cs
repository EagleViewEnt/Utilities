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

using System;
using System.Linq;

using MgbUtilties.Core.Types.JsonString;
using MgbUtilties.Core.Types.JsonString.Extensions;
using MgbUtilties.External.Weather.Models;

namespace MgbUtilties.External.Weather.Extensions
{

    public static class NoaaForecastServices
    {

        /// <summary>
        ///  Computes the heat index based on the NOAA forecast data and calculation for a given date.
        /// </summary>
        /// <param name="forecast">A NoaaForecast record</param>
        /// <param name="date">The date and time of day for heat index</param>
        /// <returns>Calculated heat index temperature</returns>
        public static int CalculateHeatIndex( this NoaaForecast forecast, DateTime date )

        {

            double T = forecast.TimeOfDayExpectedTemperature(date);
            double RH = forecast.TimeOfDayExpectedHumidity(date);

            if((T < 0) || (RH < 0))
                return -1; // Indicates missing data

            double simpleHI = 0.5 * (T + 61.0 + ((T - 68.0) * 1.2) + (RH * 0.094));
            double averageHI = (simpleHI + T) / 2.0;

            // If the average is less than 80°F, use the simple formula
            if(averageHI < 80)
                return (int)Math.Round(averageHI, 1);

            double hi = ((((-42.379) + (2.04901523 * T) + (10.14333127 * RH)) - (0.22475541 * T * RH) - (0.00683783 * T * T) - (0.05481717 * RH * RH)) + (0.00122874 * T * T * RH) + (0.00085282 * T * RH * RH)) - (0.00000199 * T * T * RH * RH);

            // Adjustment for low humidity
            if((RH < 13) && (T >= 80) && (T <= 112)) {
                double adjustment = ((13 - RH) / 4) * Math.Sqrt((17 - Math.Abs(T - 95)) / 17);
                hi -= adjustment;
            }

            // Adjustment for high humidity
            if((RH > 85) && (T >= 80) && (T <= 87)) {
                double adjustment = ((RH - 85) / 10) * ((87 - T) / 5);
                hi += adjustment;
            }

            return (int)Math.Round(hi, 1);

        }

        public static List<Period> ForecastForDate( this NoaaForecast? forecast, DateTime date )
            => forecast?
                    .Properties
                    .Periods
                    .OrderBy(o => o.Number)
                    .Where(f => f.StartTime >= date)
                    .ToList() ??
                [];

        /// <summary>
        ///  Get the NOAA hourly weather forecast for the Shreveport area.
        /// </summary>
        /// <param name="userAgent">
        ///  The user name associated with the NOAA account https://www.weather.gov/. See
        ///  https://www.weather.gov/documentation/services-web-api for more information.
        /// </param>
        /// <returns>NoaaForecast model</returns>
        public static async Task<NoaaForecast?> GetNoaaHourlyWeatherForecast( string userAgent )
        {
            string noaaUrl = "https://api.weather.gov/gridpoints/SHV/7,83/forecast/hourly?units=us";

            try {
                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
                using HttpResponseMessage response = await client.GetAsync(noaaUrl);
                response.EnsureSuccessStatusCode();
                MgbJson json = await response.Content.ReadAsStringAsync();
                return json.As<NoaaForecast>();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        ///  Get the NOAA hourly weather forecast for the Shreveport area.
        /// </summary>
        /// <param name="userAgent">
        ///  The user name associated with the NOAA account https://www.weather.gov/. See
        ///  https://www.weather.gov/documentation/services-web-api for more information.
        /// </param>
        /// <returns>NoaaForecast model</returns>
        public static async Task<Period?> GetNoaaWeatherForecastForDate( string userAgent, DateTime date )
        {
            string _noaaUrl = "https://api.weather.gov/gridpoints/SHV/7,83/forecast?units=us";

            try {
                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
                using HttpResponseMessage response = await client.GetAsync(_noaaUrl);
                response.EnsureSuccessStatusCode();
                MgbJson json = await response.Content.ReadAsStringAsync();
                NoaaForecast? forecast = json.As<NoaaForecast>();
                return forecast?.Properties.Periods.FirstOrDefault(f => f.StartTime.Date >= date.Date) ?? new();
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        ///  Returns the expected head index for a given date and time of day.
        /// </summary>
        /// <param name="forecast">A NoaaForecast record</param>
        /// <param name="date">The date and time of day for heat index</param>
        /// <returns>Returns heat index temperature</returns>
        public static int TimeOfDayExpectedHeatIndex( this NoaaForecast? forecast, DateTime date )
            => forecast?.CalculateHeatIndex(date) ?? -1;

        /// <summary>
        ///  Returns the expected probability of precipitation for a given date and time of day.
        /// </summary>
        /// <param name="forecast">A NoaaForecast record</param>
        /// <param name="date">The date and time of day for expected humidity</param>
        /// <returns>Returns Expected humidity percentage round to nearest whole number</returns>
        public static int TimeOfDayExpectedHumidity( this NoaaForecast? forecast, DateTime date )
            => forecast?
                .Properties
                .Periods
                .ToList()
                .OrderBy(o => o.Number)
                .FirstOrDefault(f => f.StartTime >= date)?.RelativeHumidity.Value ??
                -1;

        /// <summary>
        ///  Returns the expected temperature for a given date and time of day.
        /// </summary>
        /// <param name="forecast">A NoaaForecast record</param>
        /// <param name="date">The day and time for expected temperature</param>
        /// <returns></returns>
        public static int TimeOfDayExpectedTemperature( this NoaaForecast? forecast, DateTime date )
            => forecast?
                    .Properties
                    .Periods
                    .ToList()
                    .OrderBy(o => o.Number)
                    .FirstOrDefault(f => f.StartTime >= date)?.Temperature ??
                -1;

    }

}
