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
///  Provides extension methods for working with <see cref="EveBitMask{T}" /> values.
/// </summary>
public static class EveBitMaskExtensions
{

    /// <summary>
    ///  Test to see if bit(s) are in bit mask
    /// </summary>
    /// <typeparam name="T">byte, short, int, long</typeparam>
    /// <param name="value">bit mask to test</param>
    /// <param name="bitsToTest">bit values to test</param>
    /// <returns>bool</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="bitsToTest" /> is null or empty.</exception>
    /// <exception cref="NotSupportedException">Thrown when <typeparamref name="T" /> is not an integer type.</exception>
    public static bool AreBitsSet<T>( this EveBitMask<T> value, params uint[] bitsToTest ) where T : INumber<T>
    {

        if((bitsToTest == null) || (bitsToTest.Length == 0))
            throw new ArgumentException("bitsToTest cannot be null or empty", nameof(bitsToTest));

        if((typeof(T) != typeof(int))
            && (typeof(T) != typeof(long))
            && (typeof(T) != typeof(short))
            && (typeof(T) != typeof(byte))
            && (typeof(T) != typeof(uint))
            && (typeof(T) != typeof(ulong))
            && (typeof(T) != typeof(ushort))
            && (typeof(T) != typeof(sbyte)))
            throw new NotSupportedException("Only integer types are supported.");

        // Convert the value to a long for bitwise operations
        long longValue = Convert.ToInt64(value);

        foreach(uint bitValue in bitsToTest)
            if((longValue & bitValue) != bitValue)

                // If any bit value is not fully set, return false
                return false;

        return true; // All bit values are fully set
    }

}
