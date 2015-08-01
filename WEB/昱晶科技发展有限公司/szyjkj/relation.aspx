<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="relation.aspx.cs" Inherits="relation" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <style>
        .new-list { margin-bottom: 10px; }
        .new-list ul li { border-bottom: 1px dotted #808080; padding: 7px; font-size: 14px; }
        a { text-decoration: none; }
        .new-title { }
        .news-date { float: right; }
    </style>
    <script>
        $(function () {
            $.tw.lm =0; 
        });
    </script> 
    <div class="d-content" style="margin-top: 5px; margin-bottom: 20px;">
        <div style="margin: 0 10px 0 0; float: left;">
            <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        </div>
        <div style="margin-left: 10px; overflow: hidden;">
            <div class="d-nvtitle ">
                <span class="ico"></span>
                <a href="/">首页</a>
                <span class="guai">></span>
                <a href="/lianxi.html">联系我们</a> 
            </div>

            <div >
                 <%=Details %>
            </div>
        </div>
    </div>
</asp:Content>
