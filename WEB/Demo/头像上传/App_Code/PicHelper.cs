using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;


public class PicHelper
{
    private int _x = 0;
    private int _y = 0;
    public Image Picture { set; get; }
    public int OriginalWidth { set; get; }

    public int OriginalHeight { set; get; }

    public int ThumbHeight { set; get; }
    public int ThumbWidth { set; get; }
    public string Mode { set; get; }

    public int X
    {
        get { return _x; }
    }

    public int Y
    {
        get { return _y; }
    }

    public PicHelper(Image image)
    {
        Picture = image;
        OriginalHeight = Picture.Height;
        OriginalWidth = Picture.Width;
    }

    public PicHelper(Image image, int thumbHeight, int thumbWidth, string mode = "A")
    {
        Picture = image;
        OriginalHeight = Picture.Height;
        OriginalWidth = Picture.Width;
        ThumbHeight = thumbHeight;
        ThumbWidth = thumbWidth;
        Mode = mode;
        SetThumbSize();
    }

    private void SetThumbSize()
    {
        switch (Mode.ToUpper())
        {
            case "A": //默认，如果宽大于高，按宽缩放。如高大于宽，按高缩放
                if (OriginalWidth > OriginalHeight)
                {
                    ThumbHeight = OriginalHeight * ThumbWidth / OriginalWidth;
                }
                else
                {
                    ThumbWidth = OriginalWidth * ThumbHeight / OriginalHeight;

                }
                break;
            case "HW": //指定高宽缩放（可能变形）　　　　　　　　 
                break;
            case "W": //指定宽，高按比例　　　　　　　　　　 
                ThumbHeight = OriginalHeight * ThumbWidth / OriginalWidth;
                break;
            case "H": //指定高，宽按比例 
                ThumbWidth = OriginalWidth * ThumbHeight / OriginalHeight;
                break;
            case "CUT": //修剪　　　　　　　　 
                if ((double)OriginalWidth / (double)OriginalHeight > (double)ThumbWidth / (double)ThumbHeight)
                {
                    //originalHeight = originalImage.Height;
                    _y = 0;
                    OriginalWidth = OriginalHeight * ThumbWidth / ThumbHeight;
                    _x = (Picture.Width - OriginalWidth) / 2;


                }
                else
                {
                    OriginalHeight = OriginalWidth * ThumbHeight / ThumbWidth;
                    _x = 0;
                    _y = (Picture.Height - OriginalHeight) / 2;
                }
                break;
            default:
                break;
        }
    }

    public void CreateNewPic(string thumbName)
    {
        //新建一个bmp图片 
        //新建一个画板 
        //设置高质量插值法 
        using (Image bitmap = new Bitmap(ThumbWidth, ThumbHeight))
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(Picture, new Rectangle(0, 0, ThumbWidth, ThumbHeight),
                        new Rectangle(X, Y, OriginalWidth, OriginalHeight),
                        GraphicsUnit.Pixel);
            bitmap.Save(thumbName);
        }
    }
}
