<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aboutme.aspx.cs" MasterPageFile="~/master/b.master"
    Inherits="aboutme" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>


<asp:Content runat="server" ContentPlaceHolderID="mbody">
      <script>
          $(function () {
              settopmenu("关于华兴");
          })

    </script>
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/aboutme.html">公司简介</a>
        </div>
        <div>
            <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        </div>
        <div class="rtc">
            <%=Details %>
        </div>
        
    </div>
</asp:Content>
