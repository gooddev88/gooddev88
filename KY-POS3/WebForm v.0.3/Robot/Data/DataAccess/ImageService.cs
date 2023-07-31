using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class ImageService {
        public static Image ResizeImage(Image image, int width, int height, bool preserveAspectRatio = true) {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio) {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)width / (float)originalWidth;
                float percentHeight = (float)height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            } else {
                newWidth = width;
                newHeight = height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage)) {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        /// <summary>
        /// Resize an image keeping its aspect ratio (cropping may occur).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ResizeImageKeepAspectRatio(Image source, int width, int height) {
            Image result = null;

            try {
                if (source.Width != width || source.Height != height) {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height)) {
                        using (var g = System.Drawing.Graphics.FromImage(target)) {
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

                            if (newWidth > width) {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height) {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                } else {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            } catch (Exception) {
                result = null;
            }

            return result;
        }
        public static byte[] ConvertImageToByte(Image imageIn) {
            byte[] result = null;
            //using (var ms = new MemoryStream()) {
            //    imageIn.Save(ms, imageIn.RawFormat);
            //    result= ms.ToArray(); 
            //}
            ImageConverter _imageConverter = new ImageConverter();
            result = (byte[])_imageConverter.ConvertTo(imageIn, typeof(byte[]));
            return result;
        }

        public static Image ConvertByteToImage(byte[] imageBytes) {

            MemoryStream buf = new MemoryStream(imageBytes);
            Image image = Image.FromStream(buf, true);
            return image;
        }
    }
}