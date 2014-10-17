<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js"></script>
    <script>
        //(function (window, $) { })(window, $);
        $(function () {
            $.extend({
                alertCickA: function () { alert("aa"); }
            });
            $.alertCickA();
            $.fn.extend({
                alertCickB: function () {
                    alert("dd");
                }
            });

            $("aa").alertCickB();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
