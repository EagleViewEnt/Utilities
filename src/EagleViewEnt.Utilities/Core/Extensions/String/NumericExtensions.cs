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
using System.Linq;

namespace EagleViewEnt.Utilities.Core.Extensions.String;

/// <summary>
///  Provides extension methods for numeric values related to string formatting.
/// </summary>
public static class NumericExtensions
{

    /// <summary>
    ///  Converts the specified non-negative integer to a string with its English ordinal indicator.
    /// </summary>
    /// <param name="value">The non-negative integer to convert.</param>
    /// <param name="asHtml">
    ///  If <see langword="true" />, wraps the ordinal suffix in a &lt;sup&gt; element for HTML output;  otherwise
    ///  appends the suffix directly.
    /// </param>
    /// <returns>
    ///  A string containing the number followed by its ordinal indicator (e.g., <c>1st</c>, <c>2nd</c>, <c>3rd</c>,
    ///  <c>4th</c>).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  Thrown when <paramref name="value" /> is negative.
    /// </exception>
    /// <remarks>
    ///  For values ending in 11, 12, or 13, the suffix <c>th</c> is used regardless of the last digit.
    /// </remarks>
    /// <example>
    ///  <code> 1.ToStringWithOrdinalIndicator()   // "1st" 2.ToStringWithOrdinalIndicator()   // "2nd"
    ///  3.ToStringWithOrdinalIndicator()   // "3rd" 4.ToStringWithOrdinalIndicator()   // "4th"
    ///  11.ToStringWithOrdinalIndicator()  // "11th" 1.ToStringWithOrdinalIndicator(true) //
    ///  "1&lt;sup&gt;st&lt;/sup&gt;"</code>
    /// </example>
    public static string ToStringWithOrdinalIndicator( this int value, bool asHtml = false )
    {
        if(value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "The number must be non-negative.");

        int lastDigit = value % 10;
        int lastTwoDigits = value % 100;
        string suffix = (lastTwoDigits is >= 11 and <= 13)
            ? "th"
            : lastDigit switch {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };

        return asHtml ? ($"{value}<sup>{suffix}</sup>") : ($"{value}{suffix}");
    }

}
