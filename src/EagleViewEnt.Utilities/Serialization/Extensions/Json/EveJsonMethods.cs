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

using System.Dynamic;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using MgbUtilties.Core.Converters.DateAndTime;
using MgbUtilties.Core.Types.JsonString;

namespace MgbUtilties.Serialization.Extensions.Json
{

    public static class MgbJsonMethods
    {

        /// <summary>
        ///  Creates  descendant object from a json string
        /// </summary>
        /// <param name="json">Json string. If null/empty a default T will be returned.</param>
        public static T? FromJson<T>( MgbJson json
                                     , JsonSerializerOptions? options = null ) //where T : ISerializable
        {
            if(string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json), "Json string required.");
            options ??= new JsonSerializerOptions();
            if(!IsConverterRegistered<DateTimeConverterUsingDateTimeParse>(options))
                options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            try {
                T? result = JsonSerializer.Deserialize<T>(json, options);
                return result;
            } catch(Exception ex) {
                throw new SerializationException($"As: Unable to deserialize from json string. Type: {typeof(T).Name} Stack: {ex.StackTrace}", ex.InnerException);
            }
        }

        /// <summary>
        ///  Creates  descendant object from a json string
        /// </summary>
        /// <param name="fullPath">Full path to json file. If null/empty/!exists default T will be returned</param>
        public static T? FromJsonFile<T>( string fullPath ) //where T : ISerializable
        {
            fullPath = (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
                ? string.Empty
                : File.ReadAllText(fullPath);
            return FromJson<T>(fullPath);
        }

        public static bool IsConverterRegistered<T>( JsonSerializerOptions options )
        {

            // Get the converters registered with the options
            IList<JsonConverter> converters = options.Converters;

            // Check if any converter handles the specified type
            return converters.Any(c => c.CanConvert(typeof(T)));
        }

        static dynamic? ParseElement( JsonElement element )
        {
            switch(element.ValueKind) {
                case JsonValueKind.Object:
                    IDictionary<string, object?> expandoObject = new ExpandoObject();
                    foreach(JsonProperty property in element.EnumerateObject())
                        expandoObject[property.Name] = ParseElement(property.Value);
                    return expandoObject;
                case JsonValueKind.Array:
                    List<dynamic> list = [];
                    foreach(JsonElement item in element.EnumerateArray())
                        list.Add(ParseElement(item));
                    return list;
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    return element.TryGetInt64(out long l) ? l : element.GetDouble();
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return element.GetBoolean();
                case JsonValueKind.Null:
                    return null;
                default:
                    throw new InvalidOperationException($"Unsupported JSON value kind: {element.ValueKind}");
            }
        }

        public static dynamic? ToDynamicObj( string json )
        {
            using JsonDocument document = JsonDocument.Parse(json);
            return ParseElement(document.RootElement);
        }

        /// <summary>
        ///  Converts this object to a json string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prettyPrint">Formats json indented</param>
        /// <param name="useRelaxedEscaping">Removes some encoding</param>
        /// <param name="options">MgbJsonExtensions options</param>
        /// <returns></returns>
        public static string ToJson
            ( object obj
              , bool prettyPrint = true
              , bool useRelaxedEscaping = true
              , JsonSerializerOptions? options = null )
        {
            options ??= new() { WriteIndented = prettyPrint };

            if(useRelaxedEscaping)
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            return JsonSerializer.Serialize(obj, options);
        }

    }

}
