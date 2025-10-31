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
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Extensions.Numeric;
using EagleViewEnt.Utilities.Core.Types.Money.Converters;
using EagleViewEnt.Utilities.Core.Types.Money.Enum;

namespace EagleViewEnt.Utilities.Core.Types.Money;

/// <summary>
///  Strongly-typed USD money value that wraps <see cref="EveMoney" /> and guarantees the <see cref="EveCurrency.USD" />
///  currency. Provides conversions, comparisons, and arithmetic with <see cref="decimal" />, <see cref="EveMoneyUsd"
///  />, and cross-operations with <see cref="EveMoney" />.
/// </summary>

[JsonConverter(typeof(EveMoneyUsdJsonConverter)),
    TypeConverter(typeof(EveMoneyUsdEFCoreConverter))]
public readonly record struct EveMoneyUsd
{

    /// <summary>
    ///  A zero-valued USD instance.
    /// </summary>
    public static readonly EveMoneyUsd Zero = new(EveMoney.Zero);

    /// <summary>
    ///  Initializes a new instance of <see cref="EveMoneyUsd" /> from a <see cref="decimal" /> amount, normalized using
    ///  banker's rounding.
    /// </summary>
    /// <param name="value">The monetary amount.</param>
    public EveMoneyUsd( decimal value ) => AsEveMoney = EveMoney.Create(value.ToBankersRounding(), EveCurrency.USD);

    /// <summary>
    ///  Initializes a new instance of <see cref="EveMoneyUsd" /> from an existing <see cref="EveMoney" /> that must be
    ///  USD.
    /// </summary>
    /// <param name="money">The existing <see cref="EveMoney" />.</param>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="money" /> is not USD.</exception>
    EveMoneyUsd( EveMoney money )
    {
        if(money.Currency != EveCurrency.USD)
            throw new InvalidOperationException("Cannot create EveMoneyUsd from non-USD EveMoney.");
        AsEveMoney = money;
    }

    /// <summary>
    ///  Gets the underlying <see cref="EveMoney" /> instance (always USD).
    /// </summary>
    public EveMoney AsEveMoney { get; }

    /// <summary>
    ///  Gets a value indicating whether this instance represents zero (0.00 USD).
    /// </summary>
    [JsonIgnore, NotMapped, XmlIgnore]
    public bool IsZero => AsEveMoney.IsZero;

    /// <summary>
    ///  Gets the numeric value of this USD amount.
    /// </summary>
    public decimal Value => AsEveMoney.Value;

    /// <summary>
    ///  Implicitly converts a <see cref="decimal" /> to <see cref="EveMoneyUsd" /> using banker's rounding.
    /// </summary>
    /// <param name="value">The monetary amount.</param>
    public static implicit operator EveMoneyUsd( decimal value ) => new(value);

    /// <summary>
    ///  Explicitly converts an <see cref="EveMoney" /> to <see cref="EveMoneyUsd" />. The money must be USD.
    /// </summary>
    /// <param name="money">The source <see cref="EveMoney" />.</param>
    /// <exception cref="InvalidOperationException">Thrown if the currency is not USD.</exception>
    public static explicit operator EveMoneyUsd( EveMoney money ) => new(money);

    /// <summary>
    ///  Implicitly converts an <see cref="EveMoneyUsd" /> to its underlying <see cref="EveMoney" /> (USD).
    /// </summary>
    /// <param name="usd">The USD value.</param>
    public static implicit operator EveMoney( EveMoneyUsd usd ) => usd.AsEveMoney;

    /// <summary>
    ///  Implicitly converts an <see cref="EveMoneyUsd" /> to a <see cref="decimal" />.
    /// </summary>
    /// <param name="usd">The USD value.</param>
    public static implicit operator decimal( EveMoneyUsd usd ) => usd.Value;

    /// <summary>
    ///  Returns a value indicating whether the left operand is greater than or equal to the right operand.
    /// </summary>
    public static bool operator >=( EveMoneyUsd left, EveMoneyUsd right ) => left.AsEveMoney >= right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left USD operand is greater than or equal to the right money operand.
    /// </summary>
    public static bool operator >=( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney >= right;

    /// <summary>
    ///  Returns a value indicating whether the left money operand is greater than or equal to the right USD operand.
    /// </summary>
    public static bool operator >=( EveMoney left, EveMoneyUsd right ) => left >= right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left operand is greater than the right operand.
    /// </summary>
    public static bool operator >( EveMoneyUsd left, EveMoneyUsd right ) => left.AsEveMoney > right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left USD operand is greater than the right money operand.
    /// </summary>
    public static bool operator >( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney > right;

    /// <summary>
    ///  Returns a value indicating whether the left money operand is greater than the right USD operand.
    /// </summary>
    public static bool operator >( EveMoney left, EveMoneyUsd right ) => left > right.AsEveMoney;

    /// <summary>
    ///  Determines whether the specified USD and money values are equal.
    /// </summary>
    public static bool operator ==( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney == right;

    /// <summary>
    ///  Determines whether the specified money and USD values are equal.
    /// </summary>
    public static bool operator ==( EveMoney left, EveMoneyUsd right ) => left == right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left operand is less than or equal to the right operand.
    /// </summary>
    public static bool operator <=( EveMoneyUsd left, EveMoneyUsd right ) => left.AsEveMoney <= right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left USD operand is less than or equal to the right money operand.
    /// </summary>
    public static bool operator <=( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney <= right;

    /// <summary>
    ///  Returns a value indicating whether the left money operand is less than or equal to the right USD operand.
    /// </summary>
    public static bool operator <=( EveMoney left, EveMoneyUsd right ) => left <= right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left operand is less than the right operand.
    /// </summary>
    public static bool operator <( EveMoneyUsd left, EveMoneyUsd right ) => left.AsEveMoney < right.AsEveMoney;

    /// <summary>
    ///  Returns a value indicating whether the left USD operand is less than the right money operand.
    /// </summary>
    public static bool operator <( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney < right;

    /// <summary>
    ///  Returns a value indicating whether the left money operand is less than the right USD operand.
    /// </summary>
    public static bool operator <( EveMoney left, EveMoneyUsd right ) => left < right.AsEveMoney;

    /// <summary>
    ///  Adds two USD values.
    /// </summary>
    /// <param name="left">The first USD value.</param>
    /// <param name="right">The second USD value.</param>
    /// <returns>A new USD value that is the sum.</returns>
    public static EveMoneyUsd operator +( EveMoneyUsd left, EveMoneyUsd right )
        => new(left.AsEveMoney + right.AsEveMoney);

    /// <summary>
    ///  Adds a USD value and a money value.
    /// </summary>
    public static EveMoney operator +( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney + right;

    /// <summary>
    ///  Adds a money value and a USD value.
    /// </summary>
    public static EveMoney operator +( EveMoney right, EveMoneyUsd left ) => right + left.AsEveMoney;

    /// <summary>
    ///  Adds a USD value and a decimal value.
    /// </summary>
    public static EveMoneyUsd operator +( EveMoneyUsd left, decimal right ) => new(left.Value + right);

    /// <summary>
    ///  Adds a decimal value and a USD value.
    /// </summary>
    public static EveMoneyUsd operator +( decimal left, EveMoneyUsd right ) => new(left + right.Value);

    /// <summary>
    ///  Divides a USD value by a decimal.
    /// </summary>
    public static EveMoneyUsd operator /( EveMoneyUsd left, decimal right ) => new(left.Value / right);

    /// <summary>
    ///  Divides one USD value by another and returns the scalar ratio.
    /// </summary>
    public static decimal operator /( EveMoneyUsd left, EveMoneyUsd right ) => left.Value / right.Value;

    /// <summary>
    ///  Divides a USD value by a money value and returns the scalar ratio.
    /// </summary>
    public static decimal operator /( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney / right;

    /// <summary>
    ///  Divides a money value by a USD value and returns the scalar ratio.
    /// </summary>
    public static decimal operator /( EveMoney right, EveMoneyUsd left ) => right / left.AsEveMoney;

    /// <summary>
    ///  Multiplies a USD value by a decimal.
    /// </summary>
    public static EveMoneyUsd operator *( EveMoneyUsd left, decimal right ) => new(left.Value * right);

    /// <summary>
    ///  Multiplies a decimal by a USD value.
    /// </summary>
    public static EveMoneyUsd operator *( decimal left, EveMoneyUsd right ) => new(left * right.Value);

    /// <summary>
    ///  Multiplies a USD value by a money value, returning a money amount in the money's currency.
    /// </summary>
    public static EveMoney operator *( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney * right.Value;

    /// <summary>
    ///  Multiplies a money value by a USD value, returning a money amount in the money's currency.
    /// </summary>
    public static EveMoney operator *( EveMoney right, EveMoneyUsd left ) => right * left.Value;

    /// <summary>
    ///  Determines whether the specified USD and money values are not equal.
    /// </summary>
    public static bool operator !=( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney != right;

    /// <summary>
    ///  Determines whether the specified money and USD values are not equal.
    /// </summary>
    public static bool operator !=( EveMoney left, EveMoneyUsd right ) => left != right.AsEveMoney;

    /// <summary>
    ///  Subtracts one USD value from another.
    /// </summary>
    /// <returns>A new USD value that is the difference.</returns>
    public static EveMoneyUsd operator -( EveMoneyUsd left, EveMoneyUsd right )
        => new(left.AsEveMoney - right.AsEveMoney);

    /// <summary>
    ///  Subtracts a money value from a USD value.
    /// </summary>
    public static EveMoney operator -( EveMoneyUsd left, EveMoney right ) => left.AsEveMoney - right;

    /// <summary>
    ///  Subtracts a USD value from a money value.
    /// </summary>
    public static EveMoney operator -( EveMoney right, EveMoneyUsd left ) => right - left.AsEveMoney;

    /// <summary>
    ///  Subtracts a decimal from a USD value.
    /// </summary>
    public static EveMoneyUsd operator -( EveMoneyUsd left, decimal right ) => new(left.Value - right);

    /// <summary>
    ///  Subtracts a USD value from a decimal and returns a USD result.
    /// </summary>
    public static EveMoneyUsd operator -( decimal left, EveMoneyUsd right ) => new(left - right.Value);

    /// <summary>
    ///  Compares this instance to another <see cref="EveMoneyUsd" /> for ordering.
    /// </summary>
    /// <param name="other">The other USD value.</param>
    /// <returns>
    ///  Less than zero if this instance is less than <paramref name="other" />, zero if equal, greater than zero if
    ///  greater.
    /// </returns>
    public int CompareTo( EveMoneyUsd other ) => AsEveMoney.CompareTo(other.AsEveMoney);

    /// <summary>
    ///  Creates a new <see cref="EveMoneyUsd" /> from a <see cref="decimal" /> using banker's rounding.
    /// </summary>
    /// <param name="value">The monetary amount.</param>
    /// <returns>A new USD value.</returns>
    public static EveMoneyUsd Create( decimal value ) => new(value);

    /// <summary>
    ///  Indicates whether the current object is equal to another <see cref="EveMoneyUsd" />.
    /// </summary>
    /// <param name="other">The other USD value.</param>
    /// <returns><see langword="true" /> if equal; otherwise, <see langword="false" />.</returns>
    public bool Equals( EveMoneyUsd other ) => AsEveMoney.Equals(other.AsEveMoney);

    /// <summary>
    ///  Returns a hash code for this instance.
    /// </summary>
    /// <returns>An integer hash code.</returns>
    public override int GetHashCode() => AsEveMoney.GetHashCode();

    /// <summary>
    ///  Returns a currency-formatted string using the USD culture ("en-US").
    /// </summary>
    /// <returns>A string representation of the value formatted as currency.</returns>
    public override string ToString() => Value.ToString("C");

}