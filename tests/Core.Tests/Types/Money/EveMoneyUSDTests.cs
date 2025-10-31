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
using System.Text.Json;

using MgbUtilties.Core.Types.Money;

namespace MgbUtilities.Core.Tests.Types.Money;

public class MgbMoneyUsdTests
{

    [Fact]
    public void Constructor_SetsValueAndCurrency()
    {
        MgbMoneyUsd money = new MgbMoneyUsd(123.45m);
        Assert.Equal(123.45m, money.Value);
    }

    [Fact]
    public void Equality_TwoSameValues_AreEqual()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(10m);
        MgbMoneyUsd m2 = new MgbMoneyUsd(10m);
        Assert.Equal(m1, m2);
    }

    [Fact]
    public void ImplicitConversion_FromDecimal_CreatesMgbMoneyUsd()
    {
        MgbMoneyUsd money = 50.25m;
        Assert.Equal(50.25m, money.Value);
    }

    [Fact]
    public void ImplicitConversion_ToDecimal_ReturnsValue()
    {
        MgbMoneyUsd money = new MgbMoneyUsd(77.77m);
        decimal value = money;
        Assert.Equal(77.77m, value);
    }

    [Fact]
    public void JsonSerialization_SerializesAndDeSerializesCorrectly()
    {
        MgbMoneyUsd money = new MgbMoneyUsd(99.99m);
        string json = JsonSerializer.Serialize(money);
        Assert.Contains("99.99", json);

        MgbMoneyUsd? deserialized = JsonSerializer.Deserialize<MgbMoneyUsd>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(99.99m, deserialized);
    }

    [Fact]
    public void Operator_Addition_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(7.50m);
        MgbMoneyUsd m2 = new MgbMoneyUsd(2.50m);

        MgbMoneyUsd sum = m1 + m2;
        Assert.Equal(new MgbMoneyUsd(10.00m), sum);
    }

    [Fact]
    public void Operator_Comparison_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(5.00m);
        MgbMoneyUsd m2 = new MgbMoneyUsd(10.00m);

        Assert.True(m1 < m2);
        Assert.True(m2 > m1);
        Assert.True(m1 <= m2);
        Assert.True(m2 >= m1);
    }

    [Fact]
    public void Operator_Divide_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(10.00m);
        decimal divisor = 2m;

        MgbMoneyUsd result = m1 / divisor;
        Assert.Equal(new MgbMoneyUsd(5.00m), result);
    }

    [Fact]
    public void Operator_Equality_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(5.00m);
        MgbMoneyUsd m2 = new MgbMoneyUsd(5.00m);
        MgbMoneyUsd m3 = new MgbMoneyUsd(10.00m);

        Assert.True(m1 == m2);
        Assert.False(m1 == m3);
        Assert.True(m1 != m3);
        Assert.False(m1 != m2);
    }

    [Fact]
    public void Operator_Multiply_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(4.00m);
        decimal factor = 2.5m;

        MgbMoneyUsd product = m1 * factor;
        Assert.Equal(new MgbMoneyUsd(10.00m), product);

        MgbMoneyUsd product2 = factor * m1;
        Assert.Equal(new MgbMoneyUsd(10.00m), product2);
    }

    [Fact]
    public void Operator_Subtraction_WorksAsExpected()
    {
        MgbMoneyUsd m1 = new MgbMoneyUsd(10.00m);
        MgbMoneyUsd m2 = new MgbMoneyUsd(3.25m);

        MgbMoneyUsd diff = m1 - m2;
        Assert.Equal(new MgbMoneyUsd(6.75m), diff);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        decimal amount = 12.34m;
        MgbMoneyUsd money = new MgbMoneyUsd(amount);
        string expected = $"{amount:C} USD";
        string actual = money.AsMgbMoney.ToString();
        Assert.Equal(amount.ToString("C"), money.ToString());
        Assert.Equal(expected, actual);
    }

}
