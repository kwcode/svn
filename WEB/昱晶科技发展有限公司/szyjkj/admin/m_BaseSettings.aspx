<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_BaseSettings.aspx.cs" Inherits="admin_m_BaseSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-1.8.3.js" type="text/javascript"></script>
    <link href="/meditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="/meditor/third-party/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="/meditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/meditor/umeditor.min.js"></script>
    <script type="text/javascript" src="/meditor/lang/zh-cn/zh-cn.js"></script>
    <link href="/admin/style/admin.css" rel="stylesheet" type="text/css" />
    <script src="/js/layer.js" type="text/javascript"></script>
    <link href="/style/layer.css" rel="stylesheet" type="text/css" />
    <script>
        $(function () {
            var ue1 = UM.getEditor('txt_TopContent'); //实例化编辑器  
            var ue2 = UM.getEditor('txt_BottomContent'); //实例化编辑器 
            if (typeof (jsonbase) != 'undefined' && jsonbase.length > 0) {
                $("#txt_SiteName").val(jsonbase[0].SiteName);
                $("#txt_SiteTitle").val(jsonbase[0].SiteTitle);
                $("#txt_KeyWords").val(jsonbase[0].KeyWords);
                $("#txt_Description").val(jsonbase[0].Description);
                ue1.setContent(jsonbase[0].TopContent);
                ue2.setContent(jsonbase[0].BottomContent);
            }
            $("#btn_ok").click(function () {
                /**/
                var _layer = $.layer({ type: 3 });
                $.post("/admin/action/actionadmin.aspx", {
                    action: "UpdateBaseSite",
                    SiteName: $("#txt_SiteName").val(),
                    SiteTitle: $("#txt_SiteTitle").val(),
                    KeyWords: $("#txt_KeyWords").val(),
                    Description: $("#txt_Description").val(),
                    TopContent: encodeURIComponent(ue1.getContent()),
                    BottomContent: encodeURIComponent(ue2.getContent())
                }).success(function (result) {
                    layer.close(_layer);
                    if (result.res == 1) {
                        layer.alert(result.desc, 1);
                    }
                    else { layer.alert(result.desc, 9); }

                }).fail(function (ex) { layer.close(_layer); layer.alert("请求失败," + ex.responseText, 9); });
                /**/
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="e_box">
            <dl>
                <dt>网站名称：</dt>
                <dd>
                    <input id="txt_SiteName" type="text" />
                </dd>
            </dl>
            <dl>
                <dt>网站标题：</dt>
                <dd>
                    <input id="txt_SiteTitle"  style="width:500px" type="text" />
                </dd>
            </dl>
            <dl>
                <dt>网站关键字：</dt>
                <dd>
                    <input id="txt_KeyWords"  style="width:500px" type="text" />
                </dd>
            </dl>
            <dl>
                <dt>网站描述：</dt>
                <dd>
                    <textarea id="txt_Description" class="textarea1"></textarea>
                </dd>
            </dl>
            <dl class="clearfix">
                <dt>网站顶部内容：</dt>
                <dd>
                    <div class="clearfix" style="float: left">
                        <script id="txt_TopContent" type="text/plain" style="width: 1000px; max-height: 400px; min-height: 150px;"></script>
                    </div>
                </dd>
            </dl>
            <dl class="clearfix">
                <dt>网站底部内容：</dt>
                <dd>
                    <div class="clearfix" style="float: left">
                        <script id="txt_BottomContent" type="text/plain" style="width: 1000px; max-height: 400px; min-height: 150px;"></script>
                    </div>
                </dd>
            </dl>
            <div class="btn_box">
                <input value="提交" id="btn_ok" type="button" class="inpbbut3" />
            </div>
        </div>

    </form>
</body>
</html>
