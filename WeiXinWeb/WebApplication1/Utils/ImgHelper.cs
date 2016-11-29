using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace WebApplication1.Utils
{
    public class ImgHelper
    {
        public static string Base64ToImg(string base64,string filePath)
        {
            int index = base64.IndexOf("base64,");
            if (index < 0)
            {
                return "";
            }
            base64 = base64.Substring(index + 7);
            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream memStream = new MemoryStream(bytes);
           
            //BinaryFormatter binFormatter = new BinaryFormatter();
            //Image img = (Image)binFormatter.Deserialize(memStream);

            //string extName = ImgExtName(base64.Substring(0, 10));
            string imgName = Guid.NewGuid().ToString("N")+".jpg";
            //img.Save(filePath+imgName);

            Bitmap bitmap = new Bitmap(memStream);
            bitmap.Save(filePath + imgName);
            //System.Drawing.Bitmap bp = new System.Drawing.Bitmap(filePath + imgName);
            //bp.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return imgName;
        }


        private static string ImgExtName(string base64)
        {
            string exName = "jpg";
            if (base64.Contains("image/gif"))
            {
                exName = ".gif";
            }else if(base64.Contains("image/png"))
            {
                exName = ".png";
            }
            else if (base64.Contains("image/icon"))
            {
                exName = ".icon";
            }
            return exName;
        }
    }
}