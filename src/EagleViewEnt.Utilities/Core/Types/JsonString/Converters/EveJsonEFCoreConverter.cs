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

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EagleViewEnt.Utilities.Core.Types.JsonString.Converters;

/// <summary>
///  Converts <see cref="EveJson" /> values to and from their string representation for use with Entity Framework Core.
/// </summary>
public class EveJsonEFCoreConverter : ValueConverter<EveJson, string>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveJsonEFCoreConverter" /> class.
    /// </summary>
    public EveJsonEFCoreConverter()
        : base(
            v => v.ToString(), // Convert EveJson to string for storage
            v => new EveJson(v) // Convert string from storage to EveJson
        ) { }

}

