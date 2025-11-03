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

using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Types.FilePath;
using EagleViewEnt.Utilities.Core.Types.XmlString;

namespace EagleViewEnt.Utilities.Serialization.Xml;

/// <summary>
///  Provides extension-style helper methods for XML serialization and deserialization using <see cref="XmlSerializer"
///  />. These utilities work with the project's <see cref="EveXml" /> and <see cref="EveFilePath" /> value types.
/// </summary>
public static class XmlSerializerExtensions
{

    /// <summary>
    ///  Deserializes the provided XML string into an instance of <typeparamref name="T" /> using <see
    ///  cref="XmlSerializer" />.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize into.</typeparam>
    /// <param name="xml">The XML content to deserialize, wrapped in an <see cref="EveXml" /> value object.</param>
    /// <returns>
    ///  An instance of <typeparamref name="T" /> when deserialization succeeds; otherwise, <see langword="default" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///  Thrown when <paramref name="xml" /> is null or empty.
    /// </exception>
    /// <exception cref="SerializationException">
    ///  Thrown when deserialization fails for any reason. The inner exception, if present, contains the original cause.
    /// </exception>
    public static T? FromXml<T>( EveXml xml ) //where T : ISerializable
    {
        if(string.IsNullOrEmpty(xml))
            throw new ArgumentNullException(nameof(xml), "Json string required.");
        try {
            object? result = null;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using StringReader reader = new(xml);
            result = serializer.Deserialize(reader);
            return (result is not null) ? ((T)result) : default;
        } catch(Exception ex) {
            throw new SerializationException($"FromXml: Unable to deserialize from xml string. Type: {typeof(T).Name} Stack: {ex.StackTrace}", ex.InnerException);
        }
    }

    /// <summary>
    ///  Reads XML from the specified file path and deserializes it into an instance of <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize into.</typeparam>
    /// <param name="fullPath">The full path to the XML file, wrapped in an <see cref="EveFilePath" />.</param>
    /// <returns>
    ///  An instance of <typeparamref name="T" /> when deserialization succeeds; otherwise, <see langword="default" />.
    /// </returns>
    /// <remarks>
    ///  If the provided path is null, empty, or does not exist, an empty string is passed to <see
    ///  cref="FromXml{T}(EveXml)" />, which will result in an <see cref="ArgumentNullException" /> being thrown.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///  Thrown by <see cref="FromXml{T}(EveXml)" /> when the file content is empty or no path is provided.
    /// </exception>
    /// <exception cref="SerializationException">
    ///  Thrown by <see cref="FromXml{T}(EveXml)" /> when deserialization fails.
    /// </exception>
    public static T? FromXmlFile<T>( EveFilePath fullPath ) //where T : ISerializable
    {
        var json = (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            ? string.Empty
            : File.ReadAllText(fullPath);
        return FromXml<T>(json);
    }

    /// <summary>
    ///  Serializes the specified object instance to an XML string using <see cref="XmlSerializer" />.
    /// </summary>
    /// <param name="obj">The object instance to serialize.</param>
    /// <param name="omitXmlDeclaration">
    ///  When <see langword="true" />, omits the XML declaration (for example, <c>&lt;?xml version="1.0" encoding="utf-
    ///  8"?&gt;</c>). Defaults to <see langword="true" />.
    /// </param>
    /// <returns>An <see cref="EveXml" /> containing the serialized XML representation of <paramref name="obj" />.</returns>
    /// <remarks>
    ///  The serializer is configured to: - Indent output for readability. - Use UTF-8 encoding without BOM. - Suppress
    ///  namespace prefixes by emitting empty namespaces.
    /// </remarks>
    public static EveXml ToXml( this object obj
                               , bool omitXmlDeclaration = true )
    {
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = omitXmlDeclaration,
            Encoding = new UTF8Encoding(false)
        };
        XmlSerializerNamespaces emptyNameSpaces = new XmlSerializerNamespaces();
        emptyNameSpaces.Add(string.Empty, string.Empty);
        using StringWriter writer = new StringWriter();
        using XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
        serializer.Serialize(xmlWriter, obj, emptyNameSpaces);
        return writer.ToString();
    }

}

