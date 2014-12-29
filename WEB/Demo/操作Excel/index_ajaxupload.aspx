<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index_ajaxupload.aspx.cs" Inherits="index_ajaxupload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="js/ajaxfileupload.js"></script>
    <script>
        $(function () {

            $("#ufile").change(function () {
                /*Start*/
                $.ajaxFileUpload({
                    url: 'action/actionIndex.aspx?action=ajaxuploadfile', //用于文件上传的服务器端请求地址
                    //data: { action: 'ajaxuploadfile' },// 自定义参数。这个东西比较有用，当有数据是与上传的图片相关的时候，这个东西就要用到了。 //暂时还无法知道怎么用
                    secureuri: false,//是否需要安全协议，一般设置为false
                    fileElementId: 'ufile', //文件上传域的ID
                    dataType: 'json', //返回值类型 一般设置为json 
                    type: "POST",//当要提交自定义参数时，这个参数要设置成post
                    success: function (result) {
                        /*提交成功*/
                        if (result.res == 1) {
                            //alert(result.desc);
                            $("#img_file").attr("src", result.desc);
                        }
                        else {
                            alert(result.desc);
                        }
                        /*提交成功END*/
                    },//提交成功后自动执行的处理函数，参数data就是服务器返回的数据。
                    error: function () { alert("提交失败！"); }//  提交失败自动执行的处理函数。
                });
                /*END*/

            });
        });
    </script>
</head>

<body>
    <form id="form1" action="index.aspx" runat="server">
        <div>
            <input type="file" accept="xls/*" name="file" id="ufile" />
            <input type="submit" value="上传文件" />
        </div>
        <img id="img_file" src="" width="100" height="100" />
    </form>
</body>
</html>
