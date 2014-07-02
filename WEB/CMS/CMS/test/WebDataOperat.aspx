<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebDataOperat.aspx.cs"
    Inherits="CMS.admin.WebDataOperat" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="删除选中项" />
            </td>
            <td align="center">
                <webdiyer:AspNetPager ID="AspNetPager3" runat="server" ShowPageIndexBox="Always"
                    PageSize="30" OnPageChanged="ListPager_PageChanged" TextBeforeInputBox="转到第  "
                    TextAfterInputBox="  页  " ShowPageIndex="False" ShowInputBox="Always" ShowCustomInfoSection="Left"
                    PrevPageText="上一页" NextPageText="下一页" LastPageText="尾页" FirstPageText="首页" AlwaysShow="True"
                    CustomInfoSectionWidth="50%" CustomInfoTextAlign="NotSet">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" height="10">
            </td>
        </tr>
    </table>
    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0" bgcolor="#000000">
        <tr>
            <td align="center">
                <table width="100%" align="center" cellpadding="0" cellspacing="1" border="0">
                    <asp:Repeater ID="rpt_showListHoliday" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    <asp:CheckBox ID="Ckb_All" runat="server"></asp:CheckBox>全选
                                </td>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    序号
                                </td>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    项目名称
                                </td>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    建设单位
                                </td>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    修改
                                </td>
                                <td height="23" background="../images/tabletopcenter.gif">
                                    删除
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td height="25" bgcolor="#FFFFFF">
                                    <input id="txtID" type="hidden" name="txtID" value='<%#Eval("ID")%>' runat="server">
                                    <asp:CheckBox ID="Ckb_Sel" runat="server" Checked="False"></asp:CheckBox>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <%#Eval("ID")%>
                                </td>
                                <td height="25" align="left" bgcolor="#FFFFFF">
                                    <%#Eval("Name")%>
                                </td>
                                <td height="25" align="left" bgcolor="#FFFFFF">
                                    <%#Eval("NickName")%>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <a href="PubProject.aspx?id=<%#Eval("ID")%>" target="_blank">修改</a>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <asp:ImageButton CausesValidation="False" ID="ImageButton1" runat="server" ImageUrl="../images/delete.gif"
                                        CommandArgument='<%#Eval("ID")%> ' AlternateText="删除此项"></asp:ImageButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td height="25" bgcolor="#FFFFFF">
                                    <input id="txtID1" type="hidden" name="txtID1" value='<%#Eval("ID")%>' runat="server">
                                    <asp:CheckBox ID="Ckb_Sel1" runat="server" Checked="False"></asp:CheckBox>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td height="25" align="left" bgcolor="#FFFFFF">
                                    <%#Eval("Email")%>
                                </td>
                                <td height="25" align="left" bgcolor="#FFFFFF">
                                    <%#Eval("Email")%>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <a href="PubProject.aspx?id=<%#Eval("ID")%>" target="_blank">修改</a>
                                </td>
                                <td height="25" bgcolor="#FFFFFF">
                                    <asp:ImageButton CausesValidation="False" ID="ImageButton2" runat="server" ImageUrl="../images/delete.gif"
                                        CommandArgument='<%#Eval("ID")%> ' AlternateText="删除此项"></asp:ImageButton>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" height="10">
            </td>
        </tr>
    </table>
    <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center">
                <webdiyer:AspNetPager ID="pager2" runat="server" CloneFrom="AspNetPager3">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
