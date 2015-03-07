<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aboutme.aspx.cs" Inherits="admin_aboutme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title> 
    <script src="../js/jquery-1.8.3.js"></script>
    <link href="../meditor/themes/default/css/umeditor.css" rel="stylesheet" /> 
    <script src="../meditor/third-party/jquery.min.js"></script>
    <script src="../meditor/umeditor.min.js"></script>
    <script src="../meditor/umeditor.config.js"></script>
    <script src="/meditor/lang/zh-cn/zh-cn.js"></script>

    <script src="../js/layer.js"></script>
    <link href="style/admin.css" rel="stylesheet" />
    <link href="../style/style.css" rel="stylesheet" />
    <link href="../style/css.css" rel="stylesheet" />
    <link href="../style/layer.css" rel="stylesheet" />
    <script>
        $(function () {
            var ue = UM.getEditor('editor'); //实例化编辑器  
            var details = '<%=Details%>';
            ue.setContent(details);
            $("#btn_ok").click(function () {
                var _layer = $.layer({ type: 3 });
                var summary = $(".txt_summary").val();
                var details = encodeURIComponent(ue.getContent());

                /**/
                $.ajax("/admin/action/actionadmin.aspx", {
                    data: {
                        action: "saveaboutme",
                        summary: '',
                        details: details
                    },type:'POST'
                }).success(function (result) {
                    if (result.res == 1) {
                        alert(result.desc);
                    }
                    else { alert(result.desc); }
                    layer.close(_layer);
                });
                /**/
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 20px; border-bottom: 1px solid #000; margin-bottom: 10px;">
            关于我们
        </div>
        <div style="margin-bottom: 10px;">
            <input type="button" id="btn_ok" class="inpbbut1" value="保存" />

        </div>
        <div class="e-item">
            <div style="float: left;">
                <script id="editor" type="text/plain" style="width: 1200px; margin-top: 5px; min-height: 350px;"></script>
            </div>
        </div>

    </form>
</body>
</html>
