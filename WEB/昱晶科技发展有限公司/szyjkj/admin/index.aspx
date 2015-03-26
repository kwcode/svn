<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_index" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <script src="../js/jquery-1.8.3.js"></script>
    <link href="../style/style.css" rel="stylesheet" />
    <link href="../style/css.css" rel="stylesheet" />
    <link href="../style/layer.css" rel="stylesheet" />
    <style>
        body { font: 12px/1.5 "SimSun",Tahoma,Helvetica,Arial,sans-serif; line-height: 24px; background-color: #F6EFE7; margin: 0px; }
        a { cursor: pointer; }
        .d-top { height: 50px; width: 100%; margin-bottom: 5px; background: rgb(253, 232, 232); }
        .d-top ul li { float: left; line-height: 50px; font-size: 20px; margin-left: 20px; }
        .d-main { background: rgb(253, 232, 232); min-height: 600px; height: 100%; width: 100%; }
        .d-left { margin-right: 5px; float: left; width: 250px; }
        .d-left ul li { padding: 5px; text-align: center; font-size: 18px; background: red; margin: 5px; }
        .d-right { margin-left: 260px; /*==等于左边栏宽度==*/ }

        .menu_head { width: 223px; height: 47px; line-height: 47px; font-size: 14px; color: #525252; cursor: pointer; border: 1px solid #e1e1e1; position: relative; margin: 0px; font-weight: bold; background: #f1f1f1 url(../images/pro_left.png) center right no-repeat; }
        .menu_list .current { background: #f1f1f1 url(../images/pro_down.png) center right no-repeat; }
        .menu_body { width: 223px; height: auto; overflow: hidden; line-height: 38px; border-left: 1px solid #e1e1e1; backguound: #fff; border-right: 1px solid #e1e1e1; }
        .menu_body a { display: block; width: 223px; height: 38px; line-height: 38px; padding-left: 38px; color: #777777; background: #fff; text-decoration: none; border-bottom: 1px solid #e1e1e1; }
        .menu_body a:hover { text-decoration: none; }
    </style>
    <script>
        function page_jump(url) {
            document.getElementById('if_con').src = url;
        }
        $(document).ready(function () {
            $("#firstpane .menu_body:eq(0)").show();
            $("#firstpane p.menu_head").click(function () {
                $(this).addClass("current").next("div.menu_body").slideToggle(300).siblings("div.menu_body").slideUp("slow");
                $(this).siblings().removeClass("current");
            });
            $("#secondpane .menu_body:eq(0)").show();
            $("#secondpane p.menu_head").mouseover(function () {
                $(this).addClass("current").next("div.menu_body").slideDown(500).siblings("div.menu_body").slideUp("slow");
                $(this).siblings().removeClass("current");
            });

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="d-top" style="line-height: 40px;">
            <div style="float: left;">
                <h1 style="font-size: 20px; line-height: 40px;"><b>欢迎进入昱晶科技发展有限公司后台管理</b></h1>
            </div>
            <div style="float: right;">
                <span>当前登录的用户：</span>
                <span>admin</span>
            </div>
        </div>
        <div class="d-main">
            <div class="d-left">
                <div id="firstpane" class="menu_list">
                    <p class="menu_head current"><a style="margin-left: 38px;">网站首页</a>  </p>
                    <div style="display: block" class="menu_body">
                        <a href="/admin/index.aspx">网站首页</a>
                        <a onclick="page_jump('/admin/m_banner.aspx')">首页轮播图修改</a>
                        <a onclick="page_jump('/admin/relation.aspx')">联系我们</a>
                    </div>
                    <p class="menu_head"><a style="margin-left: 38px;">主营业务</a></p>
                    <div style="display: none" class="menu_body">
                        <a onclick="page_jump('/admin/products/m_procductimg_index.aspx')">产品中心</a>
                    </div>
                    <p class="menu_head"><a style="margin-left: 38px;">新闻</a></p>
                    <div style="display: none" class="menu_body">
                        <a onclick="page_jump('/admin/news/news_manager_index.aspx')">新闻中心</a>
                    </div>


                </div>
            </div>
            <div class="d-right">
                <iframe id="if_con" src="Right.aspx" width="100%" style="min-height: 600px; background: #fff" frameborder="0"
                    height="1000"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
