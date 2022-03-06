namespace CmsEngine.Application.Helpers;

public static class FileHelper
{
    public static string FormatFileSize(string filename)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = new FileInfo(filename).Length;
        var order = 0;

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
        var fullsizeImage = Image.FromFile(originalFile);

        // Prevent using images internal thumbnail
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

        if (onlyResizeIfWider && fullsizeImage.Width <= newWidth)
        {
            newWidth = fullsizeImage.Width;
        }

        var newHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
        if (newHeight > maxHeight)
        {
            // Resize with height instead
            newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
            newHeight = maxHeight;
        }

        var newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

        // Clear handle to original file so that we can overwrite it if necessary
        fullsizeImage.Dispose();

        var encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);

        // Save resized picture
        newImage.Save(newFile, GetEncoderInfo("image/jpeg"), encoderParameters);
    }

    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        var encoders = ImageCodecInfo.GetImageEncoders();
        for (var j = 0; j < encoders.Length; ++j)
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
        var bmpImage = new Bitmap(img);
        return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
    }
}
