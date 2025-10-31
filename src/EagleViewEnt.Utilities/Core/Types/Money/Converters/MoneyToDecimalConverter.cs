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
using System.Globalization;

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  Provides functionality to convert culture-specific currency-formatted strings into <see cref="decimal" /> values.
/// </summary>
/// <remarks>
///  Parsing is performed using <see cref="NumberStyles.Currency" /> with the specified culture.
/// </remarks>
public static class MoneyToDecimalConverter
{

    /// <summary>
    ///  Converts a currency-formatted string to its <see cref="decimal" /> representation using the specified culture.
    /// </summary>
    /// <param name="money">The input string representing a monetary value (for example, "$1,234.56").</param>
    /// <param name="language">
    ///  An IETF BCP 47 language tag or culture name (for example, "en-US") used to interpret the input. Defaults to
    ///  "en-US".
    /// </param>
    /// <returns>The parsed <see cref="decimal" /> monetary value.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="money" /> is null, empty, or consists only of white-space.</exception>
    /// <exception cref="FormatException">Thrown when the input cannot be parsed as a currency value for the specified culture.</exception>
    /// <exception cref="CultureNotFoundException">Thrown when the specified <paramref name="language" /> is not a supported culture.</exception>
    public static decimal Convert( string money, string language = "en-US" )
    {
        if(string.IsNullOrWhiteSpace(money))
            throw new ArgumentException("Money string cannot be null or empty.", nameof(money));

        CultureInfo cultureInfo = new CultureInfo(language);
        if(decimal.TryParse(money, NumberStyles.Currency, cultureInfo, out decimal result))
            return result;

        throw new FormatException("Invalid money format.");
    }

}
