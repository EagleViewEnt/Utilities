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

namespace EagleViewEnt.Utilities.Core.Types.Unicode;

using System.Collections.Generic;
using System.Text;

/// <summary>
///  Provides extension methods for converting standard ASCII letters and digits into their Unicode Mathematical
///  Monospace forms.
/// </summary>
public static class UnicodeConverter
{

    static readonly Dictionary<char, string> MonospaceMapping = new()
    {
        { 'A', "𝙰" },
        { 'B', "𝙱" },
        { 'C', "𝙲" },
        { 'D', "𝙳" },
        { 'E', "𝙴" },
        { 'F', "𝙵" },
        { 'G', "𝙶" },
        { 'H', "𝙷" },
        { 'I', "𝙸" },
        { 'J', "𝙹" },
        { 'K', "𝙺" },
        { 'L', "𝙻" },
        { 'M', "𝙼" },
        { 'N', "𝙽" },
        { 'O', "𝙾" },
        { 'P', "𝙿" },
        { 'Q', "𝚀" },
        { 'R', "𝚁" },
        { 'S', "𝚂" },
        { 'T', "𝚃" },
        { 'U', "𝚄" },
        { 'V', "𝚅" },
        { 'W', "𝚆" },
        { 'X', "𝚇" },
        { 'Y', "𝚈" },
        { 'Z', "𝚉" },
        { 'a', "𝚊" },
        { 'b', "𝚋" },
        { 'c', "𝚌" },
        { 'd', "𝚍" },
        { 'e', "𝚎" },
        { 'f', "𝚏" },
        { 'g', "𝚐" },
        { 'h', "𝚑" },
        { 'i', "𝚒" },
        { 'j', "𝚓" },
        { 'k', "𝚔" },
        { 'l', "𝚕" },
        { 'm', "𝚖" },
        { 'n', "𝚗" },
        { 'o', "𝚘" },
        { 'p', "𝚙" },
        { 'q', "𝚚" },
        { 'r', "𝚛" },
        { 's', "𝚜" },
        { 't', "𝚝" },
        { 'u', "𝚞" },
        { 'v', "𝚟" },
        { 'w', "𝚠" },
        { 'x', "𝚡" },
        { 'y', "𝚢" },
        { 'z', "𝚣" },
        { '0', "𝟶" },
        { '1', "𝟷" },
        { '2', "𝟸" },
        { '3', "𝟹" },
        { '4', "𝟺" },
        { '5', "𝟻" },
        { '6', "𝟼" },
        { '7', "𝟽" },
        { '8', "𝟾" },
        { '9', "𝟿" }
    };

    /// <summary>
    ///  Converts the specified string to Unicode Mathematical Monospace characters. Letters (A–Z, a–z) and digits (0–9)
    ///  are mapped to their monospace counterparts; all other characters are left unchanged.
    /// </summary>
    /// <param name="input">The source string to convert.</param>
    /// <returns>
    ///  The converted string containing Unicode Mathematical Monospace characters, or the original value if <paramref
    ///  name="input" /> is null or empty.
    /// </returns>
    public static string ToMonospaceUnicode( this string input )
    {
        if(string.IsNullOrEmpty(input))
            return input;

        StringBuilder builder = new StringBuilder(input.Length);
        foreach(char c in input)
            if((c >= 'A') && (c <= 'Z'))
                builder.Append(char.ConvertFromUtf32(0x1D670 + (c - 'A'))); // Uppercase
            else if((c >= 'a') && (c <= 'z'))
                builder.Append(char.ConvertFromUtf32(0x1D68A + (c - 'a'))); // Lowercase
            else if((c >= '0') && (c <= '9'))
                builder.Append(char.ConvertFromUtf32(0x1D7F6 + (c - '0'))); // Digits
            else
                builder.Append(c); // Other characters unchanged
        return builder.ToString();
    }

    /// <summary>
    ///  Converts the specified string to Unicode Mathematical Monospace characters with an optional style selector.
    /// </summary>
    /// <param name="input">The source string to convert.</param>
    /// <param name="fontType">
    ///  The requested style: 0 = normal monospace (default); 1 = bold monospace (not defined in Unicode; falls back to
    ///  normal monospace); 2 = italic monospace (not defined in Unicode; falls back to normal monospace for letters).
    /// </param>
    /// <returns>
    ///  The converted string using Unicode Mathematical Monospace characters based on <paramref name="fontType" />, or
    ///  the original value if <paramref name="input" /> is null or empty.
    /// </returns>
    /// <remarks>
    ///  Unicode does not define bold or italic variants for the Mathematical Monospace block. As a result, both bold
    ///  and italic requests fall back to the standard monospace mapping. Non-alphanumeric characters are not
    ///  transformed.
    /// </remarks>
    public static string ToMonospaceUnicode( this string input, int fontType = 0 )
    {
        if(string.IsNullOrEmpty(input))
            return input;

        StringBuilder builder = new StringBuilder(input.Length);

        foreach(char c in input)
            switch(fontType) {
                case 1: // Bold Monospace (not available in Unicode, fallback to Monospace)
                    // There is no official Unicode block for bold monospace, so fallback to normal monospace
                    goto case 0;
                case 2: // Italic Monospace (not available in Unicode, fallback to Monospace Italic for math, but only for a-z, A-Z)
                    if((c >= 'A') && (c <= 'Z'))
                        builder.Append(char.ConvertFromUtf32(0x1D670 + (c - 'A'))); // fallback to normal monospace
                    else if((c >= 'a') && (c <= 'z'))
                        builder.Append(char.ConvertFromUtf32(0x1D68A + (c - 'a'))); // fallback to normal monospace
                    else if((c >= '0') && (c <= '9'))
                        builder.Append(char.ConvertFromUtf32(0x1D7F6 + (c - '0')));
                    else
                        builder.Append(c);
                    break;
                case 0: // Normal Monospace
                default:
                    if((c >= 'A') && (c <= 'Z'))
                        builder.Append(char.ConvertFromUtf32(0x1D670 + (c - 'A')));
                    else if((c >= 'a') && (c <= 'z'))
                        builder.Append(char.ConvertFromUtf32(0x1D68A + (c - 'a')));
                    else if((c >= '0') && (c <= '9'))
                        builder.Append(char.ConvertFromUtf32(0x1D7F6 + (c - '0')));
                    else
                        builder.Append(c);
                    break;
            }
        return builder.ToString();
    }

}