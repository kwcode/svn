<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_product_index.aspx.cs" Inherits="admin_products_m_product_index" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="/style/icon.css" rel="stylesheet" />
    <link href="/style/easyui.css" rel="stylesheet" />

    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>


    <style>
        a { text-decoration: none; }
        .inputbtns { margin: 0 5px; padding: 2px 0 2px 16px; cursor: pointer; }
        .inputbtns:hover { border: 1px #D4D4D4 solid; }
    </style>
    <script>
        $(function () {
            //初始化
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*修改*/
                    op();
                    /*修改END*/
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
                url: '/admin/products/m_product_index.aspx',
                type: "POST",
                pageList: [10, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });


        });
        //自定义一列
        function formatOper(val, row, index) {
            var btn = ' <a href="#" class="icon-edit inputbtns"  onclick="edit(' + index + ')"title="编辑"></a>';
            var btn2 = ' <a href="#" class="icon-remove inputbtns"  onclick="del(' + index + ')" title="删除"></a>';
            var btn3 = '<a href="/admin/products/m_procductimg_index.aspx?pid=' + row.ID +'" class="icon-tip inputbtns"  title="产品详细"></a>';
            var btns = btn + btn2 + btn3;
            return btns;
        }
        function del(index) {
            $.messager.confirm("提示", "是否删除?", function (b) {
                if (b) {
                    var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
                    var id = row.ID;
                    $.ajax("/admin/action/actionadmin.aspx", {
                        data: {
                            action: "delproduct",
                            id: id
                        }
                    }).done(function (result) {
                        if (result.res > 0) {
                            //删除成功！
                            //$('#dg').datagrid('deleteRow', index); //删除一行 
                            $('#dg').datagrid('reload');//刷新
                            $('#dlg').dialog('close');
                        }
                        $.messager.alert("提示", result.desc);
                    });
                }
            });
        }
        function edit(index) {
            var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
            op(row);
        }
        function op(row) {
            var id = 0;
            var dgtitle = "新增";
            if (typeof (row) != "undefined") {
                id = row.ID;
                $(".txt_title").val(row.Title);
                $(".txt_showindex").val(row.ShowIndex);
                $(".txt_summary").val(row.Summary);
                dgtitle = "编辑";
            }
            else {
                id = 0;
                $(".txt_title").val("");
                $(".txt_showindex").val(0);
                $(".txt_summary").val("");
            }
            $("#dlg").dialog({
                title: dgtitle,
                modal: true,  //模态
                width: 593,
                height: 265,
                resizable: true,
                buttons: [{
                    text: '确认',
                    iconCls: 'icon-ok',
                    handler: function () {
                        /*保存*/
                        var title = $(".txt_title").val();
                        var showindex = $(".txt_showindex").val();
                        var summary = $(".txt_summary").val();
                        $.ajax("/admin/action/actionadmin.aspx", {
                            data: {
                                action: "savereproduct",
                                id: id,
                                title: title,
                                showindex: showindex,
                                summary: summary
                            }
                        }).done(function (result) {
                            if (result.res > 0) {
                                //更新
                                $('#dg').datagrid('reload');//刷新
                                $('#dlg').dialog('close');
                            }
                            $.messager.alert("提示", result.desc);
                        });
                        /*保存END*/
                    }
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#dlg').dialog('close')
                    }
                }]
            });
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
                <th data-options="field:'Summary',width:200,sortable:true">简介</th>

            </tr>
        </thead>
    </table>
    <div id="dlg" class="d-hide-edit">
        <div class="d-item">
            <span class="sp100">标题：</span>
            <input type="text" class="txt_title" />
        </div>
        <div class="d-item">
            <span class="sp100">排序：</span>
            <input type="text" class="txt_showindex" value="0" />
        </div>
        <div class="d-item">
            <span class="sp100">简介：</span>
            <textarea class="txt_summary" style="width: 400px; height: 100px;"></textarea>
        </div>
    </div>
</body>
</html>
