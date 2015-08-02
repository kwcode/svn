<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="news.aspx.cs" Inherits="news" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/news.html">文章</a>
        </div>
        <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        <div class="new-list rtc">
            <ul>
                <%if (DtNews != null && DtNews.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtNews.Rows)
                      { 
                %>
                <li><span class="new-title"></span><a href="/news/a-<%=type%>-<%=item["ID"]%>.html"><%=item["Title"] %></a> <span class="news-date"><%=Convert.ToDateTime(item["CreateTS"]).ToString("yyyy-MM-dd HH:mm")%> </span></li>
                <% 
                      }
                  }
                  else
                  {%>
                <li>暂无相关文章</li>
                <% } %>
            </ul>
        </div>
    </div>
</asp:Content>
