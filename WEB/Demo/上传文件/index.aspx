<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.8.3.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" action="index.aspx" runat="server">
    <div>
        <input type="file" accept="txt/*" name="file" runat="server" id="ufile" />
        <input type="submit" value="上传文件" />
    </div>
    </form>
</body>
</html>
