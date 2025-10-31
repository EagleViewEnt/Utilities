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

using System.Text.Encodings.Web;
using System.Text.Json;

using EagleViewEnt.Utilities.Core.Converters.DateAndTime;

namespace EagleViewEnt.Utilities.Core.Extensions.Object;

/// <summary>
///  Provides extension methods for serializing objects to JSON.
/// </summary>
public static class JsonExtensions
{

    /// <summary>
    ///  Converts this object to a json string
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="prettyPrint">Formats json indented</param>
    /// <param name="useRelaxedEscaping">Removes some encoding</param>
    /// <param name="options">EveJsonExtensions options</param>
    /// <returns></returns>
    public static string ToJson
        ( object obj, bool prettyPrint = true, bool useRelaxedEscaping = true, JsonSerializerOptions? options = null )
    {
        options ??= new() { WriteIndented = prettyPrint };

        if(useRelaxedEscaping)
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Serialize(obj, options);
    }

}

