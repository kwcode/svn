<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/css.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style>
        body { font: 12px/1.5 "SimSun",Tahoma,Helvetica,Arial,sans-serif; line-height: 24px; background-color: #F6EFE7; margin: 0px; }
        a { cursor: pointer; }
        .d-top { height: 50px; width: 100%; margin-bottom: 5px; background: rgb(253, 232, 232); }
        .d-top ul li { float: left; line-height: 50px; font-size: 20px; margin-left: 20px; }
        .d-main { background: rgb(253, 232, 232); min-height: 600px; height: 100%; width: 100%; }
        .d-left { background: rgb(103, 214, 232); margin-right: 5px; width: 15%; float: left; padding-top: 10px; height: 100%; }
        .d-left ul li { padding: 5px; text-align: center; font-size: 18px; background: red; margin: 5px; }
        .d-right { width: 100%; float: right; }
    </style>
    <script>
        function page_jump(url) {
            document.getElementById('if_con').src = url;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="d-top">
            <ul>
                <li><a href="/admin/index.aspx">网站首页</a></li>
                <li><a onclick="page_jump('/admin/aboutme.aspx')">关于我们</a></li>
                <li><a onclick="page_jump('/admin/products/m_procductimg_index.aspx')">主营业务</a></li>
                <li><a onclick="page_jump('/admin/news/news_manager_index.aspx')">新闻</a></li>
                <li><a>产品图片</a></li>
                <li><a onclick="page_jump('/admin/relation.aspx')">联系我们</a></li>
            </ul>
        </div>
        <div class="d-main">

            <%--<div class="d-left">
            <ul>
                <li><a onclick="page_jump('/admin/news/news_manager_index.aspx')">关于我们</a></li>
                <li><a>主营业务</a></li>
                <li><a>联系我们</a></li>
            </ul>
        </div>--%>
            <div class="d-right">
                <iframe id="if_con" src="Right.aspx" width="100%" style="min-height: 600px;" frameborder="0"
                    height="100%"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
