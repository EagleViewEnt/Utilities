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
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

namespace EagleViewEnt.Utilities.Core.Types.Checks;

/*
PSEUDOCODE PLAN
- Identify public members lacking XML documentation:
  - Class primary constructor parameter: Value
  - Method: AsAccountNumberSecured()
  - Method: ToString() override
- Add XML documentation:
  - For class: keep existing summary; add <param name="Value"> to describe the primary constructor parameter
  - For AsAccountNumberSecured(): add <summary> and <returns>
  - For ToString(): add <summary>, <returns>, and a brief <remarks>
- Do not modify any logic or signatures; only augment with XML comments
*/

/// <summary>
///  Immutable value type for AccountNumber fields.
/// </summary>
/// <param name="Value">
///  The raw, unmasked account number value. May be empty to represent an unset value.
/// </param>
[XmlRoot("AccountNumber"),
XmlType(TypeName = "AccountNumber"),
JsonConverter(typeof(StringValueTypeJsonConverter<AccountNumber>)),
TypeConverter(typeof(StringValueTypeConverter<AccountNumber>))]
public class AccountNumber( string Value ) : StringValueType<AccountNumber>(Value)
{

    /// <summary>
    ///  Implicitly converts an <see cref="AccountNumber" /> to a string.
    /// </summary>
    /// <param name="value">The <see cref="AccountNumber" /> instance.</param>
    public static implicit operator string( AccountNumber value ) => value.ToString();

    /// <summary>
    ///  Implicitly converts a string to an <see cref="AccountNumber" />.
    /// </summary>
    /// <param name="value">The value string.</param>
    public static implicit operator AccountNumber( string value ) => FromStringValue(value);

    /// <summary>
    ///  Returns a secured representation of this account number that masks sensitive digits.
    /// </summary>
    /// <returns>
    ///  An <see cref="AccountNumberSecured" /> instance that provides masked display behavior.
    /// </returns>
    public AccountNumberSecured AsAccountNumberSecured() => new AccountNumberSecured(Value);

    // Don't remove, required to override records base ToSring()
    /// <summary>
    ///  Returns the unmasked string value of the account number.
    /// </summary>
    /// <returns>The raw account number string.</returns>
    /// <remarks>
    ///  Use <see cref="EagleViewEnt.Utilities.Core.Extensions.String.MaskingExtensions.ToSecurityMaskedString(string,
    ///  int, int, char)" /> or <see cref="AsAccountNumberSecured" /> for masked display.
    /// </remarks>
    public override string ToString() => Value.ToSecurityMaskedString();

    /// <summary>
    ///  Validates the account number value for length and numeric content.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the value is empty or consists of 6 to 17 numeric digits; otherwise, <c>false</c>.
    /// </returns>
    protected override bool Validation()
        => IsEmpty || ((Value.Length >= 6) && (Value.Length <= 17) && long.TryParse(Value, out _));

}
