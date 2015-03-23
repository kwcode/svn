using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// SingleFileUpload 的摘要说明
/// </summary>
public class SingleFileUpload
{
    public SingleFileUpload()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }


    /// <summary>
    /// 主要用于上传文件类型的图片，返回全路径
    /// </summary>
    /// <param name="fileControls"></param>
    /// <returns></returns>
    public static string Upload(System.Web.UI.HtmlControls.HtmlInputFile fileControls, fileNameIndex index)
    {
        string imgsrc = "";

        if (!string.IsNullOrEmpty(fileControls.PostedFile.FileName))
        {
            //判断文件是否小于10Mb  
            if (fileControls.PostedFile.ContentLength < 10485760)
            {
                try
                {
                    string file = ((fileNameIndex)index).ToString();
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString()))
                        Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString());

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));

                    //  string filename = DateTime.Now.ToString("HHmmss") + stringCoding.RndNum(6) + fileControls.PostedFile.FileName.Substring(fileControls.PostedFile.FileName.LastIndexOf('.'));
                    string filename = stringCoding.RndNum(8);
                    string extion = fileControls.PostedFile.FileName.Substring(fileControls.PostedFile.FileName.LastIndexOf('.')); ;
                    imgsrc = HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/")) + filename;
                    fileControls.PostedFile.SaveAs(imgsrc + extion);
                    imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + filename + extion;

                }
                catch (Exception ex)
                {

                }
            }
        }

        return imgsrc;
    }



    /// <summary>
    /// 根据传递进来的子路径和枚举类型返回全路径(主要处理用户上传图片)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="index"></param>
    /// <param name="img"></param>
    /// <param name="isdate">是否根据时间来创建文件夹</param>
    /// <returns></returns>
    public static string UploadSend(string path, fileNameIndex index, string img, bool isdate)
    {
        string imgsrc = "";

        if (!string.IsNullOrEmpty(path))
        {
            //判断文件是否小于10Mb  

            try
            {
                if (isdate)
                {

                    string file = index.ToString();
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + index.ToString()))
                        Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + index.ToString());

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));


                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")) + path))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + path));

                    imgsrc = HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + path + img);
                    imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + path + img;

                }
                else
                {
                    string file = ((fileNameIndex)index).ToString();
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + index.ToString()))
                        Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + index.ToString());
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + path)))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + path));

                    imgsrc = HttpContext.Current.Server.MapPath("~/upload/" + file + path + img);
                    imgsrc = "/upload/" + file + path + img;
                }

            }
            catch (Exception ex)
            {

            }
        }

        return imgsrc;
    }

    /// <summary>
    /// 上传头像用的重载方法
    /// 地址格式 upload/type/year/month/day/file
    /// </summary>
    /// <param name="index">上传图片类型</param>
    /// <returns></returns>
    public static string UploadSend(fileNameIndex index)
    {
        string imgsrc = "";
        try
        {

            string file = ((fileNameIndex)index).ToString();
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString()))
                Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString());

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));


            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));

            imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + stringCoding.RndNum(8);

        }
        catch (Exception ex)
        {

        }
        return imgsrc;

    }

    /// <summary>
    /// 返回的结果是
    /// /upload/type/yyyy/MM/dd/
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetServiceUrl(fileNameIndex index)
    {
        string imgsrc = "";
        string file = ((fileNameIndex)index).ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString()))
            Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString());

        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));


        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));

        imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/");
        return imgsrc;
    }
    /// <summary>
    /// 返回的结果是
    /// /upload/type/yyyy/MM/dd/floder/
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetServiceUrl(fileNameIndex index, string floder)
    {
        string imgsrc = "";
        string file = ((fileNameIndex)index).ToString();
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder)))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder));
        imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + floder + "/";
        return imgsrc;
    }

    /// <summary>
    /// 上传头像用的重载方法
    /// 地址格式 upload/type/year/month/day/floder/file.jpg
    /// </summary>
    /// <param name="index">上传图片类型</param>
    /// <returns></returns>
    public static string UploadSend(fileNameIndex index, string floder, out string newFileName)
    {
        newFileName = "";
        string imgsrc = "";
        try
        {

            string file = ((fileNameIndex)index).ToString();
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString()))
                Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString());

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));


            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder));
            newFileName = stringCoding.RndNum(8);
            imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + floder + "/" + newFileName;

        }
        catch (Exception ex)
        {

        }
        return imgsrc;

    }

    /// <summary>
    /// 上传头像用的重载方法
    /// 地址格式 upload/type/year/month/day/floder/file.jpg
    /// </summary>
    /// <param name="index">上传图片类型</param>
    /// <returns></returns>
    public static string UploadSend(fileNameIndex index, string floder)
    {
        string imgsrc = "";
        try
        {

            string file = ((fileNameIndex)index).ToString();
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString()))
                Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + ((fileNameIndex)index).ToString());

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/") + file + "/" + DateTime.Now.ToString("yyyy"));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM")));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));


            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd"))))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd")));

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + floder));

            imgsrc = "/upload/" + file + "/" + DateTime.Now.ToString("yyyy/MM/dd/") + floder + "/" + stringCoding.RndNum(8);
        }
        catch (Exception ex)
        {

        }
        return imgsrc;

    }



    /// <summary>
    /// 上传头像用的重载方法
    /// </summary>
    /// <param name="path"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string UploadSend_ServerPath(string path, fileNameIndex index, bool isdate)
    {
        // string imgsrc = UploadSend(path, index, isdate);
        string imgsrc = UploadSend(index);
        return HttpContext.Current.Server.MapPath(imgsrc);
    }

    /// <summary>
    /// 根据传递不多的使用类型创建枚举
    /// </summary>
    public enum fileNameIndex
    {
        Banner = 0,
        Product = 1,

        //Authentication = 0,  //身份认证
        //OperatingLicense = 1,  //运营许可证
        //DrivingLicense = 2,  //驾驶证
        //TravelLicense = 3, //行驶证
        //GuideCertification = 4,  //导游认证
        //Cooperation = 5,   //代理合作
        //UploadWorks = 6, //上传作品
        //UploadImg = 7, //上传图片
        //Picture = 8, //用户头像
        //Advertising = 9, //广告横幅的枚举
        //Blogroll = 10, //友情链接的枚举
        //OutlinkImg = 11, //外链图片的自动保存
        //ThemesImg = 12, //主题图片
        //LeaderPhoto = 13, //主题图片
        //ActivityPicture = 14,
        //TempPicture = 99,//临时图片 
    }
}

public class stringCoding
{
    public static string RndNum(int VcodeNum)
    {
        string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] VcArray = Vchar.Split(new char[]
			{
				','
			});
        string VNum = "";
        Random rand = new Random(VcodeNum);
        for (int i = 0; i < VcodeNum; i++)
        {
            VNum += VcArray[rand.Next(VcArray.Length - 1)];
        }
        return VNum;
    }
}