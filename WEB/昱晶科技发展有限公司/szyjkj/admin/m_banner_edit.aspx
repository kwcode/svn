<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_banner_edit.aspx.cs" Inherits="admin_m_banner_edit" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                var _layer = $.layer({ type: 3 });
                var img = encodeURIComponent($("#img-adres").attr("src")); //$.tw.getimgaddress();
                var imgtype = $.tw.getimgtype();
                // alert(imgtype + img);
                var title = $("#txt_title").val();
                var showindex = $("#txt_showindex").val();
                //saverebanner
                $.ajax({
                    url: "/admin/action/actionadmin.aspx",
                    type: "POST",
                    data: {
                        action: "saverebanner",
                        img: img,
                        imgtype: imgtype,
                        title: title,
                        showindex: showindex
                    }, dataType: "json",
                    success: function (result) {
                        if (result.res > 0) {
                            alert(result.desc);
                            location.href = "/admin/m_banner.aspx";
                            layer.close(_layer);
                        }
                        else {
                            alert(result.desc);
                            layer.close(_layer);
                        }
                    }, fail: function () { alert("请求失败！"); }
                });
            });
        });

    </script>
</head>
<body>
    <form id="form2" runat="server">
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
