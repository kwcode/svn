<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridTable.aspx.cs" Inherits="CMS.admin.GridTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.8.3.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="m-table">
            <thead>
                <tr>
                    <th class="cola">
                        ID
                    </th>
                    <th class="colb">
                        用户名称
                    </th>
                    <th class="colb">
                        昵称
                    </th>
                    <th class="colb">
                        登录名
                    </th>
                    <th class="colb">
                        密码
                    </th>
                    <th class="colb">
                        邮箱
                    </th>
                    <th class="colb">
                        头像
                    </th>
                    <th class="cola" style="cursor: pointer;" class="check-item">
                        <asp:CheckBox ID="Ckb_All" runat="server" Text="全选"></asp:CheckBox>
                    </th>
                </tr>
            </thead>
            <tbody id="TableBody">
               
            </tbody>
        </table>
        <div>
            <input type="button" id="first" value="first" onclick="ClickFirst()" />
            <input type="button" id="prve" value="prve" onclick="ClickPrve()" />
            <label>
                page:</label>
            <input type="text" style="width: 15px;" id="page" onchange="ChangePage()" />
            for<label id="NumberPage">1</label>
            <input type="button" id="next" value="next" onclick="ClickNext()" />
            <input type="button" id="last" value="last" onclick="ClickLast()" />
            <select id="count" onchange="ChangeCount()">
                <option value="5">5</option>
                <option value="10">10</option>
                <option value="20">20</option>
            </select>
        </div>
        <script type="text/javascript">
            var page; //当前页数
            var count; //页条数
            var type; //操作类型 1 首页；2 上一页；3 下一页；4 末页
            jQuery(document).ready(function () {
                page = 1;
                count = $("#count").val();
                type = 1;
                $("page").val(page);
                GetDataToTable();
            });
            function ChangePage() {
                if (!isNaN($("page").val())) {
                    page = parseInt($("page").val());
                } else
                    page = 1;
                GetDataToTable();
            }
            function ChangeCount() {
                count = $("#count").val();
                GetDataToTable();
            }
            function ClickFirst() {
                page = 1;
                count = $("#count").val();
                type = 1;
                GetDataToTable();
            }
            function ClickPrve() {
                page = page - 1;
                count = $("#count").val();
                type = 2;
                GetDataToTable();
            }
            function ClickNext() {
                page = page + 1;
                count = $("#count").val();
                type = 3;
                GetDataToTable();
            }
            function ClickLast() {
                page = -1;
                count = $("#count").val();
                type = 4;
                GetDataToTable();
            }
            function GetDataToTable() {
                //alert("page:" + page + " count:" + count + " type:" + type);
                alert($("#page").val());
                $.ajax({
                    type: "post",
                    url: "GridTable_JSON.aspx",
                    data: {
                        page: page, count: count, type: type
                    },
                    dataType: "json",
                    success: function (data) {
                        if ((data.page * data.count) >= data.rowscount) {
                            $("#first").attr('disabled', false);
                            $("#prve").attr('disabled', false);
                            $("#next").attr('disabled', true);
                            $("#last").attr('disabled', true);
                        }
                        if (data.page <= 1) {
                            $("#first").attr('disabled', true);
                            $("#prve").attr('disabled', true);
                            $("#next").attr('disabled', false);
                            $("#last").attr('disabled', false);
                        }
                        $("#page").val(data.page);
                        var rows = data.rows;
                        //$("#TableBody").html("");
                        var htm = "";
                        for (var i = 0; i < data.rows.length; i++) {
                            htm += '<tr class="j-item" id="tr_ID_' + rows[i].ID + '">';
                            htm += '<td id="td_UserID_' + rows[i].UserID + '">' + rows[i].UserID + '</td>';
                            htm += '<td id="td_Name_' + rows[i].UserID + '">' + rows[i].Name + '</td>';
                            htm += '<td id="td_NickName_' + rows[i].UserID + '">' + rows[i].NickName + '</td>';
                            htm += '<td id="td_Password_' + rows[i].UserID + '">' + rows[i].Password + '</td>';
                            htm += '<td id="td_Email_' + rows[i].UserID + '">' + rows[i].Email + '</td>';
                            htm += '<td id="td_LoginName_' + rows[i].UserID + '">' + rows[i].LoginName + '</td>';
                            htm += '<td id="td_SmallPhoto_' + rows[i].UserID + '"><img alt="null" src="' + rows[i].SmallPhoto + '" /></td>';
                            htm += '<td id="td_Checked_' + rows[i].UserID + '" style="cursor: pointer;" class="check-item">';
                            htm += '<input type="checkbox" name="Checked" />';
                            htm += '</td>';
                            htm += '</tr>';
                            //$("#TableBody").append(htm);
                        }
                        $("#TableBody").html(htm);
                    }
                });
            }
        </script>
    </div>
    </form>
</body>
</html>
