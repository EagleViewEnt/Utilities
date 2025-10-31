//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
// <Disclaimer>
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//  THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Converters;

/// <summary>
///  XML serialization wrapper for <see cref="EveFilePath" /> enabling correct read/write behavior with <see
///  cref="XmlSerializer" />.
/// </summary>
/// <remarks>
///  The wrapper serializes the wrapped <see cref="EveFilePath" /> as a simple element text value and reconstructs it
///  during deserialization.
/// </remarks>
public class EveFilePathXmlWrapper : IXmlSerializable
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveFilePathXmlWrapper" /> class. Required by the <see
    ///  cref="XmlSerializer" />.
    /// </summary>
    public EveFilePathXmlWrapper() { }

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveFilePathXmlWrapper" /> class with the specified value.
    /// </summary>
    /// <param name="value">The <see cref="EveFilePath" /> to wrap.</param>
    public EveFilePathXmlWrapper( EveFilePath value ) => Value = value;

    /// <summary>
    ///  Gets or sets the wrapped <see cref="EveFilePath" /> value.
    /// </summary>
    public EveFilePath Value { get; set; } = EveFilePath.Empty;

    /// <summary>
    ///  Returns the XML schema for the serialized data. Not used; returns <see langword="null" />.
    /// </summary>
    /// <returns>Always <see langword="null" /> to indicate no schema.</returns>
    public XmlSchema? GetSchema() => null;

    /// <summary>
    ///  Generates the object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> positioned at the element to read.</param>
    public void ReadXml( XmlReader reader )
    {
        string s = reader.ReadElementContentAsString();
        Value = new EveFilePath(s);
    }

    /// <summary>
    ///  Converts the object into its XML representation.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> used to write the XML.</param>
    public void WriteXml( XmlWriter writer ) => writer.WriteString(Value.ToString());

}
