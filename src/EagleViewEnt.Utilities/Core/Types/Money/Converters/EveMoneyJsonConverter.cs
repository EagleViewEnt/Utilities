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

using EagleViewEnt.Utilities.Core.Types.Money.Enum;

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  A <see cref="JsonConverter{T}" /> for serializing and deserializing <see cref="EveMoney" /> values.
/// </summary>
/// <remarks>
///  This converter represents <see cref="EveMoney" /> as a JSON object with the following shape: { "Value": decimal,
///  "Currency": string }. The currency is written and read by its <see
///  cref="EagleViewEnt.Utilities.Core.Types.Money.Enum.EveCurrency" /> property. During deserialization, the currency
///  string is resolved via <see cref="EagleViewEnt.Utilities.Core.Types.Money.Enum.EveCurrency" />.
/// </remarks>
public class EveMoneyJsonConverter : JsonConverter<EveMoney>
{

    /// <summary>
    ///  Reads and converts the JSON representation to an <see cref="EveMoney" /> instance.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> positioned at the start of the JSON object.</param>
    /// <param name="typeToConvert">The target type (ignored; always <see cref="EveMoney" />).</param>
    /// <param name="options">Serializer options (not used).</param>
    /// <returns>
    ///  A new <see cref="EveMoney" /> created from the parsed <c>Value</c> and <c>Currency</c> properties.
    /// </returns>
    /// <exception cref="JsonException">
    ///  Thrown when the JSON is not an object or contains unexpected token types.
    /// </exception>
    public override EveMoney Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {

        if(reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        decimal value = 0m;
        EveCurrency currency = EveCurrency.Empty;

        while(reader.Read()) {
            if(reader.TokenType == JsonTokenType.EndObject)
                break;

            if(reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString() ?? "N/A";
            reader.Read();

            switch(propertyName) {
                case nameof(EveMoney.Value):
                    value = reader.GetDecimal();
                    break;
                case nameof(EveMoney.Currency):
                    string? currencyName = reader.GetString();
                    currency = EveCurrency.FromName(currencyName);
                    break;
            }
        }
        return EveMoney.Create(value, currency);
    }

    /// <summary>
    ///  Writes the specified <see cref="EveMoney" /> value as a JSON object with <c>Value</c> and <c>Currency</c>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write to.</param>
    /// <param name="value">The <see cref="EveMoney" /> instance to serialize.</param>
    /// <param name="options">Serializer options (not used).</param>
    public override void Write
        ( Utf8JsonWriter writer, EveMoney value, JsonSerializerOptions options )
    {
        writer.WriteStartObject();
        writer.WriteNumber(nameof(EveMoney.Value), value.Value);
        writer.WriteString(nameof(EveMoney.Currency), value.Currency.Name);
        writer.WriteEndObject();
    }

}
