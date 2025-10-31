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
using MgbUtilties.Media.Images.Enums;

namespace MgbUtilties.Media.Images
{

    public static class MagickImageServices
    {

        static readonly string lossLess = "lossless";

        public static byte[] CreateAnnotated
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "png"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            img.Settings.FontFamily = "Arial";
            img.Settings.FontPointsize = imageInfo.GetPointSize();
            img.Settings.TextGravity = Gravity.South;
            img.Settings.FillColor = MagickColors.Snow;
            img.Settings.BackgroundColor = MagickColors.Yellow;
            img.Annotate("This is a test", new MagickGeometry(500, imageInfo.ImageHeight), Gravity.South);
            img.Format = MagickFormat.Png32;
            img.Strip();
            img.Depth = 16;
            img.Write(fullPath);
            img.CreateWebpFile(imageInfo, destPath);
            return img.ToByteArray();
        }

        public static byte[] CreateImage
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(img);
            destPath.ThrowIfNullOrEmpty();

            destPath.CreateDirectories();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), imageInfo.FileName);

            if(img.Width <= 0)
                return [];

            if(imageInfo.ImageHeight != 0)
                img.Resize(0, imageInfo.ImageHeight);

            img.Quality = imageInfo.Quality;

            img.CreateWebpFile(imageInfo, destPath); // make sure to create a webp file

            if(fullPath.FileExt().Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                || fullPath.FileExt().Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) {
                img.Format = MagickFormat.Pjpeg;
                fullPath = Path.ChangeExtension(fullPath, "jpg"); // normalize jpeg to jpg
            }
            if(fullPath.FileExt().Equals(".gif", StringComparison.OrdinalIgnoreCase))
                img.Format = MagickFormat.Gif;

            if(fullPath.FileExt().Equals(".png", StringComparison.OrdinalIgnoreCase)) {
                img.Format = MagickFormat.Png32;
                img.Depth = 8;
            }

            switch(imageInfo.PhotoStyle) {
                case PhotoStyles.SoftEdge:
                    return img.CreateSoftEdge(imageInfo, destPath);
                case PhotoStyles.TornPaper:
                    return img.CreateTornPaper(imageInfo, destPath);
                case PhotoStyles.Vignette:
                    return img.CreateVignette(imageInfo, destPath);
                case PhotoStyles.Polaroid:
                    return img.CreatePolaroid(imageInfo, destPath);
                case PhotoStyles.Thumbnail:
                    return img.CreateThumbnail(imageInfo, destPath);
                default:
                    img.Write(fullPath);
                    break;
            }
            return img.ToByteArray();
        }

        public static void CreateJpgFile
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "jpg"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            if((imageInfo.ImageWidth > 0) && (img.Width > imageInfo.ImageWidth))
                img.Resize(imageInfo.ImageWidth, 0);

            img.Quality = imageInfo.Quality;
            img.Format = MagickFormat.Jpg;
            img.Settings.SetDefine(MagickFormat.Jpg, lossLess, false);
            img.Write(fullPath);
        }

        public static byte[] CreatePolaroid
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "png"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            img.Settings.FontFamily = "Arial";
            img.Settings.FontPointsize = imageInfo.GetPointSize();
            img.Settings.TextGravity = Gravity.Center;
            img.BorderColor = MagickColors.Snow;
            img.Polaroid(imageInfo.Caption ?? string.Empty, imageInfo.Rotate, PixelInterpolateMethod.Nearest);
            img.Format = MagickFormat.Avif;
            img.Strip();
            img.Depth = 16;

            //img.Write(fullPath);
            img.CreateWebpFile(imageInfo, destPath);
            return img.ToByteArray();
        }

        public static byte[] CreateSoftEdge
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();
            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "png"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            img.Alpha(AlphaOption.Set);
            img.VirtualPixelMethod = VirtualPixelMethod.Transparent;
            img.Blur(8, 8, Channels.Alpha);
            img.Level(new Percentage(50), new Percentage(100), Channels.Alpha);
            using IMagickImage<ushort> clone = img.Clone();
            clone.Rotate(imageInfo.Rotate);
            clone.Format = MagickFormat.Png32;
            clone.Depth = 8;
            clone.Write(fullPath);
            ((MagickImage)clone).CreateWebpFile(imageInfo, destPath);
            return clone.ToByteArray();
        }

        public static byte[] CreateThumbnail
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath, uint height = 300 )
        {
            destPath.ThrowIfNullOrEmpty();

            if(!destPath.DirectoryExists())
                destPath.CreateDirectories();

            img.AutoOrient();
            img.Thumbnail(0, height);
            img.Format = MagickFormat.Pjpeg;
            img.Quality = 100;
            img.Write(destPath);
            img.Format = MagickFormat.WebP;
            img.Settings.SetDefine(MagickFormat.WebP, lossLess, false);
            img.Write(Path.ChangeExtension(destPath, ".webp"));
            return img.ToByteArray();
        }

        public static byte[] CreateTornPaper
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "png"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            using IMagickImage<ushort> clone = img.Clone();
            clone.Alpha(AlphaOption.Extract);
            clone.VirtualPixelMethod = VirtualPixelMethod.Black;
            clone.Spread(10);
            clone.Blur(0, 3);
            clone.Threshold((Percentage)50);
            clone.Spread(1);
            clone.Blur(0, 0.7);
            img.Alpha(AlphaOption.Off);
            img.Composite(clone, CompositeOperator.CopyAlpha);
            img.Format = MagickFormat.Png32;
            img.Strip();
            using IMagickImage<ushort> rotateClone = img.Clone();
            rotateClone.Rotate(imageInfo.Rotate);
            rotateClone.Depth = 8;
            rotateClone.Write(fullPath);
            ((MagickImage)clone).CreateWebpFile(imageInfo, destPath);
            return rotateClone.ToByteArray();
        }

        public static byte[] CreateVignette
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "png"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            img.Alpha(AlphaOption.Set);
            img.BackgroundColor = MagickColors.Transparent;
            img.Vignette();
            img.Format = MagickFormat.Png32;
            img.Strip();
            if(img.Width > imageInfo.ImageWidth)
                img.Resize(imageInfo.ImageWidth, 0);
            using IMagickImage<ushort> clone = img.Clone();
            clone.Rotate(imageInfo.Rotate);
            clone.Depth = 8;
            clone.Write(fullPath);
            ((MagickImage)clone).CreateWebpFile(imageInfo, destPath);
            return clone.ToByteArray();
        }

        public static void CreateWebpFile
            ( this MagickImage img, MgbMagickImageInfo imageInfo, MgbFilePath destPath )
        {
            ArgumentNullException.ThrowIfNull(imageInfo.FileName, nameof(imageInfo.FileName));
            destPath.ThrowIfNullOrEmpty();

            MgbFilePath fullPath = Path.Combine(destPath.FilePath(), Path.ChangeExtension(imageInfo.FileName, "webp"));

            if(!fullPath.DirectoryExists())
                fullPath.CreateDirectories();

            if((imageInfo.ImageWidth > 0) && (img.Width > imageInfo.ImageWidth))
                img.Resize(imageInfo.ImageWidth, 0);

            img.Quality = imageInfo.Quality;
            img.Format = MagickFormat.WebP;
            img.Settings.SetDefine(MagickFormat.WebP, lossLess, false);
            img.Write(fullPath);
        }

        static double GetPointSize( this MgbMagickImageInfo imageInfo )
        {
            ImageSizes imageSizes = (ImageSizes)imageInfo.ImageWidth;
            return imageSizes switch {
                ImageSizes.xlarge => 72.0
                , ImageSizes.large => 58.0
                , ImageSizes.medium => 36.0
                , ImageSizes.small => 24.0
                , ImageSizes.xsmall => 18.0
                , ImageSizes.thumbnail => 16.0
                , _ => 12.0 // Default point size for original or unknown sizes
            };
        }

    }

}
