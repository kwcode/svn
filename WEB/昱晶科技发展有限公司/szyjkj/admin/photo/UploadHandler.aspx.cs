using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_photo_UploadHandler : AjaxBase
{
    int _res = 0;//0失败 1成功
    object _msg = "上传失败！";//消息
    string _path = "/";//相对路径
    protected override object loadAjaxData()
    {
        HttpPostedFile file = Request.Files["Filedata"];
        string ext = System.IO.Path.GetExtension(file.FileName);
        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
        float size = file.ContentLength > 0 ? file.ContentLength / 1024 : 0;
        string folder = Request["folder"] ?? "uploads";
        //string uploadPath = HttpContext.Current.Server.MapPath(folder);

        int bookid = 0;
        int.TryParse(Request["bookid"], out bookid);
        System.Drawing.Image orig_image = System.Drawing.Image.FromStream(file.InputStream);
        try
        {
            //创建最小200*200的原始缩略图
            System.Drawing.Image tn_image = ImageHelper.GetThumbnailImage(orig_image, ThumbnailImageType.Cut, 200, 200);

            //创建摄影栏目专用浏览图:制定最大宽度为206,高度随宽度比例缩减.
            //thumbnailForPC_image = ImageHelper.GetThumbnailImage(original_image, ThumbnailImageType.Cut, 206, 143);
            //pc改为　225*150
            System.Drawing.Image show_image = ImageHelper.GetThumbnailImage(orig_image, ThumbnailImageType.Cut, 310, 213);
            string codestr = GetCodeString(8);
            string Tn = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Picture, "Tn") + codestr + ext;
            string Show = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Picture, "Show") + codestr + ext;
            string Orig = SingleFileUpload.GetServiceUrl(SingleFileUpload.fileNameIndex.Picture, "Orig") + codestr + ext;
            orig_image.Save(Server.MapPath(Tn));
            orig_image.Save(Server.MapPath(Show));
            orig_image.Save(Server.MapPath(Orig));
            _res = WSCommon.AddPhoto(UserID, bookid, fileName, size, ext, Tn, Show, Orig, "PC");
            _res = 1;
            _msg = "上传成功！";
            _path = "/" + folder + "/Tn/" + codestr + ext;
        }
        catch (Exception)
        {

            throw;
        }


        //string path = Path.Combine(uploadPath, codestr + ext);
        //if (file != null)
        //{
        //    if (!Directory.Exists(uploadPath))
        //    {
        //        Directory.CreateDirectory(uploadPath);
        //    }

        //    file.SaveAs(path);
        //    _res = 1;
        //    _msg = "上传成功！";
        //    _path = "/" + folder + "/" + codestr + ext;
        //}
        return new { res = _res, msg = _msg, path = _path };
    }
    //获取随机数字
    //获取随机验证码字符
    public string GetCodeString(int length)
    {
        string[] verifycodeRange = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y" };
        Random rnd = new Random();
        string result = "";
        for (int i = 0; i < length; i++)
        {
            int r = rnd.Next(0, verifycodeRange.Length);
            result += verifycodeRange[r];
        }
        return result;
    }
}