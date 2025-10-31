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
using System.Text.RegularExpressions;

namespace EagleViewEnt.Utilities.Core.Extensions.String;

/// <summary>
///  Provides extension methods for formatting phone numbers.
/// </summary>
public static partial class PhoneNumberExtensions
{

    /// <summary>
    ///  Formats the current string as a North American phone number in the form "(###) ###-####". Non-digit characters
    ///  are removed before formatting. If the sanitized input does not contain exactly 10 digits, the sanitized value
    ///  is returned unchanged.
    /// </summary>
    /// <param name="phoneNumber">The input string to format.</param>
    /// <returns>
    ///  The formatted phone number, or the sanitized input (digits only) if the input does not contain exactly 10
    ///  digits.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="phoneNumber" /> is null.</exception>
    public static string FormatPhoneNumber( this string phoneNumber )
    {
        phoneNumber = RemovePhoneNonDigitsRegex().Replace(phoneNumber, string.Empty);
        return PhoneMaskRegex().Replace(phoneNumber, "($1) $2-$3");
    }

    [GeneratedRegex(@"(\d{3})(\d{3})(\d{4})")]
    internal static partial Regex PhoneMaskRegex();

    [GeneratedRegex(@"\D")]
    internal static partial Regex RemovePhoneNonDigitsRegex();

}
