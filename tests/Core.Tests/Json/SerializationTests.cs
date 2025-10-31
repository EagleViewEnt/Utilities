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
using System.Linq;

using MgbUtilities.Json;

namespace MgbUtilities.Core.Tests.Json;

public class SerializationTests
{

    [Fact]
    public void FromJsonFileTest()
    {
        string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\TestData", "configsettings.json");
        ConfigSettings? settings = Serialization.FromJsonFile<ConfigSettings>(jsonFile);

        Assert.NotNull(settings);
    }

    [Fact]
    public void FromJsonTest()
    {
        string jsonFile = @"{""RequestUrl"":""http://localhost:7171/MockDataApi"",""Polling"":15,""OBSAddress"":""ws://127.0.0.1:4455"",""OBSSceneName"":""Main"",""OBSMapping"":[{""SourceName"":""Group"",""MinTrigger"":0,""SceneDuration"":5,""Exact"":false},{""SourceName"":""Hal9000"",""MinTrigger"":200,""SceneDuration"":5,""Exact"":false},{""SourceName"":""My Brains"",""MinTrigger"":300,""SceneDuration"":5,""Exact"":false},{""SourceName"":""Mostly Dead"",""MinTrigger"":400,""SceneDuration"":5,""Exact"":false},{""SourceName"":""Inconceivable"",""MinTrigger"":111,""SceneDuration"":5,""Exact"":true},{""SourceName"":""Countdown"",""MinTrigger"":222,""SceneDuration"":5,""Exact"":true}]}";

        ConfigSettings? settings = Serialization.FromJson<ConfigSettings>(jsonFile);

        Assert.NotNull(settings);
    }

    [Fact]
    public void ToJsonTest()
    {
        string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\TestData", "configsettings.json");
        string json = File.ReadAllText(jsonFile);
        ConfigSettings? configSettings = new ConfigSettings().Init(jsonFile);

        Assert.NotNull(configSettings);

        string result = Serialization.ToJson(configSettings);

        Assert.NotNull(result);
        Assert.Equal(json, result);
    }

}