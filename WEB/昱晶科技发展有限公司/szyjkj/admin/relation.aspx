<%@ Page Language="C#" AutoEventWireup="true" CodeFile="relation.aspx.cs" Inherits="admin_relation" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-1.8.3.js" type="text/javascript"></script>
    <link href="/meditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="/meditor/third-party/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="/meditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/meditor/umeditor.min.js"></script>
    <script type="text/javascript" src="../../meditor/lang/zh-cn/zh-cn.js"></script>
    <link href="/admin/style/admin.css" rel="stylesheet" type="text/css" />
    <link href="/style/css.css" rel="stylesheet" type="text/css" />
    <script src="/js/layer.js" type="text/javascript"></script>
    <link href="/style/layer.css" rel="stylesheet" type="text/css" />

    <script>
        $(function () {
            var ue = UM.getEditor('editor'); //实例化编辑器  
            init();

            $("#btn_ok").click(function () {
                var _layer = $.layer({ type: 3 });
                var summary = $(".txt_summary").val();
                var details = encodeURIComponent(ue.getContent());

                /**/
                $.ajax("/admin/action/actionadmin.aspx", {
                    data: {
                        action: "saverelation",
                        summary: '',
                        details: details
                    }, type: 'POST'
                }).success(function (result) {
                    if (result.res == 1) {
                        alert(result.desc);
                    }
                    else { alert(result.desc); }
                    layer.close(_layer);

                });
                /**/
            });

            function init() {
                if (typeof (jsonrelation) != 'undefined' && jsonrelation.length > 0) {
                    ue.setContent(jsonrelation[0].Details);
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="e_box">
            <dl>
                <dt>内容</dt>
                <dd class="fl">
                    <script id="editor" type="text/plain" style="width: 1000px; max-height: 400px; min-height: 250px;"></script>
                </dd>
            </dl>
            <div class="btnbox">
                <input type="button" id="btn_ok" class="inpbbut1" value="保存" /></div>
        </div>
    </form>
</body>
</html>

