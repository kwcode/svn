<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aboutme.aspx.cs" MasterPageFile="~/master/m.master"
    Inherits="aboutme" %>

<asp:Content runat="server" ContentPlaceHolderID="mbody">

    <div class="d-content">
        <div class="d-nvtitle">当前位置：首页>公司简介</div>
        <div>
            <%=Details %>
        </div>
    </div>
</asp:Content>
