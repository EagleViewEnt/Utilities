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

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  A <see cref="JsonConverter{T}" /> for <see cref="EveMoneyUsd" /> that serializes the value as a JSON number and
///  deserializes only from JSON numeric tokens.
/// </summary>
/// <remarks>
///  - Write: emits the <see cref="EveMoneyUsd.Value" /> as a numeric JSON token. - Read: expects a numeric JSON token
///  and constructs an <see cref="EveMoneyUsd" /> from it.
/// </remarks>
public class EveMoneyUsdJsonConverter : JsonConverter<EveMoneyUsd>
{

    /// <summary>
    ///  Reads an <see cref="EveMoneyUsd" /> value from the provided <see cref="Utf8JsonReader" />.
    /// </summary>
    /// <param name="reader">The JSON reader positioned at a numeric token.</param>
    /// <param name="typeToConvert">The type to convert (ignored).</param>
    /// <param name="options">The serializer options (ignored).</param>
    /// <returns>The deserialized <see cref="EveMoneyUsd" /> instance.</returns>
    /// <exception cref="JsonException">Thrown when the current token is not a JSON number.</exception>
    public override EveMoneyUsd Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        if(reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected decimal value for EveMoneyUsd.");
        decimal value = reader.GetDecimal();
        return new EveMoneyUsd(value);
    }

    /// <summary>
    ///  Writes the specified <see cref="EveMoneyUsd" /> value as a JSON number.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options (ignored).</param>
    public override void Write
        ( Utf8JsonWriter writer, EveMoneyUsd value, JsonSerializerOptions options )
        => writer.WriteNumberValue(value.Value);

}
