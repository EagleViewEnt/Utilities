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

using EagleViewEnt.Utilities.Core.Converters.DateAndTime;
using EagleViewEnt.Utilities.Core.Types.FilePath;
using EagleViewEnt.Utilities.Core.Types.FilePath.Extensions;

namespace EagleViewEnt.Utilities.Core.Types.JsonString.Extensions;

/// <summary>
///  Provides extension methods for serializing and deserializing JSON strings and objects, including dynamic object
///  conversion and file-based JSON operations.
/// </summary>
public static class EveJsonExtensions
{

    /// <summary>
    ///  Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize. Cannot be null or empty.</param>
    /// <param name="options">
    ///  Optional. The <see cref="JsonSerializerOptions" /> to use during deserialization. If null, default options are
    ///  used.
    /// </param>
    /// <returns>An object of type <typeparamref name="T" /> deserialized from the JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="json" /> is null or empty.</exception>
    /// <exception cref="SerializationException">Thrown if deserialization fails.</exception>
    public static T? As<T>( this EveJson json
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
    ///  Converts a JSON string into a dynamic object.
    /// </summary>
    /// <remarks>
    ///  This method parses the provided JSON string and returns a dynamic object that can be used to access JSON
    ///  properties using dynamic syntax. The returned object reflects the structure of the JSON, allowing for flexible
    ///  access to its elements.
    /// </remarks>
    /// <param name="json">The JSON string to be converted. Cannot be null or empty.</param>
    /// <returns>A dynamic object representing the JSON structure, or <see langword="null" /> if the JSON is invalid.</returns>
    public static dynamic? AsDynamicObj( this EveJson json )
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return ParseElement(document.RootElement);
    }

    /// <summary>
    ///  Deserializes the contents of a file into an object of the specified type.
    /// </summary>
    /// <remarks>
    ///  This method reads the entire content of the specified file and attempts to deserialize it into an object of
    ///  type <typeparamref name="T" />. Ensure that the file exists and contains valid JSON data that matches the
    ///  structure of <typeparamref name="T" />.
    /// </remarks>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="fullPath">The full path to the file containing the JSON data.</param>
    /// <returns>
    ///  An instance of type <typeparamref name="T" /> populated with data from the file, or <see langword="null" /> if
    ///  the file does not exist.
    /// </returns>
    public static T? AsFromFile<T>( EveFilePath fullPath ) //where T : ISerializable
    {
        if(!fullPath.FileExists())
            return default;

        EveJson json = File.ReadAllText(fullPath);

        return As<T>(json);
    }

    /// <summary>
    ///  Serializes the specified object to a JSON string.
    /// </summary>
    /// <remarks>
    ///  The method applies a custom converter for <see cref="DateTime" /> objects, ensuring they are serialized using a
    ///  specific format.
    /// </remarks>
    /// <param name="obj">The object to serialize to JSON.</param>
    /// <param name="prettyPrint">
    ///  If <see langword="true" />, the JSON output is formatted with indentation; otherwise, it is compact. The
    ///  default is <see langword="true" />.
    /// </param>
    /// <param name="useRelaxedEscaping">
    ///  If <see langword="true" />, uses relaxed escaping for special characters in the JSON output. The default is
    ///  <see langword="true" />.
    /// </param>
    /// <param name="options">
    ///  Optional. The <see cref="JsonSerializerOptions" /> to use for serialization. If <see langword="null" />,
    ///  default options are used with the specified formatting and escaping settings.
    /// </param>
    /// <returns>A JSON string representation of the specified object.</returns>
    public static EveJson AsJson
        ( this  object obj,
          bool prettyPrint = true,
          bool useRelaxedEscaping = true,
          JsonSerializerOptions? options = null )
    {
        options ??= new() { WriteIndented = prettyPrint };

        if(useRelaxedEscaping)
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Serialize(obj, options);
    }

    /// <summary>
    ///  Determines whether a JSON converter is registered for the specified type.
    /// </summary>
    /// <typeparam name="T">The type to check for a registered converter.</typeparam>
    /// <param name="options">The <see cref="JsonSerializerOptions" /> containing the registered converters.</param>
    /// <returns>
    ///  <see langword="true" /> if a converter is registered for the specified type; otherwise, <see langword="false"
    ///  />.
    /// </returns>
    public static bool IsConverterRegistered<T>( JsonSerializerOptions options )
    {

        // Get the converters registered with the options
        IList<JsonConverter> converters = options.Converters;

        // Check if any converter handles the specified type
        return converters.Any(c => c.CanConvert(typeof(T)));
    }

    /// <summary>
    ///  Parses a <see cref="JsonElement" /> and converts it into a dynamic object representation.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" /> to parse. Must be a valid JSON element.</param>
    /// <returns>
    ///  A dynamic object representing the JSON structure. Returns an <see cref="ExpandoObject" /> for JSON objects, a
    ///  <see cref="List{T}" /> for JSON arrays, a <see cref="string" /> for JSON strings, a <see cref="long" /> or <see
    ///  cref="double" /> for JSON numbers, a <see langword="true" /> or <see langword="false" /> for JSON booleans, and
    ///  <see langword="null" /> for JSON null values.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="element" /> contains an unsupported JSON value kind.</exception>
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

}
