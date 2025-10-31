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

namespace EagleViewEnt.Utilities.Core.Types.SequentialGuid
{

    /// <summary>
    ///  Provides robust sequential GUID generation suitable for database keys.
    /// </summary>
    public static class EveSequentialGuid
    {

        /// <summary>
        ///  Generates a sequential GUID using the COMB (combined GUID/timestamp) algorithm.
        /// </summary>
        public static Guid NewGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // Overwrite the last 6 bytes with the timestamp (little-endian)
            guidArray[10] = timestampBytes[0];
            guidArray[11] = timestampBytes[1];
            guidArray[12] = timestampBytes[2];
            guidArray[13] = timestampBytes[3];
            guidArray[14] = timestampBytes[4];
            guidArray[15] = timestampBytes[5];

            return new Guid(guidArray);
        }

    }

}
