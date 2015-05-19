<%@ Control Language="C#" AutoEventWireup="true" CodeFile="uc_leftmenu.ascx.cs" Inherits="master_uc_uc_leftmenu" %>
<div style="width: 980px; margin: 0 auto;">
    <link href="/style/style.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <style>
        /*li { display: list-item; text-align: -webkit-match-parent; }*/
        a { text-decoration: none; cursor: pointer; }
        .fl { float: left; }
        .lfbox { width: 237px; margin-top: 10px; }
        .category { border: 1px solid #eee; }
            .category li a.active { background-color: #45a4fc!important; color: #fff!important; }
            .category a { display: block; width: 235px; height: 40px; line-height: 40px; color: #333; font-size: 14px; text-decoration: none; color: #333; }
                .category a:hover { background-color: #f4f4f4; text-shadow: 2px 2px 4px #b6b6b6; }
            .category li a.active { background-color: #45a4fc!important; color: #fff!important; }
                .category li a.active .category-arrow { background: transparent url(/images/left-menu-category2-icon.png) -12px 0 no-repeat!important; }

            .category .category-arrow { float: right; width: 7px; height: 9px; font-size: 1px; line-height: 1px; background: transparent url(/images/left-menu-category2-icon.png) -5px 0 no-repeat; margin-right: 22px; margin-top: 14px; padding: 0; }

            .category .category-icon { background: transparent url(/images/left-menu-category-icon.png) 0 0 no-repeat; _background: transparent url(/images/left-menu-category-icon.png) 0 0 no-repeat; width: 24px; height: 27px; float: left; margin: 9px 12px 0 15px; }

            .category .category-icon1 { background-position: -18px -13px; }
            .category .category-icon2 { background-position: -17px -60px; }
            .category .category-icon3 { background-position: -17px -104px; }
            .category .category-icon4 { background-position: -17px -150px; }
            .category .category-icon5 { background-position: -16px -189px; }
            .category .category-icon6 { background-position: -16px -239px; }
            .category .category-icon7 { background-position: -17px -283px; }
            .category .category-icon8 { background-position: -17px -330px; }
            .category .category-icon9 { background-position: -17px -382px; }
            .category a.active .category-icon1 { background-position: -60px -13px; }
            .category a.active .category-icon2 { background-position: -59px -60px; }
            .category a.active .category-icon3 { background-position: -59px -104px; }
            .category a.active .category-icon4 { background-position: -59px -150px; }
            .category a.active .category-icon5 { background-position: -58px -189px; }
            .category a.active .category-icon6 { background-position: -58px -239px; }
            .category a.active .category-icon7 { background-position: -59px -283px; }
            .category a.active .category-icon8 { background-position: -59px -330px; }
            .category a.active .category-icon9 { background-position: -53px -382px; }
    </style>
    <script>
        $(function () {
            function GetQueryString(name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]);
                return null;
            }
            var selectmenu = GetQueryString("mu");
            if (selectmenu == null) {
                $(".category").find("a").eq(0).addClass("active");
            }
            else {
                $(".category").find("a").eq(selectmenu).addClass("active");
            }
        });
    </script>
    <div class="lfbox fl">
        <ul class="category">
            <%if (MenuList != null && MenuList.Count > 0)
              {
                  foreach (LeftMenuCL item in MenuList)
                  {
            %>
            <li>
                <a href="<%=item.Url%>">
                    <span class="category-<%=item.Ico%> category-icon"></span>
                    <span class="category-arrow"></span>
                    <span><%=item.Name%></span>
                </a>
            </li>
            <%
                  }
              }
              else
              {
            %>
            <li><a href="/">
                <span class="category-ico1 category-icon"></span>
                <span class="category-arrow"></span>
                <span>首页</span>
            </a></li>
            <%
              }
            %>
        </ul>
    </div>

</div>
