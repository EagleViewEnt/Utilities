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
///  Represents a validated check number value object backed by a string.
/// </summary>
/// <param name="Value">
///  The input check number string. A null value is treated as an empty string and leading/trailing whitespace is
///  trimmed.
/// </param>
/// <remarks>
///  Empty values are allowed. Non-empty values must be purely numeric and contain fewer than 6 characters.
/// </remarks>
[XmlRoot("CheckNumber"),
JsonConverter(typeof(StringValueTypeJsonConverter<CheckNumber>)),
TypeConverter(typeof(StringValueTypeConverter<CheckNumber>))]
public sealed class CheckNumber( string Value ) : StringValueType<CheckNumber>(Value)
{

    /// <summary>
    ///  Implicitly converts an <see cref="CheckNumber" /> to a string.
    /// </summary>
    /// <param name="value">The <see cref="CheckNumber" /> instance.</param>
    public static implicit operator string( CheckNumber value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a string to an <see cref="CheckNumber" />.
    /// </summary>
    /// <param name="value">The value string.</param>
    public static implicit operator CheckNumber( string value ) => FromStringValue(value);

    /// <summary>
    ///  Validates the <see cref="CheckNumber" /> value. Returns <c>true</c> if the value is empty or is a numeric
    ///  string with less than 6 characters; otherwise, <c>false</c>.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the value is empty or a valid check number; otherwise, <c>false</c>.
    /// </returns>
    protected override bool Validation() => IsEmpty || ((Value.Length < 6) && long.TryParse(Value, out _));

}