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

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  A JSON converter for <see cref="decimal" /> that parses string tokens using <see cref="decimal.TryParse(string, out
///  decimal)" /> and writes values as JSON strings.
/// </summary>
/// <remarks>
///  - <c>Read</c> expects the JSON token to be a string and uses the current culture for parsing.<br /> - <c>Write</c>
///  serializes the value as a JSON string using the current culture.
/// </remarks>
/// <seealso cref="JsonConverter{T}" />
/// <seealso cref="Utf8JsonReader" />
/// <seealso cref="Utf8JsonWriter" />
public class MoneyConverterUsingMoneyParse : JsonConverter<decimal>
{

    /// <summary>
    ///  Reads and converts a JSON string to a <see cref="decimal" />.
    /// </summary>
    /// <param name="reader">The reader positioned at a JSON string token that contains a numeric value.</param>
    /// <param name="typeToConvert">The type to convert to. This converter targets <see cref="decimal" />.</param>
    /// <param name="options">Serialization options (not used).</param>
    /// <returns>The parsed <see cref="decimal" /> value.</returns>
    /// <exception cref="JsonException">Thrown when the value cannot be parsed to <see cref="decimal" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///  Thrown by <see cref="Utf8JsonReader.GetString()" /> if the current token is not a JSON string.
    /// </exception>
    /// <remarks>
    ///  Uses <see cref="decimal.TryParse(string, out decimal)" /> with the current culture.
    /// </remarks>
    public override decimal Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        string? value = reader.GetString();
        if(decimal.TryParse(value, out decimal amount))
            return amount;
        throw new JsonException("Invalid value for decimal type");
    }

    /// <summary>
    ///  Writes the <see cref="decimal" /> value as a JSON string.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The <see cref="decimal" /> value to write.</param>
    /// <param name="options">Serialization options (not used).</param>
    /// <remarks>
    ///  Uses <see cref="decimal.ToString()" /> with the current culture, producing a JSON string token.
    /// </remarks>
    public override void Write
        ( Utf8JsonWriter writer, decimal value, JsonSerializerOptions options )
        => writer.WriteStringValue(value.ToString());

}

