<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news_m_edit.aspx.cs" Inherits="admin_news_news_m_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../js/jquery-1.8.3.js" type="text/javascript"></script>
    <link href="../../meditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../../meditor/third-party/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../meditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../meditor/umeditor.min.js"></script>
    <script type="text/javascript" src="../../meditor/lang/zh-cn/zh-cn.js"></script>
    <link href="../style/admin.css" rel="stylesheet" type="text/css" />
    <link href="../../style/css.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <link href="../../style/layer.css" rel="stylesheet" type="text/css" />
    <script>
        $(function () {

            var id = '<%=ID%>';
            var ue = UM.getEditor('editor'); //实例化编辑器  
            init();



            $("#btn_ok").click(function () {
                var _layer = $.layer({ type: 3 });
                var title = $(".txt_title").val();
                var showindex = $(".txt_showindex").val();
                var summary = $(".txt_summary").val();
                var details = encodeURIComponent(ue.getContent());

                /**/
                $.ajax("/admin/news/actionnews.aspx", {
                    data: {
                        action: "savenew",
                        id: id,
                        title: title,
                        showindex: showindex,
                        summary: summary,
                        details: details
                    }, type: "POST"
                }).success(function (result) {
                    if (result.res == 1) {
                        alert(result.desc);
                        location.href = "/admin/news/news_manager_index.aspx";
                    }
                    else { alert(result.desc); }
                    layer.close(_layer);
                }).fail(function () { alert("请求失败！"); layer.close(_layer); });
                /**/
                //alert(details.toString());
            });
            //初始化
            function init() {
                if (typeof (jsonnews) != 'undefined' && jsonnews.length > 0) {
                    $(".txt_title").val(jsonnews[0].Title);
                    $(".txt_showindex").val(jsonnews[0].ShowIndex);
                    $(".txt_summary").val(jsonnews[0].Summary);
                    ue.setContent(jsonnews[0].Details);
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float: left">
            <div class="e-item">
                <span class="sp150">标题：</span>
                <input type="text" maxlength="200" class="txt_title" runat="server" id="txt_title" />
            </div>
            <div class="e-item">
                <span class="sp150">排序：</span>
                <input type="text" maxlength="5" class="txt_showindex" runat="server" id="txt_showindex" />
            </div>
            <div class="e-item">
                <span class="sp150">简介：</span>
                <textarea id="txt_summary" maxlength="1000" runat="server" class="txt_summary" cols="20"
                    rows="5"></textarea>
            </div>
            <div class="e-item">
                <span class="sp150">详细内容：</span>
                <div style="float: left;">
                    <script id="editor" type="text/plain" style="width: 700px; margin-top: 5px; max-height: 300px; min-height: 150px;"></script>
                </div>
            </div>
        </div>
        <div class="btn-content" style="margin-left: 200px; margin-top: 10px;">
            <input type="button" id="btn_ok" class="inpbbut1" value="确认" />
        </div>
    </form>
</body>
</html>
