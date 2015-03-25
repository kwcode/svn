<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news_manager_index.aspx.cs"
    Inherits="admin_news_news_manager_index" %>

<!DOCTYPE html>
<head runat="server">
    <title></title>
    <link href="/style/css.css" rel="stylesheet" type="text/css" />
    <link href="/style/layer.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.js"></script>
    <script src="/js/layer.js"></script>
    <script>
        $(function () {
            $(".btn_del").click(function () {
                if (!confirm("是否删除？！")) {
                    return;
                }
                var _layer = $.layer({ type: 3 });
                var id = $(this).data().id;
                $.ajax("/admin/action/actionadmin.aspx", {
                    data: {
                        action: "delnews",
                        id: id
                    }
                }).done(function (result) {
                    location.reload();
                    alert(result.desc);
                    layer.close(_layer);
                });

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <a class="inpbbut1" href="news_m_edit.aspx">新建</a>
            </div>
            <table class="m-table">
                <asp:Repeater ID="repeaterlist" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th width="15%">操作</th>
                                <th width="8%">排序
                                </th>
                                <th width="8%">ID
                                </th>
                                <th width="15%">标题
                                </th>
                                <th>简介
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a class="inpbbut1" href='news_m_edit.aspx?id=<%#Eval("ID") %>'>编辑</a>
                                <a class="inpbbut1 btn_del" data-id='<%#Eval("ID")%>'>删除</a>
                            </td>
                            <td>
                                <%#Eval("ShowIndex") %>
                            </td>
                            <td>
                                <%#Eval("ID") %>
                            </td>
                            <td>
                                <%#Eval("Title") %>
                            </td>
                            <td>
                                <%#Eval("Summary")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </form>
</body>
</html>
