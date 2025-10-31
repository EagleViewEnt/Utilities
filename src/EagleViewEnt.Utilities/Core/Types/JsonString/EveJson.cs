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

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.JsonString.Converters;

namespace EagleViewEnt.Utilities.Core.Types.JsonString;

/// <summary>
///  Strongly-typed wrapper for a JSON payload stored as a raw string. Provides cached validation via <see
///  cref="IsValid" /> and convenience conversions/operators for comparisons and serialization.
/// </summary>
[JsonConverter(typeof(EveJson_JsonConverter))]
public readonly struct EveJson : IEquatable<EveJson>, IEquatable<string>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveJson" /> struct with an empty JSON string. Intended for
    ///  deserialization scenarios (e.g., System.Text.Json / EF Core).
    /// </summary>
    public EveJson() : this(string.Empty) { }

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveJson" /> struct.
    /// </summary>
    /// <param name="value">The JSON string to wrap. Null becomes an empty string. Leading/trailing whitespace is trimmed.</param>
    public EveJson( string? value )
    {
        Value = value?.Trim() ?? string.Empty;
        IsValid = IsValidJson(Value);
    }

    /// <summary>
    ///  Returns whether the current instance is empty.
    /// </summary>
    [JsonIgnore, XmlIgnore, NotMapped]
    public bool IsEmpty => string.IsNullOrEmpty(Value);

    /// <summary>
    ///  This caches the result of the IsValidJson method during construction for performance reasons.
    /// </summary>
    [JsonIgnore, XmlIgnore, NotMapped]
    public bool IsValid { get; }

    /// <summary>
    ///  The raw JSON string value for serialization and EF Core mapping.
    /// </summary>
    [JsonPropertyName("Value"), XmlText]
    public string Value { get; }

    /// <summary>
    ///  Implicitly converts an <see cref="EveJson" /> to its underlying JSON string.
    /// </summary>
    /// <param name="jsonString">The <see cref="EveJson" /> instance.</param>
    /// <returns>The wrapped JSON string.</returns>
    public static implicit operator string( EveJson jsonString ) => jsonString.Value;

    /// <summary>
    ///  Implicitly converts a JSON string to an <see cref="EveJson" /> instance.
    /// </summary>
    /// <param name="value">The JSON string to wrap.</param>
    /// <returns>A new <see cref="EveJson" /> instance.</returns>
    public static implicit operator EveJson( string? value ) => new(value);

    /// <summary>
    ///  Determines whether two <see cref="EveJson" /> instances have the same underlying string value (ordinal
    ///  comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( EveJson left, EveJson right )
        => string.Equals(left.Value, right.Value, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether an <see cref="EveJson" /> and a string have the same value (ordinal comparison).
    /// </summary>
    /// <param name="left">The <see cref="EveJson" /> operand.</param>
    /// <param name="right">The string operand.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( EveJson left, string? right )
        => string.Equals(left.Value, right, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether a string and an <see cref="EveJson" /> have the same value (ordinal comparison).
    /// </summary>
    /// <param name="left">The string operand.</param>
    /// <param name="right">The <see cref="EveJson" /> operand.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( string? left, EveJson right )
        => string.Equals(left, right.Value, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether two <see cref="EveJson" /> instances differ in their underlying string value (ordinal
    ///  comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( EveJson left, EveJson right )
        => !string.Equals(left.Value, right.Value, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether an <see cref="EveJson" /> and a string differ in value (ordinal comparison).
    /// </summary>
    /// <param name="left">The <see cref="EveJson" /> operand.</param>
    /// <param name="right">The string operand.</param>
    /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( EveJson left, string? right )
        => !string.Equals(left.Value, right, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether a string and an <see cref="EveJson" /> differ in value (ordinal comparison).
    /// </summary>
    /// <param name="left">The string operand.</param>
    /// <param name="right">The <see cref="EveJson" /> operand.</param>
    /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( string? left, EveJson right )
        => !string.Equals(left, right.Value, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether this instance and a specified object have the same value.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public override bool Equals( object? obj )
        => obj switch {
            string s => Equals(s),
            EveJson j => Equals(j),
            _ => false
        };

    /// <summary>
    ///  Determines whether this instance and another <see cref="EveJson" /> have the same value (ordinal comparison).
    /// </summary>
    /// <param name="other">The other <see cref="EveJson" /> to compare.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public bool Equals( EveJson other ) => string.Equals(Value, other.Value, StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether this instance and a string have the same value (ordinal comparison).
    /// </summary>
    /// <param name="other">The other string to compare.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public bool Equals( string? other ) => string.Equals(Value, other, StringComparison.Ordinal);

    /// <summary>
    ///  Returns a hash code for this instance based on the underlying string (ordinal).
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    ///  Determines whether a string contains valid JSON using <see cref="System.Text.Json.JsonDocument.Parse(string,
    ///  JsonDocumentOptions)" />.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns><c>true</c> if the string is valid JSON; otherwise, <c>false</c>.</returns>
    static bool IsValidJson( string value )
    {
        try {
            using JsonDocument _ = JsonDocument.Parse(value);
            return true;
        } catch(JsonException) {
            return false;
        }
    }

    /// <summary>
    ///  Returns the underlying JSON string.
    /// </summary>
    /// <returns>The wrapped JSON string.</returns>
    public override string ToString() => Value;

}
