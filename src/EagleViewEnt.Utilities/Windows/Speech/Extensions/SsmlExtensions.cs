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
using System.Text.RegularExpressions;

namespace MgbUtilities.Windows.Speech.Extensions;

/// <summary>
///  Provides extension methods for converting text to SSML (Speech Synthesis Markup Language) format.
/// </summary>
/// <remarks>
///  This class includes methods that transform plain text into SSML, which can be used for text-to-speech applications.
///  The methods handle punctuation and insert appropriate SSML tags to enhance speech synthesis.
/// </remarks>
public static partial class SsmlExtensions
{

    [GeneratedRegex("[.,!?]")]
    internal static partial Regex BreakInsertionRegex();

    [GeneratedRegex("[.,!?]")]
    internal static partial Regex PunctuationStripRegex();

    /// <summary>
    ///  Converts the specified text into an SSML (Speech Synthesis Markup Language) format.
    /// </summary>
    /// <remarks>
    ///  The method inserts a short break of 25 milliseconds between certain elements in the text to improve speech
    ///  synthesis clarity. It also removes punctuation from the input text before generating the SSML output.
    /// </remarks>
    /// <param name="value">The text to be converted into SSML.</param>
    /// <param name="language">The language code to be used in the SSML output, specified as a BCP-47 language tag.</param>
    /// <returns>A string containing the SSML representation of the input text.</returns>
    public static string ToTextSsml( this string value, string language )
    {
        const string BreakTag = "<break time=\"25ms\"/>";
        StringBuilder builder = new StringBuilder();
        builder.Append($"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"{language}\">");

        value = BreakInsertionRegex().Replace(value, BreakTag);
        builder.Append(PunctuationStripRegex().Replace(value, string.Empty));

        builder.Append("</speak>");
        return builder.ToString();
    }

}
