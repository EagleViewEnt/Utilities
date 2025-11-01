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
using System.Text.RegularExpressions;

using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

namespace EagleViewEnt.Utilities.Core.Types.Checks;

/// <summary>
///  Provides a strongly-typed representation of a bank MICR line, extracting routing, account, and optional check
///  numbers.
/// </summary>
/// <remarks>
///  Expected format: "@{routing9}@{account6-17}[+{check0-10}]". Parsing and validation are performed using <see
///  cref="MicrRegEx" />. An empty value is allowed and results in empty component values.
/// </remarks>
[XmlRoot("Micr"),
JsonConverter(typeof(StringValueTypeJsonConverter<Micr>)),
TypeConverter(typeof(StringValueTypeConverter<Micr>))]
public sealed partial class Micr : StringValueType<Micr>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="Micr" /> class from a raw MICR string.
    /// </summary>
    /// <param name="micrRaw">The raw MICR string to parse. May be <see langword="null" /> or empty.</param>
    /// <exception cref="ArgumentException">Thrown when the value is non-empty and does not match the expected MICR format.</exception>
    public Micr( string? micrRaw )
        : base(micrRaw, true)
    {
        (AccountNumber, RoutingNumber, CheckNumber) = ParseMicr(Value.ToString());
        if(!Validation())
            throw new ArgumentException($"Invalid value for {typeof(Micr).Name}: '{Value}'", nameof(micrRaw));

    }

    /// <summary>
    ///  Gets the parsed account number component of the MICR line.
    /// </summary>
    public AccountNumber AccountNumber { get; }

    /// <summary>
    ///  Gets the parsed check number component of the MICR line, if present; otherwise empty.
    /// </summary>
    public CheckNumber CheckNumber { get; }

    /// <summary>
    ///  Gets a value indicating whether the current MICR value is non-empty and contains valid routing and account
    ///  numbers.
    /// </summary>
    public bool IsValid => !IsEmpty && !AccountNumber.IsEmpty && !RoutingNumber.IsEmpty;

    /// <summary>
    ///  Gets the parsed routing number component of the MICR line.
    /// </summary>
    public RoutingNumber RoutingNumber { get; }

    /// <summary>
    ///  Converts a <see cref="Micr" /> to its raw MICR string representation.
    /// </summary>
    /// <param name="value">The <see cref="Micr" /> instance to convert.</param>
    /// <returns>The raw MICR string.</returns>
    public static implicit operator string( Micr value ) => value.Value;

    /// <summary>
    ///  Creates a <see cref="Micr" /> from a raw MICR string.
    /// </summary>
    /// <param name="micrRaw">The raw MICR string. May be <see langword="null" /> or empty.</param>
    /// <returns>A new <see cref="Micr" /> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is non-empty and does not match the expected MICR format.</exception>
    public static implicit operator Micr( string? micrRaw ) => new(micrRaw);

    /// <summary>
    ///  Gets the regular expression used to validate and parse MICR strings.
    /// </summary>
    /// <returns>
    ///  A compiled <see cref="Regex" /> exposing named groups: 'routing', 'account', and 'check'.
    /// </returns>
    /// <remarks>
    ///  Pattern: @(?&lt;routing&gt;\d{9})@(?&lt;account&gt;\d{6,17})\+?(?&lt;check&gt;\d{0,10})?
    /// </remarks>
    [GeneratedRegex(@"@(?<routing>\d{9})@(?<account>\d{6,17})\+?(?<check>\d{0,10})?")]
    internal static partial Regex MicrRegEx();

    /// <summary>
    ///  Parses a MICR string into its routing, account, and check number components.
    /// </summary>
    /// <param name="micr">The raw MICR string. May be <see langword="null" /> or empty.</param>
    /// <returns>
    ///  A tuple containing the parsed <see cref="AccountNumber" />, <see cref="RoutingNumber" />, and <see
    ///  cref="CheckNumber" />. Returns empty components when the input is null or empty.
    /// </returns>
    static (AccountNumber account, RoutingNumber routing, CheckNumber check) ParseMicr( string micr )
    {
        if(string.IsNullOrEmpty(micr))
            return (new AccountNumber(string.Empty), new RoutingNumber(string.Empty), new CheckNumber(string.Empty));

        Match match = MicrRegEx().Match(micr);

        RoutingNumber routing = match.Groups["routing"].Value;

        AccountNumber account = match.Groups["account"].Value;

        CheckNumber check = match.Groups["check"].Value;

        return (account, routing, check);

    }

    /// <summary>
    ///  Returns the raw MICR string value. TODO: Use formatted output in the future.
    /// </summary>
    /// <returns>The unmodified MICR string.</returns>
    public override string ToString() => Value;

    /// <summary>
    ///  Validates the underlying MICR value.
    /// </summary>
    /// <returns>
    ///  <c>true</c> if the value is empty or matches the MICR pattern; otherwise, <c>false</c>.
    /// </returns>
    protected override bool Validation() => IsEmpty || MicrRegEx().IsMatch(Value);

}