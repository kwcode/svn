<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="relation.aspx.cs" Inherits="relation" %>

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
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/lianxi.html">联系我们</a>
        </div>
        <div>
            <%=Details %>
        </div>
    </div>
</asp:Content>
