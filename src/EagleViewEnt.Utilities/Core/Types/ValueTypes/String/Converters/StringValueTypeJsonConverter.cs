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
using System.Text.Json;
using System.Text.Json.Serialization;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Interfaces;

namespace EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

/// <summary>
///  Provides a <see cref="JsonConverter{T}" /> for string-backed value types.
/// </summary>
/// <typeparam name="T">
///  The concrete string value type that implements <see cref="IStringValueType" /> and exposes a public constructor
///  that accepts a single <see cref="string" /> parameter.
/// </typeparam>
/// <remarks>
///  This converter expects <typeparamref name="T" /> to define a public constructor with the signature <c>.ctor(string
///  value)</c>. When reading, a JSON string token is mapped to an instance of <typeparamref name="T" /> using that
///  constructor. When writing, the instance is serialized as a JSON string via <see cref="object.ToString" />.
/// </remarks>
public class StringValueTypeJsonConverter<T> : JsonConverter<T> where T : IStringValueType
{

    /// <summary>
    ///  Reads a JSON string and constructs an instance of <typeparamref name="T" />.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader" /> positioned at the value to read.</param>
    /// <param name="typeToConvert">The type to convert to. This should be <typeparamref name="T" />.</param>
    /// <param name="options">The serializer options to use.</param>
    /// <returns>
    ///  An instance of <typeparamref name="T" /> created from the JSON string value; or <see langword="default" /> if
    ///  the token is <see langword="null" />.
    /// </returns>
    /// <exception cref="JsonException">Thrown if the current JSON token cannot be read as a string.</exception>
    /// <exception cref="MissingMethodException">
    ///  Thrown if <typeparamref name="T" /> does not have a public constructor that accepts a single <see cref="string"
    ///  /> parameter.
    /// </exception>
    /// <exception cref="System.Reflection.TargetInvocationException">Thrown if the constructor of <typeparamref name="T" /> throws an exception.</exception>
    public override T? Read
        ( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        string? str = reader.GetString();
        if(str is null)
            return default;
        return (T?)Activator.CreateInstance(typeof(T), str);
    }

    /// <summary>
    ///  Writes the specified <typeparamref name="T" /> instance as a JSON string.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter" /> to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options to use.</param>
    public override void Write
        ( Utf8JsonWriter writer, T value, JsonSerializerOptions options )
        => writer.WriteStringValue(value?.ToString());

}
