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

using System.Text.Json.Serialization;

using Ardalis.SmartEnum;

using EagleViewEnt.Utilities.Core.Types.Money.Converters;

namespace EagleViewEnt.Utilities.Core.Types.Money.Enum;

/// <summary>
///  Represents United States currency denominations as smart enum instances, including both coins and bills, with
///  associated decimal values and denomination types.
/// </summary>
[JsonConverter(typeof(USDenominationConverter))]
public sealed class USDenominations : SmartEnum<USDenominations, decimal>
{

    /// <summary>
    ///  Represents a U.S. dime coin with a value of $0.10.
    /// </summary>
    public static readonly USDenominations Dime = new USDenominations(nameof(Dime), 0.1000M, DenominationTypes.Coin);

    /// <summary>
    ///  Represents a U.S. one-dollar bill with a value of $1.00.
    /// </summary>
    public static readonly USDenominations One = new USDenominations(nameof(One), 1.0000M, DenominationTypes.Bill);

    /// <summary>
    ///  Represents a U.S. fifty-dollar bill with a value of $50.00.
    /// </summary>
    public static readonly USDenominations Fifty = new USDenominations(nameof(Fifty), 50.0000M, DenominationTypes.Bill);

    /// <summary>
    ///  Represents a U.S. five-dollar bill with a value of $5.00.
    /// </summary>
    public static readonly USDenominations Five = new USDenominations(nameof(Five), 5.0000M, DenominationTypes.Bill);

    /// <summary>
    ///  Represents a U.S. half-dollar coin with a value of $0.50.
    /// </summary>
    public static readonly USDenominations HalfDollar = new USDenominations(
        nameof(HalfDollar),
        0.5000M,
        DenominationTypes.Coin);

    /// <summary>
    ///  Represents a U.S. one-hundred-dollar bill with a value of $100.00.
    /// </summary>
    public static readonly USDenominations Hundred = new USDenominations(
        nameof(Hundred),
        100.0000M,
        DenominationTypes.Bill);

    /// <summary>
    ///  Marker denomination indicating multiple denominations may apply; has a value of $0.00.
    /// </summary>
    public static readonly USDenominations Multiple = new USDenominations(
        nameof(Multiple),
        0.0000M,
        DenominationTypes.Both);

    /// <summary>
    ///  Represents a U.S. nickel coin with a value of $0.05.
    /// </summary>
    public static readonly USDenominations Nickel = new USDenominations(nameof(Nickel), 0.0500M, DenominationTypes.Coin);

    /// <summary>
    ///  Represents a U.S. penny coin with a value of $0.01.
    /// </summary>
    public static readonly USDenominations Penny = new USDenominations(nameof(Penny), 0.0100M, DenominationTypes.Coin);

    /// <summary>
    ///  Represents a U.S. quarter coin with a value of $0.25.
    /// </summary>
    public static readonly USDenominations Quarter = new USDenominations(
        nameof(Quarter),
        0.2500M,
        DenominationTypes.Coin);

    /// <summary>
    ///  Represents a U.S. ten-dollar bill with a value of $10.00.
    /// </summary>
    public static readonly USDenominations Ten = new USDenominations(nameof(Ten), 10.0000M, DenominationTypes.Bill);

    /// <summary>
    ///  Represents a U.S. twenty-dollar bill with a value of $20.00.
    /// </summary>
    public static readonly USDenominations Twenty = new USDenominations(
        nameof(Twenty),
        20.0000M,
        DenominationTypes.Bill);

    /// <summary>
    ///  Marker denomination indicating a zero value; has a value of $0.00.
    /// </summary>
    public static readonly USDenominations Zero = new USDenominations(nameof(Zero), 0M, DenominationTypes.Both);

    /// <summary>
    ///  Initializes a new instance of the <see cref="USDenominations" /> smart enum with the specified name, monetary
    ///  value, and denomination classification.
    /// </summary>
    /// <param name="name">The display name of the denomination.</param>
    /// <param name="value">The monetary value of the denomination in U.S. dollars.</param>
    /// <param name="type">Indicates whether the denomination is a bill, coin, or applicable to both.</param>
    /// <remarks>
    ///  This constructor is used to create the predefined denomination instances and to support JSON deserialization.
    /// </remarks>
    [JsonConstructor]
    USDenominations
        ( string name, decimal value, DenominationTypes type )
        : base(name, value)
        => CurrencyType = type;

    /// <summary>
    ///  Gets the denomination category for this instance, indicating whether it is a bill, a coin, or applicable to
    ///  both.
    /// </summary>
    public DenominationTypes CurrencyType { get; }

    /// <summary>
    ///  Gets the pluralized name of the denomination (for example, "Penny" becomes "Pennies").
    /// </summary>
    public string PluralName => Name.EndsWith('y') ? ($"{Name[..^1]}ies") : ($"{Name}s");

    /// <summary>
    ///  Returns the set of U.S. currency denominations that are bills. The result also includes the special <see
    ///  cref="Multiple" /> and <see cref="Zero" /> markers.
    /// </summary>
    /// <returns>An enumerable of bill denominations.</returns>
    public static IEnumerable<USDenominations> Bills() => [ Fifty, Five, Hundred, One, Ten, Twenty, Multiple, Zero ];

    /// <summary>
    ///  Returns the set of U.S. currency denominations that are coins. The result also includes the special <see
    ///  cref="Multiple" /> and <see cref="Zero" /> markers.
    /// </summary>
    /// <returns>An enumerable of coin denominations.</returns>
    public static IEnumerable<USDenominations> Coins() => [ Dime, HalfDollar, Nickel, Penny, Quarter, Multiple, Zero ];

}
