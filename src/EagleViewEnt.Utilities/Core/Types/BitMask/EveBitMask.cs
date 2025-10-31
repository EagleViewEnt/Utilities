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

using System.Numerics;

namespace EagleViewEnt.Utilities.Core.Types.BitMask;

/// <summary>
///  Provides a strongly-typed bit mask wrapper over a numeric value.
/// </summary>
/// <typeparam name="T">A numeric type that implements <see cref="INumber{TSelf}" />.</typeparam>
/// <param name="Value">The underlying numeric value representing the bit mask.</param>
public readonly record struct EveBitMask<T>( T Value ) : IConvertible where T : INumber<T>
{

    /// <summary>
    ///  Implicitly converts an <see cref="EveBitMask{T}" /> to its underlying numeric value.
    /// </summary>
    /// <param name="mask">The bit mask instance.</param>
    /// <returns>The underlying numeric value.</returns>
    public static implicit operator T( EveBitMask<T> mask ) => mask.Value;

    /// <summary>
    ///  Implicitly converts a numeric value to an <see cref="EveBitMask{T}" />.
    /// </summary>
    /// <param name="value">The numeric value to wrap.</param>
    /// <returns>The created bit mask.</returns>
    public static implicit operator EveBitMask<T>( T value ) => new(value);

    /// <summary>
    ///  Gets the <see cref="TypeCode" /> of the underlying value if it implements <see cref="IConvertible" />
    /// </summary>
    /// <returns>The <see cref="TypeCode" /> of the underlying value; otherwise <see cref="TypeCode.Object" />.</returns>
    public TypeCode GetTypeCode() => (Value as IConvertible)?.GetTypeCode() ?? TypeCode.Object;

    /// <summary>
    ///  Converts the underlying value to a <see cref="bool" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted boolean value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public bool ToBoolean( IFormatProvider? provider )
        => (Value as IConvertible)?.ToBoolean(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="byte" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted byte value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public byte ToByte( IFormatProvider? provider )
        => (Value as IConvertible)?.ToByte(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="char" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted char value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public char ToChar( IFormatProvider? provider )
        => (Value as IConvertible)?.ToChar(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="DateTime" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted <see cref="DateTime" /> value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public DateTime ToDateTime( IFormatProvider? provider )
        => (Value as IConvertible)?.ToDateTime(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="decimal" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted decimal value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public decimal ToDecimal( IFormatProvider? provider )
        => (Value as IConvertible)?.ToDecimal(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="double" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted double value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public double ToDouble( IFormatProvider? provider )
        => (Value as IConvertible)?.ToDouble(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 16-bit signed integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 16-bit signed integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public short ToInt16( IFormatProvider? provider )
        => (Value as IConvertible)?.ToInt16(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 32-bit signed integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 32-bit signed integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public int ToInt32( IFormatProvider? provider )
        => (Value as IConvertible)?.ToInt32(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 64-bit signed integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 64-bit signed integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public long ToInt64( IFormatProvider? provider )
        => (Value as IConvertible)?.ToInt64(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to an 8-bit signed integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 8-bit signed integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public sbyte ToSByte( IFormatProvider? provider )
        => (Value as IConvertible)?.ToSByte(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a <see cref="float" />.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted single-precision floating-point value.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public float ToSingle( IFormatProvider? provider )
        => (Value as IConvertible)?.ToSingle(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Returns the string representation of the underlying value.
    /// </summary>
    /// <returns>A string representation of the underlying value.</returns>
    public override string ToString() => Value.ToString() ?? string.Empty;

    /// <summary>
    ///  Converts the underlying value to its string representation by using the specified format provider.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the underlying value.</returns>
    public string ToString( IFormatProvider? provider ) => (Value as IConvertible)?.ToString(provider) ?? string.Empty;

    /// <summary>
    ///  Converts the underlying value to the specified <paramref name="conversionType" />.
    /// </summary>
    /// <param name="conversionType">The type to which to convert the value.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>An object that is equivalent to the underlying value, converted to the specified type.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public object ToType( Type conversionType, IFormatProvider? provider )
        => (Value as IConvertible)?.ToType(conversionType, provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 16-bit unsigned integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 16-bit unsigned integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public ushort ToUInt16( IFormatProvider? provider )
        => (Value as IConvertible)?.ToUInt16(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 32-bit unsigned integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 32-bit unsigned integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public uint ToUInt32( IFormatProvider? provider )
        => (Value as IConvertible)?.ToUInt32(provider) ?? throw new InvalidCastException();

    /// <summary>
    ///  Converts the underlying value to a 64-bit unsigned integer.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted 64-bit unsigned integer.</returns>
    /// <exception cref="InvalidCastException">Thrown when the underlying value does not support conversion.</exception>
    public ulong ToUInt64( IFormatProvider? provider )
        => (Value as IConvertible)?.ToUInt64(provider) ?? throw new InvalidCastException();

}
