<%@ Page Language="C#" AutoEventWireup="true" CodeFile="usermanager.aspx.cs" Inherits="admin_user_usermanager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/style/icon.css" rel="stylesheet" />
    <link href="/style/easyui.css" rel="stylesheet" />

    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>
    <script>
        $(function () {
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*修改*/
                    $('#dlg').dialog({
                        title: "用户新增",//对话框的标题文字。
                        modal: true,  //模态
                        width: 400,
                        height: 220,
                        resizable: true,
                        buttons: [{
                            text: '确认',
                            iconCls: 'icon-ok',
                            handler: function () {
                                var loginname = $("#txt_loginname").val();
                                var nickname = $("#txt_nickname").val();
                                var role = $("#sel_role").combobox('getValue'); // $("#sel_role").val();
                                var pwd = $("#txt_pwd").val();
                                if (loginname == "" || nickname == "" || pwd == "") {
                                    $.messager.alert("提示", "请输入相关信息！");
                                    return;
                                }
                                /*保存*/
                                $.ajax("/admin/action/actionadmin.aspx", {
                                    data: {
                                        action: "adduser", 
                                        loginname: loginname,
                                        nickname: nickname,
                                        role: role,
                                        pwd: pwd
                                    }
                                }).success(function (result) {
                                    if (result.res > 0) {
                                        $.messager.alert("提示", result.desc);
                                        //更新
                                        $('#dg').datagrid('reload');//刷新
                                        //$('#dg').datagrid('insertRow', {
                                        //    row: { UserID: 3, LoginName: loginname, NickName: nickname, Role: role }
                                        //});
                                        $('#dlg').dialog('close')
                                    }
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
                    /*修改END*/
                }
            }, '-', {
                text: '修改',
                iconCls: 'icon-edit',
                handler: function () {
                    var row = $('#dg').datagrid('getSelected'); //第一个选中的行或者 null。
                    var index = $('#dg').datagrid('getRowIndex', row); //返回指定行的索引，row 参数可以是一个行记录或者一个 id 字段的值。 
                    if (row) {
                        $("#txt_loginname").val(row.LoginName);
                        $("#txt_nickname").val(row.NickName);
                        $("#sel_role").combobox('setValue', row.Role)
                        /*修改*/
                        $('#dlg').dialog({
                            title: "用户修改",//对话框的标题文字。
                            modal: true,  //模态
                            width: 400,
                            height: 220,
                            resizable: true,
                            buttons: [{
                                text: '确认',
                                iconCls: 'icon-ok',
                                handler: function () {
                                    var id = row.UserID;
                                    var loginname = $("#txt_loginname").val();
                                    var nickname = $("#txt_nickname").val();
                                    var role = $("#sel_role").combobox('getValue'); // $("#sel_role").val();
                                    var pwd = $("#txt_pwd").val();
                                    if (loginname == "" || nickname == "" || pwd == "") {
                                        $.messager.alert("提示", "请输入相关信息！");
                                        return;
                                    }
                                    /*保存*/
                                    $.ajax("/admin/action/actionadmin.aspx", {
                                        data: {
                                            action: "upduser",
                                            id: id,
                                            loginname: loginname,
                                            nickname: nickname,
                                            role: role,
                                            pwd: pwd
                                        }
                                    }).success(function (result) {
                                        if (result.res > 0) {
                                            $.messager.alert("提示", result.desc);
                                            //更新
                                            $('#dg').datagrid('updateRow', {
                                                index: index,
                                                row: { UserID: id, LoginName: loginname, NickName: nickname, Role: role }
                                            });
                                            $('#dlg').dialog('close')
                                        }
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
                    } else { $.messager.alert("提示", "请选择要修改的数据！"); }
                    /*修改END*/
                }
            }, '-', {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    /*删除*/
                    //var row = $('#dg').datagrid('getSelected'); //获取选中的行 
                    //var index = $('#dg').datagrid('getRowIndex', row); //返回指定行的索引，row 参数可以是一个行记录或者一个 id 字段的值。 
                    //if (row) { 
                    //删除行
                    //$('#dg').datagrid('deleteRow', index); //删除一行   
                    //$('#dg').datagrid('selectRow', index); //选中一行  
                    //}
                    var rows = $('#dg').datagrid('getSelections'); //获取所有选中的行 
                    if (rows.length > 0) {
                        var ids = [];
                        $.each(rows, function (index, item) {
                            ids.push(item.UserID);
                        });
                        /*真正的删除行*/
                        var jsonuserids = JSON.stringify(ids);
                        $.ajax("/admin/action/actionadmin.aspx", { data: { action: "deluser", jsonuserids: jsonuserids } }).
                            success(function (result) {
                                if (result.res > 0) {
                                    $('#dg').datagrid('reload');//刷新
                                }
                                else {
                                    $.messager.alert("提示", result.desc);
                                }
                            });
                        /*真正的删除行END*/
                    } else { $.messager.alert("提示", "请选择要删除的数据！"); }
                    /*删除END*/
                }
            }];
            //表格
            $('#dg').datagrid({
                checkbox: true,//是否出现复选框
                singleSelect: false,//是否单选
                //collapsible: true,
                remoteSort: false,//定义是否从服务器给数据排序。
                // multiSort: true,
                toolbar: _toolbar,
                pagination: true,//分页控件 
                rownumbers: true,//行号  
                fit: true,//自动大小
                pageNumber: 1,
                pageSize: 50,//每页显示的记录条数，默认为10 
                url: '/admin/user/usermanager.aspx',
                type: "POST",
                pageList: [10, 40, 60, 100, 200]//可以设置每页记录条数的列表 
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });
        });
    </script>
</head>
<body>
    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true"></th>
                <th data-options="field:'UserID',width:80,sortable:true">UserID</th>
                <th data-options="field:'LoginName',width:100,sortable:true">登录帐号</th>
                <th data-options="field:'CreateTS',width:100,sortable:true">创建时间</th>
                <th data-options="field:'NickName',width:100,sortable:true">昵称</th>
                <th data-options="field:'Role',width:100,sortable:true">角色</th>
            </tr>
        </thead>
    </table>

    <style>
        .sp100 { width: 100px; float: left; text-align: right; height: 26px; line-height: 26px; margin-right: 2px; font-weight: bold; }
        .e-item { padding-top: 5px; float: left; width: 100%; }
        input[type='text'] { width: 150px; }
        input[type='text']:hover { box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); }
        select { width: 155px; }
    </style>
    <div id="dlg" data-userid="0">
        <div class="e-item">
            <span class="sp100">帐号：</span>
            <input class="easyui-textbox easyui-validatebox" id="txt_loginname" type="text" name="name" data-options="required:true" />
        </div>
        <div class="e-item">
            <span class="sp100">密码：</span>
            <input class="easyui-textbox easyui-validatebox" id="txt_pwd" type="text" name="name" data-options="required:true" />
        </div>
        <div class="e-item">
            <span class="sp100">昵称：</span>
            <input class="easyui-textbox easyui-validatebox" id="txt_nickname" type="text" name="name" data-options="required:true" />
        </div>
        <div class="e-item">
            <span class="sp100">角色：</span>
            <select class="easyui-combobox" id="sel_role" data-options="editable:false" name="role">
                <option value="0">普通</option>
                <option value="1">高级</option>
                <option value="2">超级管理员</option>
            </select>
        </div>
    </div>

</body>
</html>
