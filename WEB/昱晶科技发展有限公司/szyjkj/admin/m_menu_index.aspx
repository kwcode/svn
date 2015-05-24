<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_menu_index.aspx.cs" Inherits="admin_m_menu_index" %>

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
    <script>
        $(function () {

            //初始化一些数据
            if (typeof (jsonadminPage) != "undefined" && jsonadminPage.length > 0) {
                for (var i = 0; i < jsonadminPage.length; i++) {
                    var _option = '<option value="' + jsonadminPage[i] + '">' + jsonadminPage[i] + '</option>';
                    $("#sel_url").append(_option);
                }

            }
            //初始化ICO
            if (typeof (jsonicos) != "undefined" && jsonicos.length > 0) {
                for (var i = 0; i < jsonicos.length; i++) {
                    var _option = '<option value="' + jsonicos[i] + '">' + jsonicos[i] + '</option>';
                    $("#sel_ico").append(_option);
                }

            }
            $("#sel_ico").change(function () {
                var $that = $(this);
                var ico = $that.val();
                $("#sp_ico").attr("class", ico);
            });

            /*END*/

            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*ADD*/
                    $('#dlg').dialog({
                        title: "新增菜单",
                        modal: true,
                        width: 400,
                        height: 250,
                        resizable: true,
                        buttons: [{
                            text: '确认',
                            iconCls: 'icon-ok',
                            handler: function () {
                                $('#dlg').dialog('close');
                            }
                        }, {
                            text: '取消',
                            iconCls: 'icon-cancel',
                            handler: function () {
                                $('#dlg').dialog('close')
                            }
                        }]
                    });
                    /*ADDEND*/
                }
            }];
            //表格数据 
            $('#dg').treegrid({
                idField: 'id',
                treeField: 'id',
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
                url: '/admin/m_menu_index.aspx',
                type: "POST",
                pageList: [10, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });
        });
        function formatOper(val, row, index) {
            var btn = ' <a href="/admin/m_banner_edit.aspx?id=' + row.ID + '" class="icon-edit inputbtns"   title="编辑"></a>';
            var btn2 = ' <a href="#" class="icon-remove inputbtns"  onclick="del(' + index + ')" title="删除"></a>';
            var btns = btn + btn2;
            return btns;
        }
        function del(index) {
            $.messager.confirm("提示", "是否删除?", function (b) {
                if (b) {
                    var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
                    var id = row.ID;
                    $.ajax("/admin/action/actionadmin.aspx", {
                        data: {
                            action: "delbanner",
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
                <th data-options="field:'id',width:50,sortable:true">ID</th>
                <th data-options="field:'text',width:50,sortable:true">名称</th>
                <th data-options="field:'url',width:50,sortable:true">URL</th>
                <th data-options="field:'iconCls',width:50,sortable:true">ICO</th>
                <th data-options="field:'showindex',width:50,sortable:true">排序</th>
            </tr>
        </thead>
    </table>
    <form id="form1" runat="server">
        <div id="dlg" data-id="0" class="easyui-form" data-options="novalidate:true">
            <div class="e-item">
                <span class="sp100">名称：</span>
                <input class="easyui-textbox" id="txt_name" type="text" name="name" data-options="required:true" />
            </div>
            <div class="e-item">
                <span class="sp100">URL：</span>
                <%--<input class="easyui-textbox" id="txt_url" type="text" name="url" data-options="required:true" />--%>
                <select id="sel_url" data-options="required:true">
                </select>
            </div>
            <div class="e-item">
                <span class="sp100">父级：</span>
                <select id="sel_pid" data-options="required:true">
                    <option value="0">无</option>
                </select>
            </div>
            <div class="e-item">
                <span class="sp100">ICO：</span>
                <select id="sel_ico" style="float: left;" data-options="required:true">
                </select>
                <span class="" id="sp_ico" style="width: 16px; height: 16px; float: left; margin-top: 4px; margin-left: 5px; display: block;"></span>
            </div>
            <div class="e-item">
                <span class="sp100">排序：</span>
                <input class="easyui-textbox" id="txt_showindex" type="text" name="showindex" data-options="required:true" value="0" />
            </div>
        </div>
    </form>
</body>
</html>
