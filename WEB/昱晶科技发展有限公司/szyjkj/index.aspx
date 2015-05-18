<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" MasterPageFile="~/master/m.master"
    Inherits="index" %>

<%@ Register Src="~/master/uc/uc_banner.ascx" TagName="banner" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        /*.product { margin-top: 10px; }
        .product ul { margin-left: -17px; }
        .product ul li { float: left; margin-left: 17px; }
        .product ul li img { width: 378px; height: 300px; border-radius: 10px; padding: 5px; }
        .pro-title { padding: 10px; border-bottom: 1px solid #0094ff; text-align: left; }*/
        .kw { width: 1200px; margin-bottom: 5px; }
        .kw-item1 { float: left; width: 660px; height: 180px; border: 1px solid #808080; overflow: hidden; }
        .kw-item2 { float: left; width: 528px; height: 180px; margin-left: 8px; border: 1px solid rgb(128, 128, 128); overflow: hidden; }
        .kw-item-title { background-image: url('/images/news_bg1.gif'); background-repeat: no-repeat; height: 28px; line-height: 28px; padding-left: 31px; }
        .kw-item-c { height: 150px; padding: 5px; }
        .kw-item-proc { width: 1220px; margin-left: -20px; }
        .kw-item-proc ul { }
        .kw-item-proc ul li { float: left; margin-left: 33px; margin-top: 10px; overflow: hidden; }
        .kw-item-proc ul li img { transition: All 0.4s ease-in-out; -webkit-transition: All 0.4s ease-in-out; -moz-transition: All 0.4s ease-in-out; -o-transition: All 0.4s ease-in-out; width: 365px; height: 300px; }
        .kw-item-proc ul li img:hover { transform: scale(1.1); -webkit-transform: scale(1.1); -moz-transform: scale(1.1); -o-transform: scale(1.1); -ms-transform: scale(1.1); }
        /*.kw-item-proc a img:hover { background: #fff url(about:blank); opacity: 0.8; filter: alpha(opacity=80); }*/
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mbody">
    <div class="d-content">
        <%--   <div style="text-align: center;">
            <img src="images/banae.jpg" style="width: 940px; border-radius: 5px;" alt="昱晶科技" />
            <uc1:banner runat="server" />
        </div>--%>
        <!--产品图片-->
        <div class="kw clearfix">
            <div class="kw-item-title">
                <a style="float: right;" href="/product.aspx">More>></a>
                <b>产品图片</b>
            </div>
            <div class="kw-item-proc  clearfix">
                <ul>
                    <%if (DtProductImgs != null && DtProductImgs.Rows.Count > 0)
                      {
                          foreach (System.Data.DataRow item in DtProductImgs.Rows)
                          {
                    %><li><a href="/productdesc.aspx?id=<%=item["ID"]%>">
                        <img src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="<%=item["Title"] %>-昱晶科技" title="<%=item["Title"] %>" /></a></li>
                    <%
                          }
                      }
                      else
                      {
                    %><li>暂时无产品</li>
                    <%
                      } %>
                </ul>
            </div>
        </div>
        <!--公司简介和新闻-->
        <div class="kw clearfix">
            <div class="kw-item1">
                <div class="kw-item-title">
                    <b>公司简介</b>
                </div>
                <div class="kw-item-c">
                    深圳市昱晶科技发展有限公司是一家集科研、设计、销售为一体的高新技术企业，专业致力于蓝牙技术以及蓝牙模组的研制研发和销售（涵盖蓝牙耳机、蓝牙音响以及防丢器和蓝牙手表等）等服务，依靠科技求发展，不断为客户提供满意的高科技产品，是我们始终不变的追求。凭借勤恳耕耘、稳健发展、专业服务和不断追求，在该行业迅速崛起。
                本公司提供蓝牙CSR、RDA等解决方案. 同时欢迎OEM合作. 深圳市昱晶科技发展有限公司全体同仁热忱欢迎您的光临，愿能与您：互惠互利、实现双赢、共创辉煌，我们的进步离不开您的指导！
                </div>
            </div>
            <div class="kw-item2">
                <div class="kw-item-title">
                    <b>新闻</b>
                </div>
                <div class="kw-item-c">
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
        </div>

    </div>

</asp:Content>
