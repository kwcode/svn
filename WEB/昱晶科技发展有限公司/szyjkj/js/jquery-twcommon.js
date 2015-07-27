
(function (window, $, undefined) {
    $.trip = $.trip || {};
    $.tw = $.tw || {};
})(window, $);
/* $.trip.url 解析器 依赖jQuery*/
(function ($) {
    function isNumeric(arg) {
        return !isNaN(parseFloat(arg)) && isFinite(arg);
    }

    var _url = function (arg, url) {
        var _ls = url || window.location.toString();

        if (!arg) {
            return _ls;
        }
        else {
            arg = arg.toString();
        }

        if (_ls.substring(0, 2) === '//') {
            _ls = 'http:' + _ls;
        }
        else if (_ls.split('://').length === 1) {
            _ls = 'http://' + _ls;
        }

        url = _ls.split('/');
        var _l = { auth: '' }, host = url[2].split('@');

        if (host.length === 1) {
            host = host[0].split(':');
        }
        else {
            _l.auth = host[0];
            host = host[1].split(':');
        }

        _l.protocol = url[0];
        _l.hostname = host[0];
        _l.port = (host[1] || '80');
        _l.pathname = ((url.length > 3 ? '/' : '') + url.slice(3, url.length).join('/').split('?')[0].split('#')[0]);
        var _p = _l.pathname;

        if (_p.charAt(_p.length - 1) === '/') {
            _p = _p.substring(0, _p.length - 1);
        }
        var _h = _l.hostname, _hs = _h.split('.'), _ps = _p.split('/');

        if (arg === 'hostname') {
            return _h;
        }
        else if (arg === 'domain') {
            return _hs.slice(-2).join('.');
        }
        else if (arg === 'sub') {
            return _hs.slice(0, _hs.length - 2).join('.');
        }
        else if (arg === 'port') {
            return _l.port || '80';
        }
        else if (arg === 'protocol') {
            return _l.protocol.split(':')[0];
        }
        else if (arg === 'auth') {
            return _l.auth;
        }
        else if (arg === 'user') {
            return _l.auth.split(':')[0];
        }
        else if (arg === 'pass') {
            return _l.auth.split(':')[1] || '';
        }
        else if (arg === 'path') {
            return _l.pathname;
        }
        else if (arg.charAt(0) === '.') {
            arg = arg.substring(1);
            if (isNumeric(arg)) {
                arg = parseInt(arg, 10);
                return _hs[arg < 0 ? _hs.length + arg : arg - 1] || '';
            }
        }
        else if (isNumeric(arg)) {
            arg = parseInt(arg, 10);
            return _ps[arg < 0 ? _ps.length + arg : arg] || '';
        }
        else if (arg === 'file') {
            return _ps.slice(-1)[0];
        }
        else if (arg === 'filename') {
            return _ps.slice(-1)[0].split('.')[0];
        }
        else if (arg === 'fileext') {
            return _ps.slice(-1)[0].split('.')[1] || '';
        }
        else if (arg.charAt(0) === '?' || arg.charAt(0) === '#') {
            var params = _ls, param = null;

            if (arg.charAt(0) === '?') {
                params = (params.split('?')[1] || '').split('#')[0];
            }
            else if (arg.charAt(0) === '#') {
                params = (params.split('#')[1] || '');
            }

            params = params.split('&');
            if (!arg.charAt(1)) {
                var obj = {};
                for (var i = 0, ii = params.length; i < ii; i++) {
                    param = params[i].split('=');
                    param[0] && (obj[param[0]] = decodeURIComponent(param[1] || ''));
                }
                return obj;
            }

            arg = arg.substring(1);

            for (var i = 0, ii = params.length; i < ii; i++) {
                param = params[i].split('=');
                if (param[0] === arg) {
                    return decodeURIComponent(param[1] || '');
                }
            }

            return null;
        }

        return '';
    };

    $.extend($.trip, { url: _url });
})($);

