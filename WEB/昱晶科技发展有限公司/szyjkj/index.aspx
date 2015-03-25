<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" MasterPageFile="~/master/m.master"
    Inherits="index" %>

<%@ Register Src="~/master/uc/uc_banner.ascx" TagName="banner" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        .product { margin-top: 10px; }

        .product ul { margin-left: -17px; }

        .product ul li { float: left; margin-left: 17px; }

        .product ul li img { width: 378px; height: 300px; border-radius: 10px; padding: 5px; }

        .pro-title { padding: 10px; border-bottom: 1px solid #0094ff; text-align: left; }
        .pro-title b { }
        .jianjie { float: left; width: 670px; height: 180px; }
        .jianjie-title { text-align: left; padding: 10px; }
        .news { float: left; width: 500px; height: 180px; margin-left: 20px; }
        .news ul li { text-align: left; }
        .news-title { border-bottom: 1px solid; text-align: left; }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mbody">
    <div class="d-content">
        <%--   <div style="text-align: center;">
            <img src="images/banae.jpg" style="width: 940px; border-radius: 5px;" alt="昱晶科技" />
            <uc1:banner runat="server" />
        </div>--%>
        <!--产品图片-->
        <div class="product clearfix">
            <div class="pro-title">
                <b>产品图片</b> <a style="float: right;" href="/product.aspx">More</a>
            </div>
            <div class="clearfix"></div>
            <ul>
                <%if (DtProductImgs != null && DtProductImgs.Rows.Count > 0)
                  {
                      foreach (System.Data.DataRow item in DtProductImgs.Rows)
                      {
                %><li><a>
                    <img src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="<%=item["Title"] %>-昱晶科技" title="<%=item["Title"] %>"></a></li>
                <%
                      }
                  }
                  else
                  {
                %><li>暂时无产品</li>
                <%
                  } %>
                <%--<li><a>
                    <img src="images/kk.jpg" /></a></li>
                <li><a>
                    <img src="images/kk.jpg" /></a></li>
                <li><a>
                    <img src="images/kk.jpg" /></a></li>--%>
            </ul>
        </div>
        <!--公司简介和新闻-->
        <div class="jianjie ">
            <div class="jianjie-title">
                <b>公司简介</b>
            </div>
            <div>
                深圳市昱晶科技发展有限公司是一家集科研、设计、销售为一体的高新技术企业，专业致力于蓝牙技术以及蓝牙模组的研制研发和销售（涵盖蓝牙耳机、蓝牙音响以及防丢器和蓝牙手表等）等服务，依靠科技求发展，不断为客户提供满意的高科技产品，是我们始终不变的追求。凭借勤恳耕耘、稳健发展、专业服务和不断追求，在该行业迅速崛起。
                本公司提供蓝牙CSR、RDA等解决方案. 同时欢迎OEM合作. 深圳市昱晶科技发展有限公司全体同仁热忱欢迎您的光临，愿能与您：互惠互利、实现双赢、共创辉煌，我们的进步离不开您的指导！
            </div>
        </div>
        <div class="news">
            <div class="news-title">
                <b>新闻</b>
            </div>
            <%if (dtNews != null && dtNews.Rows.Count > 0)
              {
            %>
            <ul>
                <%foreach (System.Data.DataRow item in dtNews.Rows)
                  {
                %>
                <li><a href="/newsdetails.aspx?id=<%=item["ID"] %>"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> <%=item["Title"] %> </a></li>
                <%
                  } %>
            </ul>
            <%
              }
              else
              { 
            %>
            <a>暂无新闻</a>
            <%
              } %>
        </div>

    </div>

</asp:Content>
