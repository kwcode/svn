<%@ Control Language="C#" AutoEventWireup="true" CodeFile="n_uc_banner.ascx.cs" Inherits="master_uc_n_uc_banner" %>
<div>
    <script src="/js/zzsc.js"></script>
    <style>
        * { margin: 0; padding: 0; }
        .banner-list a:link, .banner-list a:visited { color: #fff; text-decoration: none; }
        banner-list a img { border: 0; }
        div.wrap { width: 1200px; margin: 0 auto; text-align: left; }
        div#top div#nav { float: left; clear: both; width: 1200px; height: 52px; margin: 22px 0 0; }
        div#top div#nav ul { float: left; width: 1000px; height: 52px; list-style-type: none; }
        div#nav ul li { float: left; height: 52px; }
        div#nav ul li a { border: 0; height: 52px; display: block; line-height: 52px; text-indent: -9999px; }
        div#header { margin: -1px 0 0; }
        div#video-header { height: 683px; margin: -1px 0 0; }
        div#header div.wrap { height: 299px; background-position: 50% 0; background-repeat: no-repeat; }
        div#header div#slide-holder { z-index: 40; width: 1200px; height: 299px; position: absolute; }
        div#header div#slide-holder div#slide-runner { margin: 0 auto; /*top: 9px; left: 9px;*/ width: 1200px; height: 290px; overflow: hidden; position: absolute; }
        div#header div#slide-holder img { margin: 0; display: none; position: absolute; }
        div#header div#slide-holder div#slide-controls { left: 0; bottom: 0px; width: 1200px; height: 46px; display: none; position: absolute; background: url(../images/slide-bg.png) 0 0; }
        div#header div#slide-holder div#slide-controls p.text { float: left; color: #fff; display: inline; font-size: 10px; line-height: 16px; margin: 15px 0 0 20px; text-transform: uppercase; }
        div#header div#slide-holder div#slide-controls p#slide-nav { float: right; height: 24px; display: inline; margin: 11px 15px 0 0; }
        div#header div#slide-holder div#slide-controls p#slide-nav a { float: left; width: 24px; height: 24px; display: inline; font-size: 11px; margin: 0 5px 0 0; line-height: 24px; font-weight: bold; text-align: center; text-decoration: none; background-position: 0 0; background-repeat: no-repeat; }
        div#header div#slide-holder div#slide-controls p#slide-nav a.on { background-position: 0 -24px; }
        div#header div#slide-holder div#slide-controls p#slide-nav a { background-image: url(../images/silde-nav.png); }
        div#nav ul li a { background: url(../images/nav.png) no-repeat; }
        .banner-list { position: relative; width: 100%; overflow: hidden; }
    </style>
    <script>
        $(function () {
            var slidata = [];
            if (typeof (jsonbanner) != "undefined" && jsonbanner.length > 0) {
                for (var i = 0; i < jsonbanner.length; i++) {
                    var banner = {};
                    banner.id = jsonbanner[i].ID;
                    banner.client = jsonbanner[i].Title;
                    banner.desc = "";//jsonbanner[i].Title;
                    slidata.push(banner);
                }
            }

            if (window.slider) {
                window.slider.data = slidata;
                window.slider.init();
            }
        });

    </script>
    <!-- 代码 开始 -->
    <form id="formbanner" runat="server">
        <div class="banner-list">
            <div id="header" style="margin-top: 2px;">
                <div class="wrap">
                    <div id="slide-holder">
                        <div id="slide-runner">
                            <%if (DtBanner != null && DtBanner.Rows.Count > 0)
                              {
                                  foreach (System.Data.DataRow item in DtBanner.Rows)
                                  {
                            %>
                            <a href="<%=item["URL"]%>" style="width: 1200px;" target="_blank">
                                <img id="<%=item["ID"]%>" style="" src="<%=item["ImgAddress"].ToString().Replace("Tn","Orig")%>" class="slide" title="<%=item["Title"]%>" alt="<%=item["Title"]%>" /></a>
                            <%
                              }
                          } %>

                            <div id="slide-controls">
                                <p id="slide-client" class="text"><strong></strong><span></span></p>
                                <p id="slide-desc" class="text"></p>
                                <p id="slide-nav"></p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</div>
