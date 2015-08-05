<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/b.master" CodeFile="product.aspx.cs" Inherits="product" %>

<%@ Register Src="~/master/uc/uc_leftmenu.ascx" TagPrefix="uc1" TagName="uc_leftmenu" %>



<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    
    <script>
        $(function () {
            settopmenu("楼盘展示");
        })

    </script>
    <div class="d-content">
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/product.html">楼盘展示</a>
        </div>
        <div class="pro-list">
            <uc1:uc_leftmenu runat="server" ID="uc_leftmenu" />
        </div>
        <div class="pro-content">
            <ul>
                <%if (DtproductImgs != null && DtproductImgs.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtproductImgs.Rows)
                      { 
                %>
                <li><a href="/productdesc.aspx?id=<%=item["ID"]%>">
                    <img id="img-adres" data-type="1" style="width: 190px; height: 190px;" src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />

                    <div style="text-align: center;"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></div>
                </a></li>
                <% 
                      }
                  }
                  else
                  {%>
                <li>该类暂时楼盘展示</li>
                <% } %>
            </ul>
        </div>
    </div>
    <div class="clearfix"></div>
</asp:Content>
