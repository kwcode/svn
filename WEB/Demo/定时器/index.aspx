<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js"></script>
    <script>
        $(function () {
            //开始定时器
            setInterval(time, 1000);
        });
        var count = 0;
        function time() {
            $("#a-time").text(count);
            if (count == 10) {
                clearInterval();
                return;
            }
            count++;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>开始定时器</div>
        <div>
            <a style="color: red" id="a-time">0</a>
        </div>
        <div>

        </div>
    </form>
</body>
</html>
