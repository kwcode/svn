<%@ Page Language="C#" MasterPageFile="~/master/b.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mbody">
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
    <div class="d-content">
        <!--通知公告-->
        <div class="kw_box clearfix">
            <div class="kw_item fl" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>通知公告</b></div>
                </div>
                <div class="kw_item_info clearfix " style="padding: 10px;">
                    <marquee direction="up" scrollamount="2" onmouseover="this.stop()"
                        onmouseout="this.start()">
                     <%if (dtNews != null && dtNews.Rows.Count > 0)
                       { 
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          { 
                               
                        %>
                        <li class="kw-newsitem" style="border:0px;">
                            <a href="/news/a-<%=item["ID"]%>.html"> 
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>55?item["Title"].ToString().Substring(0,55):item["Title"]%></i>
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

                    </marquee>
                </div>
            </div>
            <div class="kw_item rt" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>新闻中心</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%if (dtNews != null && dtNews.Rows.Count > 0)
                      {
                          int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem" style="line-height: 70px;"><a href="/news/a-<%=item["ID"]%>.html">
                            <img class="newsimg" src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                            <i style="color: red; font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>20?item["Title"].ToString().Substring(0,20):item["Title"]%></i>
                            <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                        </a>
                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newspan"></span>
                                <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                            </a>
                        </li>
                        <%
                              }
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

        <div class="kw_box clearfix">
            <div class="kw_item fl" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>物业服务</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%if (dtNews != null && dtNews.Rows.Count > 0)
                      {
                          int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem" style="line-height: 70px;"><a href="/news/a-<%=item["ID"]%>.html">
                            <img class="newsimg" src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                            <i style="color: red; font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>20?item["Title"].ToString().Substring(0,20):item["Title"]%></i>
                            <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                        </a>
                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newspan"></span>
                                <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                            </a>
                        </li>
                        <%
                              }
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
            <div class="kw_item rt" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>人力资源</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%if (dtNews != null && dtNews.Rows.Count > 0)
                      {
                          int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem" style="line-height: 70px;"><a href="/news/a-<%=item["ID"]%>.html">
                            <img class="newsimg" src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                            <i style="color: red; font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>20?item["Title"].ToString().Substring(0,20):item["Title"]%></i>
                            <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                        </a>
                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newspan"></span>
                                <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                            </a>
                        </li>
                        <%
                              }
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

        <div class="kw_box clearfix">
            <div class="kw_item fl" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>推荐项目</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%if (dtNews != null && dtNews.Rows.Count > 0)
                      {
                          int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtNews.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem" style="line-height: 70px;"><a href="/news/a-<%=item["ID"]%>.html">
                            <img class="newsimg" src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                            <i style="color: red; font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>20?item["Title"].ToString().Substring(0,20):item["Title"]%></i>
                            <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                        </a>
                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newspan"></span>
                                <span class="newtime"><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                            </a>
                        </li>
                        <%
                              }
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
            <div class="kw_item rt" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="tit2"><b>公司简介</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <img class="jimg" src="/upload/Banner/2015/05/08/CqLPjpMd.jpg" />
                    <div class="cpt">
                        深圳市昱晶科技发展有限公司是一家集科研、设计、销售为一体的高新技术企业，专业致力于蓝牙技术以及蓝牙模组的研制研发和销售（涵盖蓝牙耳机、蓝牙音响以及防丢器和蓝牙手表等）等服务，依靠科技求发展，不断为客户提供满意的高科技产品，是我们始终不变的追求。凭借勤恳耕耘、稳健发展、专业服务和不断追求，在该行业迅速崛起。
                本公司提供蓝牙CSR、RDA等解决方案. 同时欢迎OEM合作. 深圳市昱晶科技发展有限公司全体同仁热忱欢迎您的光临，愿能与您：互惠互利、实现双赢、共创辉煌，我们的进步离不开您的指导！
                    </div>
                </div>
            </div>
        </div>

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
