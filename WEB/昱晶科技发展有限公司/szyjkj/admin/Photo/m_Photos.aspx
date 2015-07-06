<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_Photos.aspx.cs" Inherits="admin_Photo_m_Photos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/style/icon.css" rel="stylesheet" />
    <link href="/style/easyui.css" rel="stylesheet" />
    <link href="/admin/style/admin.css?v=12" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/jquery.easyui.min.js"></script>
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <link href="/style/jquery.validationEngine.css" rel="stylesheet" />
    <script src="/js/jquery.validationEngine-zh_CN.js"></script>
    <script src="/js/jquery.validationEngine.js"></script>
    <script src="/js/jquery-twExt.js"></script>
    <link href="/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="/js/uploadify/jquery.uploadify.js"></script>
    <script>
        var bookid = '<%=BookId%>';
        $(function () {
            //表格工具栏 
            var _toolbar = [{
                text: 'Add',
                iconCls: 'icon-add',
                id: "btn_start"
            }];

            //表格数据 
            $('#dg').datagrid({
                singleSelect: true,//是否单选 
                remoteSort: false,//定义是否从服务器给数据排序。
                // multiSort: true,
                fitColumns: true,//True 就会自动扩大或缩小列的尺寸以适应表格的宽度并且防止水平滚动
                toolbar: _toolbar,
                pagination: true,//分页控件 
                rownumbers: true,//行号  
                fit: true,//自动大小
                pageNumber: 1,
                pageSize: 50,//每页显示的记录条数，默认为10 
                url: '/admin/Photo/m_Photos.aspx?bookid=' + bookid,
                type: "POST",
                pageList: [20, 40, 60, 100, 200]//可以设置每页记录条数的列表   
            }).datagrid('getPager').pagination({
                //设置分页控件 
                beforePageText: '第',//页数文本框前显示的汉字 
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            });

            /*上传图片*/
            //上传图片
            $("#btn_start").uploadify({
                auto: true,
                buttonCursor: "hand",
                buttonText: "上传图片",
                swf: '/js/uploadify/uploadify.swf',
                fileTypeExts: "",
                fileSizeLimit: "4194304",
                removeTimeout: "3",
                method: "POST",
                uploader: '/admin/Photo/uploadhandler.aspx?bookid=' + bookid,
                onSelect: function () {
                    var _layerIndex = $.layer({
                        type: 1,
                        title: "上传图片",
                        area: ['400px', '300px'],
                        page: {
                            dom: "#btn_start-queue"
                        },
                        success: function ($layer) {
                        }, close: function () {
                            $('#dg').datagrid('reload');//刷新 
                            layer.closeAll();
                        }
                    });

                },
                onUploadSuccess: function (file, data, response) {
                    $('#' + file.id).find('.data').html(' - 完成');
                    console.log(file, data, response);
                    var result = JSON.parse(data);
                    $("#img").attr("src", result.path);
                },
                OnRemoveTimeout: function () {
                    $('#dg').datagrid('reload');//刷新 
                    layer.closeAll();
                }, onCancel: function () {
                    $('#dg').datagrid('reload');//刷新  
                }



            });
            /*上传图片END*/
        });
        function formatOper(val, row, index) {
            var btn = ' <a href="#"  class="icon-edit inputbtns"  onclick="edit(' + index + ')"  title="编辑"></a>';
            var btn2 = ' <a href="#" class="icon-remove inputbtns"  onclick="del(' + index + ')" title="删除"></a>';
            var btns = btn + btn2;
            return btns;
        }
        function formatImgurl(val, row, index) {
            var img = ' <img src="' + row.Tn + '" style="width: 30px; height: 30px;" />';
            return img;
        }
    </script>
</head>
<body class="easyui-layout">
    <%--<form id="form1" runat="server">
        <div class="Hide_EditPhoto hide_box">
            <div>
            </div>
            <div>
                <img src="/images/kk.jpg" class="photo" id="img" />
            </div>
            <div class="btnbox">
                <input class="inpbbut3 btn_ok" value="确定" type="button" />
                <input class="inpbbut3 btn_cancel" value="取消" type="button" />
            </div>
        </div>
    </form>--%>

    <table id="dg" data-options="autoRowHeight:false">
        <thead>
            <tr>
                <th data-options="field:'op',width:80,align:'center',formatter:formatOper">操作</th>
                <th data-options="field:'ID',width:50,sortable:true">ID</th>
                <th data-options="field:'FileName',width:50,sortable:true">名称</th>
                <th data-options="field:'Extension',width:150,sortable:true">后缀</th>
                <th data-options="field:'Size',width:50,sortable:true">大小(KB)</th>
                <th data-options="field:'Source',width:50,sortable:true">来源</th>
                <th data-options="field:'Tn',width:150,sortable:true, formatter:formatImgurl">微缩图</th>
            </tr>
        </thead>
    </table>

</body>
</html>
