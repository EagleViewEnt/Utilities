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
///  Provides string validation extension methods for common null/empty checks and guard clauses.
/// </summary>
public static class ValidationExtensions
{

    /// <summary>
    ///  Determines whether the specified string is <see langword="null" /> or an empty string ("").
    /// </summary>
    /// <param name="value">The string instance to test.</param>
    /// <returns>
    ///  <see langword="true" /> if <paramref name="value" /> is <see langword="null" /> or <see cref="string.Empty" />;
    ///  otherwise, <see langword="false" />.
    /// </returns>
    public static bool IsEmptyOrNull( this string value ) => string.IsNullOrEmpty(value);

    /// <summary>
    ///  Returns the original string if it is not <see langword="null" /> or empty; otherwise returns the specified
    ///  default value.
    /// </summary>
    /// <param name="value">The string instance to evaluate.</param>
    /// <param name="defaultValue">The value to return when <paramref name="value" /> is <see langword="null" /> or empty.</param>
    /// <returns>
    ///  <paramref name="value" /> when it is not <see langword="null" /> or empty; otherwise <paramref
    ///  name="defaultValue" />.
    /// </returns>
    public static string IsEmptyOrNull( this string value, string defaultValue )
        => value.IsEmptyOrNull() ? defaultValue : value;

    /// <summary>
    ///  Determines whether the specified string is not <see langword="null" /> and not empty.
    /// </summary>
    /// <param name="value">The string instance to test.</param>
    /// <returns>
    ///  <see langword="true" /> if <paramref name="value" /> is not <see langword="null" /> and not <see
    ///  cref="string.Empty" />; otherwise, <see langword="false" />.
    /// </returns>
    public static bool IsNotEmptyOrNull( this string value ) => !string.IsNullOrEmpty(value);

    /// <summary>
    ///  Throws an <see cref="ArgumentNullException" /> if the provided string is <see langword="null" /> or empty.
    /// </summary>
    /// <param name="value">The string instance to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> is <see langword="null" /> or empty.</exception>
    public static void ThrowIfNullOrEmpty( this string? value )
    {
        if(string.IsNullOrEmpty(value))
            throw new ArgumentNullException(value, "Value cannot be null or empty.");
    }

}
