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

namespace EagleViewEnt.Utilities.Core.Types.JsonString.Converters;

/// <summary>
///  A System.Text.Json <see cref="JsonConverter{T}" /> for the <see cref="EveJson" /> value type.
/// </summary>
/// <remarks>
///  - When the reader is positioned at <see cref="JsonTokenType.StartObject" />, the raw JSON for that object is
///  captured via <see cref="System.Text.Json.JsonElement.GetRawText()" /> and wrapped in <see cref="EveJson" />. - When
///  the reader is positioned at <see cref="JsonTokenType.String" />, the string value is used directly. - Any other
///  token results in a <see cref="JsonException" />.
/// </remarks>
public class EveJson_JsonConverter : JsonConverter<EveJson>
{

    /// <summary>
    ///  Reads an <see cref="EveJson" /> instance from the JSON payload.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> positioned at the JSON to read.</param>
    /// <param name="typeToConvert">The type being converted (ignored).</param>
    /// <param name="options">Serializer options (ignored).</param>
    /// <returns>An <see cref="EveJson" /> wrapping the raw JSON string.</returns>
    /// <exception cref="JsonException">Thrown when an unexpected token is encountered.</exception>
    public override EveJson Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        if(reader.TokenType == JsonTokenType.StartObject)
            using(JsonDocument doc = JsonDocument.ParseValue(ref reader)) {
                string jsonString = doc.RootElement.GetRawText();
                return new EveJson(jsonString);
            }
        else if(reader.TokenType == JsonTokenType.String) {
            string? json = reader.GetString();
            return new EveJson(json);
        }
        throw new JsonException("Unexpected token type");
    }

    /// <summary>
    ///  Writes the <see cref="EveJson" /> value as a JSON string using its <see cref="EveJson.Value" />.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write to.</param>
    /// <param name="value">The <see cref="EveJson" /> instance being serialized.</param>
    /// <param name="options">Serializer options (ignored).</param>
    public override void Write
        ( Utf8JsonWriter writer, EveJson value, JsonSerializerOptions options )
        => writer.WriteStringValue(value.Value);

}
