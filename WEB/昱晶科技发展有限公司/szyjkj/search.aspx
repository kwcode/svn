<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="search" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <script>
        $(function () {
            $("#txt_keywords").val('<%=keywords%>');
        });
    </script>
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/search.html">搜索</a>
        </div>
        <div style="padding: 10px;">
            <%if (dtSearch != null && dtSearch.Rows.Count > 0)
              {
                  foreach (System.Data.DataRow item in dtSearch.Rows)
                  {
            %>
            <div style="padding: 5px; border-bottom: 1px dashed #0094ff">
                <div class="fr"><%=item["CreateTS"]%></div>
                <div><a href="<%=item["URL"]%>">[<%=item["MODULE"]%>]<%=item["Title"]%></a></div>
            </div>
            <%
                  }
              }
              else
              {
            %>
            <div style="text-align: center;">
                关键字：<span class="blue" style="font-size: 16px;"><%=keywords%></span> 没有所有到任何信息
            </div>
            <%
              } %>
        </div>
    </div>
</asp:Content>
