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
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

namespace EagleViewEnt.Utilities.Core.Types.FilePath;

/// <summary>
///  Represents a strongly-typed file path string with validation and file system related checks. The input value is
///  normalized to lowercase to enable case-insensitive equality and hashing.
/// </summary>
/// <param name="value">
///  The file system path to wrap. May be null or empty. The value is normalized to lowercase and validated.
/// </param>

[XmlRoot("EveFilePath"),
JsonConverter(typeof(StringValueTypeJsonConverter<EveFilePath>)),
TypeConverter(typeof(StringValueTypeConverter<EveFilePath>))]
public sealed class EveFilePath( string value ) : StringValueType<EveFilePath>(value?.ToLower())
{

    /// <summary>
    ///  Windows reserved device names that cannot be used as file names.
    /// </summary>
    static readonly string[] ReservedNames = [ "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" ];

    /// <summary>
    ///  Implicitly converts an <see cref="EveFilePath" /> to its underlying string representation.
    /// </summary>
    /// <param name="value">The <see cref="EveFilePath" /> instance to convert.</param>
    /// <returns>The normalized path string.</returns>
    public static implicit operator string( EveFilePath value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a string to an <see cref="EveFilePath" /> value. The input string is normalized to
    ///  lowercase and validated.
    /// </summary>
    /// <param name="value">The path string to convert.</param>
    /// <returns>The created <see cref="EveFilePath" />.</returns>
    public static implicit operator EveFilePath( string value ) => FromStringValue(value.ToLower());

    /// <summary>
    ///  Determines whether two <see cref="EveFilePath" /> instances are equal (case-insensitive).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if both are equal or reference equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( EveFilePath? left, EveFilePath? right )
    {
        if(ReferenceEquals(left, right))
            return true;
        if((left is null) || (right is null))
            return false;
        return left.Equals(right);
    }

    /// <summary>
    ///  Determines whether an <see cref="EveFilePath" /> and a string are equal (case-insensitive).
    /// </summary>
    /// <param name="left">The <see cref="EveFilePath" /> operand.</param>
    /// <param name="right">The string operand.</param>
    /// <returns><c>true</c> if values are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( EveFilePath? left, string? right )
    {
        if((left is null) && (right is null))
            return true;
        if((left is null) || (right is null))
            return false;
        return string.Equals(left.Value, right, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///  Determines whether a string and an <see cref="EveFilePath" /> are equal (case-insensitive).
    /// </summary>
    /// <param name="left">The string operand.</param>
    /// <param name="right">The <see cref="EveFilePath" /> operand.</param>
    /// <returns><c>true</c> if values are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==( string? left, EveFilePath? right )
    {
        if((left is null) && (right is null))
            return true;
        if((left is null) || (right is null))
            return false;
        return string.Equals(left, right.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///  Determines whether two <see cref="EveFilePath" /> instances are not equal.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if operands are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( EveFilePath? left, EveFilePath? right ) => !(left == right);

    /// <summary>
    ///  Determines whether an <see cref="EveFilePath" /> and a string are not equal.
    /// </summary>
    /// <param name="left">The <see cref="EveFilePath" /> operand.</param>
    /// <param name="right">The string operand.</param>
    /// <returns><c>true</c> if operands are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( EveFilePath? left, string? right ) => !(left == right);

    /// <summary>
    ///  Determines whether a string and an <see cref="EveFilePath" /> are not equal.
    /// </summary>
    /// <param name="left">The string operand.</param>
    /// <param name="right">The <see cref="EveFilePath" /> operand.</param>
    /// <returns><c>true</c> if operands are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=( string? left, EveFilePath? right ) => !(left == right);

    /// <summary>
    ///  Determines whether the specified object is equal to the current <see cref="EveFilePath" />.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///  <c>true</c> if <paramref name="obj" /> is a string or <see cref="EveFilePath" /> equal to this instance (case-
    ///  insensitive); otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object? obj )
    {
        if(ReferenceEquals(this, obj))
            return true;
        if(obj is string str)
            return string.Equals(Value, str, StringComparison.OrdinalIgnoreCase);
        if(obj is not EveFilePath other)
            return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///  Determines whether the specified <see cref="EveFilePath" /> is equal to the current instance (case-
    ///  insensitive).
    /// </summary>
    /// <param name="other">The other <see cref="EveFilePath" /> to compare.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public bool Equals( EveFilePath? other )
    {
        if(ReferenceEquals(this, other))
            return true;
        if(other is null)
            return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///  Returns a hash code for the current <see cref="EveFilePath" /> using case-insensitive rules.
    /// </summary>
    /// <returns>A case-insensitive hash code for the underlying path.</returns>
    public override int GetHashCode() => Value?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;

    /// <summary>
    ///  Returns the underlying path string.
    /// </summary>
    /// <returns>The normalized path string.</returns>
    public override string ToString() => Value;

    /// <summary>
    ///  Validates the current path value.
    /// </summary>
    /// <remarks>
    ///  Validation checks: - Empty or null values are considered valid. - Path must not contain invalid path
    ///  characters. - If a file name exists, it must not be a Windows reserved device name. - The path must be well-
    ///  formed and rooted when resolved via <c>Path.GetFullPath</c>.
    /// </remarks>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    protected override bool Validation()
    {
        if(string.IsNullOrEmpty(Value))
            return true;

        if(Value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            return false;

        string? fileName = Path.GetFileName(Value);
        if(string.IsNullOrEmpty(fileName))
            return true;

        if(ReservedNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
            return false;

        try {
            string fullPath = Path.GetFullPath(Value);
            return Path.IsPathRooted(fullPath);
        } catch {
            return false;
        }
    }

}
