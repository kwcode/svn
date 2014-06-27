<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataTable.aspx.cs" Inherits="CMS.admin.DataTable" %>

<!DOCTYPE html>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-2.1.1.js"></script>
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
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="m-table">
                <asp:Repeater ID="rpt_showListHoliday" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th class="cola">ID</th>
                                <th class="colb">用户名称</th>
                                <th class="colb">昵称</th>
                                <th class="colb">登录名</th>
                                <th class="colb">密码</th>
                                <th class="colb">邮箱</th>
                                <th class="colb">头像</th>
                                <th class="cola" style="cursor: pointer;">
                                    <input type="checkbox" class="j-item-cbsenuser" id="all" onclick="AllSelect()" />
                                    <label for="all">全部</label></th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="j-item">
                            <td><%#Eval("ID")%></td>
                            <td><%#Eval("Name")%></td>
                            <td><%#Eval("NickName")%></td>
                            <td><%#Eval("Password")%></td>
                            <td><%#Eval("Email")%></td>
                            <td><%#Eval("LoginName")%></td>
                            <td>
                                <img src="<%#Eval("SmallPhoto")%>" />
                            </td>
                            <td style="cursor: pointer;">
                                <input type="checkbox" class="j-item-cbsenuser" onclick="AllSelect()" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div id="PaginationResult" class="m-page m-page-lt" runat="server"></div>
            <%-- <webdiyer:AspNetPager ID="ListPager" UrlPaging="true" CssClass="paginator" CurrentPageButtonClass="cpb"
                LayoutType="div"
                runat="server" AlwaysShow="True" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                PageSize="10" PrevPageText="上一页" ShowCustomInfoSection="Left" ShowInputBox="Never"
                CustomInfoTextAlign="Left" OnPageChanged="ListPager_PageChanged">
            </webdiyer:AspNetPager>--%>
        </div>
    </form>
</body>
</html>
