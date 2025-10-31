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

using MgbUtilties.Core.Extensions.Numeric;

namespace MgbUtilities.Core.Tests.Extensions.Numeric;

public class DecimalExtensionsTest
{

    [Fact]
    public void ToCurrencyString()
    {
        decimal test = 180.82908989898989m;
        decimal result = test.ToBankersRounding();
        Assert.Equal(result.ToString("C"), test.ToCurrencyString());
    }

    [Theory,
    InlineData(1.2345, 2, 1.23),
    InlineData(1.2355, 2, 1.24),
    InlineData(-1.2345, 2, -1.23),
    InlineData(-1.2355, 2, -1.24),
    InlineData(0.005, 2, 0.01),
    InlineData(-0.005, 2, -0.01),
    InlineData(0.004, 2, 0.00),
    InlineData(-0.004, 2, -0.00),
    InlineData(123.456, 0, 123),
    InlineData(123.556, 0, 124),
    InlineData(-123.456, 0, -123),
    InlineData(-123.556, 0, -124)]
    public void ToRoundedAwayFromZero
        ( decimal value, int decimals, decimal expected )
        => Assert.Equal(expected, value.ToRoundedAwayFromZero(decimals));

}