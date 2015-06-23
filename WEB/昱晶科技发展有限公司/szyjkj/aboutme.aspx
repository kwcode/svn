<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aboutme.aspx.cs" MasterPageFile="~/master/m.master"
    Inherits="aboutme" %>

<asp:Content runat="server" ContentPlaceHolderID="mbody">

    <div class="d-content"> 
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/aboutme.html">公司简介</a>
        </div>
        <div>
            <%=Details %>
        </div>
    </div>
</asp:Content>
