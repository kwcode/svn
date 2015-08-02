<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_productimg_edit.aspx.cs" Inherits="admin_products_m_productimg_edit" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-1.8.3.js"></script>
    <link href="/admin/style/admin.css" rel="stylesheet" />
    <script src="/js/layer.js" type="text/javascript"></script>
    <link href="/style/layer.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery-twExt.js"></script>
    <%--  <link href="/style/css.css" rel="stylesheet" />
    <script src="/js/jquery-uploadimg.js"></script>--%>
    <script>
        $(function () {
            var id = '<%=ID%>';
            init();

            $(".btn_uploadimg").click(function () {
                $.tw.photo.uploadImage({ single: true, area: ['800px', '400px'] }).done(function (result) {
                    var tn = result.result[0].tn;
                    var id = result.result[0].id;
                    $(".u-imgaddress").prop("src", tn);
                });
            });


            $("#btn_ok").click(function () {
                var _layer = $.layer({ type: 3 });
                var img = $(".u-imgaddress").prop("src");
                var title = $("#txt_title").val();
                var showindex = $("#txt_showindex").val();
                var proid = $("#sel_type").val();
                $.ajax({
                    url: "/admin/action/actionadmin.aspx",
                    type: "POST",
                    data: {
                        action: "savereproductimg",
                        id: id,
                        img: img,
                        title: title,
                        showindex: showindex,
                        pid: proid
                    }, dataType: "json",
                    success: function (result) {
                        if (result.res > 0) {
                            alert(result.desc);
                            location.href = "/admin/products/m_procductimg_index.aspx?pid=<%=Pid%>";
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
        var init = function () {
            if (typeof (jsonproctype) != 'undefined' && jsonproctype.length > 0) {
                for (var i = 0; i < jsonproctype.length; i++) {
                    var op = '<option value="' + jsonproctype[i].ID + '">' + jsonproctype[i].Title + '</option>';
                    $("#sel_type").append(op);
                }
            }

            if (typeof (jsonprocimg) != 'undefined' && jsonprocimg.length > 0) {
                $(".txt_title").val(jsonprocimg[0].Title);
                $(".txt_showindex").val(jsonprocimg[0].ShowIndex);
                $(".u-imgaddress").prop("src", jsonprocimg[0].ImgUrl);
            }
        }

    </script>
</head>
<body>
    <form id="form2" runat="server">
        <div class="e_box">
            <div class="e-item">
                <span class="sp150">标题：</span>
                <input type="text" maxlength="200" class="txt_title" id="txt_title" />
            </div>
            <div class="e-item">
                <span class="sp150">所属于分类：</span>
                <select id="sel_type"></select>
            </div>
            <div class="e-item">
                <span class="sp150">排序：</span>
                <input type="text" maxlength="5" class="txt_showindex" id="txt_showindex" />
            </div>
            <div class="e-item">
                <span class="sp150">图片：</span>
                <img class="u-imgaddress" width="200" height="130" src="" />
                <a class="inpbbut3 btn_uploadimg">选择图片</a>
            </div>
        </div>
        <div class="btn-content" style="margin-left: 200px;">
            <input type="button" id="btn_ok" class="inpbbut3" value="保存" />
        </div>
    </form>
</body>
</html>
