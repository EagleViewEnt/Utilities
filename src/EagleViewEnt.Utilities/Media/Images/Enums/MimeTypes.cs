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

using Ardalis.SmartEnum;

namespace MgbUtilities.Platform.All.IO
{

    public sealed class MimeTypes : SmartEnum<MimeTypes, string>
    {

        public static readonly MimeTypes Avif = new MimeTypes(nameof(Avif), "image/avif");
        public static readonly MimeTypes Bmp = new MimeTypes(nameof(Bmp), "image/bmp");
        public static readonly MimeTypes Gif = new MimeTypes(nameof(Gif), "image/gif");
        public static readonly MimeTypes Heic = new MimeTypes(nameof(Heic), "image/heic");
        public static readonly MimeTypes Ico = new MimeTypes(nameof(Ico), "image/x-icon");
        public static readonly MimeTypes Jpeg = new MimeTypes(nameof(Jpeg), "image/jpeg");
        public static readonly MimeTypes Png = new MimeTypes(nameof(Png), "image/png");
        public static readonly MimeTypes Svg = new MimeTypes(nameof(Svg), "image/svg+xml");
        public static readonly MimeTypes Tiff = new MimeTypes(nameof(Tiff), "image/tiff");
        public static readonly MimeTypes Webp = new MimeTypes(nameof(Webp), "image/webp");

        MimeTypes( string name
                  , string value )
            : base(name, value) { }

        public static MimeTypes FromExtension( string extension )
        {
            extension = extension.Replace(".", string.Empty);

            return List.FirstOrDefault(mime => mime.Name.Equals(extension, StringComparison.OrdinalIgnoreCase)) ??
                new MimeTypes(extension, "application/octet-stream");
        }

    }

}
