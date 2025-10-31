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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Interfaces;

namespace EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

/// <summary>
///  Provides an XML-serialization helper for string-backed value types that implement <see cref="IStringValueType" />.
/// </summary>
/// <typeparam name="T">
///  The string-based value type to serialize and deserialize. Must implement <see cref="IStringValueType" /> and
///  provide a public constructor that accepts a single <see cref="string" /> argument.
/// </typeparam>
/// <remarks>
///  Deserialization creates instances via <see cref="Activator.CreateInstance(Type, object?[])" /> with the string
///  content read from the XML element.
/// </remarks>
public class StringValueTypeXmlConverter<T> : IXmlSerializable where T : IStringValueType
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="StringValueTypeXmlConverter{T}" /> class with no initial value.
    /// </summary>
    public StringValueTypeXmlConverter() { }

    /// <summary>
    ///  Initializes a new instance of the <see cref="StringValueTypeXmlConverter{T}" /> class with the specified value.
    /// </summary>
    /// <param name="value">The initial value to wrap for serialization.</param>
    public StringValueTypeXmlConverter( T value ) => Value = value;

    /// <summary>
    ///  Gets or sets the value being serialized or deserialized.
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    ///  Returns the XML schema for this object. This method is reserved and should return <see langword="null" />.
    /// </summary>
    /// <returns>Always <see langword="null" />.</returns>
    public XmlSchema? GetSchema() => null;

    /// <summary>
    ///  Deserializes the object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> positioned at the element to read.</param>
    /// <exception cref="MissingMethodException">
    ///  Thrown if <typeparamref name="T" /> does not expose a public constructor that accepts a single <see
    ///  cref="string" /> parameter.
    /// </exception>
    public void ReadXml( XmlReader reader )
    {
        string str = reader.ReadElementContentAsString();
        Value = (T?)Activator.CreateInstance(typeof(T), str);
    }

    /// <summary>
    ///  Serializes the object into its XML representation.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> used to write the XML content.</param>
    /// <remarks>
    ///  Writes the underlying string value if available; otherwise writes an empty string.
    /// </remarks>
    public void WriteXml( XmlWriter writer ) => writer.WriteString(Value?.Value ?? string.Empty);

}
