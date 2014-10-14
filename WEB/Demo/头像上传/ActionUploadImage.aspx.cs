using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Drawing.Drawing2D;

public partial class ActionUploadImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "uploadImage":
                UploadImage();
                break;
            case "saveimg":
                SaveImage();
                break;
            default:
                break;
        }
    }

    private void SaveImage()
    {
        string path = Server.MapPath(Request["path"]);
        int w = Convert.ToInt32(Request["w"].ToString());
        int h = Convert.ToInt32(Request["h"].ToString());
        int x = Convert.ToInt32(Request["x"].ToString());
        int y = Convert.ToInt32(Request["y"].ToString());
        byte[] CropImage = CropImg(path, w, h, x, y);
        using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
        {
            ms.Write(CropImage, 0, CropImage.Length);
            using (System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true))
            {
                string saveTo = string.Format("images/{0}.jpg", Guid.NewGuid().ToString().Replace("-", ""));
                CroppedImage.Save(Server.MapPath(saveTo), CroppedImage.RawFormat);
                Response.Write(saveTo);
            }
        }
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
    private void UploadImage()
    {
        string fileName = Request["filename"] ?? "";

        string extion = System.IO.Path.GetExtension(fileName);
        string path = Server.MapPath("images/" + RndNum(8) + "." + extion);//转换成服务器上的物理路径 

        this.Response.ContentType = "text/plain";
        if (this.Request.Files.Count > 0)
        {
            var file = this.Request.Files[0];
            System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
        }
        //Stream str = GetPostedFileSteam();

        //System.Drawing.Image img = System.Drawing.Image.FromStream(str);

        //img.Save(path);
        ////System.Web.UI.HtmlControls.HtmlInputFile fileControls = (System.Web.UI.HtmlControls.HtmlInputFile)(object)(Request["filecontrols"]);
        ////fileControls.PostedFile.SaveAs(path);
        ////HttpFileCollection files = HttpContext.Current.Request.Files;
        ////HttpPostedFile fuImage = files[0];
        ////fuImage.SaveAs(path);
        ////fuImage.PostedFile.SaveAs(path);//保存图片
        ///创建WebClient实例 
        WebClient myWebClient = new WebClient();
        //设定windows网络安全认证 方法1
        myWebClient.Credentials = CredentialCache.DefaultCredentials;
        //要上传的文件 
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        BinaryReader r = new BinaryReader(fs);
        byte[] postArray = r.ReadBytes((int)fs.Length);
        Stream postStream = myWebClient.OpenWrite(path, "PUT");
        if (postStream.CanWrite)
        {
            postStream.Write(postArray, 0, postArray.Length);
        }
        Response.Clear();
        Response.Write(path);
        Response.End();
    }
    private Stream GetPostedFileSteam()
    {
        if (Request.Browser.Browser == "IE"
            && System.Web.HttpContext.Current.Request.Files != null
            && System.Web.HttpContext.Current.Request.Files.Count > 0)
        {
            var postedFile = System.Web.HttpContext.Current.Request.Files[0];
            return postedFile.InputStream;
        }
        else
        {
            return Request.InputStream;
        }
    }
    public static string RndNum(int VcodeNum)
    {
        string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] VcArray = Vchar.Split(new char[]
			{
				','
			});
        string VNum = "";
        int seed = int.Parse(DateTime.Now.Ticks.ToString().Substring(10));
        Random rand = new Random(seed);
        for (int i = 0; i < VcodeNum; i++)
        {
            VNum += VcArray[rand.Next(VcArray.Length - 1)];
        }
        return VNum;
    }
}