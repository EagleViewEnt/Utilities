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

namespace MgbUtilities.Core.Tests.Extensions
{

    public class ObjectMappingExtensionsTests
    {

        [Fact]
        public void AsTests()
        {

            // Arrange
            TestRecord obj1 = new TestRecord
            {
                StringProperty = "Test"
                , IntegerProperty = 25
                , DecimalProperty = 50.33M
                , ListProperty = { 1, 2, 3, 4, 5 }
            };

            // Act
            TestRecord2 result = obj1.As<TestRecord2>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(obj1.StringProperty, result.StringProperty);
            Assert.Equal(obj1.IntegerProperty, result.IntegerProperty);
            Assert.Equal(obj1.DecimalProperty, result.DecimalProperty);
            Assert.Equal(obj1.ListProperty, result.ListProperty);
            Assert.Equal(obj1.ListProperty.Count, result.ListProperty.Count);

        }

        [Fact]
        public void GetChecksumTests()
        {
            //// Arrange
            //var objectMappingExtensions = new ObjectMappingExtensions();
            //object obj = null;

            //// Act
            //var result = objectMappingExtensions.GetChecksum(
            //    obj);

            //// Assert
            //Assert.True(false);
        }

        [Fact]
        public void MapFromTests()
        {
            //// Arrange
            //var objectMappingExtensions = new ObjectMappingExtensions();
            //object destination = null;
            //object source = null;

            //// Act
            //objectMappingExtensions.MapFrom(
            //    destination,
            //    source);

            //// Assert
            //Assert.True(false);
        }

        record TestRecord
        {
            public decimal DecimalProperty { get; set; }

            public int IntegerProperty { get; set; }

            public List<int> ListProperty { get; set; } = new();

            public string StringProperty { get; set; } = string.Empty;
        }
        record TestRecord2
        {
            public decimal DecimalProperty { get; set; }

            public int IntegerProperty { get; set; }

            public List<int> ListProperty { get; set; } = new();

            public string StringProperty { get; set; } = string.Empty;
        }

    }

}
