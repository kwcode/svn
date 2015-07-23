<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="newsdetails.aspx.cs" Inherits="newsdetails" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <style>
        .new-list ul li { border-bottom: 1px dotted #000; padding: 10px; font-size: 14px; }
        a { text-decoration: none; }
        .new-title { }
        .news-date { float: right; }
    </style>
    <script>
        $(function () {
            //$.tw.lm = 2;
        })
    </script>
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/news.html">相关新闻</a>
        </div>
        <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        <div class="new-list rtc">

            <%if (DtNews != null && DtNews.Rows.Count > 0)
              {
                  foreach (System.Data.DataRow item in DtNews.Rows)
                  { 
            %>
            <div>
                <div style="font: bold; font-size: 20px; text-align: center;"><%=item["Title"] %></div>
                <div style="font: bold; font-size: 12px; text-align: center;">发布日期：<%=Convert.ToDateTime(item["CreateTS"]).ToString("yyyy-MM-dd HH:mm")%> <span style="margin-left: 10px;">浏览量：<%=item["ClickCount"]%></span></div>

                <div style="padding-bottom: 10px;"><%=item["Content"] %></div>
            </div>
            <% 
                  }
              }
              else
              {%>
                暂时无新闻
                <% } %>
        </div>
    </div>
</asp:Content>
