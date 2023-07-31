using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Helper {
    public static class ImageService
    {
        public static Image ResizeImage(Image image, int width, int height, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)width / (float)originalWidth;
                float percentHeight = (float)height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }


        //public static byte[] ConvertImageToByte(Image imageIn) {
        //    byte[] result = null;
        //    ImageConverter _imageConverter = new ImageConverter();
        //    result = (byte[])_imageConverter.ConvertTo(imageIn, typeof(byte[]));
        //    return result;
        //}

        public static Image ConvertByteToImage(byte[] imageBytes)
        {
            Image image = null;
            try
            {
                MemoryStream buf = new MemoryStream(imageBytes);
                image = Image.FromStream(buf, true);

            }
            catch (Exception)
            {

            }

            return image;


        }

        #region base 64 image

        public static string GetFileExtensionFromBase64(string base64String) {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper()) {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
        #endregion


        #region new image management

        public static Bitmap MakeSquarePhoto(Bitmap bmp, int size) {
            Bitmap res = new Bitmap(size, size);
            try {

                Graphics g = Graphics.FromImage(res);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);
                int t = 0, l = 0;
                if (bmp.Height > bmp.Width)
                    t = (bmp.Height - bmp.Width) / 2;
                else
                    l = (bmp.Width - bmp.Height) / 2;
                g.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle(l, t, bmp.Width - l * 2, bmp.Height - t * 2), GraphicsUnit.Pixel);

            } catch { }
            
            return res;
        }
        public static string Convert2Base64ImageSource(string b64,string ext) {
            
            return @"data:Image/" + ext + ";base64," + b64;
        }
        //public static byte[] ImageToByte(Image img) {
        //    ImageConverter converter = new ImageConverter();
        //    return (byte[])converter.ConvertTo(img, typeof(byte[]));
        //}
        public static byte[] ConvertImageToByte(Image imageIn) {
            byte[] result = null; 
            using (var ms = new MemoryStream()) {
                imageIn.Save(ms, imageIn.RawFormat);
                result = ms.ToArray();
            }
            return result;
        }

        public static Bitmap ByteToBitmap(byte[] imgByte) {
            //using (MemoryStream memstr = new MemoryStream(imgByte)) {
            //    Image img = Image.FromStream(memstr);
            //    return img;
            //} 
            Bitmap bmp;
            using (var ms = new MemoryStream(imgByte)) {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }
        public static byte[] CreateThumb(byte[] input, int size) {
            byte[] output = null;
            try {
                var bitmap = ByteToBitmap(input);
                var bitmap_crop = MakeSquarePhoto(bitmap, size);
                output = ConvertImageToByte(bitmap_crop);
            } catch (Exception) {
            }

            return output;
        }

        public static byte[] SaveCroppedImage(byte[] data, int maxWidth, int maxHeight, string filePath) {
            byte[] returnImg = null;
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap image = (Bitmap)tc.ConvertFrom(data);


            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height) {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width) {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    } else {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                } else {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height) {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    } else {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            } catch { }
            try {
                using (EncoderParameters encParams = new EncoderParameters(1)) {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    if (filePath != "") {
                        finalImage.Save(filePath, jpgInfo, encParams);

                    } else {
                        var mst = new MemoryStream();
                        finalImage.Save(mst, jpgInfo, encParams);
                        returnImg = mst.ToArray();
                        mst.Dispose();
                    }
                }

            } catch { }
            //using (var mst = new MemoryStream()) {
            //    finalImage.Save(mst, finalImage.RawFormat);
            //    returnImg = mst.ToArray();
            //}
            if (bitmap != null) {
                bitmap.Dispose();
            }
            return returnImg;
        }
        #endregion
    }
}
