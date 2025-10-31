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

namespace EagleViewEnt.Utilities.Core.Extensions.Numeric;

/// <summary>
///  Provides extension methods for <see cref="decimal" /> values, including banker's rounding, currency formatting,
///  fractional string conversion, and rounding away from zero.
/// </summary>
public static class DecimalExtensions
{

    /// <summary>
    ///  Rounds a value to the specified number of decimal places using banker's rounding (to even).
    /// </summary>
    /// <param name="value">The decimal value to round.</param>
    /// <param name="decimals">The number of decimal places to round to. The default is 2.</param>
    /// <returns>The value rounded to the specified number of decimal places using midpoint-to-even.</returns>
    public static decimal ToBankersRounding( this decimal value
                                            , int decimals = 2 )
        => Math.Round(value, decimals, MidpointRounding.ToEven);

    /// <summary>
    ///  Formats the value as a currency string after applying banker's rounding.
    /// </summary>
    /// <param name="value">The decimal value to format.</param>
    /// <returns>A culture-specific currency string formatted with the "C" standard numeric format.</returns>
    /// <remarks>
    ///  Uses the current culture for currency symbols, group separators, and decimal separators.
    /// </remarks>
    public static string ToCurrencyString( this decimal value ) => value.ToBankersRounding().ToString("C");

    /// <summary>
    ///  Converts the value to a fractional string representation using a best-fit denominator up to a specified
    ///  maximum.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="maxDenominator">The maximum allowed denominator when approximating the fractional part. The default is 1000.</param>
    /// <returns>
    ///  A string representing the value as: - "w n/d" when both whole and fractional parts exist, - "n/d" when there is
    ///  no whole part, - or "w" when there is no fractional part.
    /// </returns>
    /// <remarks>
    ///  Negative values preserve their sign. The method searches for the fraction whose value is closest to the
    ///  fractional part with a denominator not exceeding <paramref name="maxDenominator" />.
    /// </remarks>
    public static string ToFractionString( this decimal value
                                          , int maxDenominator = 1000 )
    {
        int sign = Math.Sign(value);
        value = Math.Abs(value);

        int wholePart = (int)value;
        decimal fractionalPart = value - wholePart;

        if(fractionalPart == 0)
            return wholePart.ToString();

        int numerator = 0;
        int denominator = 1;
        decimal bestDifference = decimal.MaxValue;

        for(int d = 1; d <= maxDenominator; d++) {
            int n = (int)Math.Round(fractionalPart * d);
            decimal fraction = ((decimal)n) / d;
            decimal difference = Math.Abs(fraction - fractionalPart);

            if(difference < bestDifference) {
                numerator = n;
                denominator = d;
                bestDifference = difference;
            }
        }

        if(denominator > maxDenominator)

            // If the denominator exceeds the maxDenominator, return the previous fraction
            return (sign * wholePart).ToString();
        else if(wholePart == 0)

            // No whole part
            return $"{sign * numerator}/{denominator}";
        else

            // Both whole part and fractional part
            return $"{sign * wholePart} {numerator}/{denominator}";
    }

    /// <summary>
    ///  Rounds a value to the specified number of decimal places, with midpoint values rounded away from zero.
    /// </summary>
    /// <param name="value">The decimal value to round.</param>
    /// <param name="decimals">The number of decimal places to round to. The default is 2.</param>
    /// <returns>The value rounded to the specified number of decimal places using midpoint-away-from-zero.</returns>
    public static decimal ToRoundedAwayFromZero( this decimal value
                                                , int decimals = 2 )
        => Math.Round(value, decimals, MidpointRounding.AwayFromZero);

}
