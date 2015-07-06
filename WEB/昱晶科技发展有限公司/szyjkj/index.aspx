<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" MasterPageFile="~/master/m.master"
    Inherits="index" %>

<%@ Register Src="~/master/uc/uc_banner.ascx" TagName="banner" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        /*---------------------------------简介和新闻----------------------------*/
        .kw { width: 1200px; margin-bottom: 5px; }
        .kw-item1 { float: left; width: 660px; height: 260px; border: 1px solid #ccc; overflow: hidden; }
        .kw-item1 .kw-item-c img { width: 140px; height: 140px; border-radius: 50%; float: left; }
        .kw-item1 .kw-item-c .cpt { float: left; width: 480px; padding: 5px; margin-left: 10px; text-indent: 2em; letter-spacing: 1px; }
        .kw-item2 { float: left; width: 528px; height: 260px; margin-left: 8px; border: 1px solid #ccc; overflow: hidden; }
        .kw-item1 .kw-item-title { background-image: url('/images/fen_title_bg.gif'); }
        .kw-item1 .kw-item-title .tit2 { background-image: url('/images/fen_title.gif'); background-repeat: no-repeat; text-align: left; padding-left: 31px; height: 31px; line-height: 31px; font-size: 14px; font-weight: bold; color: #ffffff; }
        .kw-item-title { height: 31px; line-height: 31px; }
        .kw-item2 .kw-item-title { background-image: url('/images/news1.gif'); }
        .kw-item2 .kw-item-title .tit2 { background-image: url('/images/news_tit.gif'); background-repeat: no-repeat; text-align: left; padding-left: 31px; height: 31px; line-height: 31px; font-size: 14px; font-weight: bold; color: #ffffff; }
        .more { float: right; padding-right: 10px; line-height: 28px; }
        .more:hover { color: #f93; }
        .kw-newsitem { border-bottom: 1px dotted #cccccc; padding: 4px; }
        .kw-newsitem em { background: url(/images/news2.gif) no-repeat left center; float: left; text-align: left; padding: 10px 0 0 8px; width: 9px; height: 9px; }
        .kw-newsitem .newspan { background: url(/images/n_new.gif) no-repeat left center; text-align: left; padding: 10px 0 0 33px; width: 17px; height: 5px; }

        .kw-item-c .newtime { float: right; }

        /*---------------------------------首页轮播图片----------------------------*/
        .lb { border: 1px solid #ccc; height: 215px; overflow: hidden; margin-bottom: 5px; }
        .lb .lb_title { background-image: url('/images/title_bg.gif'); background-repeat: no-repeat; }
        .lb .lb_tit2 { text-align: left; padding-left: 28px; height: 28px; line-height: 31px; font-size: 14px; font-weight: bold; color: #ffffff; }

        .lb_c ul li { float: left; padding: 10px; }
        .lb_c ul li img { width: 213px; height: 150px; }
        .lb_c { overflow: hidden; margin: 0px 5px; }
        .slidebox { width: 3999px; }

        /*---------------------------------推荐产品----------------------------*/
        .rp_box { border: 1px solid #ccc; height: 645px; overflow: hidden; margin-bottom: 5px; }
        .rp_tit { border-bottom: 1px solid #3b8fb3; line-height: 36px; }
        .rp_tit2 { background: url('/images/rp_tit2.gif'); background-repeat: no-repeat; text-align: left; padding-left: 28px; height: 28px; line-height: 31px; font-size: 14px; font-weight: bold; color: #ffffff; }
        .rp_c { }
        .rp_c ul li { float: left; padding: 10px; }
        .rp_c ul li img { width: 274px; height: 250px; }
        /*---------------------------------友链----------------------------*/
        .footlink { width: 1200px; margin: auto; }
        .linkxian { width: 1200px; margin: 0 auto; text-align: center; font-size: 16px; font-weight: 100; color: #666; padding: 20px 0; }
        .linkxian span { border-left: #b3b3b3 1px solid; border-right: #b3b3b3 1px solid; padding: 0 50px; background: #fff; }
        .link_box { position: relative; }
        .link_box p { height: 1px; margin: 0; padding: 0; background: #b3b3b3; width: 100%; position: absolute; top: 30px; z-index: -1; }
        .link_c ul li { float: left; padding: 0 16px; margin-bottom: 30px; }
    </style>
    <script>
        $(function () {
            var _speed = 30;
            var _slide = $(".lb_c");
            var _slideli1 = $(".slide_1");
            var _slideli2 = $(".slide_2");
            _slideli2.html(_slideli1.html());
            function Marquee() {
                // console.log(_slide.scrollLeft() + "," + _slideli1.width());
                if (_slide.scrollLeft() >= _slideli1.width())
                    _slide.scrollLeft(0);
                else {
                    _slide.scrollLeft(_slide.scrollLeft() + 1);
                }
            }
            //两秒后调用
            var sliding = setInterval(Marquee, _speed)
            _slide.hover(function () {
                //鼠标移动DIV上停止
                clearInterval(sliding);
            }, function () {
                //离开继续调用
                sliding = setInterval(Marquee, _speed);
            });
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mbody">
    <div class="d-content">

        <!--公司简介和新闻-->
        <div class="kw clearfix">
            <div class="kw-item1">
                <div class="kw-item-title">
                    <div class="tit2"><b>公司简介</b></div>
                </div>
                <div class="kw-item-c  clearfix">
                    <img src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                    <div class="cpt">
                        深圳市昱晶科技发展有限公司是一家集科研、设计、销售为一体的高新技术企业，专业致力于蓝牙技术以及蓝牙模组的研制研发和销售（涵盖蓝牙耳机、蓝牙音响以及防丢器和蓝牙手表等）等服务，依靠科技求发展，不断为客户提供满意的高科技产品，是我们始终不变的追求。凭借勤恳耕耘、稳健发展、专业服务和不断追求，在该行业迅速崛起。
                本公司提供蓝牙CSR、RDA等解决方案. 同时欢迎OEM合作. 深圳市昱晶科技发展有限公司全体同仁热忱欢迎您的光临，愿能与您：互惠互利、实现双赢、共创辉煌，我们的进步离不开您的指导！
                    </div>
                </div>
            </div>
            <div class="kw-item2">
                <div class="kw-item-title">
                    <a class="more" href="/news.aspx">更多</a>
                    <div class="tit2">
                        <b>新闻</b>
                    </div>
                </div>
                <div class="kw-item-c">
                    <%if (dtNews != null && dtNews.Rows.Count > 0)
                      {
                    %>
                    <ul>
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ID"]%>.html">
                                <em></em>
                                <i><%=item["Title"]%></i>
                                <span class="newspan"></span>
                                <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                            </a>
                        </li>
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
        <div class="clearfix"></div>
        <!--公司简介和新闻END-->

        <!--首页产品轮播图片-->
        <div class="lb">
            <div class="lb_title">
                <a class="more" href="/product.aspx">更多</a>
                <div class="lb_tit2">产品展示</div>
            </div>
            <div class="lb_c">
                <div class="slidebox">
                    <ul class="slide_1" style="float: left">
                        <%
                            foreach (System.Data.DataRow item in DtScrollProcducts.Rows)
                            {
                        %><li><a href="/pro/a-<%=item["ID"]%>.html">
                            <img src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="<%=item["Title"] %>-昱晶科技" title="<%=item["Title"] %>" /></a>
                            <br />
                            <a style="text-align: center;" href="/pro/a-<%=item["ID"]%>.html" target="_blank" title="<%=item["Title"]%>">
                                <h1><%=item["Title"]%></h1>
                            </a>

                        </li>
                        <%
                            } %>
                    </ul>
                    <ul class="slide_2" style="float: left">
                    </ul>
                </div>
            </div>
        </div>
        <!--首页轮播图片END-->

        <!--推荐产品-->
        <div class="rp_box">
            <div class="rp_tit">
                <a class="more" href="/procduct.html">更多</a>
                <div class="rp_tit2">
                    <a>推荐产品</a>
                </div>
                <div class="rp_c">
                    <ul>
                        <%
                            foreach (System.Data.DataRow item in DtHomeTopProcducts.Rows)
                            {
                        %><li><a href="/productdesc.aspx?id=<%=item["ID"]%>">
                            <img src="<%=item["ImgUrl"] %>" onerror="this.onerror=null;this.src='/images/nophoto1.jpg'" alt="<%=item["Title"] %>-昱晶科技" title="<%=item["Title"] %>" /></a>
                            <br />
                            <a style="text-align: center;" href="/productdesc.aspx?id=<%=item["ID"]%>" target="_blank" title="<%=item["Title"]%>">
                                <h1><%=item["Title"]%></h1>
                            </a>

                        </li>
                        <%
                            } %>
                    </ul>
                </div>

            </div>
        </div>
        <!--推荐产品END-->
        <!--友情链接-->
        <div class="footlink">
            <div class="link_box">
                <div class="linkxian">
                    <span><a>友情链接</a></span>
                </div>
                <p></p>
            </div>
            <div class="link_c">
                <ul>
                    <li><a href="http://www.baidu.com" target="_blank">百度</a></li>
                    <li><a href="http://www.belltrip.cn" target="_blank">驼铃网</a></li>
                </ul>
            </div>
        </div>
        <!--友情链接END-->



    </div>

</asp:Content>
