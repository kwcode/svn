(function (window, $, undefined) {
    if (window.jqueryExtLoaded) { return; }//在重复加载的情况下不会出错
    window.jqueryExtLoaded = true;

    // 所有我们自己写的jQuery插件都挂载在 $.trip 下
    $.trip = $.trip || {};
    //避免 IE 下调试代码出错
    window.console = window.console || { log: function () { }, dir: function () { } };

    /*对浏览器函数的Hack*/
    window.String.prototype.trim = function () {
        return $.trim(this);
    };

    //在被覆盖之前保存系统原生函数
    var preAlert = window.alert, preConfirm = window.confirm;

    $.trip.alert = function (msg, options) {
        var title = '', icon = true;
        if (typeof options === 'string') {
            title = options;
        } else if (options) {
            title = options.title;
            icon = 'icon' in options ? options.icon : icon;
        }

        var def = $.Deferred(), alertIndex = 0;
        if ($.layer) {
            $.trip.template('/communal/template.html')
                .render('tpl_alert', { msg: msg, title: title, icon: icon })
                    .done(function (html) {
                        alertIndex = $.layer({
                            type: 1,
                            title: false,
                            closeBtn: [0, false],
                            border: [5, 0.5, '#666', false],
                            bgcolor: '',
                            offset: ['140px', ''],
                            area: ['auto', 'auto'],
                            page: {
                                html: html
                            },
                            success: function (elem) {
                                $(elem).on('click', '.ico_close_d,.confirm', function (e) {
                                    e.preventDefault();
                                    layer.close(alertIndex);
                                    def.resolve();
                                })
                            }
                        });
                    })
                .fail(function () {
                    preAlert(msg);
                    def.resolveWith();
                });
        } else {//有些页面没有 $.layer
            setTimeout(function () {
                preAlert(msg);
                def.resolveWith();
            }, 1);
        }

        return $.extend(def.promise(), {
            close: function () {
                layer.close(alertIndex);
                def.resolve();
            }
        });
        //return def.promise();
    };

    $.trip.confirm = function (msg, options, context) {
        var title = '', icon = true;
        if (typeof options === 'string') {
            title = options;
        } else if (options) {
            title = options.title;
            icon = 'icon' in options ? options.icon : icon;
        }

        var def = $.Deferred();

        if ($.layer) {
            $.trip.template('/communal/template.html')
            .render('tpl_confirm', { msg: msg, title: title })
                .done(function (html) {
                    var i = $.layer({
                        type: 1,
                        title: false,
                        closeBtn: [0, false],
                        border: [5, 0.5, '#666', false],
                        bgcolor: '',
                        offset: ['140px', ''],
                        area: ['auto', 'auto'],
                        page: {
                            html: html
                        },
                        success: function (elem) {
                            $(elem).on('click', '.ico_close_d,.cancelwd2', function (e) {
                                e.preventDefault();

                                def.rejectWith(context);

                                layer.close(i);
                            }).on('click', '.confirm', function (e) {
                                e.preventDefault();
                                def.resolveWith(context);

                                //如果弹出的内容是纯粹的 Form 表单， 则可以提供便捷的 submit
                                if (msg.indexOf("<")>-1 && $(msg).length > 0 && $(msg).is('form')) {
                                    $('form', elem).submit();
                                } else {
                                    layer.close(i);
                                }
                            });

                            //成功弹出后的回调
                            if (options && $.isFunction(options.onPopUp)) {
                                options.onPopUp(elem);
                            }
                        }
                    });
                })
                .fail(function () {
                    if (preConfirm(msg)) {
                        def.resolveWith(context);
                    } else {
                        def.rejectWith(context);
                    }
                });
        } else {//有些页面没有 $.layer
            setTimeout(function () {
                if (preConfirm(msg)) {
                    def.resolveWith(context);
                } else {
                    def.rejectWith(context);
                }
            }, 1);
        }
        return def.promise();
    };

    window.alert = $.trip.alert;

    //日期格式转换
    $.trip.formatDate = function (d, format) {
        if (!d) return '';

        d = $.type(d) === 'date' ? d : new Date(d);
        if (d.toString() == 'Invalid Date') return '';

        //处理客户端时区不同导致的问题
        //480 是UTC+8
        var utc8Offset = 480;
        d.setMinutes(d.getMinutes() + (d.getTimezoneOffset() + 480));

        format = format || 'MM/dd hh:mm:ss tt';

        var hour = d.getHours();
        var month = FormatNum(d.getMonth() + 1)

        var re = format.replace('YYYY', d.getFullYear())
        .replace('YY', FormatNum(d.getFullYear() % 100))
        .replace('MM', FormatNum(month))
        .replace('dd', FormatNum(d.getDate()))
        .replace('hh', hour == 0 ? '12' : FormatNum(hour <= 12 ? hour : hour - 12))
        .replace('HH', FormatNum(hour))
        .replace('mm', FormatNum(d.getMinutes()))
        .replace('ss', FormatNum(d.getSeconds()))
        .replace('tt', (hour < 12 ? 'AM' : 'PM'))
        .replace('M', month)
        .replace('d', d.getDate());

        return re;

        function FormatNum(num) {
            num = Number(num);
            return num < 10 ? ('0' + num) : num.toString();
        }
    };

    //右上角通知消息弹出层
    $.trip.showMsgTip = function (msgHtml) {
        function _showMsgTip(msgContent) {
            $.trip.template('/communal/template.html').render('tpl_MsgTip', { MsgHtml: msgHtml }).done(function (html) {
                $(".MsgTipBox ul").append(html);

                $(".MsgTipBox .closeBtn").click(function (e) {
                    e.preventDefault();
                    $(".header .MsgTipBox").remove();
                });
            });
        }
        if ($(".header .MsgTipBox").length == 0) {
            $.trip.template('/communal/template.html').render('tpl_MsgTipBox').done(function (html) {
                $(".header").append(html);
                _showMsgTip(msgHtml);
            });
        } else {
            _showMsgTip(msgHtml);
        }
    };

    /* $.trip.template 模板渲染 依赖jQuery, jsrender */
    (function ($) {
        var templates = {};
        var supportEngines = { jsRender: 'jsRender', artTemplate: 'artTemplate' };

        var templateEngine;

        function setupEngine() {
            templateEngine = templateEngine || ($.templates ? supportEngines.jsRender : (window.template && window.template.compile) ? supportEngines.artTemplate : '');

            return templateEngine;
        }

        function loadTemplateFile(tplFile) {
            setupEngine();

            var that = templates[tplFile] = {};

            var callbacks = $.Callbacks();
            var loaded = false;
            var cacheTemplates = {};

            var loadTemplateFile = function () {
                if (!setupEngine()) setTimeout(loadTemplateFile, 5); //此时 jsrender 模版js 可能载入还没完成

                $.ajax(tplFile, { data: { _: new Date().toLocaleDateString() } }).done(function (data) {
                    var obj = {}
                    $(data).filter('script').each(function (index, item) {
                        var id = item.id;
                        if (id) {
                            obj[item.id] = $(item).html();
                        }
                    });
                    //预编译
                    switch (templateEngine) {
                        case supportEngines.jsRender:
                            {
                                cacheTemplates = $.templates(obj);
                                break;
                            }
                        case supportEngines.artTemplate:
                            {
                                for (var key in obj) {
                                    cacheTemplates.render[key] = template.compile(key, obj[key]);
                                }
                                break;
                            }
                        default:
                    }

                    callbacks.fire();

                    loaded = true;
                });
            }

            loadTemplateFile();

            that.render = function (tplId, json, helpersOrContext) {
                var def = $.Deferred();
                if (loaded) {
                    if (cacheTemplates.render && cacheTemplates.render[tplId]) {
                        var html = cacheTemplates.render[tplId](json, helpersOrContext);
                        def.resolve(html);
                    } else {
                        def.reject();
                    }
                } else {
                    callbacks.add(function () {
                        if (cacheTemplates.render && cacheTemplates.render[tplId]) {
                            var html = cacheTemplates.render[tplId](json, helpersOrContext);
                            def.resolve(html);
                        } else {
                            def.reject();
                        }
                    });
                }
                return def.promise();
            };
            return that;
        }

        var _template = function (tplFile) {
            var tpl = templates[tplFile] || loadTemplateFile(tplFile);

            var that = {};

            that.render = function (tplId, json, helpersOrContext) {
                if (!helpersOrContext || !helpersOrContext.console) {
                    helpersOrContext = $.extend(helpersOrContext, { console: window.console });
                }

                //为artTemplate添加辅助方法
                if (helpersOrContext && templateEngine === supportEngines.artTemplate) {
                    for (var key in helpersOrContext) {
                        template.helper(key, helpersOrContext[key]);
                    }
                }

                return tpl.render(tplId, json, helpersOrContext);
            }

            return that;
        }

        //  调用方法 
        //  var t = $.trip.template(tplFile)
        //  t.render(tplId, json [, helpersOrContext]).done(function(html){  
        //      ... html 为渲染后的字符串
        //  })
        $.extend($.trip, {
            template: _template
        });

        //预加载全局模版
        $.trip.template('/communal/template.html');
    })($);

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

    /* $.trip.user 用户登录模块， 依赖jQuery, $.trip.template */
    (function ($) {
        var $user = {}, loginIndex = 0;

        var layerTips = 0;
        var showTips = function (msg) {
            var $tip = $(".loginPop .body .login .xubox_tips");
            $tip.find(".xubox_tipsMsg").html(msg);
            $tip.css("visibility", "visible");
        }, closeTips = function () {
            var $tip = $(".loginPop .body .login .xubox_tips");
            $tip.css("visibility", "hidden");
        };
        //, refreshVCode = function () {
        //    $('#imgVCode').attr('src', '/user/ValidateCode.aspx?id=' + Math.random())
        //}
        var refreshLogin = function (loginName, loginPwd, txtVCode, txtcheck) {
            var $loginDef = $.Deferred(), $loginButton = $(".loginPop .body .content .login .commit input");
            if ($loginButton.data("loging") == "1") {
                $loginDef.reject();
                return $loginDef.promise();
            }
            $loginButton.val("登录中...");
            $loginButton.data("loging", "1");
            var $HeaderMenuNumber = $("#HeaderMenuNumber"), MenuNumber = 0;
            if ($HeaderMenuNumber.length > 0) {
                MenuNumber = $HeaderMenuNumber.data("value");
            }
            $.ajax({
                type: "POST",
                url: "/user/info/RefreshLogin.aspx",
                dataType: "json",
                data: { UserName: loginName, UserPwd: loginPwd, VCode: txtVCode, check: txtcheck, MenuNumber: MenuNumber }
            }).done(function (data) {
                if (/OK/i.test(data.reply)) {
                    try {
                        var _u = data.user;
                        $("#hidLoginUserId").val(_u.UserID);
                        $("#hidLoginUserPhoto").val(_u.Photo);
                        $("#loginNickname").val(_u.NickName);
                        $("#LevelJifen").val(_u.Jifen);
                        $("#Sex").val(_u.Sex);
                        //html
                        var $newHtml = $(data.html.html);
                        if (data.html != null) {
                            var className = "replaceHeadHtml", selector = ".replaceHeadHtml", $thisReplace = $(selector);
                            if ($newHtml.find(selector).length > 0) {
                                $thisReplace.replaceWith($newHtml.find(selector));
                            } else {
                                $newHtml.each(function () {
                                    var $that = $(this);
                                    if ($that.hasClass(className)) {
                                        $thisReplace.replaceWith($that);
                                        return false;
                                    } else if ($that.find(selector).length > 0) {
                                        $thisReplace.replaceWith($that.find(selector));
                                        return false;
                                    }
                                });
                            }
                        }
                        //cookie
                        if (data.cookie != null) {
                            var _cookie = data.cookie;
                            var expires = null;
                            for (var i = 0; i < _cookie.length; i++) {
                                if (_cookie[i] != null) {
                                    expires = new Date(_cookie[i].expires);
                                    $.trip.cookie.set(_cookie[i].name, _cookie[i].value, expires);
                                }
                            }
                        }

                        $.trip.pageInit();

                        closeTips();
                        if (loginIndex > 0) {
                            layer.close(loginIndex);
                            loginIndex = 0;
                        }
                        $loginDef.resolve();
                    }
                    catch (e) {
                        console.dir(e);
                        $loginDef.reject();
                    }
                } else {
                    //refreshVCode();
                    $loginDef.reject(data.msg);
                }
            }).always(function () {
                $loginButton.val("登录");
                $loginButton.data("loging", "0");
            });
            return $loginDef.promise();
        };

        $user.showLogin = function (jumpurl, jumptarget, regurl, regtarget) {
            if (loginIndex > 0) { return; }
            $user.loginThree.url = jumpurl;
            var $def = $.Deferred();
            //初始化弹出层
            $.trip.template('/communal/template.html').render('tplPopLogin', { url: encodeURIComponent(window.location.href), regurl: encodeURIComponent(regurl) }).done(function (html) {
                loginIndex = $.layer({
                    type: 1,
                    title: false,
                    closeBtn: [0, false],
                    border: [3, 0.5, '#666', false],
                    offset: ['140px', ''],
                    area: ['392px', '416px'],
                    page: {
                        html: html
                    },
                    success: function (elem) {
                        $(elem).on('click', '.close-btn', function (e) {
                            e.preventDefault();
                            closeTips();
                            layer.close(loginIndex);
                            loginIndex = 0;
                            $def.reject();
                        });

                        $(elem).on('click', '.loginPop .body .content .login .username span, .loginPop .body .content .login .password span, .loginPop .body .content .login .username label, .loginPop .body .content .login .password label', function (e) {
                            e.preventDefault();
                            $(this).parents("p").find("input").focus();
                        });

                        function setupInputTip(e) {
                            var $that = $(this);
                            if ($that.val().length > 0) {
                                $that.parents("p").find("span").hide();
                            } else {
                                $that.parents("p").find("span").show();
                            }
                        }
                        var _inputSelector = ".loginPop .body .content .login .username input, .loginPop .body .content .login .password input";
                        $(elem).on('keydown', _inputSelector, setupInputTip).on('keyup', _inputSelector, setupInputTip).on('keypress', _inputSelector, setupInputTip).on('change', _inputSelector, setupInputTip);
                        $(elem).find(".loginPop .body .content .login .username input").focus().css("border", "1px solid #1b8e1f");
                        $(elem).on('blur', _inputSelector, function () {
                            $(this).css("border", "1px solid #ddd");
                        }).on('focus', _inputSelector, function () {
                            $(this).css("border", "1px solid #1b8e1f");
                        });
                        //dragMove
                        function initLocation() {
                            var $w = $("#xubox_layer" + loginIndex);
                            if ($w.length > 0) {
                                var _w = String($w.css("margin-left")).replace("px", "");
                                $w.css("margin-left", "0");
                                $w.css({ "left": $w.offset().left + parseInt(_w) });
                            } else {
                                setTimeout(initLocation, 100);
                            }
                        }
                        initLocation();
                        var OX = 0, OY = 0, MX = 0, MY = 0, draging = false;
                        function setupLocation(t, e) {
                            var w = $(window).width(), h = $(window).height(), $dragObject = $("#xubox_layer" + loginIndex), OW = $dragObject.outerWidth(), OH = $dragObject.outerHeight();
                            if (t == 1) {
                                OX = $dragObject.offset().left;
                                OY = $dragObject.offset().top;
                                MX = e.pageX;
                                MY = e.pageY;

                                draging = true;
                            } else if (t == 2) {
                                if (draging) {
                                    var x = OX - (MX - e.pageX), y = OY - (MY - e.pageY);
                                    if (x < 0) { x = 0; }
                                    else if (x > (w - OW)) { x = w - OW; }
                                    if (y < 0) { y = 0; }
                                    else if (y > (h - OH)) { y = h - OH; }
                                    $dragObject.css({ "left": x, "top": y });
                                }
                            }
                            else if (t == 3) {
                                draging = false;
                            }
                        }
                        $(document).mousemove(function (e) {
                            if (draging) {
                                e.preventDefault();
                                setupLocation(2, e);
                            }

                        }).mouseup(function (e) {
                            if (draging) {
                                if (e.which == 1) {
                                    e.preventDefault();
                                    setupLocation(3, e);
                                }
                            }
                        });
                        $(elem).on("mousedown", ".t-title", function (e) {
                            if (e.which == 1) {
                                e.preventDefault();
                                setupLocation(1, e);
                            }
                        });

                        function refreshLoginSuccess() {
                            if (typeof (jumpurl) != "undefined" && jumpurl != null && jumpurl != "") {
                                if (typeof (jumptarget) != "undefined" && jumptarget != null && jumptarget != "") {
                                    window.open(jumpurl, jumptarget);
                                }
                                else {
                                    window.location = jumpurl;
                                }
                            }

                            if ($.trip.url("path").toLowerCase() == "/user/login.aspx") {
                                window.location.reload();
                            }
                            $def.resolve();
                        }

                        //$('#imgVCode').click(refreshVCode);

                        $('#formPopLogin').submit(function (e) {
                            e.preventDefault();
                            var loginName = $.trim($("#login_ipt_email").val());
                            var loginPwd = $("#login_ipt_pwd").val();
                            var txtVCode = "";//$('#txtVCode').val();
                            var txtcheck = document.getElementById("checklogin").checked;
                            if (loginName.length < 1 || loginPwd.length < 1) {
                                showTips('请输入正确的用户名和密码');
                                return false;
                            }
                            //if (txtVCode.length != 4) {
                            //    showTips('无效的验证码');
                            //    return false;
                            //}
                            closeTips();

                            refreshLogin(loginName, loginPwd, txtVCode, txtcheck).done(refreshLoginSuccess).fail(function (msg) {
                                showTips(msg);
                            });
                        });
                    }
                });
                if (loginIndex <= 0) {
                    $def.reject();
                }
                return;
            });

            function checkThreeLogin() {
                setTimeout(function () {
                    if ($user.loginThree.loginResult) {
                        $def.resolve();
                    } else {
                        checkThreeLogin();
                    }
                }, 10);
            }
            checkThreeLogin();

            return $def.promise();
        };

        $(document).on("click", ".threeLoginC .lb", function (e) {
            e.preventDefault();
            var url = $(this).attr("href");
            var x = ($(window).width() - 480) / 2, y = ($(window).height() - 420) / 2;
            $.trip.user.loginThree.loginWindow = window.open(url, "_blank", "resizable=yes,width=480,height=420,top=" + y + ",left=" + x);//screenX,screenY
        });

        $user.loginThree = {
            completed: function () {
                refreshLogin().done(function () {
                    $user.loginThree.loginResult = 1;
                    if ($.trip.user.loginThree.loginWindow) {
                        $.trip.user.loginThree.loginWindow.opener = null;
                        $.trip.user.loginThree.loginWindow.close();
                    }
                    if ($user.loginThree.url) {
                        window.location = $user.loginThree.url;
                    }
                    if ($.trip.url("path").toLowerCase() == "/user/login.aspx") {
                        window.location.reload();
                    }
                })
            },
            url: "",
            loginResult: 0,
            resize: function () {
                $.trip.user.loginThree.loginWindow.resizeTo(700, 520);
            }
        };

        $user.getLoginId = function () {
            return +$('#hidLoginUserId').val() || 0;
        }

        $user.isLogin = function () {
            return this.getLoginId() > 0;
        }

        //检查登录, 已登录则返回 true , 否则弹出登录框并返回 false
        $user.checkLogin = function () {
            var _islogin = $user.isLogin();
            if (!_islogin) {
                $user.showLogin();
            }
            return _islogin;
        }

        $user.getLoginPhoto = function () {
            return $('#hidLoginUserPhoto').val();
        }

        $user.getLoginNickname = function () {
            return $('#hidLoginNickname').val();
        }

        $user.msg = (function () {
            //保存当前页面默认标题
            var srctitle = document.title;
            var enable = true;
            var remindHandle = 0;
            var resHandle = 0;

            function getUserMsg() {
                $.get("/user/comm/Public.aspx", { action: "geturmsg" }, function (json) {
                    if (String(json) == "" || String(json) == "null") { resTitle(); return; }
                    json = $.parseJSON(json);
                    var totalCount = 0;
                    for (var i = 0; i < json.length; i++) {
                        var msgCount = json[i].MsgCount;
                        totalCount += msgCount;
                        $('#ulMsgType span:eq(' + json[i].mType + ')').data("msgCount", msgCount);
                        $('#ulMsgType span:eq(' + json[i].mType + ')').html(msgCount ? ('(' + msgCount + ')') : '');
                    }
                    if (totalCount && totalCount > 0) {
                        $('#spanMsgCount').data("msgCount", totalCount);
                        $('#spanMsgCount').html('(' + totalCount + ')');
                        resHandle = setInterval(remindTitle, 1000);
                    }
                    else {
                        resTitle();
                    }
                });
            }

            function resTitle() {
                document.title = srctitle;
                window.clearInterval(resHandle);
                window.clearTimeout(remindHandle);
            }

            function remindTitle() {
                document.title = "【您有新消息】" + srctitle;
                remindHandle = setTimeout(repeatRemindTitle, 1000);
            }

            function repeatRemindTitle() {
                document.title = "【　　　　　】" + srctitle;
                window.clearTimeout(remindHandle);
            }

            var that = {
                disable: function () {
                    enable = false;
                },
                init: function () {
                    if ($.trip.user && $.trip.user.isLogin()) {//登录状态下才需要定时抓取消息
                        getUserMsg();//页面加载完毕后马上获取一次最新消息

                        setInterval(function () {
                            if (enable) {
                                getUserMsg();
                            }
                        }, 30000);
                    }
                }
            };
            return that;
        })();

        $(document).ready(function () {
            //对需要登录的操作绑定点击弹出登录框的事件
            $(document).on('click', '.requireLogin', function (e) {
                if (!$user.isLogin()) {
                    e.preventDefault();
                    var $that = $(this), url = ($that.is("a") ? $that.attr("href") : ""), target = (url != "" ? ($that.attr("target") == undefined ? "_self" : $that.attr("target")) : "");
                    if (url == "#" || url == "javascript:void(0);") { url = ""; }
                    if ($that.data().jumpurl) { url = $that.data().jumpurl; }
                    $user.showLogin(url, target, (url == "" ? window.location.href : url), target);
                }
            });
            if ($.trip.url("?loginthree")) {
                window.opener.$.trip.user.loginThree.completed();
            } else if ($.trip.url("path").toLowerCase() == "/user/loginbander.aspx") {
                window.opener.$.trip.user.loginThree.resize();
            }
        });
        $.trip.user = $.trip.user || {};
        $.extend($.trip.user, $user);
    })(jQuery);

    /* 评论模块 */
    (function (window, $) {
        //分页插件，评论模块的分页使用 #comment 标签表示第几页
        var setupPaged = function (pageNow, pageSize, totalCount) {

            var html = [], totalPage = parseInt(totalCount / pageSize) + 1;

            if (totalPage > 1) {
                html.push(getLink(pageNow - 1, '上一页'));
                for (var i = 1; i <= totalPage; i++) {
                    html.push(getLink(i, i));
                }
                html.push(getLink(pageNow + 1, '下一页'));
            }

            return html.join('');

            function getLink(p, text) {
                var href = p > 0 && p <= totalPage ? ' href="#comment=' + p + '" ' : ''
                    , css = pageNow == p ? ' class="currentpage" ' : ''
                    , data = p > 0 && p <= totalPage ? ' data-p="' + p + '" ' : '';
                return '<a ' + data + css + href + '>' + text + '</a>'
            }
        };

        //处理评论的数据结构
        var processCommentData = function (commentData) {
            var comments = [], lastItem;
            for (var i = 0; i < commentData.length; i++) {
                var item = commentData[i];
                if (item.commentId == item.replyId) {
                    item.reply = [];
                    lastItem = item;
                    comments.push(item);
                } else {
                    lastItem.reply.push(item);
                }
            }
            return comments;
        }

        var setupBindingFace = function (html) {
            var $that = null, $face = null;;
            if (html) {
                $face = $(html).find(".comm_comment_face");
            }
            else {
                $face = $(".comm_comment_face");
            }
            $face.each(function () {
                $that = $(this);
                $.trip.face.bind("#" + $that.attr("id"), $that.data("selector"));
            });
        }

        $.fn.extend({
            comment: function () {
                var $commPanel = $(this);
                if (!$commPanel.length) return $commPanel;

                var module = $commPanel.data('module')
                    , moduleId = $commPanel.data('id')
                    , moduleUserId = $commPanel.data('uid') || 0  //关联内容的创建者
                    , title = $commPanel.data('title') || ''  //关联内容的创建者
                    , pageSize = $commPanel.data('size')
                    , templates = $.trip.template('/communal/template.html')
                    , loading = false
                    , $commList;

                var loadComments = function (pageNow) {
                    pageNow = +pageNow;
                    if (!loading && pageNow > 0) {
                        loading = true;
                        $.ajax({
                            url: '/communal/ws_comment.aspx',
                            data: {
                                action: 'list',
                                module: module,
                                moduleId: moduleId,
                                pageNow: pageNow,
                                pageSize: pageSize
                            },
                            dataType: 'json',
                            type: 'GET'
                        }).done(function (json) {
                            json.dataList = processCommentData(json.dataList);

                            templates.render('tpl_comment_list', json.dataList).done(function (html) {
                                $commPanel.find('.commList').html(html);

                                setupBindingFace($commPanel);
                            });
                            $commPanel.find('.page').html(setupPaged(json.pageNow, json.pageSize, json.total));
                            loading = false;

                            if ($.trip.user && $.trip.user.showUserCardFromHtml) {
                                $.trip.user.showUserCardFromHtml($commPanel);
                            }

                        });
                    }
                };
                var addComment = function (content, replyId, replyUser) {
                    return $.ajax({
                        url: '/communal/ws_comment.aspx',
                        data: {
                            action: 'add',
                            module: module,
                            moduleId: moduleId,
                            content: content,
                            replyId: replyId,
                            replyUser: replyUser,
                            moduleUserId: moduleUserId,
                            title: title
                        },
                        dataType: 'json',
                        type: 'POST'
                    });
                };

                templates.render('tpl_comment_panel', {
                    loginId: $.trip.user.getLoginId(),
                    loginPhoto: $.trip.user.getLoginPhoto(),
                    loginNickname: $.trip.user.getLoginNickname()
                }).done(function (html) {
                    $commPanel.html(html);
                    $commList = $commPanel.find('.commList');

                    var urlHash = $.trip.url('#');

                    if (urlHash.comment != undefined) {
                        var pos = $commPanel.offset().top - 100;
                        $("html,body").animate({ scrollTop: pos }, 500);
                    }

                    loadComments(urlHash.comment || 1);

                    //点击分页
                    $commPanel.on('click', '.page>a', function (e) {
                        loadComments($(this).data('p'));
                    });

                    //提交主留言
                    $commPanel.find('#btnAddComment').click(function (e) {
                        if ($.trip.user.isLogin()) {
                            var txt = $.trim($('#txtComment').val());
                            if (txt.length < 1) {
                                alert('请输入你的留言！');
                                return false;
                            }
                            addComment(txt, 0, 0).done(function (json) {
                                templates.render('tpl_comment_list', json)
                                .done(function (html) {
                                    $commList.prepend(html);

                                    setupBindingFace(html);
                                });
                            });
                            $('#txtComment').val('')
                        }
                    });

                    //点击取消按钮
                    $commPanel.on('click', '.replyCancel', function (e) {
                        e.preventDefault();

                        $(this).parent().parent().hide();
                    })

                    //提交回复
                    $commPanel.on('click', '.replyAdd', function (e) {
                        e.preventDefault();

                        var replyId = $(this).data('id');
                        var replyUser = $(this).data('user');
                        var txtInpput = $("#txtComment" + replyId)//$(this).prev(':text');
                        var txt = $.trim(txtInpput.val());

                        var txt = $.trim(txtInpput.val());
                        if (txt.length < 1) {
                            alert('请输入你的回复！');
                            return false;
                        }
                        txtInpput.val('');
                        addComment(txt, replyId, replyUser).done(function (json) {
                            templates.render('tpl_reply', json)
                            .done(function (html) {
                                $('#replyList' + replyId).prepend(html);
                                $('#replyContain' + replyId).hide();
                            });
                        });
                    });

                    //点击回复链接，显示回复框
                    $commPanel.on('click', 'a.toReply', function (e) {
                        if ($.trip.user.isLogin()) {
                            var replyId = $(this).data('id');

                            $('#replyContain' + replyId).show();
                            e.preventDefault();
                        }
                    });

                    $commPanel.on('mouseover', '.bbs', function () {
                        $(this).find('.toReply').show();
                    });
                    $commPanel.on('mouseout', '.bbs', function () {
                        $(this).find('.toReply').hide();
                    });

                    setupBindingFace($commPanel);
                });

                return $commPanel;
            }
        });
    })(window, $);

    /* 输入框选择表情 */
    (function ($) {
        $.trip = $.trip || {};
        if (typeof ($.trip.face) == "object") { return; }
        $.extend($.trip, {
            face: {
                params: { delayIntr: null, inited: false, initStart: false, bindElem: { bc: [], oc: [], isfix: [] } },
                bind: function (_bindControl, _OutputControl, _isFix) {
                    var tmpl = $.trip.template('/communal/template.html');
                    if ($("#facelist").length == 0 || $(_bindControl).length == 0) {
                        if (!$.trip.face.params.inited && !$.trip.face.params.initStart) {
                            this.params.initStart = true;
                            initialize();
                        }
                        this.params.bindElem.bc.push(_bindControl);
                        this.params.bindElem.oc.push(_OutputControl);
                        this.params.bindElem.isfix.push(_isFix);
                        if (this.params.delayIntr == null) {
                            this.params.delayIntr = setInterval(function () {
                                if ($("#facelist").length > 0) {
                                    while ($.trip.face.params.bindElem.bc.length > 0) {
                                        binding($.trip.face.params.bindElem.bc.shift(), $.trip.face.params.bindElem.oc.shift(), $.trip.face.params.bindElem.isfix.shift());
                                    }
                                    if ($.trip.face.params.delayIntr != null) {
                                        //window.clearInterval($.trip.face.params.delayIntr);
                                        //$.trip.face.params.delayIntr = null;
                                    }
                                }
                            }, 200);
                        }
                    }
                    else {
                        binding(_bindControl, _OutputControl, _isFix);
                    }

                    function initialize() {
                        if ($("#facelist").length > 0) { return; }
                        tmpl.render("tpl_faceImage").done(function (html) {
                            var $html = $(html);
                            $html.find("li").click(function (e) {
                                var outputObj = null;
                                var bindlist = String($("#facelist").val()).split("|");
                                for (var i = 0; i < bindlist.length; i++) {
                                    if (bindlist[i] == "") continue;
                                    var tmp = bindlist[i].split(",");
                                    if (tmp.length > 0 && $(tmp[0]).attr("id") == $("#facelist").data("outputobj"))
                                        outputObj = tmp[1];
                                }
                                if (outputObj != null)
                                    $(outputObj).val($(outputObj).val() + $(this).attr("data-text"));
                            }).mouseenter(function () { $(this).addClass("face_slc_li"); }).mouseleave(function () { $(this).removeClass("face_slc_li"); });
                            $(function () {
                                if (!$.trip.face.params.inited) {
                                    $("form").append($html);
                                }
                                $.trip.face.params.inited = true;
                            });
                        });
                    }

                    //绑定元素选择表情
                    function binding(bindControl, OutputControl, isFix) {
                        var isEixsts = false;
                        if ($("#facelist").val() != "") {
                            var bindlist = String($("#facelist").val()).split("|");
                            for (var i = 0; i < bindlist.length; i++) {
                                var tmp = bindlist[i].split(",");
                                if (tmp.length > 0 && tmp[0] == bindControl) {
                                    isEixsts = true;
                                    break;
                                }
                            }
                        }
                        if (!isEixsts) {
                            if ($("#facelist").val() != undefined && $("#facelist").val() != "")
                                $("#facelist").val($("#facelist").val() + "|" + bindControl + "," + OutputControl);
                            else
                                $("#facelist").val(bindControl + "," + OutputControl);

                            var $bindElement = $(bindControl);
                            if (isFix) {
                                $bindElement.data("isFix", "1");
                            }
                            $bindElement.on("click", function () {
                                var $FaceImageList = $(".faceImage");
                                //用于评论时表情
                                $FaceImageList.bind("click", $bindElement.data("bindingReset"));

                                $("#facelist").data("outputobj", $bindElement.attr("id"));
                                var $this = $(this);
                                var _isfix = "";
                                if (typeof ($this.data("isFix")) != "undefined") {
                                    _isfix = $this.data("isFix");
                                }
                                if (!isFix) {
                                    $FaceImageList.css({ top: ($(this).offset().top + $(this).height()), left: $(this).offset().left }).css("position", "absolute").show();
                                }
                                else {
                                    $FaceImageList.css({
                                        top: $this.offset().top + $this.outerHeight() - $(document).scrollTop(), left: $this.offset().left - $(document).scrollLeft()
                                    }).css("position", "fixed").show();
                                    $FaceImageList.data("close", "false");
                                }

                                $FaceImageList.bind("mouseover", function () {
                                    $FaceImageList.data("close", "true");
                                }).bind("mouseleave", function () {
                                    $FaceImageList.data("close", "false");
                                });
                                $(window.document.body).bind("mousedown", function () {
                                    if ($FaceImageList.data("close") == "false") {
                                        $FaceImageList.hide();
                                    }
                                });


                            });
                        }
                    }
                }
            }
        });

    })(jQuery);

    /*常用联系人选择*/
    (function ($) {
        $.fn.extend({
            contactSel: function (single, callback) {
                var $contactPanel = $(this), contactData = null, contactRender = function () {
                    if (contactData.length > 0) {
                        $.trip.template('/communal/template.html').render('tpl_Contact', contactData).done(function (html) {
                            var $html = $("<ul class=\"clearfix\">" + html + "<li class=\"item\"><a href=\"/user/info/ContactMgr.aspx\" target=\"_blank\">【管理联系人】</a></li></ul>");
                            $html.on("change", "input", function () {
                                var $that = $(this), checked = ($that.attr("checked") == true || $that.attr("checked") == "checked"), thisID = $that.data("id");
                                if (single) {
                                    $that.parents("ul").find("input").attr("checked", false);
                                    $that.attr("checked", checked);
                                }
                                for (var i = 0; i < contactData.length; i++) {
                                    if (thisID == contactData[i].ID) {
                                        callback(checked, contactData[i]);
                                        break;
                                    }
                                }
                            });
                            $contactPanel.append($html);
                        });
                    } else {
                        $contactPanel.append("您还没有添加过任何联系人，<a href=\"/user/info/ContactSave.aspx\" style=\"color: #38a23b;\" target=\"_blank\">点此去添加</a>")
                    }
                };
                if (contactData == null) {
                    $.post("/user/info/contactmgr.aspx", { act: "get" }, function (json) {
                        contactData = $.parseJSON(json);
                        contactRender();
                    });
                } else {
                    contactRender();
                }
            }
        });
        $.trip.user = $.trip.user || {};
        $.trip.user.contactUpdate = function (name, sex, cardtype, cardcode, phone, birthday, Email) {
            return $.post("/user/info/contactmgr.aspx", { act: "update", name: name, sex: sex, cardtype: cardtype, cardcode: cardcode, phone: phone, birthday: birthday, Email: Email });
        };
    })(jQuery);

    /* 分页插件 */
    (function (window, $) {
        var $paged = function (dataAjax, ajaxCallBack) {
            var $this = $(this), pageParam = $this.data('pageparam') || 'page';

            var loadData = function (page) {
                var option = $.isFunction(dataAjax) ? dataAjax() : dataAjax;

                option = $.extend(option, { dataType: 'json' });
                option.data = option.data || {};
                option.data[pageParam] = +page || +$.trip.url('?' + pageParam) || 1;

                if (window.layer) window.layer.load();
                var _ajax = $.ajax(option).done(function (json) {
                    if (json) {
                        $this.buildPage(json.total, json.pageSize);
                        ajaxCallBack(json);
                    }
                }).always(function () {
                    if (window.layer) window.layer.loadClose();
                });

                return _ajax;
            };

            $this.reload = function () {
                loadData(1);
            }

            //自动载入
            loadData();

            return $this;
        };

        //生成链接
        var getLink = function (p, text, totalPage, pageparam) {
            var href = '', pageNow = +$.trip.url('?' + pageparam) || 1;
            if (p > 0 && p <= totalPage) {
                var params = $.trip.url('?');
                params[pageparam] = p;
                var query = '?';
                for (var key in params) {
                    query += '&' + key + '=' + params[key];
                }
                href = ' href="' + window.location.pathname + query + '" ';
            }

            var css = pageNow === +text ? ' class="currentpage" ' : ''
                , data = p > 0 && p <= totalPage ? ' data-p="' + p + '" ' : '';
            return '<a ' + data + css + href + '>' + text + '</a>';
        };

        var $buildPage = function (totalCount, pageSize) {
            var $this = $(this), pageparam = $this.data('pageparam') || 'page', pageSize = pageSize || +$this.data('size') || 10, pageNow = +$.trip.url('?' + pageparam) || 1;
            totalCount = totalCount || +$this.data('total');
            if (!totalCount) {
                $this.html('');
                return;
            }

            var html = [], totalPage = Math.ceil(totalCount / pageSize);

            if (totalPage > 1) {
                html.push(getLink(pageNow - 1, '上一页', totalPage, pageparam));

                if (pageNow > 2) {
                    html.push(getLink(1, 1, totalPage, pageparam));
                }

                if (pageNow > 3) {
                    html.push(getLink(0, '...', totalPage, pageparam));
                }

                if (pageNow > 1) {
                    html.push(getLink(pageNow - 1, pageNow - 1, totalPage, pageparam));
                }

                html.push(getLink(pageNow, pageNow, totalPage, pageparam));

                if (pageNow < totalPage) {
                    html.push(getLink(pageNow + 1, pageNow + 1, totalPage, pageparam));
                    html.push(getLink(0, '...', totalPage, pageparam));
                }

                html.push(getLink(pageNow + 1, '下一页', totalPage, pageparam));
            }

            var html = html.join('');
            $this.html(html);
            return $this;
        }

        //分页插件
        $.fn.extend({ paged: $paged, buildPage: $buildPage });

    })(window, $);

    /* 赞插件 */
    (function (window, $) {
        var $doLike = function (selector, module, callBack) {
            var $this = $(selector), clickBack = $.Callbacks();
            clickBack.add(callBack);
            $this.click(function (e) {
                e.preventDefault();

                if ($.trip.user.checkLogin()) {
                    var elm = this, $elm = $(this), cid = $elm.data('id'), title = $elm.data('title'), uid = $elm.data('uid');
                    $.ajax({
                        type: "POST",
                        url: '/communal/WSDoLike.aspx',
                        data: { m: module, cid: cid, title: title, uid: uid },
                        dataType: 'json'
                    }).then(function (json) {
                        if (json.res == 0) {
                            clickBack.fireWith(elm, [json]);
                        }
                    });
                }
            });

            return $this;
        };

        $.trip.doLike = $doLike;
    })(window, $);

    /* 收藏插件 */
    (function (window, $) {

        var $collection = function (selector) {
            //如果服务端已经渲染了是否收藏则无效通过Ajax初始化
            var $selector = $(selector);
            var itemArr = [], modules = [], moduleIds = [];
            $selector.each(function (i, item) {
                var $elm = $(item);
                if (!$.isNumeric($elm.data('cid'))) {
                    modules.push($elm.data('module'));
                    moduleIds.push($elm.data('moduleid'));
                    itemArr.push(item);
                }
            });

            if (itemArr.length > 0) {
                $.ajax('/communal/ws_collection.aspx', { data: { op: 'user', module: modules.join(','), moduleId: moduleIds.join(',') }, type: 'post', dataType: 'json' })
                 .done(function (json) {
                     for (var i = 0; i < json.length; i++) {
                         var item = json[i], $elm = $(itemArr[i]);

                         if (item.CollectionId > 0) {
                             $elm.addClass('collected');
                             $elm.attr('title', '已收藏');
                             $elm.data('cid', json.collectionId);
                         }
                         $elm.data('total', json.Total);
                         $elm.html(json.Total);
                     }
                 });
            }

            $selector.click(function (e) {
                e.preventDefault();

                if (!$.trip.user.checkLogin()) return;

                var $this = $(this), module = $this.data('module'), moduleId = $this.data('moduleid');

                if (!$this.hasClass('collected')) {
                    //点击添加收藏
                    $.ajax('/communal/ws_collection.aspx', { data: { op: 'add', module: module, moduleId: moduleId, moduleUserId: $this.data('moduleuid') || 0 }, type: 'post', dataType: 'json' })
                     .done(function (json) {
                         $this.addClass('collected');
                         $this.attr('title', '已收藏');
                         $this.data('cid', json.collectionId);
                         var total = $this.data('total') + 1;
                         $this.data('total', total);
                         $this.html(total);

                         alert("<b>收藏成功！<br/>你可以点 <a href=\"/user/collection/list.aspx\"> [我的收藏] </a> 查看！</b>")

                         //$.layer({
                         //    type: 1,
                         //    title: false,
                         //    move: ['', false],
                         //    closeBtn: [0, true],
                         //    border: [5, 0.5, '#666', true],
                         //    offset: ['140px', ''],
                         //    area: ['200px', '60px'],
                         //    page: {
                         //        html: '<div style="text-align: center;width: 180px;line-height: 18px;margin: 10px;">收藏成功，你可以点此查看<br><a class="inpbbut1" href="/user/collection/list.aspx">我的收藏</a></div>'
                         //    }
                         //});
                     });
                }
            });
        };

        $.trip.collction = $collection;

    }(window, $));

    /* 求捡, 发起活动, 写游记 */
    (function (window, $) {

        //在 活动、攻略、游记的详细页面会放置一个 class="module" 的元素，其中存储着目的地、主题等信息
        var getModuleInfo = function () {
            var $info = $('.module');

            var destStr = $info.data('dest') || $.trip.url('?dest') || '';
            var dests = [];
            if (destStr) {
                var ar = destStr.split(/[\|,]/);
                if (ar.length && ar.length % 2 === 0) {
                    for (var i = 0; i < ar.length; i += 2) {
                        dests.push(ar[i + 1]);
                    }
                } else {
                    dests.push(destStr);
                    destStr = '-1|' + destStr;
                }
            }

            var themeStr = $info.data('theme') || $.trip.url('?theme') || '';
            var themes = [];
            if (themeStr) {
                var ar = themeStr.split(/[\|,]/);
                if (ar.length && ar.length % 2 === 0) {
                    for (var i = 0; i < ar.length; i += 2) {
                        themes.push(ar[i + 1]);
                    }
                } else {
                    themes.push(themeStr);
                    themeStr = '-1|' + themeStr;
                }
            }

            return {
                module: $info.data('module') || '',
                moduleId: $info.data('mid') || 0,
                moduleUserId: $info.data('uid') || 0,
                dests: dests,
                destStr: destStr,
                themes: themes,
                themeStr: themeStr
            };
        }

        var $pickup = function (selector) {
            $(selector).click(function (e) {
                e.preventDefault();

                if (!$.trip.user.checkLogin()) return false;

                var $this = $(this), module = getModuleInfo();


                $.trip.template('/communal/template.html')
                    .render('tpl_pickup_pop', { dests: module.dests, destStr: module.destStr }).done(function (html) {

                        var i = $.layer({
                            type: 1,
                            title: false,
                            closeBtn: [0, true],
                            border: [5, 0.5, '#666', true],
                            offset: ['140px', ''],
                            area: ['auto', '240px'],
                            page: {
                                html: html
                            },
                            success: function () {
                                $('#btnPickAdd').click(function (e) {
                                    e.preventDefault();

                                    var wantTo = $.trim($('#tbWantTo').val()), freeDate = $('#tbFreeDate').val(), freeDays = +$('#tbFreeDays').val();
                                    if (!wantTo && !freeDate) {
                                        alert('想去的地方和空闲日期至少填一项！');
                                        return false;
                                    }
                                    if (freeDate && !freeDays) {
                                        alert('请填写有效的空闲天数！');
                                        return false;
                                    }

                                    $.ajax('/communal/ws_pick.aspx', {
                                        data: {
                                            tbWantTo: wantTo,
                                            taRemark: $('#taRemark').val(),
                                            module: module.module,
                                            moduleId: module.moduleId,
                                            moduleUserId: module.moduleUserId,
                                            tbFreeDays: freeDays,
                                            tbFreeDate: freeDate
                                        }, type: 'post'
                                    }).done(function () {

                                        layer.closeAll();
                                        $.layer({
                                            type: 1,
                                            title: false,
                                            move: ['', false],
                                            closeBtn: [0, true],
                                            border: [5, 0.5, '#666', true],
                                            offset: ['140px', ''],
                                            area: ['200px', '60px'],
                                            page: {
                                                html: '<div style="text-align: center;width: 180px;line-height: 18px;margin: 10px;">发布成功，你可以点此查看<br><a class="inpbbut1" href="/pickup/PickupMgr.aspx">我的求捡</a></div>'
                                            }
                                        });
                                    });
                                });
                            }
                        });
                    });
                return false;
            });
        };

        var $activityAdd = function (selector) {
            $(selector).click(function (e) {
                e.preventDefault();

                if (!$.trip.user.checkLogin()) return false;
                var module = getModuleInfo();

                window.location = '/huodong/add.aspx?module=' + module.module + '&moduleId=' + module.moduleId + '&dest=' + module.destStr + '&theme=' + module.themeStr;
            });
        }

        var $diaryAdd = function (selector) {
            $(selector).click(function (e) {
                e.preventDefault();

                if (!$.trip.user.checkLogin()) return false;

                var module = getModuleInfo();

                window.location = '/diary/TravelDiaryAdd.aspx?module=' + module.module + '&moduleId=' + module.moduleId + '&dest=' + module.destStr + '&theme=' + module.themeStr;
            });
        }

        $(document).ready(function () {
            //$pickup('.pickup');
            $activityAdd('.actAdd');
            $diaryAdd('.diaryAdd');
        });
    }(window, $));

    /* 通过重载 $.ajax 实现防止重复点击，最少间隔 500ms */
    (function (window, $) {
        var preAjax = $.ajax,
            //用每个Ajax请求的Url做Key
            ajaxHolder = {};

        $.ajax = function (url, options) {
            if (typeof url === "object") {
                options = url;
                url = undefined;
            }
            var setup = jQuery.ajaxSetup({}, options);
            var key = (url || setup.url) + '&' + $.param(options.data || '');
            if (key.toLowerCase().indexOf("repeat=1".toLowerCase()) == -1 && ajaxHolder[key]) {
                return $.Deferred().reject();
            }

            ajaxHolder[key] = true;

            if (key.indexOf('.aspx') > -1) {
                options = $.extend(options, { cache: false });
            }

            var _ajax = preAjax(url, options).always(function () {
                setTimeout(function () {
                    delete ajaxHolder[key];
                }, setup.interval || 500);
            });
            return _ajax;
        }
    }(window, $));

    /* Cookie 操作*/
    (function (window, $) {
        $.trip.cookie = {
            get: function (c_name) {
                if (document.cookie.length > 0) {　　//先查询cookie是否为空，为空就return ""
                    c_start = document.cookie.indexOf(c_name + "=")　　//通过String对象的indexOf()来检查这个cookie是否存在，不存在就为 -1　　
                    if (c_start != -1) {
                        c_start = c_start + c_name.length + 1　　//最后这个+1其实就是表示"="号啦，这样就获取到了cookie值的开始位置
                        c_end = document.cookie.indexOf(";", c_start)　　//其实我刚看见indexOf()第二个参数的时候猛然有点晕，后来想起来表示指定的开始索引的位置...这句是为了得到值的结束位置。因为需要考虑是否是最后一项，所以通过";"号是否存在来判断
                        if (c_end == -1) c_end = document.cookie.length
                        return unescape(document.cookie.substring(c_start, c_end))　　//通过substring()得到了值。想了解unescape()得先知道escape()是做什么的，都是很重要的基础，想了解的可以搜索下，在文章结尾处也会进行讲解cookie编码细节
                    }
                }
                return "";
            },
            set: function (c_name, value, expires) {
                var exdate = new Date();
                if (expires != null) {
                    if ($.isNumeric(expires)) {
                        exdate.setDate(exdate.getDate() + expires);
                    } else {
                        exdate = expires;
                    }
                }
                var cookie = c_name + "=" + escape(value) + ';path=/' + ((expires == null) ? "" : ";expires=" + exdate.toGMTString());
                var domain = $('#hidCookieDomain').val();
                if (domain) {
                    cookie = cookie + ';domain=' + domain;
                }
                document.cookie = cookie;
            }
        };
    })(window, $);

    /* 城市选择器 */
    (function (window, $) {
        var $citySelector, bindingElm, bindingCallback, myCityCode, myCityName;
        //var bindingElmSelector = {};
        //从 Cookie 中读取用户所在城市
        var cookieCity = $.trip.cookie.get('city');
        if (cookieCity && (cookieCity = cookieCity.split('|')).length === 2) {
            myCityCode = cookieCity[0].indexOf('0086') === 0 ? cookieCity[0].substr(4) : cookieCity[0];
            myCityName = cookieCity[1];
        }

        var showDiv = function (top, left) {
            $citySelector.offset({ top: top, left: left }).show();
        }

        var hideDiv = function () {
            bindingElm = null;
            $citySelector && $citySelector.offset({ top: 0, left: 0 }).hide();
        }

        $.fn.extend({
            citySel: function (showEvent, callback, offset) {
                $(this).each(function () {
                    $(this).on(showEvent, function (event) {

                        hideDiv();

                        bindingElm = this;
                        bindingCallback = callback;

                        var elmOffset = $(bindingElm).offset(), positionLeft = elmOffset.left, positionTop = elmOffset.top + $(bindingElm).outerHeight();

                        if (offset) {
                            positionLeft = positionLeft + (offset.left || 0);
                            positionTop = positionTop + (offset.top || 0);
                        }

                        if (!$citySelector) {
                            var tpls = $.trip.template('/communal/template.html');
                            tpls.render('tpl_city').done(function (html) {

                                $('body').append(html);

                                $citySelector = $('.J_ChinaCity');

                                if (myCityCode) {
                                    $citySelector.find('dd:first span').filter(function () {
                                        return $('a', this).data('zip').toString() === myCityCode;
                                    }).remove();

                                    $citySelector.find('span.b').after('<span class="b"><a data-zip="' + myCityCode + '" href="#">' + myCityName + '</a></span>');
                                }

                                var $lis = $citySelector.find('li');

                                $citySelector.on('click', 'li', function () {
                                    var $li = $(this);
                                    if ($li.hasClass('selected')) return;

                                    if ($li.hasClass('ico_close_d')) {
                                        hideDiv();
                                        return;
                                    }

                                    $citySelector.find('li.selected').removeClass('selected');
                                    $li.addClass('selected');

                                    var index = $lis.index($li);

                                    $citySelector.find('.tab-pannel').hide();
                                    $citySelector.find('.tab-pannel:eq(' + index + ')').show();
                                });

                                $citySelector.on('click', 'a', function (eClick) {
                                    eClick.preventDefault();

                                    if ($.isFunction(bindingCallback)) {
                                        var zip = $(this).data('zip');
                                        bindingCallback.call(bindingElm, zip ? ('0086' + $(this).data('zip')) : zip, $(this).text());
                                    }
                                    hideDiv();
                                });

                                showDiv(positionTop, positionLeft);
                            });
                        } else {
                            showDiv(positionTop, positionLeft);
                        }
                    })
                })
            }
        });

        $.extend({
            citySel: {
                hide: function () {
                    hideDiv();
                }
            }
        });

    })(window, $);

    // 对 sceditor 的扩展, 参考文档 http://www.sceditor.com/
    // 以下代码依赖于 /js/sceditor/jquery.sceditor.bbcode.min.js
    (function ($, window, document) {
        var isSceditorSetuped = false;

        var setupSceditor = function () {
            if (isSceditorSetuped) return;

            isSceditorSetuped = true;

            //语言处理
            $.sceditor.locale["cn"] = {
                "Bold": "粗体",
                "Italic": "斜体",
                "Underline": "下划线",
                "Strikethrough": "删除线",
                "Subscript": "下标",
                "Superscript": "上标",
                "Align left": "靠左对齐",
                "Center": "置中",
                "Align right": "靠右对齐",
                "Justify": "两端对齐",
                "Font Name": "字体",
                "Font Size": "字号",
                "Font Color": "字色",
                "Remove Formatting": "格式清除",
                "Cut": "剪切",
                "Your browser does not allow the cut command. Please use the keyboard shortcut Ctrl/Cmd-X": "您的浏览器不支持剪切命令，请使用快捷键 Ctrl/Cmd-X",
                "Copy": "拷贝",
                "Your browser does not allow the copy command. Please use the keyboard shortcut Ctrl/Cmd-C": "您的浏览器不支持拷贝命令，请使用快捷键 Ctrl/Cmd-C",
                "Paste": "粘贴",
                "Your browser does not allow the paste command. Please use the keyboard shortcut Ctrl/Cmd-V": "您的浏览器不支持粘贴命令，请使用快捷键 Ctrl/Cmd-V",
                "Paste your text inside the following box:": "请在下面贴入您的文本",
                "Paste Text": "粘贴纯文本",
                "Bullet list": "符号列表",
                "Numbered list": "编号列表",
                "Undo": "恢复",
                "Redo": "撤消",
                "Rows:": "行数",
                "Cols:": "列数",
                "Insert a table": "插入表格",
                "Insert a horizontal rule": "插入分隔符",
                "Code": "代码",
                "Width (optional):": "宽度(选填)",
                "Height (optional):": "高度(选填)",
                "Insert an image": "插入图片",
                "E-mail:": "Email地址",
                "Insert an email": "插入Email地址",
                "URL:": "网址",
                "Insert a link": "插入链接",
                "Unlink": "取消链接",
                "More": "更多",
                "Insert an emoticon": "插入表情符号",
                "Video URL:": "视频地址",
                "Insert": "插入",
                "Insert a YouTube video": "插入YouTube视频",
                "Insert current date": "插入当前日期",
                "Insert current time": "插入当前时间",
                "Print": "打印",
                "View source": "查看代码",
                "Description (optional):": "描述(选填)",
                "Enter the image URL:": "输入图片地址",
                "Enter the e-mail address:": "输入email地址",
                "Enter the displayed text:": "输入显示文字",
                "Enter URL:": "输入网址",
                "Enter the YouTube video URL or ID:": "输入YouTube地址或编号",
                "Insert a Quote": "插入引用",
                "Invalid YouTube video": "无效的YouTube视频",

                dateFormat: "year-month-day"
            };

            // 图片选择器
            $.sceditor.plugins.bbcode.bbcode.set('img', {
                html: function (token, attrs, content) {
                    if (attrs.mid) {
                        return '<img data-mid="' + attrs.mid + '" src="' + content + '" />';
                    } else {
                        return '<img src="' + content + '" />';
                    }
                },
                format: function (element, content) {
                    var mid = element.data('mid'), src = element.attr('src');
                    if (!this.opts.outlinkImg && src.indexOf('http://') > -1 && src.indexOf(window.location.host) < 0) {//不允许粘贴站外图片
                        return '';
                    } else if (mid) {
                        return '[img mid=' + element.data('mid') + ']' + src + '[/img]';
                    } else {
                        return '[img]' + src + '[/img]';
                    }
                }
            })

            //链接
            $.sceditor.plugins.bbcode.bbcode.set('url', {
                html: function (token, attrs, content) {
                    //所有链接使用新窗口打开
                    return '<a target="_blank" data-a="1" href="' + (attrs.defaultattr || '#') + '">' + content + '</a>'
                },
                format: function (element, content) {
                    //处理外链
                    var href = (element.attr('href') || '#'), hostname = window.location.hostname;
                    if (href && href.indexOf('http://') > -1 && href.indexOf(hostname) < 0 && $(element).text() != href) {
                        return content;
                    }
                    return '[url=' + href + ']' + content + '[/url]';
                }
            });

            //td
            $.sceditor.plugins.bbcode.bbcode.set('td', {
                html: function (token, attrs, content) {
                    //所有链接使用新窗口打开
                    var align = attrs.align ? ' align="' + attrs.align + '"' : '',
                        style = attrs.style ? ' style="' + attrs.style + '"' : '';

                    var colspan = attrs.colspan ? ' colspan="' + attrs.colspan + '"' : '';
                    var rowspan = attrs.rowspan ? ' colspan="' + attrs.rowspan + '"' : '';

                    var attr = '';
                    for (var key in attrs) {
                        attr = attr + ' ' + key + ' ="' + attrs[key] + '"';
                    }
                    return '<td' + attr + '>' + content + '</td>'
                },
                format: function (element, content) {
                    var attr = '';
                    $(element).each(function () {
                        $.each(this.attributes, function () {
                            if (this.specified) {
                                attr = attr + ' ' + this.name + '=' + this.value;
                            }
                        });
                    });

                    var bbcode = '[td' + attr + ']' + content + '[/td]';
                    console.log(bbcode)

                    return bbcode;
                }
            });

            //图片选择器
            $.sceditor.command.set('image', {
                exec: function () {
                    imgSelector(this);
                },
                txtExec: function () {
                    imgSelector(this);
                }
            });
        }

        var imgSelector = function (thisEditor) {
            $.trip.user.uploadImage({
                single: false,
                showTopic: ($("#showTopicSelect").length > 0 ? ($("#showTopicSelect").val() == "1" ? true : false) : false)
            }).done(function (data) {
                if (data && data.result && data.result.length) {
                    for (var i = 0, item; item = data.result[i++];) {
                        thisEditor.insert('[img mid=' + item.mid + ']' + item.host + '[/img]');
                    }
                }
            });
        }

        $.extend({
            sceditorInit: function (selector) {
                var $selector = $(selector);

                if (!$selector.length) return;

                //调用前先检查是否已对sceditor做了扩展处理
                setupSceditor();

                $selector.each(function () {
                    var $textarea = $(this),
                        options = { outlinkImg: true },
                        width = parseInt($textarea[0].style.width) || 750,
                        height = parseInt($textarea[0].style.height) || 750;

                    options = $.extend(options, $textarea.data('options'));
                    //源码在数据库保存的格式，默认为 BBCode
                    var source = options.source || 'BBCode', toolbar = options.toolbar || 'color,size,bold,italic,underline,strike,left,center,quote,link,image,emoticon,table,maximize';

                    $textarea.sceditor({
                        plugins: 'bbcode',
                        source: source,
                        toolbar: toolbar,
                        style: "/style/sceditor/jquery.sceditor.default.min.css",
                        emoticonsRoot: "/images/",
                        locale: "cn",
                        width: width,
                        height: height,
                        autoUpdate: true,
                        enablePasteFiltering: true,
                        colors: 'black,red,pink|green,orange,purple|blue,brown,teal|navy,maroon,limegreen',
                        outlinkImg: options.outlinkImg || true
                    });
                    var $editor = $textarea.sceditor('instance');

                    //注意保存在数据库的格式为HTML时，需要先转为 UBB
                    $editor.val($editor.toBBCode($editor.val()));
                    $editor.updateOriginal();

                    if (options.caogao) {//提供30s自动草稿保存
                        var content = $.trim($editor.val());
                        var saveCaogao = function () {
                            var _content = $.trim($editor.val());
                            if (_content && _content != content) {
                                content = _content;
                                $.ajax('/communal/ws_caogao.aspx', {
                                    data: {
                                        module: options.caogao,
                                        content: source === 'html' ? $editor.fromBBCode(content) : content
                                    }, type: 'post', dataType: 'json'
                                }).done(function () {
                                    $editor.updateTips('内容已于 ' + $.trip.formatDate(new Date(), 'HH:mm:ss') + ' 自动保存');
                                });
                            }
                        }

                        setInterval(saveCaogao, 30 * 1000);
                    }
                });
            }
        });

        $.fn.extend({
            getMediaIds: function () {//获取使用到的相册照片Id
                var ar = [];
                $('img', $(this).sceditor('instance').getBody()).each(function () {
                    var mid = +$(this).data('mid');
                    if (mid > 0) {
                        ar.push(mid);
                    }
                });

                return ar;
            },
            getMediaImgs: function () {//获取使用到的相册照片
                return $('img', $(this).sceditor('instance').getBody()).filter(function () {
                    return +$(this).data('mid') > 0;
                });
            },
            getOutLinkImgs: function () {
                var hostname = window.location.hostname, ar = [];
                $('img', $(this).sceditor('instance').getBody()).each(function () {
                    var src = $(this).attr('src');
                    if (src && src.indexOf('http://') > -1 && src.indexOf(hostname) < 0) {
                        ar.push(src);
                    }
                });

                return ar;
            },
            getAllImgs: function (minWidth, minHeight) {
                var ar = [];
                minWidth = minWidth || 300;
                minHeight = minHeight || 200;
                $('img', $(this).sceditor('instance').getBody()).each(function () {
                    var src = $(this).attr('src'), width = parseInt($(this).css('width')), height = parseInt($(this).css('height'));
                    if (width > minWidth && height > minHeight) {
                        ar.push(src);
                    }
                });

                return ar;
            }
        });

        if ($.sceditor) setupSceditor();
    })($, window, document);

    //认证流程弹出层
    (function (window, $) {
        $.trip.user.renZhengList = function (rzindex, isJudge) {
            var $def = $.Deferred();
            $.ajax({
                url: "/user/renzheng/actionRZ.aspx"
            }).done(function (data) {
                if (data != null && data != "" && (String(data).indexOf(rzindex) > -1 || rzindex == 0)) {
                    if (typeof (isJudge) != "undefined" && isJudge) {
                        $def.reject();
                    } else {
                        $(document.body).css({ //禁止滚动条
                            "overflow-x": "hidden",
                            "overflow-y": "hidden"
                        });
                        window.trip.liuCheng.dialogIndex = $.layer({
                            type: 2,
                            title: '用户认证流程',
                            iframe: { src: '/user/renzheng/liucheng.aspx?rz=' + rzindex },
                            area: ['725px', '750px'],
                            offset: ['42px', ''],
                            end: function () {
                                $.trip.user.renZhengList(rzindex, true).done(function () {
                                    $def.resolve();
                                }).fail(function () {
                                    $def.reject();
                                });
                            },
                            success: function (layers) {
                                //$(window).innerWidth()  
                                setTimeout(function () {

                                    if ($(window).innerHeight() < 800) {
                                        layer.area(layer.index, {
                                            width: "725px",
                                            height: $(window).innerHeight() + 35
                                        });
                                    }
                                }, 1000);

                            },
                            close: function (index) {
                                $(document.body).css({ //启用滚动条
                                    "overflow-x": "auto",
                                    "overflow-y": "auto"
                                });
                                layer.close(window.trip.liuCheng.dialogIndex);
                            },
                            position: 'center'
                        });
                    }
                }
                else {
                    $def.resolve();
                }
            }).fail(function () { $def.reject(); });
            return $def.promise();
        };

        $.trip.user.renZheng = function (rzindex, url, target) {

            var $def = $.Deferred();

            //  需要认证 0：全部认证  1: 身份认证  2：领队认证  3：租车认证  
            $(document.body).css({ //启用滚动条
                "overflow-x": "auto",
                "overflow-y": "auto"
            });
            window.trip = window.trip || {};
            window.trip.liuCheng = window.trip.liuCheng || {};
            window.trip.liuCheng.close = function () {

                $(document.body).css({ //启用滚动条
                    "overflow-x": "auto",
                    "overflow-y": "auto"
                });
                if (window.trip.liuCheng.dialogIndex) {
                    layer.close(window.trip.liuCheng.dialogIndex).done(function () {

                        $(document.body).css({ //启用滚动条
                            "overflow-x": "auto",
                            "overflow-y": "auto"
                        });
                    });
                }

            };
            if ($.trip.user.isLogin()) {
                $.trip.user.renZhengList(rzindex).done(function () {
                    $def.resolve();
                }).fail(function () {
                    $def.reject();
                });
            } else {
                $.trip.user.showLogin(null, null, url, target).done(function () {
                    $.trip.user.renZhengList(rzindex).done(function () {
                        $def.resolve();
                    }).fail(function () {
                        $def.reject();
                    });
                });
            }

            return $def.promise();
        }

        $(document).on("click", ".needApprove", function (e) {
            e.preventDefault();
            var $that = $(this), url = $that.attr("href"), target = $that.attr("target") == undefined ? "_self" : $that.attr("target"), rzindex = $that.data("rzindex");
            $.trip.user.renZheng(rzindex, url, target).done(function () {
                window.open(url, target);
            });
        });
    })(window, $);

    //验证码
    (function (window, $) {
        $(document).on("click", ".tVerifyCode", function (e) {
            e.preventDefault();
            $(this).attr("src", src = "/User/ValidateCode.aspx?flag=" + new Date().getMilliseconds());
        });
        $.trip.verifyCode = function (code) {
            var $def = $.Deferred();
            $.ajax({
                type: "POST",
                url: "/user/info/CheckInfo.aspx",
                data: "code=" + code + "&cmd=vcode&repeat=1"
            }).done(function (data) {
                if (data == "1") {
                    $def.resolve();
                } else {
                    $def.reject();
                }
            }).fail(function () { $def.reject(); });
            return $def.promise();
        }
        setTimeout(function () { $(".tVerifyCode").attr("src", src = "/User/ValidateCode.aspx?flag=" + new Date().getMilliseconds()); }, 100);
    })(window, $);

    $.trip.pageInit = function () {
        //编辑器初始化
        $.sceditorInit('.sceditor');

        //评论区初始化
        $('.mainComment').comment();

        //分页初始化
        $('.page').buildPage();

        //收藏控件初始化
        $.trip.collction('.collect');

        //定时获取消息
        $.trip.user.msg.init();

        //浏览器版本
        if ($.browser.msie) {//IE
            var ver = $.browser.version == "6.0" ? "6" : ($.browser.version == "7.0" ? "7" : "");
            if (ver == "6") {
                $.trip.showMsgTip("您的IE6浏览器已经是13年前的版本，为了您的安全及更流畅的体验，请升级您的浏览器：<a href=\"http://www.microsoft.com/zh-cn/download/internet-explorer-8-details.aspx\">IE8浏览器</a> <a href=\"https://www.google.com/intl/zh-CN/chrome/browser/\" target=\"_blank\">谷歌浏览器</a>");
            } else if (ver == "7") {
                $.trip.showMsgTip("您的IE7浏览器已经是8年前的版本，为了您的安全及更流畅的体验，请升级您的浏览器：<a href=\"http://www.microsoft.com/zh-cn/download/internet-explorer-8-details.aspx\">IE8浏览器</a> <a href=\"https://www.google.com/intl/zh-CN/chrome/browser/\" target=\"_blank\">谷歌浏览器</a>");
            }
        }
    };

    //初始化
    $(function () {
        $.trip.pageInit();
    });

})(window, $);