/* 分页插件 */
(function (window, $) {
    $(function () {
        //分页初始化
        $('.page').buildPage();
    });
    //生成链接
    var getLink = function (p, text, totalPage, pageparam) {
        var href = '', pageNow = +$.trip.url('?' + pageparam) || 1;
        if (p > 0 && p <= totalPage) {
            if (/^\/huodong\/$/.test(location.pathname) || /huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d+).html/.test(location.pathname)) {
                pageNow = location.href.match(/huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d+).html/) ? location.href.match(/huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d+).html/)[9] : 1;
                href = ' href="' + $.trip.acturlRewrite(10, p) + '" ';
            }
            else {
                var params = $.trip.url('?');
                params[pageparam] = p;
                var query = '?';
                var i = 0;
                for (var key in params) {
                    if (i == 0) {
                        query += key + '=' + params[key];
                    }
                    else {
                        query += '&' + key + '=' + params[key];
                    }
                    i++;
                }

                href = ' href="' + window.location.pathname + query + '" ';
            }
        }
        var css = pageNow == text ? ' class="currentpage" ' : ''
            , data = p > 0 && p <= totalPage ? ' data-p="' + p + '" ' : '';
        return '<a ' + data + css + href + '>' + text + '</a>';
    };

    var $buildPage = function (totalCount, pageSize) {
        var $this = $(this), pageparam = $this.data('pageparam') || 'page', pageSize = pageSize || +$this.data('size') || 10, totalCount = totalCount || +$this.data('total'), pageNow = +$.trip.url('?' + pageparam) || 1;
        if (/^\/huodong\/$/.test(location.pathname) || /huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d)-(\d+).html/.test(location.pathname)) {
            pageNow = location.href.match(/huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d)-(\d+).html/) ? location.href.match(/huodong\/list-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\S*)-(\d)-(\d+).html/)[10] : 1;
        }

        if (!totalCount) {
            $this.html('');
            return;
        }
        var html = [], totalPage = Math.ceil(totalCount / pageSize);
        if (totalPage > 1) {
            var i = new StringBuffer,
                u = 8,
                o, s, f, r;
            for (pagecount = totalCount % pageSize != 0 ? parseInt(totalCount / pageSize) + 1 : parseInt(totalCount / pageSize), pageNow < 1 && (pageNow = 1), pagecount < 1 && (pagecount = 1), pageNow > pagecount && (pageNow = pagecount), o = parseInt(pageNow) - 1, s = parseInt(pageNow) + 1, o < 1 ? (i.append(' <a href="javascript:void(0);">首页<\/a>'), i.append(' <a href="javascript:void(0);">上一页<\/a>')) : (i.append(getLink(1, '首页', totalCount, pageparam)), i.append(getLink(o, '上一页', totalCount, pageparam))), f = pageNow % u == 0 ? pageNow - (u - 1) : pageNow - parseInt(pageNow % u) + 1, f > u && i.append(getLink(f - 1, '...', totalCount, pageparam)), r = f; r < f + u; r++) {
                if (r > pagecount) break;
                r == pageNow ? i.append(getLink(r, r, totalCount, pageparam)) : i.append(getLink(r, r, totalCount, pageparam))
            }
            pagecount >= pageNow + u && i.append(getLink(f + u, '...', totalCount, pageparam)), s > pagecount ? (i.append(' <a href="javascript:void(0);">下一页<\/a>'), i.append(' <a href="javascript:void(0);">尾页<\/a>')) : (i.append(getLink(s, '下一页', totalCount, pageparam)), i.append(getLink(pagecount, '尾页', totalCount, pageparam))), i.append(' <a href="javascript:void(0);">共' + totalPage + '页<\/a>'), html = i.toString()
        }
        $this.html(html);
        return $this;
    }
    //分页插件 paged: $paged, 
    $.fn.extend({ buildPage: $buildPage });

})(window, $);


//回到顶部的方法
//<p id="back-to-top"> <a href="#top"><span></span>返回顶部</a></p>
$(function () {
    //当滚动条的位置处于距顶部100像素以下时，跳转链接出现，否则消失
    $(function () {
        $(window).scroll(function () {
            if ($(window).scrollTop() > 100) {
                $("#back-to-top").fadeIn(1500);
            }
            else {
                $("#back-to-top").fadeOut(1500);
            }
        });

        //当点击跳转链接后，回到页面顶部位置

        $("#back-to-top").click(function () {
            $('body,html').animate({ scrollTop: 0 }, 1000);
            return false;
        });
    });
});


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

    $(function () {
        //if ($.tw.bar != undefined) {
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
            $(".d-header").find(".nav").find("li").each(function (index) {
                var $that = $(this);
                var href = $that.find("a").attr("href");
                if (pathname.indexOf(href) == 0) {
                    $that.addClass("cur");
                    b = true;
                }
            })
            //没有选择 默认第一个
            $(".d-header").find(".nav").find("li").eq(0).addClass("cur");
        }
        //}
        //else {
        //    //存在 匹配名字
        //    $(".d-header").find(".nav").find("li").removeClass("cur");//移除所有的
        //    $(".d-header").find(".nav").find("li").find("a").each(function () {
        //        if ($(this).text() == $.tw.bar) {
        //            $(this).closest("li").addClass("cur"); 
        //        }
        //    });
        //}

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
                else {
                    alert("设置失败");
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
})(jQuery);