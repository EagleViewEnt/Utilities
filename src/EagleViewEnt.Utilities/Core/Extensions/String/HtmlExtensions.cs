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

using Microsoft.AspNetCore.Components;

namespace EagleViewEnt.Utilities.Core.Extensions.String;

/// <summary>
///  Extension methods for working with HTML content in strings.
/// </summary>
/// <remarks>
///  Provides helpers to convert a string to a <see cref="MarkupString" /> for Blazor rendering and to remove HTML tags
///  using a generated regular expression.
/// </remarks>
public static partial class HtmlExtensions
{

    /// <summary>
    ///  Wraps the specified string in a <see cref="MarkupString" /> so Blazor renders it as raw HTML.
    /// </summary>
    /// <param name="value">The string to wrap. May be <c>null</c>.</param>
    /// <returns>A <see cref="MarkupString" /> that contains the provided value.</returns>
    public static MarkupString AsMarkupString( this string value ) => new(value);

    /// <summary>
    ///  Removes HTML tags from the specified string.
    /// </summary>
    /// <param name="value">The input string that may contain HTML markup.</param>
    /// <returns>
    ///  The input with all HTML tags removed. Returns an empty string when <paramref name="value" /> is <c>null</c> or
    ///  empty.
    /// </returns>
    public static string StripHtml( this string value )
    {
        if(string.IsNullOrEmpty(value))
            return string.Empty;
        return StripHtmlRegex().Replace(value, string.Empty);
    }

    /// <summary>
    ///  Provides a compiled regular expression that matches HTML tags.
    /// </summary>
    /// <returns>
    ///  A <see cref="Regex" /> that matches minimal sequences enclosed in angle brackets (e.g., "&lt;tag&gt;").
    /// </returns>
    [GeneratedRegex("<.*?>")]
    internal static partial Regex StripHtmlRegex();

}
