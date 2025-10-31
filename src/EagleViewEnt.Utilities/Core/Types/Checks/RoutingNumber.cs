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
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

namespace EagleViewEnt.Utilities.Core.Types.Checks;

/// <summary>
///  Represents an ABA routing transit number (RTN).
/// </summary>
/// <param name="Value">
///  The underlying string value for the routing number. Expected to be a 9-digit numeric string.
/// </param>
/// <remarks>
///  Validation enforces: - Empty value is allowed (represents <see cref="StringValueType{TSelf}.Empty" />). - Non-empty
///  values must be exactly 9 numeric digits. - Check digit must satisfy the ABA weighting algorithm (7,3,9 repeated).
/// </remarks>

[XmlRoot("RoutingNumber"),
JsonConverter(typeof(StringValueTypeJsonConverter<RoutingNumber>)),
TypeConverter(typeof(StringValueTypeConverter<RoutingNumber>))]

public class RoutingNumber( string Value ) : StringValueType<RoutingNumber>(Value)
{

    /// <summary>
    ///  Implicitly converts a <see cref="RoutingNumber" /> to its string representation.
    /// </summary>
    /// <param name="value">The <see cref="RoutingNumber" /> instance to convert.</param>
    /// <returns>The underlying routing number as a string.</returns>
    public static implicit operator string( RoutingNumber value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a string to a <see cref="RoutingNumber" />.
    /// </summary>
    /// <param name="value">The routing number string to convert.</param>
    /// <returns>A new <see cref="RoutingNumber" /> representing the specified value.</returns>
    public static implicit operator RoutingNumber( string value ) => FromStringValue(value);

    /// <summary>
    ///  Creates a secured representation of the routing number for masked display.
    /// </summary>
    /// <returns>A <see cref="RoutingNumberSecured" /> instance that masks the underlying value.</returns>
    public RoutingNumberSecured AsRoutingNumberSecured() => new RoutingNumberSecured(Value);

    /// <summary>
    ///  Validates the routing number using the ABA check digit algorithm with weights 7-3-9.
    /// </summary>
    /// <returns>
    ///  <see langword="true" /> if the checksum is valid; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///  The calculation is: (7*d1 + 3*d2 + 9*d3 + 7*d4 + 3*d5 + 9*d6 + 7*d7 + 3*d8 + 9*d9) mod 10 == 0, where d1..d9 are
    ///  the digits of the routing number.
    /// </remarks>
    bool IsValidChecksum()
    {
        long checksum =
            (7 * (Value[0] - '0')) + (3 * (Value[1] - '0')) + (9 * (Value[2] - '0')) + (7 * (Value[3] - '0')) + (3 * (Value[4] - '0')) + (9 * (Value[5] - '0')) + (7 * (Value[6] - '0')) + (3 * (Value[7] - '0')) + (9 * (Value[8] - '0'));

        return checksum % 10 == 0;
    }

    /// <summary>
    ///  Returns the underlying routing number string.
    /// </summary>
    /// <returns>The routing number as a string.</returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    ///  Performs validation of the routing number value.
    /// </summary>
    /// <returns>
    ///  <see langword="true" /> when the value is empty, or when it is a 9-digit numeric string with a valid checksum;
    ///  otherwise, <see langword="false" />.
    /// </returns>
    protected override bool Validation()
        => IsEmpty || ((Value.Length == 9) && long.TryParse(Value, out _) && IsValidChecksum());

}