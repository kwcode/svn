<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/m.master" CodeFile="product.aspx.cs" Inherits="product" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <style>
        .new-list { margin-bottom: 10px; }
        .new-list ul li { border-bottom: 1px dotted #808080; padding: 7px; font-size: 14px; }
        a { text-decoration: none; }
        .new-title { }
        .news-date { float: right; }
        .pro-list { float: left; width: 200px; text-align: center; padding: 5px; }
        .pro-list ul li { padding: 7px;  }
        .pro-content { padding: 5px; float: left;  width: 720px; }
        .pro-content ul li { float: left; width: 240px; }
    </style>
    <div class="d-content">
        <div class="d-nvtitle">当前位置：首页>主营业务</div>
        <div class="pro-list">
            <ul>
                <%if (DtProductType != null && DtProductType.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtProductType.Rows)
                      { 
                %>
                <li><a href="/product.aspx?pid=<%=item["ID"] %>"><%=item["Title"] %></a> </li>
                <% 
                      }
                  }
                  else
                  {%>
                <li>暂时无主营业务</li>
                <% } %>
            </ul>
        </div>
        <div class="pro-content">
            <ul>
                <%if (DtproductImgs != null && DtproductImgs.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtproductImgs.Rows)
                      { 
                %>
                <li><a>
                    <img id="img-adres" data-type="1" style="width: 190px; height: 190px;" src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />

                    <div style="text-align: center;"><%=item["Title"] %></div>
                </a></li>
                <% 
                      }
                  }
                  else
                  {%>
                <li>暂时无主营业务</li>
                <% } %>
            </ul>
        </div>
    </div>
    <div class="clearfix"></div>
</asp:Content>
