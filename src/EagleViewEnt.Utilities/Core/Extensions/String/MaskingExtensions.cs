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
///  Provides string extension methods for masking and extracting characters.
/// </summary>
public static class MaskingExtensions
{

    /// <summary>
    ///  Returns the last <paramref name="length" /> characters of the specified string.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <param name="length">The number of characters to return from the end of the string.</param>
    /// <returns>
    ///  The last <paramref name="length" /> characters of <paramref name="value" />, or the original string if its
    ///  length is less than or equal to <paramref name="length" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  Thrown by the underlying substring operation if <paramref name="length" /> is negative or results in an invalid
    ///  substring index.
    /// </exception>
    public static string GetLastN( this string value, int length )
        => (value.Length <= length) ? value : value.Substring(value.Length - length);

    /// <summary>
    ///  Masks a string for secure display by padding on the left with <paramref name="padCharacter" />, revealing only
    ///  the last <paramref name="visibleChars" /> characters.
    /// </summary>
    /// <param name="value">The input string to mask.</param>
    /// <param name="visibleChars">The number of trailing characters to leave visible.</param>
    /// <param name="totalLength">The total length of the masked output, including padding and visible characters.</param>
    /// <param name="padCharacter">The character used for masking/padding.</param>
    /// <returns>
    ///  An empty string if <paramref name="value" /> is null or empty; otherwise, a masked string showing only the last
    ///  <paramref name="visibleChars" /> characters. If <paramref name="totalLength" /> is less than the number of
    ///  visible characters, the visible characters are returned without truncation.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///  Thrown when the length of <paramref name="value" /> is less than or equal to <paramref name="visibleChars" />.
    /// </exception>
    public static string ToSecurityMaskedString
        ( this string value, int visibleChars = 4, int totalLength = 8, char padCharacter = '*' )
    {
        if(string.IsNullOrEmpty(value))
            return string.Empty;
        if(value.Length <= visibleChars)
            throw new ArgumentException("Value length must be greater than visible characters.", nameof(value));

        return value.GetLastN(visibleChars).PadLeft(totalLength, padCharacter);
    }

}
