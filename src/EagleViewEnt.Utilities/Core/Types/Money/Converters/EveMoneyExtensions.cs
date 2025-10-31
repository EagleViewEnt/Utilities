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

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  Provides extension methods for splitting and formatting <see cref="EveMoney" /> values.
/// </summary>
public static class EveMoneyExtensions
{

    /// <summary>
    ///  Splits the <see cref="EveMoney" /> amount into the specified number of parts, distributing any remainder (from
    ///  rounding to the nearest cent) to the first few parts.
    /// </summary>
    /// <param name="total">The total <see cref="EveMoney" /> amount to split.</param>
    /// <param name="parts">The number of parts to split the amount into. Must be greater than zero.</param>
    /// <returns>
    ///  An array of <see cref="EveMoney" /> instances representing the split parts. The sum of all parts equals the
    ///  original amount.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="parts" /> is less than or equal to zero.</exception>
    public static EveMoney[] Split( this EveMoney total, int parts )
    {
        if(parts <= 0)
            throw new ArgumentOutOfRangeException(nameof(parts), "Parts must be greater than zero.");

        decimal baseShare = Math.Floor((total.Value / parts) * 100m) / 100m;
        EveMoney[] result = new EveMoney[parts];
        decimal sum = 0m;

        for(int i = 0; i < parts; i++) {
            result[i] = EveMoney.Create(baseShare, total.Currency);
            sum += baseShare;
        }

        // Distribute the remainder (due to rounding) to the first few parts
        decimal remainder = total.Value - sum;
        for(int i = 0; (i < parts) && (remainder > 0m); i++) {
            result[i] = EveMoney.Create(result[i].Value + 0.01m, total.Currency);
            remainder -= 0.01m;
        }

        return result;
    }

    /// <summary>
    ///  Splits the EveMoney amount into the specified number of parts, each being a whole dollar (no cents). The sum of
    ///  all parts will equal the original amount, with the remainder distributed to the first few parts.
    /// </summary>
    public static EveMoney[] SplitWholeDollar( this EveMoney total, int parts )
    {
        if(parts <= 0)
            throw new ArgumentOutOfRangeException(nameof(parts), "Parts must be greater than zero.");

        decimal totalDollars = Math.Floor(total.Value);
        decimal baseShare = Math.Floor(totalDollars / parts);
        EveMoney[] result = new EveMoney[parts];
        decimal sum = 0m;

        for(int i = 0; i < parts; i++) {
            result[i] = EveMoney.Create(baseShare, total.Currency);
            sum += baseShare;
        }

        // Distribute the remainder (whole dollars) to the first few parts
        decimal remainder = totalDollars - sum;
        for(int i = 0; (i < parts) && (remainder > 0m); i++) {
            result[i] = EveMoney.Create(result[i].Value + 1m, total.Currency);
            remainder -= 1m;
        }

        return result;
    }

    /// <summary>
    ///  Returns the string representation of the value with the currency code appended.
    /// </summary>
    public static string ToStringWithCurrency( this EveMoney money )
    {
        if(money.IsZero)
            return "None";

        return string.Format(money.Currency.Culture, "{0:C} {1}", money.Value, money.Currency);
    }

    /// <summary>
    ///  Returns the total whole dollar (bill) amount of the specified <see cref="EveMoney" /> value, truncating any
    ///  fractional (coin) part. If the value is zero, returns <see cref="EveMoneyUsd.Zero" />.
    /// </summary>
    /// <param name="money">The <see cref="EveMoney" /> value to extract the bill amount from.</param>
    /// <returns>
    ///  An <see cref="EveMoney" /> instance representing only the whole dollar (bill) portion of the original value.
    /// </returns>
    public static EveMoney TotalBills( this EveMoney money )
    {
        if(money.IsZero)
            return EveMoneyUsd.Zero;

        // Truncate to get total for bills only
        return Math.Truncate(money);
    }

    /// <summary>
    ///  Returns the total coin (fractional) amount of the specified <see cref="EveMoney" /> value, representing only
    ///  the cents portion (i.e., the value after the decimal point). If the value is zero, returns <see
    ///  cref="EveMoneyUsd.Zero" />.
    /// </summary>
    /// <param name="money">The <see cref="EveMoney" /> value to extract the coin amount from.</param>
    /// <returns>
    ///  An <see cref="EveMoney" /> instance representing only the coin (fractional) portion of the original value.
    /// </returns>
    public static EveMoney TotalCoins( this EveMoney money )
    {
        if(money.IsZero)
            return EveMoneyUsd.Zero;

        // Get the fractional part for coins
        return money.Value - Math.Truncate(money.Value);
    }

}