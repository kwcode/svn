<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;

public class UploadHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.Files.Count > 0)
        {
            var file = context.Request.Files[0];
            if (System.IO.Path.GetExtension(file.FileName).ToLower() != ".jpg" || System.IO.Path.GetExtension(file.FileName).ToLower() != "png")
            {

                string extion = System.IO.Path.GetExtension(file.FileName);
                string NumFileName = RndNum(8) + extion;
                string path = context.Server.MapPath("~/images/" + NumFileName);//转换成服务器上的物理路径  
                //string filename = System.IO.Path.Combine(path, file.FileName);
                //file.SaveAs(filename);
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream))
                {
                    //将图片尺寸压缩在保存，宽度最大为600，高度最大为600
                    //按比例压缩

                    PicHelper helper = new PicHelper(image, 0, 480);
                    helper.CreateNewPic(path);
                    //image.Save(path);
                }
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var result = new { name = "/images/" + NumFileName };
                context.Response.Write(serializer.Serialize(result));
            }

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
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}