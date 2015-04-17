<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_procductimg_index.aspx.cs" Inherits="admin_products_m_procductimg_index" %>

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
        var pid = '<%=ID%>';
        //自定义一列
        function formatOper(val, row, index) {
            var btn = ' <a href="/admin/products/m_productimg_edit.aspx?pid=' + pid + '&id=' + row.ID + '" class="icon-edit inputbtns"   title="编辑"></a>';
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
                            action: "delproductimg",
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
        $(function () {
            //初始化
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                handler: function () {
                    /*修改*/
                    // op();
                    //
                    window.location.href = "/admin/products/m_productimg_edit.aspx?pid=" + pid;
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
                url: '/admin/products/m_procductimg_index.aspx?pid=' + pid,
                type: "POST",
                pageList: [10, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });

            /****************************/
           
        });
        function formatImgurl(val, row, index) {
            var img = ' <img src="' + row.ImgUrl + '" style="width: 30px; height: 30px;" />';
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
                <th data-options="field:'ImgUrl',width:150,sortable:true, formatter:formatImgurl">图片</th>
                <th data-options="field:'Summary',width:200,sortable:true">简介</th>

            </tr>
        </thead>
    </table>
    <%--  <form id="form1" runat="server">
        <div class="g-div-e">

            <div>
                <a class="inpbbut1" href="m_product_index.aspx">产品分类管理</a>
                <a class="inpbbut1" id="btn_new" href="m_productimg_edit.aspx">新建</a>
            </div>
            <div class="m-table">
                <table>
                    <thead>
                        <tr>
                            <th>操作</th>
                            <th>ID</th>
                            <th>名称</th>
                            <th>所属ID</th>
                            <th>所属名称</th>
                            <th>排序</th>
                            <th>图片</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%
                            if (DtProcdut != null && DtProcdut.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow item in DtProcdut.Rows)
                                {
                        %><tr class="j-item">
                            <td><a class="inpbbut1 btn_edit" href="m_productimg_edit.aspx?id=<%=item["ID"] %>">编辑</a>
                                <a class="inpbbut1 btn_del" data-id="<%=item["ID"]%>">删除</a>
                            </td>
                            <td><%=item["ID"] %></td>
                            <td><%=item["Title"] %></td>
                            <td><%=item["PID"] %></td>
                            <td><%=item["PTitle"] %></td>
                            <td><%=item["ShowIndex"] %></td>
                            <td>
                                <img id="img-adres" data-type="1" style="width: 50px; height: 50px;" src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />
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
