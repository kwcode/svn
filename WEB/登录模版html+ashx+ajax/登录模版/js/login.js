
$("input[type=submit]").click(function () {
    var uid = document.getElementById("username").value;
    var pwd = document.getElementById("password").value;
    $.ajax({
        type: "get",
        url: "ashx/LoginHandler.ashx?Action=Login",
        dataType: "json",
        data: { username: uid, password: pwd },
        success: function (result) {
            if (result != null) {
                if (!result.issure)
                    alert(result.msg);
                else
                    window.top.location.href = '/index.html';
            }
        }
    });
});