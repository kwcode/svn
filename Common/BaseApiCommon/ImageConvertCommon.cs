using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;

namespace BaseApiCommon
{
    /// <summary>
    /// 提供图片的转换
    /// 【注意图片的路径】
    /// </summary>
    public class ImageConvertCommon
    {


        #region 获取本地图片

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="path">文件的路径</param> 
        public static byte[] GetImage1(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);//可以是其他重载方法   
            byte[] buff = new byte[fs.Length];
            fs.Read(buff, 0, buff.Length);
            fs.Close();
            return buff;
        }
        /// <summary>
        /// 获取图片
        /// </summary> 
        public static BitmapImage GetImage2(string path)
        {
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(uri);
            return bitmap;
        }
        /// <summary>
        /// 获取Base64图片
        /// </summary> 
        public static string GetImage64(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);//可以是其他重载方法   
            byte[] buff = new byte[fs.Length];
            fs.Read(buff, 0, buff.Length);
            fs.Close();
            string base64 = Convert.ToBase64String(buff);
            return base64;
        }

        #endregion

        #region 获取下载后的图片
        public static byte[] GetUploadImage1(string imgUrl)
        {
            WebClient client = new WebClient();
            byte[] buff = client.DownloadData(imgUrl);
            return buff;
        }

        public static BitmapImage GetUploadImage2(string imgUrl)
        {
            byte[] buff = GetUploadImage1(imgUrl);
            BitmapImage bmp = ConvertByteToBitmapImage(buff);
            return bmp;
        }
        public static string GetUploadImage64(string imgUrl)
        {
            byte[] buff = GetUploadImage1(imgUrl);
            return Convert.ToBase64String(buff);
        }
        #endregion

        #region 转换
        /// <summary>
        /// 图片流转换成WPF专有的格式
        /// </summary>
        /// <param name="buff">图片流</param>
        /// <returns></returns>
        public static BitmapImage ConvertByteToBitmapImage(byte[] buff)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(buff);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }
        /// <summary>
        /// 图片转换流
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] ConvertBitmapImageToByte(BitmapImage bmp)
        {
            byte[] buff = null;
            Stream stream = bmp.StreamSource;
            if (stream != null && stream.Length > 0)
            {
                stream.Position = 0;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buff = br.ReadBytes((int)stream.Length);
                }
            }
            return buff;
        }

        /// <summary>
        /// 转换
        /// </summary> 
        public static BitmapImage ConvertBase64ToBitmapImage(string base64String)
        {
            byte[] buff = Convert.FromBase64String(base64String);
            return ConvertByteToBitmapImage(buff);
        }

        /// <summary>
        /// 转换
        /// </summary> 
        public static byte[] ConvertBase64ToByte(string base64String)
        {
            return Convert.FromBase64String(base64String);
        }
        #endregion
    }
}
