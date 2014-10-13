using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

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
            default:
                break;
        }
    }

    private void UploadImage()
    {
        string fileName = Request["filename"] ?? ""; 

        //Stream str = GetPostedFileSteam();

        //System.Drawing.Image img = System.Drawing.Image.FromStream(str);
        string extion = System.IO.Path.GetExtension(fileName);
        string path = Server.MapPath("images/" + RndNum(8) + "." + extion);//转换成服务器上的物理路径 
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