<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebDataOperat.aspx.cs"
    Inherits="CMS.admin.WebDataOperat" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/datatable.css" rel="stylesheet" />
    <link href="css/Pager.css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript">
               <!--

        function CheckAll() {
            var dom = document.all;
            var el = event.srcElement;
            if (el.id.indexOf("Ckb_All") >= 0 && el.tagName == "INPUT" && el.type.toLowerCase() == "checkbox") {
                var ischecked = false;
                if (el.checked)
                    ischecked = true;
                for (i = 0; i < dom.length; i++) {
                    if (dom[i].id.indexOf("Ckb_Sel") >= 0 && dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "checkbox")
                        dom[i].checked = ischecked;
                }
            }

            if (el.id.indexOf("Ckb_All1") >= 0 && el.tagName == "INPUT" && el.type.toLowerCase() == "checkbox") {
                var ischecked = false;
                if (el.checked)
                    ischecked = true;
                for (i = 0; i < dom.length; i++) {
                    if (dom[i].id.indexOf("Ckb_Sel") >= 0 && dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "checkbox")
                        dom[i].checked = ischecked;
                }
            }
        }

             //-->
        </script>
</head>
<body onclick="CheckAll()">
    <form id="form1" runat="server">
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="删除选中项" />
    <table class="m-table">
        <asp:Repeater ID="rpt_showListHoliday" runat="server">
            <HeaderTemplate>
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
                        <asp:CheckBox ID="Ckb_All" runat="server" Text="全选"></asp:CheckBox>
                    </th>
                </tr>
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
                        <input id="txtID" type="hidden" name="txtID" value='<%#Eval("ID")%>' runat="server">
                        <asp:CheckBox ID="Ckb_Sel" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
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
                        <input id="txtID1" type="hidden" name="txtID" value='<%#Eval("ID")%>' runat="server">
                        <asp:CheckBox ID="Ckb_Sel1" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
    </table>
    <webdiyer:AspNetPager CssClass="paginator" CurrentPageButtonClass="cpb" ID="AspNetPager3"
        OnPageChanged="ListPager_PageChanged" runat="server" FirstPageText="首页" LastPageText="尾页"
        NextPageText="下一页" PrevPageText="上一页" BorderColor="Red">
        <!--RecordCount="228" -->
    </webdiyer:AspNetPager>
    </form>
</body>
</html>
