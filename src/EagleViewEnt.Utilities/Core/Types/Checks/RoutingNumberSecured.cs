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

using EagleViewEnt.Utilities.Core.Extensions.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

namespace EagleViewEnt.Utilities.Core.Types.Checks;

/// <summary>
///  Represents a secured, masked view of a bank routing number for display and logging scenarios.
/// </summary>
/// <remarks>
///  Inherits from <see cref="RoutingNumber" /> and overrides <see cref="ToString" /> to return a security-masked value.
///  Use this type when you must avoid exposing the full routing number in logs or UI.
/// </remarks>
/// <param name="Value">
///  The raw routing number value. Validation is enforced by the base <see cref="RoutingNumber" /> type.
/// </param>
[XmlRoot("RoutingNumber"),
JsonConverter(typeof(StringValueTypeJsonConverter<RoutingNumberSecured>)),
TypeConverter(typeof(StringValueTypeConverter<RoutingNumberSecured>))]
public sealed class RoutingNumberSecured( string Value ) : RoutingNumber(Value)
{

    /// <summary>
    ///  Implicitly converts an instance of <see cref="RoutingNumberSecured" /> to its masked string representation.
    /// </summary>
    /// <param name="value">The <see cref="RoutingNumberSecured" /> instance to convert.</param>
    /// <returns>The security-masked routing number string.</returns>
    public static implicit operator string( RoutingNumberSecured value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a raw string to a new <see cref="RoutingNumberSecured" /> instance.
    /// </summary>
    /// <param name="value">The raw routing number string.</param>
    /// <returns>A new <see cref="RoutingNumberSecured" /> instance created from the provided value.</returns>
    public static implicit operator RoutingNumberSecured( string value ) => new(value);

    /// <summary>
    ///  Returns the security-masked routing number for safe display or logging.
    /// </summary>
    /// <returns>The masked routing number string.</returns>
    public override string ToString() => Value.ToSecurityMaskedString();

}