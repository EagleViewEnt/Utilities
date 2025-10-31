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

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters;

/// <summary>
///  Converts <see cref="EveMoneyUsd" /> to and from <see cref="decimal" /> for use with Entity Framework Core, mapping
///  the value to a T-SQL money column. The currency must be stored and set separately in your entity configuration.
/// </summary>
public class EveMoneyUsdEFCoreConverter : ValueConverter<EveMoneyUsd, decimal>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveMoneyUsdEFCoreConverter" /> class.
    /// </summary>
    public EveMoneyUsdEFCoreConverter()
        : base(
        v => v.Value,
        v => new EveMoneyUsd(v)) // You must set the currency separately in your entity config
    { }

}
