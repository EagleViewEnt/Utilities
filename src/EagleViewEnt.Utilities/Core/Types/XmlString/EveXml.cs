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

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EagleViewEnt.Utilities.Core.Types.XmlString;

/// <summary>
///  Represents an immutable XML string value with convenience helpers for validation and conversions. The input string
///  is trimmed during construction.
/// </summary>
public record EveXml
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveXml" /> record with the provided string.
    /// </summary>
    /// <param name="value">
    ///  The XML string value to wrap. If <see langword="null" />, an empty string is used. The value is trimmed.
    /// </param>
    public EveXml( string? value )
    {
        Value = value?.Trim() ?? string.Empty;
        IsValid = IsValidXml(Value);
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveXml" /> record with an empty string.
    /// </summary>
    EveXml() : this(string.Empty) { }

    /// <summary>
    ///  Gets a value indicating whether the current instance is empty.
    /// </summary>
    [JsonIgnore, XmlIgnore, NotMapped]
    public bool IsEmpty => string.IsNullOrEmpty(Value);

    /// <summary>
    ///  Gets a value indicating whether the underlying <see cref="Value" /> contains well-formed XML. This value is
    ///  computed once during construction using <see cref="IsValidXml(string)" /> and then cached.
    /// </summary>
    [JsonIgnore, XmlIgnore, NotMapped]
    public bool IsValid { get; }

    /// <summary>
    ///  Gets the underlying XML string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///  Implicitly converts an <see cref="EveXml" /> to its underlying string value.
    /// </summary>
    /// <param name="xmlString">The <see cref="EveXml" /> instance to convert.</param>
    /// <returns>The underlying XML string.</returns>
    public static implicit operator string( EveXml xmlString ) => xmlString.Value;

    /// <summary>
    ///  Implicitly converts a string to an <see cref="EveXml" /> instance.
    /// </summary>
    /// <param name="value">The string to wrap.</param>
    /// <returns>A new <see cref="EveXml" /> instance containing the provided string.</returns>
    public static implicit operator EveXml( string value ) => new(value);

    /// <summary>
    ///  Determines whether the specified <see cref="EveXml" /> and string have the same underlying value.
    /// </summary>
    /// <param name="xmlString">The <see cref="EveXml" /> instance to compare.</param>
    /// <param name="value">The string to compare to.</param>
    /// <returns><see langword="true" /> if the values are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==( EveXml xmlString, string value ) => string.Compare(xmlString?.Value, value) == 0;

    /// <summary>
    ///  Determines whether the specified <see cref="EveXml" /> and string have different underlying values.
    /// </summary>
    /// <param name="xmlString">The <see cref="EveXml" /> instance to compare.</param>
    /// <param name="value">The string to compare to.</param>
    /// <returns><see langword="true" /> if the values are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=( EveXml xmlString, string value ) => !(string.Compare(xmlString?.Value, value) == 0);

    /// <summary>
    ///  Determines whether the specified string contains well-formed XML.
    /// </summary>
    /// <param name="value">The XML string to validate.</param>
    /// <returns><see langword="true" /> if the string is well-formed XML; otherwise, <see langword="false" />.</returns>
    static bool IsValidXml( string value )
    {
        try {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(value);
            return true;
        } catch {
            return false;
        }
    }

}
