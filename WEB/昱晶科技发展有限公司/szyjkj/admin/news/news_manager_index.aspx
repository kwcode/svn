<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news_manager_index.aspx.cs"
    Inherits="admin_news_news_manager_index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <a class="inpbbut1" href="news_m_edit.aspx">新建</a>
        </div>
        <table class="m-table">
            <asp:Repeater ID="repeaterlist" runat="server">
                <HeaderTemplate>
                    <tr>
                        <th>
                            排序
                        </th>
                        <th>
                            ID
                        </th>
                        <th>
                            标题
                        </th>
                        <th>
                            简介
                        </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("ShowIndex") %>
                        </td>
                        <td>
                            <%#Eval("ID") %>
                        </td>
                        <td>
                            <%#Eval("Title") %>
                        </td>
                        <td>
                            <%#Eval("Summary")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
