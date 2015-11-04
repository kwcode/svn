<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="demo.aspx.cs" Inherits="OutImg.demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="ueditor/ueditor.config.js"></script>
    <script src="ueditor/ueditor.all.js"></script>
    <script>
        $(function () {
            var ue = UE.getEditor('editor', {
                allowDivTransToP: false
            });
            ue.ready(function () {
                var c = $("#hide_Content").html();
                this.setContent(c)
            })

            $("#btn_ok").click(function () {
                var content = ue.getContent();
                $.post("/demoHandler.ashx", {
                    action: "SaveDemo",
                    content: encodeURIComponent(content)
                }).done(function (result) {
                    alert(result);
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>
    <script id="editor" name="editor" type="text/plain">
        这里写你的初始化内容
    </script>

    <input type="button" value="保存" id="btn_ok" />

    <div id="hide_Content" style="display: none;"><%=Content%></div>
</body>
</html>
