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

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Enums;

/// <summary>
///  Specifies where a generated date-time stamp should be placed relative to a file name.
/// </summary>
/// <remarks>
///  Use this enumeration to control whether a timestamp is prepended, appended, or omitted when constructing file
///  paths.
/// </remarks>
public enum DateTimeStampLocation
{

    /// <summary>
    ///  No date time stamp will be added.
    /// </summary>
    None = 0,
    /// <summary>
    ///  Date time stamp will be prepended to the file name.
    /// </summary>
    Prepend = 1,
    /// <summary>
    ///  Date time stamp will be appended to the file name.
    /// </summary>
    Append = 2

}
