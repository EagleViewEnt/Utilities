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

namespace EagleViewEnt.Utilities.Core.Types.Checks;

/// <summary>
///  Provides extension methods for working with MICR (Magnetic Ink Character Recognition) data formats.
/// </summary>
public static class MicrDataExtensions
{

    /// <summary>
    ///  MICR format: @[routing]@[account]+[check] CheckNumber is set to 0 if not provided
    /// </summary>
    /// <param name="account">The account number.</param>
    /// <param name="routing">The routing number.</param>
    /// <param name="checkNumber">The check number. If not provided, defaults to 0.</param>
    /// <returns>The formatted MICR string.</returns>
    public static string ToMicr
        ( string account, string routing, string? checkNumber = null )
    {
        checkNumber ??= CheckNumber.Empty;
        string micr = $"@{routing}@{account}";
        if(!string.IsNullOrEmpty(checkNumber))
            micr = $"++{checkNumber}";

        return micr;
    }

}
