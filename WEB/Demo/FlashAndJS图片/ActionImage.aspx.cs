using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActionImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {

            case "saveimg":
                SaveImage();
                break;
            default:
                break;
        }
    }

    private void SaveImage()
    {
        string path = Request.Form["path"];
        int w = Convert.ToInt32(Request["w"].ToString());
        int h = Convert.ToInt32(Request["h"].ToString());
        int x = Convert.ToInt32(Request["x"].ToString());
        int y = Convert.ToInt32(Request["y"].ToString());
        //  byte[] CropImage = CropImg(path, w, h, x, y);
        System.Drawing.Image img = Base64ToImage(path);
        img.Save(Server.MapPath("images/aa.jpg"));
        System.Drawing.Image Coroimg = CoropImg(img, w, h, x, y);//获取裁剪的图片
        Coroimg.Save(Server.MapPath("images/cut_aa.jpg"));
        //
        Bitmap map = GetThumbnailImage(Coroimg, ThumbnailImageType.Cut, 120, 120);
        map.Save(Server.MapPath("images/small_aa.jpg"));
    }
    static byte[] CropImg(string img, int width, int height, int x, int y)
    {
        try
        {
            using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(img))
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                    using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, width, height), x, y, width, height, System.Drawing.GraphicsUnit.Pixel);
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, OriginalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
    public System.Drawing.Image CoropImg(System.Drawing.Image img, int width, int height, int x, int y)
    {
        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
        {
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bmp))
            {
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.DrawImage(img, new System.Drawing.Rectangle(0, 0, width, height), x, y, width, height, System.Drawing.GraphicsUnit.Pixel);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, img.RawFormat);
                System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true);
                return CroppedImage;
            }
        }
    }
    //转换图片
    public System.Drawing.Image Base64ToImage(string base64String)
    {
        base64String = base64String.Replace("data:image/jpeg;base64,", "");
        byte[] imageBytes = Convert.FromBase64String(base64String);
        MemoryStream ms = new MemoryStream(imageBytes, 0,
          imageBytes.Length);
        ms.Write(imageBytes, 0, imageBytes.Length);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        return image;
    }
    //等比缩放图片
    /// <summary>
    /// 根据指定大小绘制高质量缩略图
    /// </summary>
    /// <param name="original_image"></param>
    /// <param name="new_width"></param>
    /// <returns></returns>
    public static Bitmap GetThumbnailImage(System.Drawing.Image original_image, ThumbnailImageType titCase, int target_width, int target_height)
    {
        System.Drawing.Graphics graphic = null;
        Bitmap returnImage = null;

        int origin_width = original_image.Width;
        int origin_height = original_image.Height;
        int paste_x = 0, paste_y = 0;

        NewImageSize imageSize = new NewImageSize(original_image.Width, original_image.Height, target_width, target_height);
        int new_width = imageSize.NewWidth, new_height = imageSize.NewHeight;
        target_width = imageSize.NewWidth;
        target_height = imageSize.NewHeight;
        float origin_ratio = imageSize.OrignRatio;
        float target_ratio = imageSize.NewRatio;

        //缩略图生成方式
        switch (titCase)
        {
            //缩放全图到正确比例
            case ThumbnailImageType.Zoom:
                if (target_ratio > origin_ratio)
                {
                    //较宽图片
                    new_height = (int)Math.Floor((float)target_width / origin_ratio);
                    new_width = target_width;
                }
                else
                {
                    //较高图片
                    new_height = target_height;
                    new_width = (int)Math.Floor(origin_ratio * (float)target_height);
                }

                new_width = new_width > target_width ? target_width : new_width;
                new_height = new_height > target_height ? target_height : new_height;
                break;

            //截取高宽比中间部分
            case ThumbnailImageType.Cut:
                if (target_ratio > origin_ratio)
                {
                    //较高图片
                    new_height = (int)Math.Floor((float)target_width / origin_ratio);
                    new_width = target_width;
                }
                else
                {
                    //较宽图片
                    new_height = target_height;
                    new_width = (int)Math.Floor(origin_ratio * (float)target_height);
                }
                break;
        }

        paste_x = (target_width - new_width) / 2;
        paste_y = (target_height - new_height) / 2;

        // 创建缩略图
        returnImage = new System.Drawing.Bitmap(target_width, target_height);

        graphic = System.Drawing.Graphics.FromImage(returnImage);
        //指定画布大小并以透明色填充空余部分
        graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, target_width, target_height));

        //设置高质量绘画
        graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //输出缩略图
        graphic.DrawImage(original_image, paste_x, paste_y, new_width, new_height);

        return returnImage;
    }
    /// <summary>
    /// 用于计算新的图片大小
    /// </summary>
    private class NewImageSize
    {
        public int OrignWidth = 0;

        public int OrignHeight = 0;

        public float OrignRatio = 0;

        public int NewWidth = 0;

        public int NewHeight = 0;

        public float NewRatio = 0;

        public NewImageSize(int OrignWidth, int OrignHeight, int NewWidth, int NewHeight)
        {
            this.OrignWidth = OrignWidth;
            this.OrignHeight = OrignHeight;
            this.OrignRatio = (float)OrignWidth / (float)OrignHeight;

            this.NewWidth = NewWidth;
            this.NewHeight = NewHeight;
            if (this.NewWidth > 0 && this.NewHeight <= 0)
            {
                this.NewHeight = (int)Math.Floor((float)this.NewWidth / this.OrignRatio);
            }
            else if (this.NewHeight > 0 && this.NewWidth <= 0)
            {
                this.NewWidth = (int)Math.Floor((float)this.NewHeight * this.OrignRatio);
            }
            else if (this.NewHeight <= 0 && this.NewWidth <= 0)
            {
                throw new Exception("新的图片大小不能都小于零");
            }

            if (this.NewWidth >= this.OrignWidth)
            {
                this.NewWidth = this.OrignWidth;
            }

            if (this.NewHeight >= this.OrignHeight)
            {
                this.NewHeight = this.OrignHeight;
            }

            this.NewRatio = (float)this.NewWidth / (float)this.NewHeight;
        }
    }
    public enum ThumbnailImageType
    {
        /// <summary>
        /// 缩放生成
        /// </summary>
        Zoom = 1,
        /// <summary>
        /// 截取生成
        /// </summary>
        Cut = 2
    }

}