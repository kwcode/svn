(function (window, $, undefined) {
    // 所有我们自己写的jQuery插件都挂载在 $.tw 下
    //$.tw = $.tw || {};
    $.trip = $.trip || {};
    //自定义提示框 
//    $.tw.alert = function (title, msg, ico) {
//        //var ii = layer.load('加载中');
//        //  alert(msg);
//    }
//    $.trip.template("template.html").render("t_t2").done(function (html) {
//        alert(html);
//    })

    /* */

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

                $.ajax(tplFile, { data: { _: new Date().toLocaleDateString()} }).done(function (data) {
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

    /*************/
})(window, $);