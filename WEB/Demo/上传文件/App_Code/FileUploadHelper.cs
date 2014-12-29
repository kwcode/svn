using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// FileUploadHelper 的摘要说明
/// </summary>
public class FileUploadHelper
{
    public FileUploadHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //



    }
    /// <summary>
    /// 传入文件名称，得到完整的服务端路径 用于保存
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFullPath(string fileName)
    {
        string fullPath = "";
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/file/") + fileName))
            Directory.Exists(HttpContext.Current.Server.MapPath("~/file/") + fileName);
        //fileControls.PostedFile.SaveAs(imgsrc + extion);
        fullPath = HttpContext.Current.Server.MapPath("~/file/") + fileName;
        return fullPath;

    }
    /// <summary>
    /// 获取相对路径
    /// </summary>
    /// <param name="fileName"></param>
    public static string GetPath(string fileName)
    {
        string Path = "file/" + fileName;
        return Path;
    }
}