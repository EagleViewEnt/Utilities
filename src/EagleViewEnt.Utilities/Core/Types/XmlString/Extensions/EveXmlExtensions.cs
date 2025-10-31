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
using EagleViewEnt.Utilities.Core.Types.FilePath.Extensions;

namespace EagleViewEnt.Utilities.Core.Types.XmlString.Extensions;

/// <summary>
///  Provides extension helpers for working with <see cref="EveXml" /> values and XML serialization, including
///  converting objects to XML and deserializing from XML strings or files.
/// </summary>
/// <remarks>
///  These helpers use <see cref="XmlSerializer" /> with UTF-8 (no BOM) and optional XML declaration omission.
/// </remarks>
public static class EveXmlExtensions
{

    /// <summary>
    ///  Deserializes the specified XML string into an object of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize. Must be serializable.</typeparam>
    /// <param name="xml">The XML string to deserialize. Cannot be null.</param>
    /// <returns>
    ///  An instance of type <typeparamref name="T" /> if deserialization is successful; otherwise, the default value of
    ///  <typeparamref name="T" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="xml" /> is null.</exception>
    /// <exception cref="SerializationException">Thrown if deserialization fails.</exception>
    public static T? As<T>( EveXml xml ) //where T : ISerializable
    {
        if(xml.IsValid != true)
          throw new ArgumentException("Valid Xml string required.");

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
    ///  Deserializes the contents of a file into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize. Must be compatible with the XML format.</typeparam>
    /// <param name="fullPath">The full path to the file containing the XML data. The file must exist.</param>
    /// <returns>
    ///  An instance of type <typeparamref name="T" /> populated with data from the file, or <see langword="null" /> if
    ///  deserialization fails.
    /// </returns>
    /// <exception cref="FileNotFoundException">Thrown when the file at <paramref name="fullPath" /> does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fullPath" /> is invalid.</exception>
    /// <exception cref="SerializationException">Thrown if the file contents cannot be deserialized into <typeparamref name="T" />.</exception>
    public static T? AsFromFile<T>( EveFilePath fullPath ) //where T : ISerializable
    {
        fullPath.ThrowIfNotFileExists();

        EveXml xml = File.ReadAllText(fullPath);

        return As<T>(xml);
    }

    /// <summary>
    ///  Converts the specified object to its XML representation.
    /// </summary>
    /// <remarks>
    ///  The method uses UTF-8 encoding without a byte order mark (BOM) and indents the XML for readability.
    /// </remarks>
    /// <param name="obj">The object to be serialized to XML. Must not be null.</param>
    /// <param name="omitXmlDeclaration">
    ///  A boolean value indicating whether to omit the XML declaration in the output.  <see langword="true" /> to omit
    ///  the declaration; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>A string containing the XML representation of the specified object.</returns>
    public static EveXml ToXml( object obj
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

