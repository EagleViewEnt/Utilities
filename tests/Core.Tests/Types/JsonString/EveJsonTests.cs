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

using MgbUtilties.Core.Types.JsonString;
using MgbUtilties.Core.Types.JsonString.Extensions;

namespace MgbUtilities.Core.Tests.Types.JsonString
{

    public class MgbJsonTests
    {

        [Fact]
        public void Constructor_EmptyString_SetsValueAndIsValidFalse()
        {

            // Arrange
            string json = string.Empty;

            // Act
            MgbJson mgbJson = json;

            // Assert
            Assert.Equal(string.Empty, mgbJson.Value);
            Assert.False(mgbJson.IsValid);

            // Arrange
            string? nullJson = null;
            mgbJson = nullJson;

            // Act & Assert
            Assert.Equal(string.Empty, mgbJson.Value);
            Assert.False(mgbJson.IsValid);
        }

        [Fact]
        public void Constructor_InvalidJson()
        {

            // Arrange
            MgbJson invalidJson = "{not valid json}";

            // Act & Assert
            Assert.False(invalidJson.IsValid);
        }

        [Fact]
        public void Constructor_ValidJson_SetsValueAndIsValid()
        {

            // Arrange
            string json = "{\"key\":\"value\"}";

            // Act
            MgbJson mgbJson = new MgbJson(json);

            // Assert
            Assert.Equal(json, mgbJson.Value);
            Assert.True(mgbJson.IsValid);
        }

        [Fact]
        public void Equality_Operators_WorkAsExpected()
        {

            // Arrange
            MgbJson json1 = new MgbJson("{\"x\":1}");
            MgbJson json2 = new MgbJson("{\"x\":1}");
            MgbJson json3 = new MgbJson("{\"x\":2}");

            // Act & Assert
            Assert.True(json1 == json2);
            Assert.False(json1 != json2);
            Assert.False(json1 == json3);
            Assert.True(json1 != json3);
        }

        [Fact]
        public void Equals_ObjectAndString_WorkAsExpected()
        {

            // Arrange
            MgbJson json = new MgbJson("{\"y\":3}");

            // Act & Assert
            Assert.True(json.Equals("{\"y\":3}"));
            Assert.False(json.Equals("{\"y\":4}"));
            Assert.True(json.Equals((object)new MgbJson("{\"y\":3}")));
            Assert.False(json.Equals((object)new MgbJson("{\"y\":4}")));
        }

        [Fact]
        public void ImplicitConversion_FromString_CreatesMgbJson()
        {

            // Arrange
            string json = "{\"b\":2}";

            // Act
            MgbJson mgbJson = json;

            // Assert
            Assert.Equal(json, mgbJson.Value);
            Assert.True(mgbJson.IsValid);
        }

        [Fact]
        public void ImplicitConversion_ToString_ReturnsValue()
        {

            // Arrange
            string json = "{\"a\":1}";
            MgbJson mgbJson = new MgbJson(json);

            // Act
            string result = mgbJson;

            // Assert
            Assert.Equal(json, result);
        }

        [Fact]
        public void Serialization_Deserialization_Test()
        {
            TestRecord obj = new TestRecord();

            MgbJson json = obj.AsJson();

            // Assert
            Assert.True(json.IsValid);

            // Act
            MgbJson obj2 = json.As<MgbJson>();

            // Assert
            Assert.Equal<object>(json, obj2);

        }

        record TestRecord( int a = 1, int b = 2 );

    }

}