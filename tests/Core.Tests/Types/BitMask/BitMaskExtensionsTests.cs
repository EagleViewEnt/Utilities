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
using System.Linq;

using Ardalis.SmartEnum;

using MgbUtilties.Core.Types.BitMask;

namespace MgbUtilities.Core.Tests.Types.BitMask;

public class BitMaskExtensionsTests
{

    [Fact]
    public void AreBitsSet_ThrowsForNonIntegerType()
    {
        NotSupportedException ex = Assert.Throws<NotSupportedException>(()
            => {
            MgbBitMask<decimal> mask = new MgbBitMask<decimal>(2.0m);
            mask.AreBitsSet<decimal>(1); });
        Assert.Contains("integer", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AreBitsSet_ThrowsForNullOrEmptyArgs()
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(()
            => {
            MgbBitMask<decimal> mask = new MgbBitMask<decimal>(7);
            mask.AreBitsSet<decimal>(); });
        Assert.Contains("null", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AreBitsSet_WorksForByte()
    {
        MgbBitMask<byte> mask = new MgbBitMask<byte>(7);
        Assert.True(mask.AreBitsSet<byte>(1, 2, 4));
        Assert.False(mask.AreBitsSet<byte>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForInt()
    {
        MgbBitMask<int> mask = new MgbBitMask<int>(7);
        Assert.True(mask.AreBitsSet<int>(1, 2, 4));
        Assert.False(mask.AreBitsSet<int>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForLong()
    {
        MgbBitMask<long> mask = new MgbBitMask<long>(7L);
        Assert.True(mask.AreBitsSet<long>(1, 2, 4));
        Assert.False(mask.AreBitsSet<long>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForSByte()
    {
        MgbBitMask<sbyte> mask = new MgbBitMask<sbyte>(7);
        Assert.True(mask.AreBitsSet<sbyte>(1, 2, 4));
        Assert.False(mask.AreBitsSet<sbyte>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForShort()
    {
        MgbBitMask<short> mask = new MgbBitMask<short>(7);
        Assert.True(mask.AreBitsSet<short>(1, 2, 4));
        Assert.False(mask.AreBitsSet<short>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForUInt()
    {
        MgbBitMask<uint> mask = new MgbBitMask<uint>(7);
        Assert.True(mask.AreBitsSet<uint>(1, 2, 4));
        Assert.False(mask.AreBitsSet<uint>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForULong()
    {
        MgbBitMask<ulong> mask = new MgbBitMask<ulong>(7UL);
        Assert.True(mask.AreBitsSet<ulong>(1, 2, 4));
        Assert.False(mask.AreBitsSet<ulong>(8));
    }

    [Fact]
    public void AreBitsSet_WorksForUShort()
    {
        MgbBitMask<ushort> mask = new MgbBitMask<ushort>(7);
        Assert.True(mask.AreBitsSet<ushort>(1, 2, 4));
        Assert.False(mask.AreBitsSet<ushort>(8));
    }

    public sealed class Colors( string name, uint value ) : SmartEnum<Colors, uint>(name, value)
    {

        public static readonly Colors Red = new Colors(nameof(Red), 1);
        public static readonly Colors Green = new Colors(nameof(Green), 2);
        public static readonly Colors Blue = new Colors(nameof(Blue), 4);

    }

}