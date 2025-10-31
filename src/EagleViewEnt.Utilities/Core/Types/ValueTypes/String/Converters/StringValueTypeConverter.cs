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
using System.Linq;

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Interfaces;

namespace EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

/// <summary>
///  Provides a <see cref="TypeConverter" /> that converts from <see cref="string" /> to a value type implementing <see
///  cref="IStringValueType" /> by invoking a constructor that accepts a single <see cref="string" /> argument.
/// </summary>
/// <typeparam name="T">
///  The destination type that implements <see cref="IStringValueType" /> and exposes a public constructor with a single
///  <see cref="string" /> parameter.
/// </typeparam>
public class StringValueTypeConverter<T> : TypeConverter where T : IStringValueType
{

    /// <summary>
    ///  Determines whether this converter can convert an object of the specified source type to the type of this
    ///  converter.
    /// </summary>
    /// <param name="context">An optional format context; not used.</param>
    /// <param name="sourceType">The source <see cref="Type" /> to evaluate.</param>
    /// <returns>
    ///  <see langword="true" /> if <paramref name="sourceType" /> is <see cref="string" />; otherwise, <see
    ///  langword="false" />.
    /// </returns>
    public override bool CanConvertFrom( ITypeDescriptorContext? context, Type sourceType )
        => sourceType == typeof(string);

    /// <summary>
    ///  Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An optional format context; not used.</param>
    /// <param name="culture">An optional <see cref="CultureInfo" />; not used.</param>
    /// <param name="value">The value to convert. Must be a <see cref="string" />.</param>
    /// <returns>
    ///  An instance of <typeparamref name="T" /> constructed with the provided <see cref="string" /> value, or the
    ///  result of <see cref="TypeConverter.ConvertFrom(ITypeDescriptorContext, CultureInfo, object)" /> if not a
    ///  string.
    /// </returns>
    /// <exception cref="MissingMethodException">
    ///  Thrown if <typeparamref name="T" /> does not provide a public constructor that accepts a single <see
    ///  cref="string" /> parameter.
    /// </exception>
    /// <exception cref="MemberAccessException">
    ///  Thrown if the constructor exists but is not accessible.
    /// </exception>
    /// <exception cref="System.Reflection.TargetInvocationException">
    ///  Thrown if the constructor of <typeparamref name="T" /> throws an exception.
    /// </exception>
    public override object? ConvertFrom
        ( ITypeDescriptorContext? context, CultureInfo? culture, object value )
    {
        if(value is string str)
            return Activator.CreateInstance(typeof(T), str);

        return base.ConvertFrom(context, culture, value);
    }

}
