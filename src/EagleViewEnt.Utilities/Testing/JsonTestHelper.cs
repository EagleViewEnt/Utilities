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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Xunit;

namespace EagleViewEnt.Utilities.Testing;

public static class JsonTestHelper
{

    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

        // Converters = { new JsonStringEnumConverter() }
    };

    static void CompareElements
        ( JsonElement a, JsonElement b, string path, List<string> diffs, int maxDifferences )
    {
        if(diffs.Count >= maxDifferences)
            return;

        if(a.ValueKind != b.ValueKind) {
            diffs.Add($"{path}: type mismatch {a.ValueKind} vs {b.ValueKind}");
            return;
        }

        switch(a.ValueKind) {
            case JsonValueKind.Object:
                var aProps = a.EnumerateObject().ToDictionary(p => p.Name, p => p.Value, StringComparer.Ordinal);
                var bProps = b.EnumerateObject().ToDictionary(p => p.Name, p => p.Value, StringComparer.Ordinal);

                foreach(var name in aProps.Keys.Except(bProps.Keys, StringComparer.Ordinal)) {
                    if(diffs.Count >= maxDifferences)
                        return;
                    diffs.Add($"{path}: missing property '{name}' on right");
                }
                foreach(var name in bProps.Keys.Except(aProps.Keys, StringComparer.Ordinal)) {
                    if(diffs.Count >= maxDifferences)
                        return;
                    diffs.Add($"{path}: unexpected property '{name}' on right");
                }

                foreach(var name in aProps.Keys.Intersect(bProps.Keys, StringComparer.Ordinal)) {
                    if(diffs.Count >= maxDifferences)
                        return;
                    var childPath = $"{path}.{name}";
                    CompareElements(aProps[name], bProps[name], childPath, diffs, maxDifferences);
                }
                break;

            case JsonValueKind.Array:
                var aArr = a.EnumerateArray().ToArray();
                var bArr = b.EnumerateArray().ToArray();

                if(aArr.Length != bArr.Length) {
                    diffs.Add($"{path}: array length {aArr.Length} vs {bArr.Length}");
                    if(diffs.Count >= maxDifferences)
                        return;
                }

                var len = Math.Min(aArr.Length, bArr.Length);
                for(int i = 0; (i < len) && (diffs.Count < maxDifferences); i++)
                    CompareElements(aArr[i], bArr[i], $"{path}[{i}]", diffs, maxDifferences);
                break;

            case JsonValueKind.String: {
                var av = a.GetString();
                var bv = b.GetString();
                if(!StringComparer.Ordinal.Equals(av, bv))
                    diffs.Add($"{path}: string '{av}' vs '{bv}'");
                break;
            }

            case JsonValueKind.Number:
                if(a.TryGetDecimal(out var ad) && b.TryGetDecimal(out var bd)) {
                    if(ad != bd)
                        diffs.Add($"{path}: number {a.GetRawText()} vs {b.GetRawText()}");
                } else {
                    var adbl = a.GetDouble();
                    var bdbl = b.GetDouble();
                    if(adbl != bdbl)
                        diffs.Add($"{path}: number {a.GetRawText()} vs {b.GetRawText()}");
                }
                break;

            case JsonValueKind.True:
            case JsonValueKind.False: {
                var av = a.GetBoolean();
                var bv = b.GetBoolean();
                if(av != bv)
                    diffs.Add($"{path}: boolean {av} vs {bv}");
                break;
            }

            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                break;

            default:
                if(!StringComparer.Ordinal.Equals(a.GetRawText(), b.GetRawText()))
                    diffs.Add($"{path}: value differs {a.GetRawText()} vs {b.GetRawText()}");
                break;
        }
    }

    // Enumerate files under TestData/<area> matching <modelPrefix>*.json
    public static IEnumerable<string> EnumerateModelJson( string path, string modelPrefix )
    {

        if(!Directory.Exists(path))
            yield break;

        foreach(var file in Directory.EnumerateFiles(path, $"{modelPrefix}*.json", SearchOption.TopDirectoryOnly))
            yield return Path.GetFileName(file);
    }

    public static bool JsonDeepEquals( string left, string right ) => JsonElement.DeepEquals(Parse(left), Parse(right));

    public static JsonElement Parse( string json ) => JsonDocument.Parse(json).RootElement.Clone();

    public static string ReadTestJson( string filePath )
    {
        if(!File.Exists(filePath)) throw new FileNotFoundException($"Test JSON not found at: {filePath}");
        return File.ReadAllText(filePath);
    }

    // MemberData provider for ResponseStandard samples: yields "Responses/<fileName>"
    public static TheoryData<string> TestFiles( string relativePath, string modelPrefix )
    {
        string testDataPath = Path.Combine(AppContext.BaseDirectory, relativePath);
        var data = new TheoryData<string>();
        foreach(var file in JsonTestHelper.EnumerateModelJson(testDataPath, modelPrefix))
            data.Add(Path.Combine(testDataPath, file));
        return data;
    }

    public static bool TryJsonDeepEquals
        ( string left, string right, out string diff, int maxDifferences = 20 )
    {
        var a = Parse(left);
        var b = Parse(right);
        var diffs = new List<string>(capacity: maxDifferences);

        CompareElements(a, b, "$", diffs, maxDifferences);

        if(diffs.Count == 0) {
            diff = string.Empty;
            return true;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Found {diffs.Count} difference(s) (showing up to {maxDifferences}):");
        foreach(var d in diffs)
            sb.AppendLine($" - {d}");

        diff = sb.ToString();
        return false;
    }

}

