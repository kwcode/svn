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
                if (_slide.scrollLeft() >= _slideli1.width()) {
                    _slide.scrollLeft(0);
                }
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
                    <div id="demo" onmouseover="clearInterval(timer)" onmouseout="timer=setInterval(mar,100)" style="overflow: hidden; height:195px">
                        <div id="demo1">
                            <%
                                System.Data.DataTable dtga = GetArticleByTypeName("通知公告", 8);
                                if (dtga != null && dtga.Rows.Count > 0)
                                { 
                            %>
                            <ul class="clearfix">
                                <%foreach (System.Data.DataRow item in dtga.Rows)
                                  { 
                               
                                %>
                                <li class="kw-newsitem" style="border: 0px;">
                                    <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
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
                        </div>
                        <div id="demo2"></div>
                    </div>
                    <script>
                        var t = getid("demo"), t1 = getid("demo1"), t2 = getid("demo2"), sh = getid("show"), timer;
                        t2.innerHTML = t1.innerHTML;
                        function mar() {
                            if (t2.offsetTop <= t.scrollTop)
                                t.scrollTop -= t1.offsetHeight;
                            else
                                t.scrollTop++;
                        }
                        timer = setInterval(mar, 100);
                        function getid(id) {
                            return document.getElementById(id);
                        }
                    </script>
                </div>
            </div>
            <div class="kw_item rt" style="width: 590px;">
                <div class="kw_item_tl">
                    <div class="more"><a href="/news/a-<%=GetArticleByTypeName("新闻中心", 1).Rows[0]["ArticleTypeID"]%>.html">更多</a></div>
                    <div class="tit2"><b>新闻中心</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%
                        System.Data.DataTable dtNews = GetArticleByTypeName("新闻中心", 8);
                        if (dtNews != null && dtNews.Rows.Count > 0)
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
                        <li class="kw-newsitem  clearfix" style="height: 70px;">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <img class="newsimg fl" src="<%=item["ImgUrl"]%>" />
                            </a>
                            <div class="fl" style="width: 470px; height: 70px; overflow: hidden; padding-left: 5px;">
                                <i class="blue" style="font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newtime"><a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html" class="blue">查看详情</a>
                                    <%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                                <div title="<%=item["Summary"]%>">
                                    <%=item["Summary"].ToString().Length>100?item["Summary"].ToString().Substring(0,100)+"......":item["Summary"]%>
                                </div>

                            </div>

                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>30?item["Title"].ToString().Substring(0,30):item["Title"]%></i>
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
                    <div class="more"><a href="/news/a-<%=GetArticleByTypeName("物业服务", 1).Rows[0]["ArticleTypeID"]%>.html">更多</a></div>
                    <div class="tit2"><b>物业服务</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%
                        System.Data.DataTable dtWY = GetArticleByTypeName("物业服务", 8);
                        if (dtWY != null && dtWY.Rows.Count > 0)
                        {
                            int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtWY.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem  clearfix" style="height: 70px;">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <img class="newsimg fl" src="<%=item["ImgUrl"]%>" />
                            </a>
                            <div class="fl" style="width: 470px; height: 70px; overflow: hidden; padding-left: 5px;">
                                <i class="blue" style="font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newtime"><a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html" class="blue">查看详情</a><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                                <div title="<%=item["Summary"]%>">
                                    <%=item["Summary"].ToString().Length>100?item["Summary"].ToString().Substring(0,100)+"......":item["Summary"]%>
                                </div>

                            </div>

                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>30?item["Title"].ToString().Substring(0,30):item["Title"]%></i>
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
                    <div class="more"><a href="/news/a-<%=GetArticleByTypeName("人力资源", 1).Rows[0]["ArticleTypeID"]%>.html">更多</a></div>
                    <div class="tit2"><b>人力资源</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%
                        System.Data.DataTable dtRL = GetArticleByTypeName("人力资源", 8);
                        if (dtRL != null && dtRL.Rows.Count > 0)
                        {
                            int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtRL.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem  clearfix" style="height: 70px;">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <img class="newsimg fl" src="<%=item["ImgUrl"]%>" /></a>
                            <div class="fl" style="width: 470px; height: 70px; overflow: hidden; padding-left: 5px;">
                                <i class="blue" style="font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newtime"><a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html" class="blue">查看详情</a><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                                <div title="<%=item["Summary"]%>">
                                    <%=item["Summary"].ToString().Length>100?item["Summary"].ToString().Substring(0,100)+"......":item["Summary"]%>
                                </div>

                            </div>

                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>30?item["Title"].ToString().Substring(0,30):item["Title"]%></i>
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
                    <div class="more"><a href="/news/a-<%=GetArticleByTypeName("推荐项目", 1).Rows[0]["ArticleTypeID"]%>.html">更多</a></div>
                    <div class="tit2"><b>推荐项目</b></div>
                </div>
                <div class="kw_item_info clearfix ">
                    <%
                        System.Data.DataTable dtTJ = GetArticleByTypeName("推荐项目", 8);
                        if (dtTJ != null && dtTJ.Rows.Count > 0)
                        {
                            int i = 0;
                    %>
                    <ul class="clearfix">
                        <%foreach (System.Data.DataRow item in dtTJ.Rows)
                          {
                              i++;
                              if (i == 1)
                              {
                        %>
                        <li class="kw-newsitem  clearfix" style="">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <img class="newsimg fl" src="<%=item["ImgUrl"]%>" />
                            </a>
                            <div class="fl" style="width: 470px; height: 70px; overflow: hidden; padding-left: 5px;">
                                <i class="blue" style="font-size: 16px;" title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>15?item["Title"].ToString().Substring(0,15):item["Title"]%></i>
                                <span class="newtime"><a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html" class="blue">查看详情</a><%=Convert.ToDateTime( item["CreateTS"]).ToString("yyyy-MM-dd HH:mm") %> </span>
                                <div title="<%=item["Summary"]%>">
                                    <%=item["Summary"].ToString().Length>100?item["Summary"].ToString().Substring(0,100)+"......":item["Summary"]%>
                                </div>

                            </div>

                        </li>
                        <%
                              }
                              else
                              {
                        %>
                        <li class="kw-newsitem">
                            <a href="/news/a-<%=item["ArticleTypeID"]%>-<%=item["ID"]%>.html">
                                <em></em>
                                <i title="<%=item["Title"] %>"><%=item["Title"].ToString().Length>30?item["Title"].ToString().Substring(0,30):item["Title"]%></i>
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
                <div class="kw_item_info clearfix " style="padding: 5px;">
                    <%if (dtJianJie != null && dtJianJie.Rows.Count > 0)
                      {
                    %>
                    <a href="/aboutme.html">
                        <img class="jimg" src="<%=dtJianJie.Rows[0]["ImgUrl"] %>" />
                        <div class="cpt">
                            <%=dtJianJie.Rows[0]["Summary"].ToString().Length>320?dtJianJie.Rows[0]["Summary"].ToString().Substring(0,320)+"....":dtJianJie.Rows[0]["Summary"]%>
                            <a href="/aboutme.html" class="blue">查看详细</a>
                        </div>
                    </a>
                    <%
                      } %>
                </div>
            </div>
        </div>

        <!--首页产品轮播图片-->
        <div class="lb">
            <div class="lb_title">
                <a class="more" href="/product.aspx">更多</a>
                <div class="lb_tit2">楼盘展示</div>
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
        <%-- <div class="footlink">
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
        </div>--%>
        <!--友情链接END-->
    </div>
</asp:Content>
