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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using EagleViewEnt.Utilities.Core.Extensions.String;
using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Interfaces;

namespace EagleViewEnt.Utilities.Core.Types.ValueTypes.String;

/// <summary>
///  Provides a strongly-typed, validated string value object base class using CRTP (Curiously Recurring Template
///  Pattern). Derive concrete types from this class to encapsulate domain-specific validation and behavior for string
///  values.
/// </summary>
/// <typeparam name="TSelf">
///  The concrete type deriving from this base class. Must inherit from <see cref="StringValueType{TSelf}" />.
/// </typeparam>
public abstract class StringValueType<TSelf> : IStringValueType where TSelf : StringValueType<TSelf>
{

    /// <summary>
    ///  Gets the underlying string value via the <see cref="IStringValueType" /> interface.
    /// </summary>
    string IStringValueType.Value => Value;

    /// <summary>
    ///  Initializes a new instance of the <see cref="StringValueType{TSelf}" /> class.
    /// </summary>
    /// <param name="value">
    ///  The input string value. <c>null</c> is treated as an empty string. Leading and trailing whitespace are trimmed.
    /// </param>
    /// <param name="skipValidation">
    ///  When <c>true</c>, bypasses <see cref="Validation" />; when <c>false</c>, the value is validated and an
    ///  exception is thrown if invalid.
    /// </param>
    /// <exception cref="ArgumentException">
    ///  Thrown when <paramref name="skipValidation" /> is <c>false</c> and the value fails <see cref="Validation" />.
    /// </exception>
    protected StringValueType( string? value, bool skipValidation = false )
    {
        Value = value?.Trim() ?? string.Empty;
        if(!skipValidation && !Validation())
            throw new ArgumentException($"Invalid value for {typeof(TSelf).Name}: '{Value}'", nameof(value));
    }

    /// <summary>
    ///  Gets an empty instance of <typeparamref name="TSelf" />.
    /// </summary>
    public static TSelf Empty => (TSelf)Activator.CreateInstance(typeof(TSelf), string.Empty)!;

    /// <summary>
    ///  Gets the stored, trimmed string value for the value object.
    /// </summary>
    protected string Value { get; }

    /// <summary>
    ///  Gets a value indicating whether the underlying value is empty.
    /// </summary>
    [NotMapped, JsonIgnore, XmlIgnore]
    public bool IsEmpty => string.Compare(Value, string.Empty) == 0;

    /// <summary>
    ///  Explicitly converts a <see cref="string" /> to an instance of <typeparamref name="TSelf" />.
    /// </summary>
    /// <param name="value">The input string; <c>null</c> becomes empty and the value is trimmed.</param>
    /// <returns>An instance of <typeparamref name="TSelf" /> representing the provided value.</returns>
    /// <exception cref="InvalidOperationException">
    ///  Thrown when an instance of <typeparamref name="TSelf" /> cannot be created.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  May be thrown by the target constructor when validation fails.
    /// </exception>
    public static explicit operator StringValueType<TSelf>( string? value )
    {
        value = value?.Trim() ?? string.Empty;

        if(Activator.CreateInstance(typeof(TSelf), value) is not TSelf result)
            throw new InvalidOperationException($"Could not create an instance of {typeof(TSelf).FullName}.");

        return result;
    }

    /// <summary>
    ///  Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///  <c>true</c> if <paramref name="obj" /> is a <typeparamref name="TSelf" /> with the same underlying value;
    ///  otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object? obj )
    {
        if((obj is null) || (obj.GetType() != typeof(TSelf)))
            return false;
        TSelf other = (TSelf)obj;
        return string.Equals(Value, other.Value);
    }

    /// <summary>
    ///  Creates an instance of <typeparamref name="TSelf" /> from the specified string by invoking the concrete type's
    ///  constructor.
    /// </summary>
    /// <param name="value">The string value to pass to the constructor.</param>
    /// <returns>A new instance of <typeparamref name="TSelf" />.</returns>
    protected static TSelf FromStringValue( string value ) => (TSelf)Activator.CreateInstance(typeof(TSelf), value)!;

    /// <summary>
    ///  Returns a hash code for this instance based on the underlying value.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    ///  Returns a masked version of the value for display.
    /// </summary>
    /// <param name="visibleLength">The number of trailing characters to leave unmasked.</param>
    /// <param name="totalLength">The total length of the returned masked string.</param>
    /// <param name="padCharacter">The character used to mask the value.</param>
    /// <returns>A masked string suitable for secure display.</returns>
    public virtual string ToSecuredString
        ( int visibleLength = 4, int totalLength = 8, char padCharacter = '*' )
        => Value.ToSecurityMaskedString(visibleLength, totalLength, padCharacter);

    /// <summary>
    ///  Returns the underlying string value.
    /// </summary>
    /// <returns>The underlying string value.</returns>
    public override string ToString() => Value;

    /// <summary>
    ///  Validates the current <see cref="Value" /> according to the rules of the concrete type.
    /// </summary>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    protected abstract bool Validation();

}
