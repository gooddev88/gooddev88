using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace PrintMaster.Helper {
    public class ImageHelper {
        public static Image Base64ToImage(string base64String) {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length)) {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}