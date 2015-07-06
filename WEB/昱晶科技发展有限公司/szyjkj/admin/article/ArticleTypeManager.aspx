<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleTypeManager.aspx.cs" Inherits="admin_article_ArticleTypeManager" %>

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
                    var _layerIndex = $.layer({
                        type: 1,
                        title: '增加文章类型',
                        area: ['400px', '200px'],
                        page: {
                            dom: ".Hide_EditArticleType"
                        },
                        success: function ($layer) {
                            $layer.on("click", ".btn_ok", function () {
                                var b = $('#form1').validationEngine('validate');//手动验证
                                if (!b) {
                                    return;
                                }
                                var _name = $("#txt_name").val();
                                var _showindex = $("#txt_showindex").val();
                                var _layer = $.layer({ type: 3 });
                                $.post("/admin/article/ActionArticle.aspx", {
                                    action: "SaveArticleType",
                                    id: 0,
                                    name: _name,
                                    showindex: _showindex
                                }).done(function (result) {
                                    if (result.res > 0) {
                                        layer.close(_layer);
                                        layer.alert(result.desc, 1, function () {
                                            $('#dg').datagrid('reload');//刷新 
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
                url: '/admin/article/ArticleTypeManager.aspx',
                type: "POST",
                pageList: [20, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
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
                    action: "DelArticleType",
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
            var _layerIndex = $.layer({
                type: 1,
                title: '修改文章类型',
                area: ['400px', '200px'],
                page: {
                    dom: ".Hide_EditArticleType"
                },
                success: function ($layer) {
                    var row = $('#dg').datagrid("getRows")[index];//获取行数据 根据索引
                    var id = row.ID;
                    $layer.find("#txt_name").val(row.Name);
                    $layer.find("#txt_showindex").val(row.ShowIndex);

                    $layer.on("click", ".btn_ok", function () {
                        var b = $('#form1').validationEngine('validate');//手动验证
                        if (!b) {
                            return;
                        }
                        var _name = $("#txt_name").val();
                        var _showindex = $("#txt_showindex").val();
                        var _layer = $.layer({ type: 3 });
                        $.post("/admin/article/ActionArticle.aspx", {
                            action: "SaveArticleType",
                            id: id,
                            Name: _name,
                            ShowIndex: _showindex
                        }).done(function (result) {
                            if (result.res > 0) {
                                layer.close(_layer);
                                layer.alert(result.desc, 1, function () {
                                    $('#dg').datagrid('reload');//刷新 
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
        function formatImgurl(val, row, index) {
            var img = ' <img src="' + row.ImgAddress + '" style="width: 30px; height: 30px;" />';
            return img;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Hide_EditArticleType hide_box">
            <div>
                <ul>
                    <li><span>名称：</span>
                        <input type="text" class="validate[required]" id="txt_name" maxlength="30" />
                    </li>
                    <li>
                        <span>排序：</span>
                        <input class="validate[required] validate[custom[integer]]" onkeyup="this.value=this.value.replace(/\D/g,'');" onblur="if(this.value==''||this.value==0)this.value=1" type="text" id="txt_showindex" maxlength="8" value="1" />
                    </li>
                </ul>
            </div>
            <div class="btnbox">
                <input class="inpbbut3 btn_ok" value="确定" type="button" />
            </div>
        </div>
    </form>
    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <th data-options="field:'op',width:80,align:'center',formatter:formatOper">操作</th>
                <th data-options="field:'ID',width:50,sortable:true">ID</th>
                <th data-options="field:'ShowIndex',width:50,sortable:true">排序</th>
                <th data-options="field:'Name',width:150,sortable:true">名称</th>
                <th data-options="field:'CreateTS',width:100,sortable:true,
                    formatter:function(value,row,index){ return new Date(value).toLocaleString();}">创建时间</th>
            </tr>
        </thead>
    </table>
</body>
</html>
