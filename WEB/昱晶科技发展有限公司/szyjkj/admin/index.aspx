<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_index" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="content-style-type" content="text/css">
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    01.<meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>欢迎进入重庆今昔科技有限公司管理后台</title>
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
                , { id: '4', text: '产品中心', url: "/admin/products/m_product_index.aspx", iconCls: 'icon-pgin_golu' }
                , {
                    id: '5', text: '文章管理', url: "/admin/news/news_manager_index.aspx", iconCls: 'icon-new',
                    children: [{ id: '51', text: '文章分类', url: "/admin/article/ArticleTypeManager.aspx", iconCls: 'icon-new' }, { id: '51', text: '文章', url: "/admin/article/ArticleManager.aspx", iconCls: 'icon-new' }]
                }
                 , { id: '5', text: '用户管理', url: "/admin/user/usermanager.aspx", iconCls: 'icon-user' }
                 , { id: '6', text: '相册管理', url: "/admin/photo/m_PhotoBook.aspx", iconCls: 'icon-book' }
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
                url: window.location.pathname,
                method: "POST",
                //data: _tree,
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
    <div data-options="region:'north',border:false" style="height: 60px; padding: 10px">
        <h1 style="position: absolute;">欢迎进入重庆今昔科技有限公司管理后台</h1>
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
    <div data-options="region:'south',border:false" style="height: 30px; padding: 10px; text-align: center">重庆今昔科技有限公司</div>

</body>
</html>

