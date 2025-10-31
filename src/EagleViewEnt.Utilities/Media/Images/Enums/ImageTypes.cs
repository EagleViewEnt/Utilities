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

namespace MgbUtilties.Media.Images.Enums
{

    public sealed class ImageTypes
    {

        static readonly List<ImageTypes> _all = [];

        public static readonly ImageTypes Jpg = new ImageTypes(nameof(Jpg).ToLower());
        public static readonly ImageTypes Png = new ImageTypes(nameof(Png).ToLower());
        public static readonly ImageTypes Bmp = new ImageTypes(nameof(Bmp).ToLower());
        public static readonly ImageTypes Gif = new ImageTypes(nameof(Gif).ToLower());
        public static readonly ImageTypes Webp = new ImageTypes(nameof(Webp).ToLower());
        public static readonly ImageTypes Svg = new ImageTypes(nameof(Svg).ToLower());

        ImageTypes( string name )
        {
            Name = name;
            _all.Add(this);
        }

        public string Name { get; }

        public static IEnumerable<ImageTypes> ToList() => _all;

    }

}
