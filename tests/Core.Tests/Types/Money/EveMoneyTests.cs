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
using MgbUtilties.Core.Types.JsonString;
using MgbUtilties.Core.Types.JsonString.Extensions;
using MgbUtilties.Core.Types.Money;
using MgbUtilties.Core.Types.Money.Enum;

namespace MgbUtilities.Core.Tests.Types.Money
{

    public class MgbMoneyTests
    {

        [Fact]
        public void Addition_And_Subtraction_SameCurrency()
        {
            MgbMoney m1 = new MgbMoney(10m, MgbCurrency.USD);
            MgbMoney m2 = new MgbMoney(5m, MgbCurrency.USD);

            Assert.Equal(new MgbMoney(15m, MgbCurrency.USD), m1 + m2);
            Assert.Equal(new MgbMoney(5m, MgbCurrency.USD), m1 - m2);
        }

        [Fact]
        public void Addition_DifferentCurrency_Throws()
        {
            MgbMoney m1 = new MgbMoney(10m, MgbCurrency.USD);
            MgbMoney m2 = new MgbMoney(5m, MgbCurrency.EUR);

            Assert.Throws<InvalidOperationException>(() => { MgbMoney _ = m1 + m2; });
        }

        [Fact]
        public void CompareTo_ThrowsOnDifferentCurrency()
        {
            MgbMoney m1 = new MgbMoney(10m, MgbCurrency.USD);
            MgbMoney m2 = new MgbMoney(10m, MgbCurrency.EUR);

            Assert.Throws<InvalidOperationException>(() => m1.CompareTo(m2));
        }

        [Fact]
        public void Constructor_SetsValueAndCurrency()
        {
            decimal value = 100.25m;
            MgbCurrency currency = MgbCurrency.USD;

            MgbMoney money = new MgbMoney(value, currency);

            Assert.Equal(value, money.Value);
            Assert.Equal(currency, money.Currency);
        }

        [Fact]
        public void Division_By_MgbMoney_ReturnsDecimal()
        {
            MgbMoney m1 = new MgbMoney(20m, MgbCurrency.USD);
            MgbMoney m2 = new MgbMoney(5m, MgbCurrency.USD);

            Assert.Equal(4m, m1 / m2);
        }

        [Fact]
        public void ImplicitConversion_FromDecimal_DefaultsToUSD()
        {
            MgbMoney money = 50m;
            Assert.Equal(50m, money.Value);
            Assert.Equal(MgbCurrency.USD, money.Currency);
        }

        [Fact]
        public void Multiplication_And_Division()
        {
            MgbMoney m1 = new MgbMoney(20m, MgbCurrency.USD);

            Assert.Equal(new MgbMoney(40m, MgbCurrency.USD), m1 * 2m);
            Assert.Equal(new MgbMoney(10m, MgbCurrency.USD), m1 / 2m);
        }

        [Fact]
        public void Serialization_Deserialization_Test()
        {

            // Arrange  
            MgbMoney original = 123.45m;
            MgbMoney forien = new MgbMoney(123.45m, MgbCurrency.VND);
            decimal amount = 10.55m;

            // Act
            MgbJson json = original.AsJson();

            // Assert
            Assert.True(json.IsValid);

            // Arrange  
            MgbMoney? deserialized = json.As<MgbMoney>();

            // Assert
            Assert.Equal(original, deserialized);

            // Arrange
            MgbMoney mgbMoney = original + amount;
            MgbMoney total = 134m;

            // Assert
            Assert.Equal(total, mgbMoney);

            _ = Assert.Throws<InvalidOperationException>(() => original + forien);

            // Arrange
            mgbMoney = original - 23.45m;
            total = 100m;

            // Assert
            Assert.Equal(total, mgbMoney);

            // Arrange
            amount = 10.55999m;
            mgbMoney = amount;

            // Assert
            Assert.Equal(new MgbMoney(amount.ToBankersRounding(), MgbCurrency.USD), mgbMoney);

            // Arrange
            amount = 50m;
            mgbMoney = 25M;

            // Assert
            Assert.Equal<decimal>(amount.ToBankersRounding(), mgbMoney * 2);

            // Arrange
            amount = 25m;
            mgbMoney = 50M;

            // Assert
            Assert.Equal<decimal>(amount, mgbMoney / 2M);
        }

        [Fact]
        public void ToString_FormatsCorrectly()
        {
            MgbMoney m1 = new MgbMoney(12.34m, MgbCurrency.USD);
            Assert.Contains("12.34", m1.ToString());
        }

        [Fact]
        public void Zero_IsZeroValue()
        {
            Assert.True(MgbMoney.Zero.IsZero);
            Assert.Equal(0m, MgbMoney.Zero.Value);
        }

    }

}