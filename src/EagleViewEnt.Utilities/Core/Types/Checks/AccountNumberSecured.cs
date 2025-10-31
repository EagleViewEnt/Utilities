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
///  Secured account number value type that masks sensitive digits when converted to a string.
/// </summary>
/// <param name="Value">
///  The raw account number value. The value is validated by the base <see cref="AccountNumber" /> and is masked for
///  display.
/// </param>
/// <remarks>
///  This type inherits validation from <see cref="AccountNumber" /> and overrides <see cref="ToString" /> to return a
///  masked value suitable for logs, UIs, and other display scenarios.
/// </remarks>
[XmlRoot("AccountNumber"),
JsonConverter(typeof(StringValueTypeJsonConverter<AccountNumberSecured>)),
TypeConverter(typeof(StringValueTypeConverter<AccountNumberSecured>))]

public sealed class AccountNumberSecured( string Value ) : AccountNumber(Value)
{

    /// <summary>
    ///  Implicitly converts an <see cref="AccountNumberSecured" /> to a string.
    /// </summary>
    /// <param name="value">The <see cref="AccountNumberSecured" /> instance.</param>
    public static implicit operator string( AccountNumberSecured value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a string to an <see cref="AccountNumberSecured" />.
    /// </summary>
    /// <param name="value">The value string.</param>
    public static implicit operator AccountNumberSecured( string value ) => new AccountNumberSecured(value);

    /// <summary>
    ///  Returns a masked representation of the account number for secure display.
    /// </summary>
    /// <returns>
    ///  The masked account number string.
    /// </returns>
    /// <remarks>
    ///  Use <see cref="AccountNumber.ToString" /> on the base type to obtain the unmasked value if appropriate.
    /// </remarks>
    public override string ToString() => Value.ToSecurityMaskedString();

}
