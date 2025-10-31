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
///  Represents a monetary amount paired with a specific <see cref="EveCurrency" />. Provides arithmetic operations that
///  are safe only when the currencies match, implicit conversions to and from <see cref="decimal" />, and culture-aware
///  formatting.
/// </summary>
/// <param name="Value">
///  The monetary amount. Values are normalized using banker's rounding when created via <see cref="Create(decimal,
///  EveCurrency)" />.
/// </param>
/// <param name="Currency">
///  The associated <see cref="EveCurrency" /> of the monetary amount.
/// </param>
/// <remarks>
///  - Operations between two <see cref="EveMoney" /> values require the same <see cref="Currency" />. - String
///  formatting uses the culture associated with <see cref="Currency" />.
/// </remarks>
[XmlRoot("Money")]
[JsonConverter(typeof(EveMoneyJsonConverter)),
    TypeConverter(typeof(EveMoneyEFCoreConverter))]
public record struct EveMoney( decimal Value, EveCurrency Currency )
{

    /// <summary>
    ///  The default currency used when implicitly converting from <see cref="decimal" /> to <see cref="EveMoney" />.
    /// </summary>
    static readonly EveCurrency DefaultCurrency = EveCurrency.USD;

    /// <summary>
    ///  A zero-valued money instance in the default currency.
    /// </summary>
    public static readonly EveMoney Zero = Create(0m, DefaultCurrency);

    /// <summary>
    ///  Gets a value indicating whether this instance represents zero (0.00) in its currency.
    /// </summary>
    [JsonIgnore, NotMapped, XmlIgnore]
    public readonly bool IsZero => Value == Zero.Value;

    /// <summary>
    ///  Implicitly converts a <see cref="decimal" /> to an <see cref="EveMoney" /> using the default currency (<see
    ///  cref="EveCurrency.USD" />).
    /// </summary>
    /// <param name="value">The monetary amount.</param>
    /// <returns>An <see cref="EveMoney" /> instance with the specified <paramref name="value" /> and the default currency.</returns>
    public static implicit operator EveMoney( decimal value ) => Create(value, DefaultCurrency);

    /// <summary>
    ///  Implicitly converts an <see cref="EveMoney" /> to its <see cref="decimal" /> amount.
    /// </summary>
    /// <param name="money">The money instance to convert.</param>
    /// <returns>The underlying <see cref="Value" />.</returns>
    public static implicit operator decimal( EveMoney money ) => money.Value;

    /// <summary>
    ///  Adds two <see cref="EveMoney" /> values with the same currency.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the sum.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    public static EveMoney operator +( EveMoney left, EveMoney right )
    {
        EnsureSameCurrency(left, right);
        return Create(left.Value + right.Value, left.Currency);
    }

    /// <summary>
    ///  Adds a scalar <see cref="decimal" /> to an <see cref="EveMoney" /> value.
    /// </summary>
    /// <param name="left">The money value.</param>
    /// <param name="right">The scalar amount to add.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the sum.</returns>
    public static EveMoney operator +( EveMoney left, decimal right ) => Create(left.Value + right, left.Currency);

    /// <summary>
    ///  Adds a scalar <see cref="decimal" /> to an <see cref="EveMoney" /> value.
    /// </summary>
    /// <param name="left">The scalar amount to add.</param>
    /// <param name="right">The money value.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the sum.</returns>
    public static EveMoney operator +( decimal left, EveMoney right ) => Create(left + right.Value, right.Currency);

    /// <summary>
    ///  Divides an <see cref="EveMoney" /> value by a scalar <see cref="decimal" />.
    /// </summary>
    /// <param name="left">The money value (dividend).</param>
    /// <param name="right">The scalar (divisor).</param>
    /// <returns>A new <see cref="EveMoney" /> representing the quotient.</returns>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="right" /> is zero.</exception>
    public static EveMoney operator /( EveMoney left, decimal right ) => Create(left.Value / right, left.Currency);

