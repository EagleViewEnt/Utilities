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
using System.Linq;
using System.Text.RegularExpressions;

namespace EagleViewEnt.Utilities.Core.Extensions.String;

/// <summary>
///  Provides string casing-related extension methods, including camel-/Pascal-case splitting and proper-case
///  conversion.
/// </summary>
public static partial class CasingExtensions
{

    /// <summary>
    ///  Compiled regular expression used to split camelCase, PascalCase, and numeric sequences into tokens.
    /// </summary>
    /// <returns>
    ///  A <see cref="Regex" /> that matches word and number segments in a camel- or Pascal-cased identifier.
    /// </returns>
    /// <remarks>
    ///  Pattern: <c>[A-Z][a-z]*|[a-z]+|\d+</c>
    /// </remarks>
    [GeneratedRegex("[A-Z][a-z]*|[a-z]+|\\d+")]
    internal static partial Regex SplitCamelCaseRegex();

    /// <summary>
    ///  Splits a camelCase, PascalCase, or alphanumeric string into a space-separated string of tokens.
    /// </summary>
    /// <param name="source">The input string to split.</param>
    /// <returns>
    ///  A string containing the split tokens separated by a single space. Returns an empty string when no matches are
    ///  found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> is <see langword="null" />.</exception>
    /// <example>
    ///  Input: <c>"HTTPServer2Port"</c> -> Output: <c>"HTTP Server 2 Port"</c>
    /// </example>
    public static string SplitCamelCaseToString( this string source )
    {
        MatchCollection matches = SplitCamelCaseRegex().Matches(source);
        return matches.Aggregate(string.Empty, ( acc, m ) => acc += $"{m.Value} ").Trim();
    }

    /// <summary>
    ///  Converts the specified string to title case using the current culture.
    /// </summary>
    /// <param name="input">The input string to convert.</param>
    /// <returns>The title-cased equivalent of the input string.</returns>
    /// <remarks>
    ///  Uses <see cref="CultureInfo.CurrentCulture" /> and <see cref="TextInfo.ToTitleCase(string)" /> after converting
    ///  to lowercase for consistent behavior.
    /// </remarks>
    /// <exception cref="NullReferenceException">Thrown when <paramref name="input" /> is <see langword="null" />.</exception>
    public static string ToProperCase( string input )
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }

}
