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

using EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Interfaces;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters;

/// <summary>
///  EF Core value converter for AccountNumber.
/// </summary>
public class StringValueTypeEfCoreValueConverter<T> : ValueConverter<T, string> where T : class, IStringValueType
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="StringValueTypeEfCoreValueConverter{T}" /> class.
    /// </summary>
    public StringValueTypeEfCoreValueConverter()
        : base(
            v => v.Value,
            v => CreateInstance(v)) { }

    static T CreateInstance( string value )
    {
        T? instance = (T?)Activator.CreateInstance(typeof(T), value);
        if(instance == null)
            throw new InvalidOperationException($"Unable to create an instance of {typeof(T)}.");
        return instance;
    }

}