    /// <summary>
    ///  Divides one <see cref="EveMoney" /> by another with the same currency, returning a scalar ratio.
    /// </summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The scalar ratio of <paramref name="left" /> to <paramref name="right" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="right" /> has a zero value.</exception>
    public static decimal operator /( EveMoney left, EveMoney right )
    {
        EnsureSameCurrency(left, right);
        return left.Value / right.Value;
    }

    /// <summary>
    ///  Multiplies an <see cref="EveMoney" /> value by a scalar <see cref="decimal" />.
    /// </summary>
    /// <param name="left">The money value.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the product.</returns>
    public static EveMoney operator *( EveMoney left, decimal right ) => Create(left.Value * right, left.Currency);

    /// <summary>
    ///  Multiplies an <see cref="EveMoney" /> value by a scalar <see cref="decimal" />.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The money value.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the product.</returns>
    public static EveMoney operator *( decimal left, EveMoney right ) => Create(left * right.Value, right.Currency);

    /// <summary>
    ///  Subtracts one <see cref="EveMoney" /> from another with the same currency.
    /// </summary>
    /// <param name="left">The minuend.</param>
    /// <param name="right">The subtrahend.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the difference.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    public static EveMoney operator -( EveMoney left, EveMoney right )
    {
        EnsureSameCurrency(left, right);
        return Create(left.Value - right.Value, left.Currency);
    }

    /// <summary>
    ///  Subtracts a scalar <see cref="decimal" /> from an <see cref="EveMoney" /> value.
    /// </summary>
    /// <param name="left">The money value.</param>
    /// <param name="right">The scalar to subtract.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the difference.</returns>
    public static EveMoney operator -( EveMoney left, decimal right ) => Create(left.Value - right, left.Currency);

    /// <summary>
    ///  Subtracts an <see cref="EveMoney" /> value from a scalar <see cref="decimal" />.
    /// </summary>
    /// <param name="left">The scalar minuend.</param>
    /// <param name="right">The money subtrahend.</param>
    /// <returns>A new <see cref="EveMoney" /> representing the difference.</returns>
    public static EveMoney operator -( decimal left, EveMoney right ) => Create(left - right.Value, right.Currency);

    /// <summary>
    ///  Compares this instance to another <see cref="EveMoney" /> for ordering.
    /// </summary>
    /// <param name="other">The other money value.</param>
    /// <returns>
    ///  Less than zero if this instance is less than <paramref name="other" />, zero if equal, greater than zero if
    ///  greater.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    public readonly int CompareTo( EveMoney other )
    {
        EnsureSameCurrency(this, other);
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    ///  Creates a new <see cref="EveMoney" /> with the specified value and currency, normalizing the value using
    ///  banker's rounding.
    /// </summary>
    /// <param name="value">The monetary amount.</param>
    /// <param name="currency">The currency to associate with the amount.</param>
    /// <returns>A new <see cref="EveMoney" /> instance.</returns>
    public static EveMoney Create( decimal value, EveCurrency currency )
        => new EveMoney(value.ToBankersRounding(), currency);

    /// <summary>
    ///  Ensures that two <see cref="EveMoney" /> instances have the same currency.
    /// </summary>
    /// <param name="left">The first money instance.</param>
    /// <param name="right">The second money instance.</param>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    internal static void EnsureSameCurrency( EveMoney left, EveMoney right )
    {
        if(left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot operate on different currencies: {left.Currency} vs {right.Currency}");
    }

    /// <summary>
    ///  Returns a culture-aware string representation of this money.
    /// </summary>
    /// <returns>
    ///  A string formatted with the currency culture and symbol, followed by the currency code. For zero values,
    ///  returns "0.00 {Currency}".
    /// </returns>
    public override readonly string ToString()
        => IsZero ? ($"0.00 {Currency}") : ($"{string.Format(Currency.Culture, "{0:C}", Value)} {Currency}");

}
