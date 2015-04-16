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


    <script src="/js/jquery-1.8.3.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>

    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <script>
        $(function () {
            $("#btn_new").click(function () {
                layerIndex = $.layer({
                    type: 1,
                    shade: [0.3, '#000', true],
                    border: [4, 0.1, '#000', true],//默认边框
                    title: '新增',
                    area: ['593px', '265px'],
                    position: 'center',
                    shift: 'left',//从左动画弹出
                    page: {
                        dom: '.d-hide-edit'
                    },
                    success: function ($layer) {
                        $layer.on("click", ".btn_ok", function () {
                            var _layer = $.layer({ type: 3 });
                            var title = $(".txt_title").val();
                            var showindex = $(".txt_showindex").val();
                            var summary = $(".txt_summary").val();
                            $.ajax("/admin/action/actionadmin.aspx", {
                                data: {
                                    action: "savereproduct",
                                    title: title,
                                    showindex: showindex,
                                    summary: summary
                                }
                            }).done(function (result) {
                                alert(result.desc);
                                location.reload();
                                layer.closeAll();
                            });

                        });
                        $layer.on("click", ".btn_cancel", function () {
                            layer.close(layerIndex);
                        });
                    }
                });
            });
            $(".btn_edit").click(function () {
                var $that = $(this);
                layerIndex = $.layer({
                    type: 1,
                    shade: [0.3, '#000', true],
                    border: [4, 0.1, '#000', true],//默认边框
                    title: '新增',
                    area: ['593px', '265px'],
                    position: 'center',
                    shift: 'left',//从左动画弹出
                    page: {
                        dom: '.d-hide-edit'
                    },
                    success: function ($layer) {
                        var id = $that.data().id;
                        $layer.find(".txt_title").val($that.data().title);
                        $layer.find(".txt_showindex").val($that.data().showindex);
                        $layer.find(".txt_summary").val($that.data().summary);

                        $layer.on("click", ".btn_ok", function () {
                            var _layer = $.layer({ type: 3 });
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
                                alert(result.desc);
                                location.reload();
                                layer.closeAll();
                            });

                        });
                        $layer.on("click", ".btn_cancel", function () {
                            layer.close(layerIndex);
                        });
                    }
                });
            });
            $(".btn_del").click(function () {
                if (!confirm("是否删除？！")) {
                    return;
                }
                var _layer = $.layer({ type: 3 });
                var id = $(this).data().id;
                $.ajax("/admin/action/actionadmin.aspx", {
                    data: {
                        action: "delproduct",
                        id: id
                    }
                }).done(function (result) {
                    location.reload();
                    alert(result.desc);
                    layer.close(_layer);
                });
            });
            //初始化
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*修改*/

                    /*修改END*/
                }
            }, '-', {
                text: '修改',
                iconCls: 'icon-edit',
                handler: function () {

                }
            }, '-', {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    /*删除*/

                    /*删除END*/
                }

            }];
            //表格数据
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
    </script>
</head>
<body>
    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <%--  <th data-options="field:'ck',checkbox:true"></th>--%>
                <th data-options="field:'ck',width:80,sortable:true">操作</th>
                <th data-options="field:'ID',width:80,sortable:true">ID</th>
                <th data-options="field:'ShowIndex',width:50,sortable:true">排序</th>
                <th data-options="field:'Title',width:150,sortable:true">标题</th>
                <th data-options="field:'CreateTS',width:120,sortable:true">创建时间</th>
                <th data-options="field:'Summary',sortable:true">简介</th>
            </tr>
        </thead>
    </table>

    <%--<form id="form1" runat="server">
        <div class="g-div-e">
            <div class="d-hide-edit" style="display: none;">
                <div class="d-item">
                    <span class="sp150">标题：</span>
                    <input type="text" class="txt_title" />
                </div>
                <div class="d-item">
                    <span class="sp150">排序：</span>
                    <input type="text" class="txt_showindex" value="0" />
                </div>
                <div class="d-item">
                    <span class="sp150">简介：</span>
                    <textarea class="txt_summary" style="width: 400px; height: 50px;"></textarea>
                </div>
                <div style="text-align: center;">
                    <input type="button" class="inpbbut1 btn_ok" value="确定" />
                    <input type="button" class="inpbbut1 btn_cancel" value="取消" />
                </div>
            </div>
            <div>
                <a class="inpbbut1" href="m_procductimg_index.aspx">返回</a>
                <a class="inpbbut1" id="btn_new">新建</a>
            </div>
            <div class="m-table">
                <table>
                    <thead>
                        <tr>
                            <th>操作</th>
                            <th>标题</th>
                            <th>排序</th>
                            <th>简介</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%
                            if (DtProcdut != null && DtProcdut.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow item in DtProcdut.Rows)
                                {
                        %><tr class="j-item">
                            <td><a class="inpbbut1 btn_edit" data-title="<%=item["Title"] %>" data-showindex="<%=item["ShowIndex"] %>" data-summary="<%=item["Summary"] %>" data-id="<%=item["ID"]%>">编辑</a>
                                <a class="inpbbut1 btn_del" data-id="<%=item["ID"]%>">删除</a>
                            </td>
                            <td><%=item["Title"] %></td>
                            <td><%=item["ShowIndex"] %></td>
                            <td>
                                <%=item["Summary"] %>
                            </td>
                        </tr>
                        <%   }
                            }
                            else
                            {
                        %><tr>
                            <td>咱无产品分类</td>
                        </tr>
                        <%
                            } 
                        %>
                    </tbody>
                </table>
            </div>
        </div>
    </form>--%>
</body>
</html>
