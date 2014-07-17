<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CMS.admin.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ul>
                <% foreach (CMS.MenuEntity item in MenuList)
                   {
                       if (item.ParentID == 0)
                       { 
                %>
                <li><a><%=item .Name %> </a></li>
               
                 <% }
                   } %>
            </ul>
        </div>
    </form>
</body>
</html>
