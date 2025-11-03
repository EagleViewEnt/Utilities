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

using EagleViewEnt.Utilities.Core.Types.Money;
using EagleViewEnt.Utilities.Core.Types.Money.Enum;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EagleViewEnt.Utilites.EFCore.Converters;

/// <summary>
///  Converts <see cref="EveMoney" /> to <see cref="decimal" /> for storage in T-SQL money columns, and vice versa. The
///  currency is stored separately as a string column.
/// </summary>
public class EveMoneyEFCoreConverter : ValueConverter<EveMoney, decimal>
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="EveMoneyEFCoreConverter" /> class.
    /// </summary>
    public EveMoneyEFCoreConverter()
        : base(
        v => v.Value,
        v => new EveMoney(v, EveCurrency.USD)) // You must set the currency separately in your entity config
    { }

}

