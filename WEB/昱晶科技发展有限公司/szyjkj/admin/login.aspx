<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title></title>
    <link rel="shortcut icon" href="/favicon.ico" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/layer.js"></script>
    <link href="style/admin.css" rel="stylesheet" />
    <script>
        $(function () {
            $("#btn_login").click(function () {
                var username = $("#username").val();
                var password = $("#password").val();
                var ischeck = $("#loginkeeping").attr("checked") == "checked" ? "1" : "0";
                if (username == "" || password == "") {
                    layer.alert("请输入帐号或者密码！");
                    return;
                }
                $.ajax("/admin/login.aspx", {
                    data: {
                        loginname: username,
                        pwd: password,
                        ischeck: ischeck
                    }, type: 'POST'
                }).done(function (result) {
                    if (result == 1) {
                        location.href = "/admin/index.aspx";
                    }
                    else if (result == 0) {
                        layer.alert("用户名不存在！");
                    }
                    else if (result == 2) {
                        layer.alert("密码错误！");
                    }
                    else {
                        layer.alert("登录失败！错误号" + result);
                    }
                });

            });

        });
        function KeyDown() {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                $("#btn_login").click();
            }
        }
    </script>
    <style>
        body { width: 100%; height: 100%; background: url(/images/bgjianjie.png); color: #666; position: relative; z-index: 10; }
        .container {   background-color: #fff;width: 480px; padding: 10px; margin: 0 auto; box-shadow: 0 0 0 4px rgba(0,0,0,.2); border-radius: 5px; margin-top: 90px; }
        .title { text-align: center; font-size: 30px; font-family: 微软雅黑,Microsoft YaHei,Arial,"宋体"; zoom: 1; color: #00a0e9; padding: 10px; }
        .e_box dd input[type='password'] { border: 1px solid #dbdbdb; padding: 6px 8px; border-radius: 1px; height: 22px; width: 300px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="title">
            <h1><span>欢迎进入系统</span></h1>
        </div>
        <div class="e_box">
            <dl>
                <dt>帐号：</dt>
                <dd>
                    <input id="username" onkeydown="KeyDown()" name="username" type="text" placeholder="登录名" /></dd>
            </dl>
            <dl>
                <dt>密码：</dt>
                <dd>
                    <input id="password" onkeydown="KeyDown()" name="password" type="password" placeholder="密码" /></dd>
            </dl>
            <dl style="display: none;">
                <dt></dt>
                <dd>
                    <input type="checkbox" style="margin-top: 5px;" name="loginkeeping" id="loginkeeping"
                        value="loginkeeping" />
                    <label for="loginkeeping">
                        记住登录状态</label></dd>
            </dl>
        </div>
        <div class="btn_box" style="text-align: center">
            <input type="submit" id="btn_login" class="inpbbut1" value="登录" />
        </div>

    </div>

</body>
</html>
