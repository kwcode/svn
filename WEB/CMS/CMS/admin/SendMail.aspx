<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="CMS.admin.SendMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-2.1.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="text" value="aa" id="txt" />
        <input type="button" value="发送邮件" onclick="SendMail()" />
    </div>
    <script type="text/javascript">
        function SendMail() {
            $.ajax({
                type: "POST",
                url: "SendMailHandler.ashx",
                data: { Action: "GetData" },
                dataType: "json",
                success: function (result, status, data) {
                    //                    $.each(result, function (commentIndex, comment) {
                    //                        alert(comment['name']);
                    //                    });
                    if (status == "success") {

                        var length = result.data.length;
                        if (length > 0) {
                            for (var i = 0; i < length; i++) {
                                var re = Send(result.data[i]);
                                if (re) {
                                    document.getElementById("txt").value = length - i - 1;
                                }
                            }
                            alert(status);
                        }
                    }
                }
            });
        }
        function Send(data) {
            $.ajax({
                type: "POST",
                url: "SendMailHandler.ashx",
                data: { Action: "SendMail", list: data },
                dataType: "json",
                success: function (result, status, data) {
                    return true;
                }
            });
            return false;
        }
    </script>
    </form>
</body>
</html>
