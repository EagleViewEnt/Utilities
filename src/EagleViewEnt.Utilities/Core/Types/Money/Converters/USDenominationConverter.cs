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
///  JSON converter for <see cref="USDenominations" /> that supports both string and numeric representations.
/// </summary>
/// <remarks>
///  During deserialization: - If the JSON token is a string, the converter resolves the denomination by its <see
///  cref="EagleViewEnt.Utilities.Core.Types.Money.Enum.USDenominations" />. - If the JSON token is a number, the
///  converter resolves the denomination by its decimal value. During serialization, the converter emits the
///  denomination's <see cref="EagleViewEnt.Utilities.Core.Types.Money.Enum.USDenominations" /> as a JSON string.
/// </remarks>
/// <seealso cref="USDenominations" />
public class USDenominationConverter : JsonConverter<USDenominations>
{

    /// <summary>
    ///  Reads and converts JSON to a <see cref="USDenominations" /> instance.
    /// </summary>
    /// <param name="reader">A UTF-8 JSON reader positioned at the value to convert.</param>
    /// <param name="typeToConvert">The target type to convert to. Not used.</param>
    /// <param name="options">The serializer options in use. Not used.</param>
    /// <returns>
    ///  A <see cref="USDenominations" /> resolved from the JSON string (by name) or number (by value).
    /// </returns>
    /// <exception cref="JsonException">
    ///  Thrown when the JSON token is neither a string nor a number, or when conversion fails.
    /// </exception>
    public override USDenominations? Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        if(reader.TokenType == JsonTokenType.String) {
            string? name = reader.GetString();
            return USDenominations.FromName(name!);
        } else if(reader.TokenType == JsonTokenType.Number) {
            decimal value = reader.GetDecimal();
            return USDenominations.FromValue(value);
        }

        throw new JsonException("Invalid JSON format for USDenominations.");
    }

    /// <summary>
    ///  Writes a <see cref="USDenominations" /> instance to JSON as its canonical name.
    /// </summary>
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The denomination to serialize.</param>
    /// <param name="options">The serializer options in use. Not used.</param>
    public override void Write
        ( Utf8JsonWriter writer, USDenominations value, JsonSerializerOptions options )
        => writer.WriteStringValue(value.Name);

}
