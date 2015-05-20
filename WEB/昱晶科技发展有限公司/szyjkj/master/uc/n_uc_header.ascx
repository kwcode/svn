<%@ Control Language="C#" AutoEventWireup="true" CodeFile="n_uc_header.ascx.cs" Inherits="master_uc_n_uc_header" %>
<script>
    (function ($) {
        $.fn.movebg = function (options) {
            var defaults = {
                width: 120,/*移动块的大小*/
                extra: 50,/*反弹的距离*/
                speed: 300,/*块移动的速度*/
                rebound_speed: 300/*块反弹的速度*/
            };
            var defaultser = $.extend(defaults, options);
            return this.each(function () {
                var _this = $(this);
                var _item = _this.children("ul").children("li").children("a");/*找到触发滑块滑动的元素	*/
                var origin = _this.children("ul").children("li.cur").index();/*获得当前导航的索引*/
                var _mover = _this.find(".move-bg");/*找到滑块*/
                var hidden;/*设置一个变量当html中没有规定cur时在鼠标移出导航后消失*/
                if (origin == -1) { origin = 0; hidden = "1" } else { _mover.show() };/*如果没有定义cur,则默认从第一个滑动出来*/
                var cur = prev = origin;/*初始化当前的索引值等于上一个及初始值;*/
                var extra = defaultser.extra;/*声明一个变量表示额外滑动的距离*/
                _mover.css({ left: "" + defaultser.width * origin + "px" });/*设置滑块当前显示的位置*/

                //设置鼠标经过事件
                _item.each(function (index, it) {
                    $(it).mouseover(function () {
                        cur = index;/*对当前滑块值进行赋值*/
                        move();
                        prev = cur;/*滑动完成对上个滑块值进行赋值*/
                    });
                });
                _this.mouseleave(function () {
                    cur = origin;/*鼠标离开导航时当前滑动值等于最初滑块值*/
                    move();
                    if (hidden == 1) { _mover.stop().fadeOut(); }/*当html中没有规定cur时在鼠标移出导航后消失*/
                });

                //滑动方法
                function move() {
                    _mover.clearQueue();
                    if (cur < prev) { extra = -Math.abs(defaultser.extra); } /*当当前值小于上个滑块值时，额外滑动值为负数*/
                    else { extra = Math.abs(defaultser.extra) };/*当当前值大于上个滑块值时，滑动值为正数*/
                    _mover.queue(
                        function () {
                            $(this).show().stop(true, true).animate({ left: "" + Number(cur * defaultser.width + extra) + "" }, defaultser.speed),
                            function () { $(this).dequeue() }
                        }
                    );
                    _mover.queue(
                        function () {
                            $(this).stop(true, true).animate({ left: "" + cur * defaultser.width + "" }, defaultser.rebound_speed),
                            function () { $(this).dequeue() }
                        }
                    );
                };
            })
        }
    })(jQuery);

</script>
<script>
    $(function () {
        var pathname = window.location.pathname;
        $(".d-header").find(".nav").find("li").removeClass("cur");//移除所有的
        var b = false;
        $(".d-header").find(".nav").find("li").each(function (index) {
            var $that = $(this);
            var href = $that.find("a").attr("href");
            if (href == pathname) {
                $that.addClass("cur");
                b = true;
            }
        })
        if (!b) {
            //没有选择 默认第一个
            $(".d-header").find(".nav").find("li").eq(0).addClass("cur");
        }

        $(".nav").movebg({ width: 120/*滑块的大小*/, extra: 40/*额外反弹的距离*/, speed: 300/*滑块移动的速度*/, rebound_speed: 400/*滑块反弹的速度*/ });
        //设置为首页
        function SetHome(obj, vrl) {
            try {
                obj.style.behavior = 'url(#default#homepage)'; obj.setHomePage(vrl);
            }
            catch (e) {
                if (window.netscape) {
                    try {
                        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                    }
                    catch (e) {
                        alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                    }
                    var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
                    prefs.setCharPref('browser.startup.homepage', vrl);
                }
            }
        }

        //加入收藏夹
        function AddFavorite(sURL, sTitle) {
            try {
                window.external.addFavorite(sURL, sTitle);
            }
            catch (e) {
                try {
                    window.sidebar.addPanel(sTitle, sURL, "");
                }
                catch (e) {
                    alert("加入收藏失败，请使用Ctrl+D进行添加");
                }
            }
        }
        $("#btn_sethome").click(function () {
            SetHome(this, window.location);
        });
        $("#btn_addfavorite").click(function () {
            AddFavorite(window.location.href, document.title);
        });

    });
</script>
<style>
    .d-header { width: 100%; position: fixed; z-index: 1000; background: #C70757; height: 40px; line-height: 40px; }
    .wraper { width: 1200px; margin: 0 auto; }
    .nav { position: relative; width: 100%; height: 40px; background: #C70757; overflow: hidden; }
    .nav-item { position: relative; float: left; width: 120px; height: 40px; line-height: 40px; text-align: center; font-size: 14px; z-index: 1; }
    .nav-item a { display: block; height: 40px; color: #fff; }
    .nav-item a:hover { color: #fff; }
    .move-bg { display: none; position: absolute; left: 0; top: 0; width: 120px; height: 40px; background: #4D0B33; z-index: 0; }
</style>
<div class="d-header clearfix">
    <!-- 代码 开始 -->
    <div style="float: left; margin-left: 100px;">
        <a style="cursor: pointer; color: white;" id="btn_sethome">设置为首页 |</a>
        <a style="cursor: pointer; color: white;" id="btn_addfavorite">加入收藏夹 </a>
    </div>
    <div style="float: right; margin-right: 100px;"><a style="cursor: pointer; color: white;" href="tencent://message/?uin=176393068&Site=www.szyjkj.com&Menu=yes" target="_blank">QQ在线联系</a></div>
    <div class="wraper">
        <div class="nav">
            <ul>
                <li class="nav-item  "><a href="/index.aspx">网站首页</a>  </li>
                <li class="nav-item"><a href="/aboutme.aspx">关于我们</a></li>
                <li class="nav-item"><a href="/product.aspx">主营业务</a></li>
                <li class="nav-item"><a href="/news.aspx">相关新闻</a></li>
                <li class="nav-item"><a href="/relation.aspx">联系我们</a></li>
            </ul>
            <!--移动的滑动-->
            <div class="move-bg"></div>
            <!--移动的滑动 end-->
        </div>
    </div>

    <!-- 代码 结束-->

</div>
