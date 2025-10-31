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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace EagleViewEnt.Utilities.Core.Converters.DateAndTime;

/// <summary>
///  A <see cref="JsonConverter{T}" /> implementation for <see cref="DateTime" /> that: - Deserializes by calling <see
///  cref="DateTime.Parse(string)" /> on the JSON string value. - Serializes by calling <see cref="DateTime.ToString()"
///  /> on the value.
/// </summary>
/// <remarks>
///  Important notes: - Parsing uses <see cref="DateTime.Parse(string)" />, which is culture-sensitive and may throw for
///  invalid or empty strings. - Serialization uses <see cref="DateTime.ToString()" />, which is also culture-sensitive
///  and not ISO 8601 by default. - If the JSON token is not a string, <see cref="Utf8JsonReader.GetString()" /> will
///  throw <see cref="InvalidOperationException" />. Consider using <see
///  cref="System.Text.Json.Serialization.JsonStringEnumConverter" />-style ISO 8601 formatting if interoperability is
///  required.
/// </remarks>
/// <example>
///  Example usage with <see cref="JsonSerializerOptions" />: <code language="csharp"> var options = new
///  JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; options.Converters.Add(new
///  DateTimeConverterUsingDateTimeParse());  var json = JsonSerializer.Serialize(DateTime.Now, options); var dt   =
///  JsonSerializer.Deserialize&lt;DateTime&gt;(json, options);</code>
/// </example>
public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
{

    /// <summary>
    ///  Reads a JSON string and converts it to a <see cref="DateTime" /> by using <see cref="DateTime.Parse(string)"
    ///  />.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> positioned at the JSON string token.</param>
    /// <param name="typeToConvert">The type to convert (ignored; always <see cref="DateTime" />).</param>
    /// <param name="options">Serialization options (not used).</param>
    /// <returns>The parsed <see cref="DateTime" /> value.</returns>
    /// <exception cref="InvalidOperationException">
    ///  Thrown when the current JSON token is not a string.
    /// </exception>
    /// <exception cref="FormatException">
    ///  Thrown when the string is not a valid date/time representation (including empty string).
    /// </exception>
    public override DateTime Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        => DateTime.Parse(reader.GetString() ?? string.Empty);

    /// <summary>
    ///  Writes the <see cref="DateTime" /> value to JSON as a string using <see cref="DateTime.ToString()" />.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write JSON to.</param>
    /// <param name="value">The <see cref="DateTime" /> value to serialize.</param>
    /// <param name="options">Serialization options (not used).</param>
    /// <remarks>
    ///  The output is culture-sensitive and not guaranteed to be ISO 8601. For stable, round-trippable formats,
    ///  consider using <see cref="DateTime.ToString(string)" /> with an explicit format and culture.
    /// </remarks>
    public override void Write
        ( Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options )
        => writer.WriteStringValue(value.ToString());

}

