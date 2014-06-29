<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataTable.aspx.cs" Inherits="CMS.admin.DataTable" %>

<!DOCTYPE html>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../js/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="/js/layer/layer.min.js"></script>
    <link href="css/datatable.css" rel="stylesheet" />
    <link href="css/Pager.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            //var _layer = layer.load('正在获取数据...', 3);
            $.ajax({
                type: "POST",
                url: "action/DataTableHandler.aspx",
                data: { Action: "GetDataTable" },
                dataType: "json",
                success: function (result, status) {
                    if (status == "success") {
                        if (result.length > 0) {
                            for (var i = 0; i < result.length; i++) {

                            }
                            //  layer.close(_layer);//关闭层
                        }
                    }
                }
            })

            $("#check_all").click(function () {
                var ischeck = document.getElementById("check_all").checked;
                $("input[name='check_node_list']").attr("checked", ischeck);
            });
            $("#remove_dataitem").click(function () {
                var _layer = layer.load("正在删除数据。。。", 3);
                var list = GetSelectData();
                var str = JSON.stringify(list)
                $.ajax({
                    type: "POST",
                    url: "DataTable.aspx/SayHello",
                    contentType: "application/json; charset=utf-8",
                    // data: { Action: "DeleteData", uids: str },
                    data: { Action: "test", uids: str },
                    dataType: "json",
                    success: function (result, status, data) {
                        alert("delete");
                        if (status == "success") {
                            alert("删除数据");
                            RemoveData(list);
                            layer.close(_layer); //关闭层
                        }
                    }, error: function (err) {
                        alert(err);
                    }
                })
            });
            //获取选中的数据项
            function GetSelectData() {
                var list = new Array();
                var arrChk = $("input[name='check_node_list']:checked")
                for (var i = 0; i < arrChk.length; i++) {
                    list.push(arrChk[i].id);
                }
                return list;
            }
            //从界面移除数据
            function RemoveData(list) {
                if (list.length > 0) {
                    for (var i = 0; i < list.length; i++) {
                        var my = document.getElementById("tr" + list[i]);
                        if (my != null)
                            my.parentNode.removeChild(my);
                    }
                }
            }

            $("#btn_test").click(function () {
                var _layer = layer.load("test。。。", 3);
                var list = GetSelectData();
                $.ajax({
                    type: "POST",
                    url: "DataTable.aspx/DeleteData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ uids: list }),
                    success: function (result, status, data) {
                        alert("delete");
                        if (status == "success") {
                            alert("删除数据");
                            RemoveData(list);
                            layer.close(_layer); //关闭层
                        }
                    }, error: function (err) {
                        alert(err);
                    }
                })
            });

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" value="测试" id="btn_test" />
        <input type="button" value="删除" id="remove_dataitem" style="width: 100px; margin: 10px;
            height: 26px;" />
    </div>
    <div>
        <table class="m-table">
            <asp:Repeater ID="rpt_showListHoliday" runat="server">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th class="cola">
                                ID
                            </th>
                            <th class="colb">
                                用户名称
                            </th>
                            <th class="colb">
                                昵称
                            </th>
                            <th class="colb">
                                登录名
                            </th>
                            <th class="colb">
                                密码
                            </th>
                            <th class="colb">
                                邮箱
                            </th>
                            <th class="colb">
                                头像
                            </th>
                            <th class="cola" style="cursor: pointer;" class="check-item">
                                <input type="checkbox" id="check_all" />
                                <label for="check_all">
                                    全部</label>
                            </th>
                        </tr>
                    </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="j-item" id="tr<%#Eval("ID")%>">
                        <td>
                            <%#Eval("ID")%>
                        </td>
                        <td>
                            <%#Eval("Name")%>
                        </td>
                        <td>
                            <%#Eval("NickName")%>
                        </td>
                        <td>
                            <%#Eval("Password")%>
                        </td>
                        <td>
                            <%#Eval("Email")%>
                        </td>
                        <td>
                            <%#Eval("LoginName")%>
                        </td>
                        <td>
                            <img src="<%#Eval("SmallPhoto")%>" />
                        </td>
                        <td style="cursor: pointer;" class="check-item">
                            <input type="checkbox" name="check_node_list" id="<%#Eval("ID")%>" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <%--   <div id="PaginationResult" class="m-page m-page-lt" runat="server">
        </div>--%>
        <%--     <webdiyer:AspNetPager ID="ListPager" runat="server" UrlPaging="true" CssClass="paginator" 
         LayoutType="div" runat="server" AlwaysShow="True" FirstPageText="首页" LastPageText="尾页"
            NextPageText="下一页" PageSize="3" PrevPageText="上一页" ShowCustomInfoSection="Left"
            ShowInputBox="Never" CustomInfoTextAlign="Left" OnPageChanged="ListPager_PageChanged">
        </webdiyer:AspNetPager>--%>
        <webdiyer:AspNetPager CssClass="paginator" CurrentPageButtonClass="cpb" ID="AspNetPager3"
            OnPageChanged="ListPager_PageChanged" runat="server" FirstPageText="首页" LastPageText="尾页"
            NextPageText="下一页" PrevPageText="上一页" BorderColor="Red">
            <!--RecordCount="228" -->
        </webdiyer:AspNetPager>
    </div>
    </form>
</body>
</html>
