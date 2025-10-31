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
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO MgbNT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System;

using System.Linq;

using ImageMagick;

using MgbUtilties.Core.Types.FilePath;
using MgbUtilties.Core.Types.FilePath.Extensions;

namespace MgbUtilties.Media.Images
{

    public static class ImageServices
    {

        public static void CreateOptimizeImage
            ( this MagickImage img
              , MgbMagickImageInfo imageInfo
              , MgbFilePath thumbFilePath
              , MgbFilePath optimizedFilePath )
        {
            thumbFilePath.ThrowIfEmpty();
            optimizedFilePath.ThrowIfEmpty();

            img.CreateImage(imageInfo, optimizedFilePath);
        }

        public static void CreateOptimizeImageWithThumbnail
            ( this MagickImage img
              , MgbMagickImageInfo imageInfo
              , MgbFilePath thumbFilePath
              , MgbFilePath optimizedFilePath )
        {
            thumbFilePath.ThrowIfEmpty();
            optimizedFilePath.ThrowIfEmpty();

            img.CreateThumbnail(imageInfo, thumbFilePath);
            img.CreateImage(imageInfo, optimizedFilePath);
        }

        public static void CreateWebImages
            ( this MagickImage img
              , MgbMagickImageInfo imageInfo
              , MgbFilePath thumbFilePath
              , MgbFilePath optimizedFilePath )
        {

            //ArgumentNullException.ThrowIfNull(imageInfo);
            //thumbFilePath.ThrowIfEmpty();
            //optimizedFilePath.ThrowIfEmpty();
            //string file = string.IsNullOrEmpty(imageInfo.FileBase64)? imageInfo.FileName :
            //    imageInfo.FileBase64;
            //img.CreateImage(imageInfo, optimizedFilePath);
        }

    }

}
