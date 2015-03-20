<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_banner.aspx.cs" Inherits="admin_m_banner" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-1.8.3.js"></script>
    <link href="/admin/style/admin.css" rel="stylesheet" />
    <script src="/js/layer.js" type="text/javascript"></script>
    <link href="/style/layer.css" rel="stylesheet" type="text/css" />
    <link href="/style/css.css" rel="stylesheet" />
    <script src="/js/jquery-uploadimg.js"></script>
    <script>
        $(function () {
            $.tw.loadimg();

            $("#btn_ok").click(function () {
                var img = $.tw.getimgaddress();
                var imgtype = $.tw.getimgtype();
                // alert(imgtype + img);
                var title = $("#txt_title").val();
                var showindex = $("#txt_showindex").val();
                //saverebanner
                $.post("/admin/action/actionadmin.aspx",
                    {
                        action: "saverebanner",
                        img: img,
                        imgtype: imgtype,
                        title: title,
                        showindex: showindex
                    },
                    function (result) {
                        alert(result.desc);
                    })
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float: left" class="d-admin-content">
            <div class="e-item">
                <span class="sp150">标题：</span>
                <input type="text" maxlength="200" class="txt_title" runat="server" id="txt_title" />
            </div>
            <div class="e-item">
                <span class="sp150">排序：</span>
                <input type="text" maxlength="5" class="txt_showindex" runat="server" id="txt_showindex" />
            </div>
            <div class="e-item">
                <span class="sp150">图片：</span>
                <div class="u-button" style="float: left; width: 330px;">
                </div>

            </div>
            <div class="e-item u-imgaddress" style="margin-left: 150px; margin-top: 10px;">
            </div>
        </div>
        <div class="btn-content" style="margin-left: 200px;">
            <input type="button" id="btn_ok" class="inpbbut1" value="保存" />
        </div>
    </form>
</body>
</html>
