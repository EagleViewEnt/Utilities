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

using MgbUtilties.Platform.All.Core.Types.Checks;

namespace MgbUtilities.Core.Tests.Types.Checks;

public class MicrDataTests
{

    public readonly string micrRawValid = $"@{routing}@{account}+{check}";
    public const string routing = "125000024";
    public const string account = "123454321";
    public const string check = "12345";
    public const string micrRawNoCheck = $"@{routing}@{account}";

    [Fact]
    public void Constructor_WithMissingCheckNumber_ParsesFieldsCorrectly()
    {
        Micr micr = new Micr(micrRawNoCheck);

        Assert.False(micr.IsEmpty);
        Assert.True(micr.IsValid);
        Assert.Equal(routing, micr.RoutingNumber);
        Assert.Equal(account, micr.AccountNumber);
        Assert.True(micr.CheckNumber.IsEmpty);
    }

    [Fact]
    public void Constructor_WithNullRaw_SetsPropertiesToEmpty()
    {
        Micr micr = new Micr(null);

        Assert.True(micr.IsEmpty);
        Assert.False(micr.IsValid);
        Assert.Equal(string.Empty, micr);
        Assert.True(micr.AccountNumber.IsEmpty);
        Assert.True(micr.RoutingNumber.IsEmpty);
        Assert.True(micr.CheckNumber.IsEmpty);
    }

    [Fact]
    public void Constructor_WithValidMicrRaw_ParsesFieldsCorrectly()
    {
        Micr micr = new Micr(micrRawValid);

        Assert.False(micr.IsEmpty);
        Assert.True(micr.IsValid);
        Assert.Equal(micrRawValid, micr);
        Assert.Equal(account, micr.AccountNumber);
        Assert.Equal(routing, micr.RoutingNumber);
        Assert.Equal(check, micr.CheckNumber);
    }

    [Fact]
    public void ImplicitConversion_FromString_Works()
    {
        Micr micr = micrRawValid;

        Assert.Equal(micrRawValid, micr);
    }

    [Fact]
    public void ImplicitConversion_ToString_Works()
    {
        Micr micr = new Micr(micrRawValid);

        string raw = micr;

        Assert.Equal(micrRawValid, raw);
    }

    [Fact]
    public void ToString_ReturnsRaw()
    {
        Micr micr = new Micr(micrRawValid);

        Assert.Equal(micrRawValid, micr.ToString());
    }

}
