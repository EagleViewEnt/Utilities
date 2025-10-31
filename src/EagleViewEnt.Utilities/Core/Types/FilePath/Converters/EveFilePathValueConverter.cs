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

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Converters;

/// <summary>
///  Converts <see cref="EveFilePath" /> to and from <see cref="string" /> for EF Core value conversion.
/// </summary>
public class EveFilePathValueConverter : ValueConverter<EveFilePath, string>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveFilePathValueConverter" /> class.
    /// </summary>
    public EveFilePathValueConverter()
        : base(
            v => v.ToString(),
            v => new EveFilePath(v)) { }

}

