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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Converters;

/// <summary>
///  A <see cref="JsonConverter{T}" /> that serializes and deserializes <see cref="EveFilePath" /> values as JSON
///  strings.
/// </summary>
/// <remarks>
///  This converter delegates normalization and validation to the <see cref="EveFilePath" /> type. When deserializing, a
///  JSON null string value results in an empty <see cref="EveFilePath" />.
/// </remarks>
public class EveFilePathJsonConverter : JsonConverter<EveFilePath>
{

    /// <summary>
    ///  Reads a JSON string and converts it to an <see cref="EveFilePath" /> instance.
    /// </summary>
    /// <param name="reader">The JSON reader positioned at the string token to read.</param>
    /// <param name="typeToConvert">The target type to convert to (ignored; always <see cref="EveFilePath" />).</param>
    /// <param name="options">Serializer options in use for the current operation.</param>
    /// <returns>
    ///  A new <see cref="EveFilePath" /> constructed from the JSON string value; if the JSON value is null, an empty
    ///  <see cref="EveFilePath" /> is returned.
    /// </returns>
    /// <remarks>
    ///  The <see cref="EveFilePath" /> constructor performs normalization (lowercasing) and validation.
    /// </remarks>
    public override EveFilePath Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        string? value = reader.GetString();
        return new EveFilePath(value ?? string.Empty);
    }

    /// <summary>
    ///  Writes the specified <see cref="EveFilePath" /> to JSON as a string.
    /// </summary>
    /// <param name="writer">The JSON writer to which the value is written.</param>
    /// <param name="value">The <see cref="EveFilePath" /> to serialize.</param>
    /// <param name="options">Serializer options in use for the current operation.</param>
    /// <remarks>
    ///  The string representation is provided by <see cref="EveFilePath.ToString()" />.
    /// </remarks>
    public override void Write
        ( Utf8JsonWriter writer, EveFilePath value, JsonSerializerOptions options )
        => writer.WriteStringValue(value.ToString());

}
