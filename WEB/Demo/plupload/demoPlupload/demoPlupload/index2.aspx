<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index2.aspx.cs" Inherits="demoPlupload.index2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.min.js"></script>

      <script src="js/plupload/moxie.js"></script>
    <script src="js/plupload/plupload.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <button class="btn_upload" id="browse">test</button>
            <button class="btn_upload" id="btn_up">点击</button>
        </div>
    </form>
    <script>
        //$("#btn_up"). 
        //实例化一个plupload上传对象
        var uploader = new plupload.Uploader({
            browse_button: 'browse', //触发文件选择对话框的按钮，为那个元素id
            runtimes: 'html5,flash,silverlight,html4',
            url: '/Core/UploadHandler.ashx',
            flash_swf_url: '/js/plupload/Moxie.swf',
            silverlight_xap_url: '/js/plupload/Moxie.xap',
        });
        uploader.init();
        $(function () {
            $("#btn_up").click(function () {
                uploader.addFile();
            })

        })
    </script>
</body>
</html>
