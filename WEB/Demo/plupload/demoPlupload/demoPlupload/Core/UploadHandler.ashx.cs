using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
 
using System.Web;

namespace trip
{
    /// <summary>
    /// UploadHandler1 的摘要说明
    /// </summary>
    public class UploadHandler : BaseHandler
    {
        ResponseData responseData = new ResponseData()
        {
            code = ReturnCode.error,
            msg = "未配置",
            data = null
        };
        protected override ResponseData ProcessResponse(HttpContext context)
        {
            Response.CacheControl = "no-cache";
            string dir = GetUploadPath();
            if (Request.Files.Count > 0)
            {
                try
                {
                    for (int j = 0; j < Request.Files.Count; j++)
                    {
                        int offset = Convert.ToInt32(Request["chunk"]); //当前分块
                        int total = Convert.ToInt32(Request["chunks"]);//总的分块数量
                        string name = Request["name"];
                        HttpPostedFile uploadFile = Request.Files[j];
                        if (total == 1)
                        {
                            if (uploadFile.ContentLength > 0)
                            {
                                string extname = Path.GetExtension(uploadFile.FileName);//.jpg
                                string filename = uploadFile.FileName; //xx.jpg
                                string path = dir + RndNum(6) + extname;
                                uploadFile.SaveAs(context.Server.MapPath(path));
                                responseData.msg = "上传成功";
                                responseData.data = path;
                            }
                        }
                        else
                        {
                            //文件 分成多块上传
                            string tempPath = WriteTempFile(uploadFile, offset);
                            if (total - offset == 1)
                            {
                                //如果是最后一个分块文件 ，则把文件从临时文件夹中移到上传文件夹中

                                string extname = Path.GetExtension(name);//.jpg
                                string newPath = dir + RndNum(6) + extname;
                                System.IO.FileInfo fi = new System.IO.FileInfo(context.Server.MapPath(tempPath));//临时文件名 
                                //把临时文件移动到新目录并重名
                                fi.MoveTo(context.Server.MapPath(newPath));
                                responseData.msg = "上传成功";
                                responseData.data = newPath;
                            }
                            else
                            {
                                responseData.msg = "上传成功";
                                responseData.data = tempPath;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseData.msg = ex.Message;
                    responseData.data = ex.ToString();
                }

            }
            return responseData;
        }

        /// <summary>
        /// 保存临时文件-返回相对路径
        /// </summary>
        /// <param name="uploadFile">文件流</param>
        /// <param name="chunk">第几个分块 从0开始</param>
        /// <param name="context"></param>
        /// <returns></returns>
        private string WriteTempFile(HttpPostedFile uploadFile, int chunk)
        {
            string tempPath = GetTempPath() + uploadFile.FileName + ".part"; //临时相对路径
            string saveTempPath = HttpContext.Current.Server.MapPath(tempPath);
            if (chunk == 0)
            {
                uploadFile.SaveAs(saveTempPath); //如果是第一个分块，则直接保存
            }
            else
            {
                //如果是其他分块文件 ，则原来的分块文件，读取流，然后文件最后写入相应的字节
                FileStream fs = new FileStream(saveTempPath, FileMode.Append);
                if (uploadFile.ContentLength > 0)
                {
                    int FileLen = uploadFile.ContentLength;
                    byte[] input = new byte[FileLen];
                    System.IO.Stream MyStream = uploadFile.InputStream;
                    MyStream.Read(input, 0, FileLen);
                    fs.Write(input, 0, FileLen);
                    fs.Close();
                }
            }

            return tempPath;
        }

        /// <summary>
        /// 该方法用于生成指定位数的随机数
        /// </summary>
        /// <param name="VcodeNum">参数是随机数的位数</param>
        /// <returns>返回一个随机数字符串</returns>
        public string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            //string Vchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(',');//拆分成数组

            string VNum = "";
            seed++;
            Random rand = new Random(seed);
            for (int i = 0; i < VcodeNum; i++)
            {
                VNum += VcArray[rand.Next(VcArray.Length - 1)];
            }
            return VNum;
        }
        private static int _seed = int.Parse(DateTime.Now.Ticks.ToString().Substring(10));
        private static int seed { get { return _seed; } set { _seed = value; } }

        //获取存放路径
        public static string GetUploadPath()
        {
            string path = HttpContext.Current.Server.MapPath("/upload/");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return "/upload/";
        }
        /// <summary>
        /// 获取临时目录 相对路径
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            string path = GetUploadPath();
            path = HttpContext.Current.Server.MapPath("/upload/temp/");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return "/upload/temp/";
        }
    }
}