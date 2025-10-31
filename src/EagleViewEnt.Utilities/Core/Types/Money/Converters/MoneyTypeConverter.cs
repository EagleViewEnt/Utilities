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
using System.ComponentModel;
using System.Globalization;

namespace EagleViewEnt.Utilities.Core.Types.Money.Converters
{

    public class MoneyTypeConverter : TypeConverter
    {

        public override bool CanConvertFrom( ITypeDescriptorContext? context, Type sourceType )
            => (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);

        public override object? ConvertFrom
            ( ITypeDescriptorContext? context, CultureInfo? culture, object value )
        {
            if(value is string str) {
                string numericPart = new string([.. str.Where(c
                    => char.IsDigit(c) || (c == '.') || (c == '-') || (c == '+'))]);
                return new Money(amount: decimal.Parse(numericPart), language: culture?.Name ??
                    CultureInfo.CurrentCulture.Name);
            }
            return base.ConvertFrom(context, culture, value);
        }

    }

}