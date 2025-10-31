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
using System.Globalization;

using Ardalis.SmartEnum;

namespace EagleViewEnt.Utilities.Core.Types.Money.Enum;

/// <summary>
///  Represents a currency with an associated culture, using SmartEnum for type-safe enumeration.
/// </summary>
public sealed class EveCurrency : SmartEnum<EveCurrency>
{

    /// <summary>
    ///  Represents an empty or unspecified currency. Uses the "en-US" culture by default.
    /// </summary>
    public static readonly EveCurrency Empty = new EveCurrency(nameof(Empty), 0, new CultureInfo("en-US"));

    /// <summary>
    ///  United States Dollar currency. Uses the "en-US" culture.
    /// </summary>
    public static readonly EveCurrency USD = new EveCurrency(nameof(USD), 1, new CultureInfo("en-US"));

    /// <summary>
    ///  Euro currency. Uses the "fr-FR" culture.
    /// </summary>
    public static readonly EveCurrency EUR = new EveCurrency(nameof(EUR), 2, new CultureInfo("fr-FR"));

    /// <summary>
    ///  British Pound Sterling currency. Uses the "en-GB" culture.
    /// </summary>
    public static readonly EveCurrency GBP = new EveCurrency(nameof(GBP), 3, new CultureInfo("en-GB"));

    /// <summary>
    ///  Mexican Peso currency. Uses the "es-MX" culture.
    /// </summary>
    public static readonly EveCurrency MXN = new EveCurrency(nameof(MXN), 4, new CultureInfo("es-MX"));

    /// <summary>
    ///  Vietnamese Dong currency. Uses the "vi-VN" culture.
    /// </summary>
    public static readonly EveCurrency VND = new EveCurrency(nameof(VND), 5, new CultureInfo("vi-VN"));

    EveCurrency
        ( string name, int value, CultureInfo culture )
        : base(name, value)
        => Culture = culture;

    /// <summary>
    ///  Gets the culture associated with this currency.
    /// </summary>
    public CultureInfo Culture { get; }

}
