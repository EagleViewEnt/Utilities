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

public class AccountNumberSecuredTests
{

    [Fact]
    public void EmptyAccountNumber_IsEmpty()
    {
        AccountNumberSecured acc = new AccountNumberSecured(string.Empty);
        Assert.True(acc.IsEmpty);
    }

    [Fact]
    public void ImplicitConversion_FromString_Works()
    {
        AccountNumberSecured acc = "123456789";
        Assert.Equal("****6789", acc);
    }

    [Fact]
    public void ImplicitConversion_ToString_Works()
    {
        AccountNumberSecured acc = new AccountNumberSecured("123456789");
        string str = acc;
        Assert.Equal("****6789", str);
    }

    [Fact]
    public void InvalidAccountNumber_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AccountNumberSecured("abc")); // must be numeric
        Assert.Throws<ArgumentException>(() => new AccountNumberSecured("123")); // Too short
    }

    [Fact]
    public void JsonSerialization_SecuredString()
    {
        AccountNumberSecured acc = new AccountNumberSecured("123456789");
        string json = JsonSerializer.Serialize(acc);
        Assert.Contains("***6789", json); // Default mask: last 4 visible
    }

    [Fact]
    public void ValidAccountNumber_ConstructsSuccessfully()
    {
        AccountNumberSecured acc = new AccountNumberSecured("123456789");
        Assert.Equal("****6789", acc.ToString());
        Assert.False(acc.IsEmpty);
    }

    //[Fact]
    //public void XmlSerialization_SecuredString()
    //{
    //    AccountNumberSecured acc = new AccountNumberSecured("123456789");
    //    XmlSerializer serializer = new XmlSerializer(typeof(AccountNumberSecured));
    //    using StringWriter sw = new StringWriter();
    //    serializer.Serialize(sw, acc);
    //    string xml = sw.ToString();
    //    Assert.Contains("***6789", xml); // Default mask: last 4 visible
    //}

}
