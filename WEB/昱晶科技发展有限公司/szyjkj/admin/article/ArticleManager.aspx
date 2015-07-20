<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleManager.aspx.cs" Inherits="admin_article_ArticleManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/style/icon.css" rel="stylesheet" />
    <link href="/style/easyui.css" rel="stylesheet" />
    <link href="/admin/style/admin.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <link href="/style/jquery.validationEngine.css" rel="stylesheet" />
    <script src="/js/jquery.validationEngine-zh_CN.js"></script>
    <script src="/js/jquery.validationEngine.js"></script>
    <script>
        $(function () {
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*ADD*/
                    window.location.href = "/admin/article/ArticleEdit.aspx";
                    /*ADDEND*/
                }
            }];

            //表格数据 
            $('#dg').datagrid({
                //checkbox: true,//是否出现复选框
                singleSelect: true,//是否单选
                //collapsible: true,
                remoteSort: false,//定义是否从服务器给数据排序。
                // multiSort: true,
                fitColumns: true,//True 就会自动扩大或缩小列的尺寸以适应表格的宽度并且防止水平滚动
                toolbar: _toolbar,
                pagination: true,//分页控件 
                rownumbers: true,//行号  
                fit: true,//自动大小
                pageNumber: 1,
                pageSize: 50,//每页显示的记录条数，默认为10 
                url: '/admin/article/ArticleManager.aspx',
                type: "POST",
                pageList: [20, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
            });
        });
        //自定义一列
        function formatOper(val, row, index) {
            var btn = ' <a onclick="edit(' + index + ')" class="icon-edit inputbtns"   title="编辑"></a>';
            var btn2 = ' <a href="#" class="icon-remove inputbtns"  onclick="del(' + index + ')" title="删除"></a>';
            var btns = btn + btn2;
            return btns;
        }
        function del(index) {
            var _layer = layer.confirm("是否删除？", function () {
                var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
                var id = row.ID;
                $.post("/admin/article/ActionArticle.aspx", {
                    action: "DelArticle",
                    id: id
                }).done(function (result) {
                    layer.close(_layer);
                    if (result.res > 0) {
                        //删除成功！
                        $('#dg').datagrid('deleteRow', index); //删除一行 
                        //  $('#dg').datagrid('reload');//刷新 
                        layer.closeAll();
                    }
                    else {
                        layer.alert(result.desc);
                    }
                }).fail(function (ex) {
                    layer.close(_layer); layer.alert("请求失败" + ex.responseText);
                });
            });
        }
        function edit(index) {
            var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
            var id = row.ID;
            window.location.href = "/admin/article/ArticleEdit.aspx?id=" + id;
        }
        function formatImgurl(val, row, index) {
            var img = ' <img src="' + row.ImgAddress + '" style="width: 30px; height: 30px;" />';
            return img;
        }
    </script>
</head>
<body>

    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <th data-options="field:'op',width:80,align:'center',formatter:formatOper">操作</th>
                <th data-options="field:'ID',width:50,sortable:true">ID</th>
                <th data-options="field:'ShowIndex',width:50,sortable:true">排序</th>
                <th data-options="field:'Title',width:150,sortable:true">标题</th>
                <th data-options="field:'CreateTS',width:100,sortable:true,
                    formatter:function(value,row,index){ return new Date(value).toLocaleString();}">创建时间</th>
            </tr>
        </thead>
    </table>
</body>
</html>
