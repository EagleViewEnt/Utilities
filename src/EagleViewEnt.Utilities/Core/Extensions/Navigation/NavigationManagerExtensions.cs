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

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;


namespace EagleViewEnt.Utilities.Core.Extensions.Navigation;

/// <summary>
///  Provides extension methods for <see cref="NavigationManager" /> to assist with query string parameter retrieval.
/// </summary>
public static class NavigationManagerExtensions
{

    /// <summary>
    ///  Retrieves the value of a query string parameter from the current URI and attempts to convert it to the
    ///  specified type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type to which the query parameter value should be converted.</typeparam>
    /// <param name="navManager">The <see cref="NavigationManager" /> instance used to access the current URI.</param>
    /// <param name="key">The key of the query string parameter to retrieve.</param>
    /// <returns>
    ///  The value of the query string parameter converted to type <typeparamref name="T" />, or <c>default</c> if the
    ///  parameter is not found or cannot be converted.
    /// </returns>
    public static T? GetQueryParam<T>( this NavigationManager navManager
                                      , string key )
    {
        T? result = default;

        Uri uri = navManager.ToAbsoluteUri(navManager.Uri);

        if(QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out StringValues valueFromQueryString)) {
            if((typeof(T) == typeof(int)) && int.TryParse(valueFromQueryString, out int valueAsInt))
                result = (T)(object)valueAsInt;

            if(typeof(T) == typeof(string))
                result = (T)(object)valueFromQueryString.ToString();
            if(typeof(T) == typeof(bool))
                result = valueFromQueryString.ToString().ToLower() switch {
                    "1" or "true" => (T)(object)true,
                    _ => (T)(object)false,
                };
            if((typeof(T) == typeof(decimal)) && decimal.TryParse(valueFromQueryString, out decimal valueAsDecimal))
                result = (T)(object)valueAsDecimal;
        }

        return result ?? default;
    }

}

