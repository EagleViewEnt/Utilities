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

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  Provides XML serialization support for a monetary value consisting of an amount and a language/culture identifier.
/// </summary>
public class MoneyXmlConverter : IXmlSerializable
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="MoneyXmlConverter" /> class.
    /// </summary>
    public MoneyXmlConverter() { }

    /// <summary>
    ///  Initializes a new instance of the <see cref="MoneyXmlConverter" /> class with the specified amount and
    ///  language.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="language">The language or culture identifier (for example, "en-US"). Defaults to "en-US".</param>
    public MoneyXmlConverter( decimal amount, string language = "en-US" )
    {
        Amount = amount;
        Language = language;
    }

    /// <summary>
    ///  Gets or sets the monetary amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///  Gets or sets the language or culture identifier used in conjunction with the monetary value.
    /// </summary>
    public string Language { get; set; } = "en-US";

    /// <summary>
    ///  This method is reserved and should return <see langword="null" />.
    /// </summary>
    /// <returns>Always returns <see langword="null" />.</returns>
    public XmlSchema? GetSchema() => null;

    /// <summary>
    ///  Generates the object from its XML representation.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> positioned at the element to read from.</param>
    public void ReadXml( XmlReader reader )
    {
        reader.MoveToContent();
        Amount = decimal.Parse(reader.GetAttribute("Amount") ?? "0");
        Language = reader.GetAttribute("Language") ?? "en-US";
        reader.Read();
    }

    /// <summary>
    ///  Converts the object into its XML representation.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> used to write the XML.</param>
    public void WriteXml( XmlWriter writer )
    {
        writer.WriteAttributeString("Amount", Amount.ToString());
        writer.WriteAttributeString("Language", Language);
    }

}
