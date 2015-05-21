<%@ Page Language="C#" MasterPageFile="~/master/m.master" AutoEventWireup="true" CodeFile="news.aspx.cs" Inherits="news" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <style>
        .new-list { margin-bottom: 10px; }
            .new-list ul li { border-bottom: 1px dotted #808080; padding: 7px; font-size: 14px; }
        a { text-decoration: none; }
        .new-title { }
        .news-date { float: right; }
    </style>
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a>首页</a>
            <span class="guai">></span>
            <a>相关新闻</a>
        </div>
        <div class="new-list">
            <ul>
                <%if (DtNews != null && DtNews.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtNews.Rows)
                      { 
                %>
                <li><span class="new-title"></span><a href="/newsdetails.aspx?id=<%=item["ID"] %>"><%=item["Title"] %></a> <span class="news-date"><%=Convert.ToDateTime(item["CreateTS"]).ToString("yyyy-MM-dd HH:mm")%> </span></li>
                <% 
                      }
                  }
                  else
                  {%>
                <li>暂时无新闻</li>
                <% } %>
            </ul>
        </div>
    </div>
</asp:Content>
