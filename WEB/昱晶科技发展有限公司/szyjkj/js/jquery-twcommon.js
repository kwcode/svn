
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

