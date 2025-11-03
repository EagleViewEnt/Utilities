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

using EagleViewEnt.Utilities.Core.Types.FilePath;
using EagleViewEnt.Utilities.Core.Types.JsonString;
using EagleViewEnt.Utilities.Serialization.Json.Converters;

namespace EagleViewEnt.Utilities.Serialization.Json;

/// <summary>
///  Helper extensions for JSON serialization and deserialization using <see cref="System.Text.Json" />. Includes
///  convenience methods for converting to/from JSON strings, files, and dynamic objects, and pre-configured serializer
///  options.
/// </summary>
public static class JsonSerializerExtensions
{

    /// <summary>
    ///  Pre-configured <see cref="JsonSerializerOptions" /> for human-readable (indented) JSON output. Adds <see
    ///  cref="DateTimeConverterUsingDateTimeParse" /> and uses <see cref="ReferenceHandler.IgnoreCycles" />.
    /// </summary>
    static JsonSerializerOptions PrettyOptions
    {
        get
        {
            JsonSerializerOptions result = new()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 16
            };
            result.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            return result;
        }

    }

    /// <summary>
    ///  Pre-configured <see cref="JsonSerializerOptions" /> for compact (non-indented) JSON output. Adds <see
    ///  cref="DateTimeConverterUsingDateTimeParse" /> and uses <see cref="ReferenceHandler.IgnoreCycles" />.
    /// </summary>
    static JsonSerializerOptions StandardOptions
    {
        get
        {
            var result = new JsonSerializerOptions
            {
                WriteIndented = false,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 16
            };
            result.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            return result;
        }
    }

    /// <summary>
    ///  Deserializes a JSON string to an instance of <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="json" cref="EveJson">The JSON string to deserialize . Must not be null or empty.</param>
    /// <param name="options">
    ///  Optional serializer options. If missing, a new instance is created and <see
    ///  cref="DateTimeConverterUsingDateTimeParse" /> is added when not already present.
    /// </param>
    /// <returns>The deserialized instance, or default when the JSON literal is null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="json" /> is null or empty.</exception>
    /// <exception cref="SerializationException">Thrown when deserialization fails.</exception>
    public static T? FromJson<T>( this EveJson json
                                 , JsonSerializerOptions? options = null ) //where T : ISerializable
    {
        if(string.IsNullOrWhiteSpace(json))
            throw new ArgumentNullException(nameof(json), "Json string required.");
        options ??= new JsonSerializerOptions();
        if(!IsConverterRegistered<DateTimeConverterUsingDateTimeParse>(options))
            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        try {
            var result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        } catch(Exception ex) {
            throw new SerializationException($"FromJson: Unable to deserialize from json string. Type: {typeof(T).Name} Stack: {ex.StackTrace}", ex.InnerException);
        }
    }

    /// <summary>
    ///  Reads a JSON file from disk and deserializes its contents to <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="fullPath">Full path to the JSON file.</param>
    /// <returns>The deserialized instance, or default when the JSON literal is null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fullPath" /> is null/empty or the file does not exist.</exception>
    /// <exception cref="SerializationException">Thrown when deserialization fails.</exception>
    public static T? FromJsonFile<T>( EveFilePath fullPath ) //where T : ISerializable
    {
        var json = (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            ? string.Empty
            : File.ReadAllText(fullPath);

        return FromJson<T>(json);
    }

    /// <summary>
    ///  Creates a compact JSON string describing the exception chain for diagnostics.
    /// </summary>
    /// <param name="ex">The exception to serialize.</param>
    /// <returns>A JSON string with error details.</returns>
    static string GetErrorJson( Exception ex )
    {
        var exceptionList = new List<object>();
        var current = ex;
        while(current is not null) {
            exceptionList.Add(new { current.Message });
            current = current.InnerException;
        }

        var errorObj = new
        {
            Class = nameof(Serialization),
            Method = nameof(ToJson),
            Exceptions = exceptionList
        };

        return JsonSerializer.Serialize(errorObj, StandardOptions);
    }

    /// <summary>
    ///  Determines whether the provided <see cref="JsonSerializerOptions" /> contains a converter that can handle
    ///  <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type to check for.</typeparam>
    /// <param name="options">The serializer options to inspect.</param>
    /// <returns><c>true</c> if a suitable converter is registered; otherwise, <c>false</c>.</returns>
    public static bool IsConverterRegistered<T>( JsonSerializerOptions options )
    {

        // Get the converters registered with the options
        var converters = options.Converters;

        // Check if any converter handles the specified type
        return converters.Any(c => c.CanConvert(typeof(T)));
    }

    static dynamic? ParseElement( JsonElement element )
    {
        switch(element.ValueKind) {
            case JsonValueKind.Object:
                IDictionary<string, object?> expandoObject = new ExpandoObject();
                foreach(var property in element.EnumerateObject())
                    expandoObject[property.Name] = ParseElement(property.Value);
                return expandoObject;
            case JsonValueKind.Array:
                List<dynamic> list = [];
                foreach(var item in element.EnumerateArray())
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

    /// <summary>
    ///  Converts the specified <see cref="EveJson" /> object into a dynamic object representation.
    /// </summary>
    /// <remarks>
    ///  This method parses the JSON data contained in the <paramref name="json" /> parameter and converts it into a
    ///  dynamic object. The resulting object allows for flexible access to the JSON structure without requiring
    ///  predefined types.
    /// </remarks>
    /// <param name="json">The <see cref="EveJson" /> object to convert. Must contain valid JSON data.</param>
    /// <returns>A dynamic object representing the JSON structure, or <see langword="null" /> if the input is invalid.</returns>
    public static dynamic? ToDynamicObj( EveJson json )
    {
        using var document = JsonDocument.Parse(json);
        return ParseElement(document.RootElement);
    }

    /// <summary>
    ///  Converts the specified object to its JSON representation.
    /// </summary>
    /// <remarks>
    ///  This method uses standard serialization options for JSON conversion.  Ensure that the object being serialized
    ///  is compatible with the JSON serializer.
    /// </remarks>
    /// <param name="obj">The object to be serialized to JSON.</param>
    /// <returns>
    ///  An <see cref="EveJson" /> instance containing the JSON representation of the object. If an error occurs during
    ///  serialization, an <see cref="EveJson" /> instance representing the error is returned.
    /// </returns>
    public static EveJson ToJson
        ( this object obj )
    {

        try {
            return JsonSerializer.Serialize(obj, StandardOptions);
        } catch(Exception ex) {
            return GetErrorJson(ex);
        }
    }

    /// <summary>
    ///  Converts the specified object to its JSON representation.
    /// </summary>
    /// <remarks>
    ///  This method allows customization of the JSON output through the <paramref name="prettyPrint" />  and <paramref
    ///  name="useRelaxedEscaping" /> parameters, as well as through the optional  <paramref name="options" />
    ///  parameter. By default, relaxed escaping is enabled, and the output  is not indented.
    /// </remarks>
    /// <param name="obj">The object to serialize to JSON.</param>
    /// <param name="prettyPrint">
    ///  A value indicating whether the resulting JSON should be formatted with indentation for readability.  Defaults
    ///  to <see langword="false" />.
    /// </param>
    /// <param name="useRelaxedEscaping">
    ///  A value indicating whether to use relaxed escaping rules for JSON encoding.  Defaults to <see langword="true"
    ///  />.
    /// </param>
    /// <param name="options">
    ///  Optional <see cref="JsonSerializerOptions" /> to customize the serialization process.  If <see langword="null"
    ///  />, default options are used.
    /// </param>
    /// <returns>
    ///  A JSON representation of the object as an <see cref="EveJson" /> instance.  If an error occurs during
    ///  serialization, an error JSON object is returned.
    /// </returns>
    public static EveJson ToJson
        ( this object obj,
          bool prettyPrint = false,
          bool useRelaxedEscaping = true,
          JsonSerializerOptions? options = null )
    {
        try {
            options ??= StandardOptions;
            options.WriteIndented = prettyPrint;

            if(useRelaxedEscaping)
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            return JsonSerializer.Serialize(obj, options);
        } catch(Exception ex) {
            return GetErrorJson(ex);
        }
    }

    /// <summary>
    ///  Converts this object to a pretty JSON representation.
    /// </summary>
    /// <param name="obj">The object to serialize to JSON.</param>
    /// <param name="useRelaxedEscaping">
    ///  A value indicating whether to use relaxed escaping rules for JSON encoding.  Defaults to <see langword="true"
    ///  />.
    /// </param>
    /// <param name="options">
    ///  Optional <see cref="JsonSerializerOptions" /> to customize the serialization process.  If <see langword="null"
    ///  />, default options are used.
    /// </param>
    /// <returns>
    ///  A pretty JSON representation of the object as an <see cref="EveJson" /> instance.  If an error occurs during
    ///  serialization, an error JSON object is returned.
    /// </returns>
    public static EveJson ToJsonPretty
        ( this object obj, bool useRelaxedEscaping = true, JsonSerializerOptions? options = null )
        => obj.ToJson(useRelaxedEscaping, true, options);

}
