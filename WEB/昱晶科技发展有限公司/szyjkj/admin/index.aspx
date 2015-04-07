<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_index" %>

<%--
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
</html>--%>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>欢迎进入深圳市昱晶科技发展有限公司管理后台</title>
    <!--说明：cs 文件 是在js 文件前面引用 否则会出现页面 样式问题-->
    <link href="/style/easyui.css" rel="stylesheet" />
    <link href="/style/icon.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>

    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <script>
        $(function () {
            addpanel("网站管理", "/admin/Right.aspx")
            //•id: 绑定到节点的标识值.
            //•text:显示文本.
            //•iconCls: 显示icon的css样式.
            //•checked: 节点是否选中.
            //•state: 节点状态, 'open' 或者 'closed'.
            //•attributes: 绑定到节点的自定义属性.
            //•target: 目标 DOM 对象.
            //children 子节点
            var _children = [
                { id: '2', text: '首页轮播图修改', iconCls: 'icon-pencil', url: "/admin/m_banner.aspx" }
                , { id: '3', text: '联系我们', url: "/admin/relation.aspx", iconCls: 'icon-phone' }
                , { id: '4', text: '产品中心', url: "/admin/products/m_procductimg_index.aspx", iconCls: 'icon-plugin_go' }
                , { id: '5', text: '新闻中心', url: "/admin/news/news_manager_index.aspx", iconCls: 'icon-new' }

            ];
            var _tree = [{ id: '1', text: '网站管理', state: 'open', children: _children, iconCls: 'icon-house', url: "/admin/Right.aspx" }]; //导航树构造

            //url :一个从远程服务器检索数据的URL.将你的菜单拼成这个json格式id，text，child等
            //method:检索数据的http方法类型.
            //checkbox:是否显示checkbox在所有节点之前.
            //cascadeCheck:定义是否级联选择.
            //onlyLeafCheck:仅仅只是在叶子节点显示checkbox
            //lines:否显示树线.
            //dnd:定义是否启用drag and drop.
            //data:数组
            //onClick：当用户点击节点的时候触发 
            //onDblClick：当用户双击一个节点的时候触发

            //动态-构造导航树
            $("#tree_nva").tree({
                data: _tree,
                lines: true,//将你的菜单拼成这个json格式id，text，child等 
                onClick: function (node) {
                    //点击后在右边的框架里打开url
                    addpanel(node.text, node.url);
                },
                onDblClick: function (node) {
                    $(this).tree(node.state === 'closed' ? 'expand' : 'collapse', node.target);
                    node.state = node.state === 'closed' ? 'open' : 'closed';//修改完该节点是否展开之后，要修改node的state属性，不然下次就无法折叠上
                }
            });
            //增加Panel
            function addpanel(title, url) {
                var html = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                $('#panel_c').panel({
                    content: html,
                    title: title,
                    //tools: [{
                    //    iconCls: 'icon-add',
                    //    handler: function () { alert('new') }
                    //}], //右侧的按钮
                    onLoad: function () {
                        alert("a");
                    }
                });
            }
            //增加tabs
            function addtabs(url) {

            }
            //

            //控制面板
            $('#mm').menu({
                onClick: function (item) {
                    if (item.id == "btn_Out")//注销
                    {
                        $.ajax("/admin/action/actionadmin.aspx", { data: { action: "outuser" } }).success(function (result) {
                            if (result.res > 0) {
                                location.href = "/admin/login.aspx";
                            }
                            else {
                                layer.alert("注销失败！");
                            }
                        });
                    }
                }
            });


        });
    </script>
</head>

<body class="easyui-layout">
    <div data-options="region:'north',border:false" style="height: 60px; line-height: 0px; padding: 10px">
        <h1 style="position: absolute;">欢迎进入深圳市昱晶科技发展有限公司管理后台</h1>
        <div style="position: absolute; right: 0; line-height: 20px;">
            <span>当前登录的用户：</span>
            <span style="color: #ff6a00"><%=SessionAccess.NickName%></span>
            <a href="#" class="easyui-menubutton" data-options="menu:'#mm',iconCls:'icon-application_xp'">控制面板</a>
        </div>

        <div id="mm">
            <div data-options="iconCls:'icon-door_out'" id="btn_Out">注销</div>
        </div>
    </div>
    <div data-options="region:'west',split:true,title:'导航菜单'" style="width: 180px; padding: 10px;">
        <!--导航树-->
        <ul class="easyui-tree" id="tree_nva">
        </ul>
        <!--导航树END-->
    </div>
    <div data-options="region:'center',title:'网站管理'" id="panel_c" class="easyui-panel" style="padding: 5px;">
        <!--中间内容-->
        <!--中间内容END-->
    </div>
    <div data-options="region:'south',border:false" style="height: 30px; padding: 10px; text-align: center">深圳市昱晶科技发展有限公司</div>

</body>
</html>

