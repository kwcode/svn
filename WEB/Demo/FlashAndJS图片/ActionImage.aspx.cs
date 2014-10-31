using System;
using System.Collections.Generic;
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
        string path = Request["path"];
        int w = Convert.ToInt32(Request["w"].ToString());
        int h = Convert.ToInt32(Request["h"].ToString());
        int x = Convert.ToInt32(Request["x"].ToString());
        int y = Convert.ToInt32(Request["y"].ToString());
        //  byte[] CropImage = CropImg(path, w, h, x, y);
        System.Drawing.Image img = Base64ToImage(path);
        img.Save(Server.MapPath("images/aa.jpg"));
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
    public System.Drawing.Image Base64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        MemoryStream ms = new MemoryStream(imageBytes, 0,
          imageBytes.Length);
        ms.Write(imageBytes, 0, imageBytes.Length);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        return image;
    }

}