<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_product_index.aspx.cs" Inherits="admin_products_m_product_index" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/style/style.css" rel="stylesheet" />
    <link href="/style/css.css" rel="stylesheet" />
    <link href="/style/layer.css" rel="stylesheet" />
    <link href="/admin/style/admin.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.js"></script>
    <script src="/js/layer.js"></script>
    <script>
        $(function () {
            $("#btn_new").click(function () {
                layerIndex = $.layer({
                    type: 1,
                    shade: [0.3, '#000', true],
                    border: [4, 0.1, '#000', true],//默认边框
                    title: '新增',
                    area: ['593px', '265px'],
                    position: 'center',
                    shift: 'left',//从左动画弹出
                    page: {
                        dom: '.d-hide-edit'
                    },
                    success: function ($layer) {
                        $layer.on("click", ".btn_ok", function () {
                            var title = $(".txt_title").val();
                            var showindex = $(".txt_showindex").val();
                            var summary = $(".txt_summary").val();
                            $.ajax("/admin/action/actionadmin.aspx", {
                                data: {
                                    action: "savereproduct",
                                    title: title,
                                    showindex: showindex,
                                    summary: summary
                                }
                            }, function (result) {
                                alert(result.desc);
                            })
                            location.reload();
                            layer.close(layerIndex);
                        });
                        $layer.on("click", ".btn_cancel", function () {
                            layer.close(layerIndex);
                        });
                    }
                });
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="g-div-e">
            <div class="d-hide-edit" style="display: none;">
                <div class="d-item">
                    <span class="sp150">标题：</span>
                    <input type="text" class="txt_title" />
                </div>
                <div class="d-item">
                    <span class="sp150">排序：</span>
                    <input type="text" class="txt_showindex" value="0" />
                </div>
                <div class="d-item">
                    <span class="sp150">简介：</span>
                    <textarea class="txt_summary" style="width: 400px; height: 50px;"></textarea>
                </div>
                <div style="text-align: center;">
                    <input type="button" class="inpbbut1 btn_ok" value="确定" />
                    <input type="button" class="inpbbut1 btn_cancel" value="取消" />
                </div>
            </div>
            <div>
                <a class="inpbbut1" href="m_procductimg_index.aspx">返回</a>
                <a class="inpbbut1" id="btn_new">新建</a>
            </div>
            <div class="m-table">
                <table>
                    <thead>
                        <tr>
                            <th>操作</th>
                            <th>标题</th>
                            <th>排序</th>
                            <th>简介</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%
                            if (DtProcdut != null && DtProcdut.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow item in DtProcdut.Rows)
                                {
                        %><tr class="j-item">
                            <td><a class="inpbbut1 btn_edit">编辑</a></td>
                            <td><%=item["Title"] %></td>
                            <td><%=item["ShowIndex"] %></td>
                            <td>
                                <%=item["Summary"] %>
                            </td>
                        </tr>
                        <%   }
                            }
                            else
                            {
                        %><tr>
                            <td>咱无产品分类</td>
                        </tr>
                        <%
                            } 
                        %>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
