using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace CmsEngine.Domain.Helpers
{
    public static class FileHelper
    {
        public static string FormatFileSize(string filename)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = new FileInfo(filename).Length;
            int order = 0;

            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len /= 1024;
            }

            return string.Format("{0:0} {1}", len, sizes[order]);
        }

        public static bool IsImage(string fileName)
        {
            return fileName.EndsWith(".jpeg", true, CultureInfo.InvariantCulture)
                || fileName.EndsWith(".jpg", true, CultureInfo.InvariantCulture)
                || fileName.EndsWith(".png", true, CultureInfo.InvariantCulture)
                || fileName.EndsWith(".gif", true, CultureInfo.InvariantCulture)
                || fileName.EndsWith(".bmp", true, CultureInfo.InvariantCulture);
        }

        public static void ResizeImage(string originalFile, string newFile, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            Image fullsizeImage = Image.FromFile(originalFile);

            // Prevent using images internal thumbnail
            fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

            if (onlyResizeIfWider && fullsizeImage.Width <= newWidth)
            {
                newWidth = fullsizeImage.Width;
            }

            int newHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
                newHeight = maxHeight;
            }

            Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            fullsizeImage.Dispose();

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);

            // Save resized picture
            newImage.Save(newFile, GetEncoderInfo("image/jpeg"), encoderParameters);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                {
                    return encoders[j];
                }
            }
            return null;
        }

        public static Bitmap CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
    }
}
