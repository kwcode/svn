<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_procductimg_index.aspx.cs" Inherits="admin_products_m_procductimg_index" %>

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
 
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="g-div-e">

            <div>
                <a class="inpbbut1" href="m_product_index.aspx">产品分类管理</a>
                <a class="inpbbut1" id="btn_new" href="m_productimg_edit.aspx">新建</a>
            </div>
            <div class="m-table">
                <table>
                    <thead>
                        <tr>
                            <th>操作</th>
                            <th>ID</th>
                            <th>名称</th>
                            <th>所属ID</th>
                            <th>所属名称</th>

                            <th>图片</th>
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
                            <td><%=item["ID"] %></td>
                            <td><%=item["Title"] %></td>
                            <td><%=item["PID"] %></td>
                            <td><%=item["PTitle"] %></td>

                            <td>
                                <img id="img-adres" data-type="1" style="width: 50px; height: 50px;" src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />
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
