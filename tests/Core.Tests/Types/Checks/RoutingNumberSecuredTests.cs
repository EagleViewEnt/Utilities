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

using System.Text.Json;

using MgbUtilties.Core.Types.Checks;

namespace MgbUtilities.Core.Tests.Types.Checks;

public class RoutingNumberSecuredTests
{

    const string validRoutingNumber = "021000021";
    const string validRoutingNumberWithMask = "****0021";
    const string badChecksumRoutingNumber = "123456789";
    const string toShortRoutingNumber = "123456";
    const string nonNumericRoutingNumber = "Abc";

    [Fact]
    public void EmptyRoutingNumber_IsEmpty()
    {
        RoutingNumberSecured routingNumber = new RoutingNumberSecured(string.Empty);
        Assert.True(routingNumber.IsEmpty);
    }

    [Fact]
    public void ImplicitConversion_FromString_Works()
    {
        RoutingNumberSecured routing = validRoutingNumber;
        Assert.Equal(validRoutingNumberWithMask, routing);
    }

    [Fact]
    public void ImplicitConversion_ToString_Works()
    {
        RoutingNumberSecured routing = new RoutingNumberSecured(validRoutingNumber);
        string str = routing;
        Assert.Equal(validRoutingNumberWithMask, str);
    }

    [Fact]
    public void InvalidRoutingNumber_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new RoutingNumberSecured(nonNumericRoutingNumber)); // Non-numeric
        Assert.Throws<ArgumentException>(() => new RoutingNumberSecured(toShortRoutingNumber)); // Too short
        Assert.Throws<ArgumentException>(() => new RoutingNumberSecured(badChecksumRoutingNumber)); // Checksum failure
    }

    [Fact]
    public void JsonSerialization_SecuredString()
    {
        RoutingNumberSecured routing = new RoutingNumberSecured(validRoutingNumber);
        string json = JsonSerializer.Serialize(routing);
        Assert.Contains(validRoutingNumberWithMask, json); // Default mask: last 4 visible
    }

    [Fact]
    public void ValidRoutingNumber_ConstructsSuccessfully()
    {
        RoutingNumberSecured routing = new RoutingNumberSecured(validRoutingNumber);
        Assert.Equal(validRoutingNumberWithMask, routing.ToString());
        Assert.False(routing.IsEmpty);
    }

    //[Fact]
    //public void XmlSerialization_SecuredString()
    //{
    //    RoutingNumberSecured routingNumber = new RoutingNumberSecured(validRoutingNumber);
    //    XmlSerializer serializer = new XmlSerializer(typeof(RoutingNumberSecured));
    //    using StringWriter sw = new StringWriter();
    //    serializer.Serialize(sw, routingNumber);
    //    string xml = sw.ToString();
    //    Assert.Contains("***6789", xml); // Default mask: last 4 visible
    //}

}