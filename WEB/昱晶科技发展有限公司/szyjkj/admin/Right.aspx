<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Right.aspx.cs" Inherits="admin_Right" %>

<%@ Register Src="~/master/uc/uc_header.ascx" TagPrefix="uc1" TagName="uc_header" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <div>
            <a class="inpbbut1" href="/admin/m_banner.aspx">首页轮播图修改</a>
            <a href="m_menu_index.aspx">导航菜单管理</a>
        </div>
    </form>
</body>
</html>
