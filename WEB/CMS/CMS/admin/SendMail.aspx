<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="CMS.admin.SendMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../js/jquery-2.1.1.js"></script> 
    <link href="css/sendmail.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/layer/layer.min.js"></script>
    <script type="text/javascript">
        
        $(function () {
            $.ajax({
                type: "POST",
                url: "SendMailHandler.aspx",
                data: { Action: "GetData" },
                dataType: "json",
                success: function (result, status) {
                    if (status == "success") {

                        for (var i = 0; i < result.length; i++) {
                            $("#tbody_data").append(" <tr> <td>" + result[i].UserID + "</td> <td>" + result[i].LoginName + "</td>  <td>" + result[i].Password + "</td>   <td>" + result[i].NickName + "</td> <td>" + result[i].Email + "</td> <td> <input type=\"checkbox\" name=\"ckb_list\" id=\"" + result[i].UserID + "\" class=\"j-item-cbsenuser\" /> <label for=" + result[i].UserID + "></label></td> </tr>");
                        }
                    }
                }
            });
        })
        function AllSelect() {
            var ischeck = document.getElementById("all").checked;
            $("input[name='ckb_list']").attr("checked", ischeck);
            //   $("#Checkbox1").attr("checked", ischeck); 
        }
        function GetSelectData() {
            var list = new Array();
            // var arrChk = $("input[name='ckb_list]:checked");
            var arrChk = $("input[name='ckb_list']:checked")
            for (var i = 0; i < arrChk.length; i++) {
                list.push(arrChk[i].id);
            }
            return list;
        }
        function sendMail() {
            var list = GetSelectData();

            $.post("SendMailHandler.aspx", { Action: "SendMail", uids: list }, function (data, status) {
                alert(status);
                if (status == "success") {
                }
            });
        }
        function layer() {
            $.layer({
                type: 1,
                shade: [0],
                area: ['auto', 'auto'],
                title: false,
                border: [0],
                page: { dom: '.layer_notice' }
            });

        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post" autocomplete="off">
        <div>
            <input type="button" onclick="GetSelectData()" id="btnResend" value="发送邮件" />
            <input type="button" onclick="layer()" id="btnTest" value="测试" />
        </div>
        <div class="dtuser ">
            <table class="m-table">
                <thead>
                    <tr class="tr_title">
                        <th class="colb">用户ID</th>
                        <th class="colb">登录名</th>
                        <th class="colb">密码</th>
                        <th class="colb">用户名</th>
                        <th class="cola">邮箱</th>
                        <th class="cola">
                            <input type="checkbox" class="j-item-cbsenuser" id="all" onclick="AllSelect()" />
                            <label for="all">全部</label></th>
                    </tr>
                </thead>
                <tbody id="tbody_data">
                </tbody>
            </table>
        </div>


    </form>
</body>
</html>
