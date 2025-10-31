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
using System.Reflection;

using MgbUtilties.Core.Types.FilePath;
using MgbUtilties.Core.Types.FilePath.Extensions;
using MgbUtilties.Core.Types.JsonString;
using MgbUtilties.Core.Types.JsonString.Extensions;

namespace MgbUtilities.Core.Tests.Types.FilePath;

// Shared test fixture for MgbFilePath tests
public class MgbFilePathTestFixture
{

    public readonly string ValidPath;

    public MgbFilePathTestFixture() => ValidPath = @"C:\Temp\file.txt";

    public override bool Equals( object? obj )
        => (obj is MgbFilePathTestFixture other) && (ValidPath == other.ValidPath);

    public override int GetHashCode() => ValidPath.GetHashCode();

}

public class MgbFilePathTests
{

    readonly string _validFile = Assembly.GetExecutingAssembly().Location;
    readonly string _upperCaseFile;
    readonly string _badFile = "C:<>\\bad|file.txt";

    public MgbFilePathTests() => _upperCaseFile = _validFile.ToUpper();

    [Fact]
    public void Constructor_EmptyString_IsValid()
    {
        MgbFilePath filePath = new MgbFilePath(string.Empty);
        Assert.Equal(string.Empty, filePath);
        Assert.True(filePath.IsEmpty);
    }

    [Fact]
    public void Constructor_InvalidPath_ThrowsArgumentException()
        => Assert.Throws<ArgumentException>(() => new MgbFilePath(_badFile));

    [Fact]
    public void Constructor_ValidPath_SetsValue()
    {
        MgbFilePath filePath = new MgbFilePath(_validFile);
        Assert.True(filePath.Equals(_validFile));
    }

    [Fact]
    public void Equality_Operators_Work_CaseInsensitive()
    {
        MgbFilePath path = _validFile;
        Assert.True(path == _upperCaseFile);
        Assert.True(path.Equals(_upperCaseFile));
    }

    [Fact]
    public void Equals_Object_And_HashCode_CaseInsensitive()
    {
        MgbFilePath path1 = _validFile;
        MgbFilePath path2 = _validFile;
        Assert.True(path1.Equals(path2));
        Assert.Equal(path1.GetHashCode(), path2.GetHashCode());
    }

    [Fact]
    public void ImplicitOperator_MgbFilePathToString_Works()
    {
        MgbFilePath filePath = new MgbFilePath(_validFile);
        string path = filePath; // Implicit conversion to string
        Assert.True(filePath.Equals(path));
    }

    [Fact]
    public void ImplicitOperator_StringToMgbFilePath_AndBack()
    {
        MgbFilePath filePath = _validFile;
        string result = filePath;
        Assert.True(filePath.Equals(result));
    }

    [Fact]
    public void ImplicitOperator_StringToMgbFilePath_Works()
    {
        MgbFilePath filePath = _validFile; // Implicit conversion from string
        Assert.True(filePath.Equals(_validFile));
        Assert.True(filePath.FileExists());
    }

    [Fact]
    public void ReservedNames_AreInvalid()
    {
        foreach(string reserved in new[]
        {
            "CON"
            , "PRN"
            , "AUX"
            , "NUL"
            , "COM1"
            , "LPT1"
        }) {
            string path = $@"C:\Temp\{reserved}";
            Assert.Throws<ArgumentException>(() => new MgbFilePath(path));
        }
    }

    [Fact]
    public void Serialization_Deserialization_Tests()
    {

        // Arrange  
        MgbFilePathTestFixture original = new();

        // Act
        MgbJson json = original.AsJson();

        MgbFilePathTestFixture? filePath = json.As<MgbFilePathTestFixture>();

        // Assert
        Assert.Equal<MgbFilePathTestFixture>(original, filePath);

    }

}