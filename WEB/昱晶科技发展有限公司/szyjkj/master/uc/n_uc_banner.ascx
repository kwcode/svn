<%@ Control Language="C#" AutoEventWireup="true" CodeFile="n_uc_banner.ascx.cs" Inherits="master_uc_n_uc_banner" %>
<div>
    <script src="/js/zzsc.js"></script>
    <style>
      
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
                        <a href="<%=item["URL"]%>" class="url" target="_blank">
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

</div>
