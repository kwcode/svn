<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/b.master" CodeFile="productdesc.aspx.cs" Inherits="productdesc" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
    <script src="js/jquery.imageLens.js"></script>
    <script>
        $(function () { 
            $("#img-adres").imageLens({ lensSize: 200 });
        });
    </script>
    <div class="d-content"> 
        <div class="d-nvtitle">
            <span class="ico"></span>
            <a href="/">首页</a>
            <span class="guai">></span>
            <a href="/product.html">展示</a>
        </div>
        <div style="padding: 10px; text-align: center;">
            <%
                if (DtProductdesc != null && DtProductdesc.Rows.Count > 0)
                {
            %>
            <img id="img-adres" data-type="1" style="max-height: 500px; max-width: 800px;" src="<%=DtProductdesc.Rows[0]["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />
            <div></div>
            <%
                }
                else
                {
            %>该类暂时楼盘展示<%
                }
            %>
        </div>
    </div>

</asp:Content>
