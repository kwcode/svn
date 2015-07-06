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
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <link href="/style/jquery.validationEngine.css" rel="stylesheet" />
    <script src="/js/jquery.validationEngine-zh_CN.js"></script>
    <script src="/js/jquery.validationEngine.js"></script>
    <script>

        $(function () {

            //初始化一些数据
            if (typeof (jsonadminPage) != "undefined" && jsonadminPage.length > 0) {
                for (var i = 0; i < jsonadminPage.length; i++) {
                    var _option = '<option value="' + jsonadminPage[i] + '">' + jsonadminPage[i] + '</option>';
                    $("#sel_url").append(_option);
                }

            }
            //初始化 父级
            if (typeof (jsontree) != "undefined" && jsontree.length > 0) {
                for (var i = 0; i < jsontree.length; i++) {
                    var _option = '<option value="' + jsontree[i].ID + '">' + "[" + jsontree[i].ID + "]" + jsontree[i].Name + '</option>';
                    $("#sel_pid").append(_option);
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
                    var _layerIndex = $.layer({
                        type: 1,
                        title: '添加菜单',
                        area: ['400px', '300px'],
                        page: {
                            dom: ".Hide_Menu"
                        },
                        success: function ($layer) {
                            $("#txt_name").val("");
                            $layer.on("click", ".btn_ok", function () {
                                var b = $('#form1').validationEngine('validate');//手动验证
                                if (!b) {
                                    return;
                                }
                                //var _name = $("#txt_name").val();
                                //var _showindex = $("#txt_showindex").val();
                                var _layer = $.layer({ type: 3 });
                                $.post("/admin/action/actionadmin.aspx", {
                                    action: "AddPmMenu",
                                    name: $("#txt_name").val(),
                                    url: $("#sel_url").val(),
                                    pid: $("#sel_pid").val(),
                                    showindex: $("#txt_showindex").val(),
                                    ico: $("#sel_ico").val()
                                }).done(function (result) {
                                    layer.close(_layer);
                                    if (result.res > 0) {
                                        layer.alert(result.desc, 1, function () {
                                            window.location.reload();
                                            //$('#dg').treegrid('reload');//刷新 
                                            layer.closeAll();
                                        });
                                    }
                                    else {
                                        layer.alert(result.desc);
                                    }
                                }).fail(function (ex) {
                                    layer.close(_layer); layer.alert("请求失败" + ex.responseText);
                                });
                            });
                        }
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
                pageList: [20, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).treegrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });
        });

        function formatOper(val, row, index) {
            if (typeof (row) == "undefined" || typeof (row.id) == "undefined") {
                return;
            }
            var btn = ' <a onclick="edit(' + row.id + ')" class="icon-edit inputbtns"   title="编辑"></a>';
            var btn2 = ' <a href="#" class="icon-remove inputbtns"  onclick="del(' + row.id + ')" title="删除"></a>';
            var btns = btn + btn2;
            return btns;
        }
        function del(id) {
            var _layer = layer.confirm("是否删除？", function () {
                //var row = $('#dg').treegrid("find", id);//
                //var id = row.ID;
                $.post("/admin/action/actionadmin.aspx", {
                    action: "DelPmMenu",
                    id: id
                }).done(function (result) {
                    layer.close(_layer);
                    if (result.res > 0) {
                        //删除成功！
                        $('#dg').treegrid('remove', id); //删除一行 
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
        function edit(id) {
            var _layerIndex = $.layer({
                type: 1,
                title: '修改菜单',
                area: ['400px', '300px'],
                page: {
                    dom: ".Hide_Menu"
                },
                success: function ($layer) {
                    var row = $('#dg').treegrid("find", id);//
                    $layer.find("#txt_name").val(row.text);
                    $layer.find("#txt_showindex").val(row.showindex);
                    $layer.find("#sel_url").val(row.url);
                    $layer.find("#sel_pid").val(row.pid);
                    $layer.find("#sel_ico").val(row.iconCls);
                    $layer.find("#sp_ico").attr("class", row.iconCls);

                    $layer.on("click", ".btn_ok", function () {
                        var b = $('#form1').validationEngine('validate');//手动验证
                        if (!b) {
                            return;
                        }
                        //var _name = $("#txt_name").val();
                        //var _showindex = $("#txt_showindex").val();
                        var _layer = $.layer({ type: 3 });
                        $.post("/admin/action/actionadmin.aspx", {
                            action: "UpdatePmMenu",
                            id: id,
                            name: $("#txt_name").val(),
                            url: $("#sel_url").val(),
                            pid: $("#sel_pid").val(),
                            showindex: $("#txt_showindex").val(),
                            ico: $("#sel_ico").val()
                        }).done(function (result) {
                            if (result.res > 0) {
                                layer.close(_layer);
                                layer.alert(result.desc, 1, function () {
                                    $('#dg').treegrid('reload');//刷新 
                                    layer.closeAll();
                                });
                            }
                            else {
                                layer.close(_layer);
                                layer.alert(result.desc);
                            }
                        }).fail(function (ex) {
                            layer.close(_layer); layer.alert("请求失败" + ex.responseText);
                        });
                    });
                }
            });
        }
        function formatICO(val, row, index) {
            var ICO = '<i class="' + val + '" style="float: left; height: 16px;  width: 16px;"></i> <span>' + val + '</span>';
            return ICO;
        }
    </script>
</head>
<body class="s">

    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <th data-options="field:'op',width:80,align:'center',formatter:formatOper">操作</th>
                <th data-options="field:'id',width:50,sortable:true">ID</th>
                <th data-options="field:'text',width:50,sortable:true">名称</th>
                <th data-options="field:'url',width:50,sortable:true">URL</th>
                <th data-options="field:'iconCls',width:50,sortable:true,formatter:formatICO">ICO</th>
                <th data-options="field:'showindex',width:50,sortable:true">排序</th>
            </tr>
        </thead>
    </table>
    <form id="form1" runat="server">
        <div class="Hide_Menu hide_box">
            <div>
                <ul>
                    <li><span>名称：</span>
                        <input type="text" class="validate[required]" id="txt_name" maxlength="30" />
                    </li>
                    <li>
                        <span>URL：</span>
                        <select id="sel_url" class="validate[required]">
                        </select>
                    </li>
                    <li><span>父级：</span>
                        <select id="sel_pid" class="validate[required]">
                            <option value="0">无</option>
                        </select>
                    </li>
                    <li style="float: left"><span>ICO：</span>
                        <select id="sel_ico" style="float: left; width: 160px;">
                        </select>
                        <em id="sp_ico" style="float: left; width: 16px; height: 16px; margin-top: 4px; margin-left: 5px; display: block;"></em>
                    </li>
                    <li class="clearfix"></li>
                    <li>
                        <span class="sp100">排序：</span>
                        <input id="txt_showindex" type="text" name="showindex" value="0" />
                    </li>
                </ul>
            </div>
            <div class="btnbox">
                <input class="inpbbut3 btn_ok" value="确定" type="button" />
            </div>

        </div>
    </form>
</body>
</html>
