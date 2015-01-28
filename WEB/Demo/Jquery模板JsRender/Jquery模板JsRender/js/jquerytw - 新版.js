; (function (window, $, undefined) {
    // 所有我们自己写的jQuery插件都挂载在 $.tw 下 
    $.tw = $.tw || {};


    /* $.trip.template 模板渲染 依赖jQuery, jsrender */
    (function ($) {
        //加载模板

        var templates = {};
        function loadTemplateFile(tplFile) {
            var that = templates[tplFile] = {};
            var callbacks = $.Callbacks();
            var loaded = false;
            var cacheTemplates = {};
            var loadTemplateFile = function () {
                // 载入 模板
                $.ajax(tplFile).done(function (data) {
                    var obj = {}
                    $(data).filter('script').each(function (index, item) {
                        var id = item.id;
                        if (id) {
                            obj[item.id] = $(item).html();
                        }
                    });
                    //预编译 
                    cacheTemplates = $.templates(obj);
                    //回调
                    callbacks.fire();
                    //已经加载了模板
                    loaded = true;
                });
            }
            loadTemplateFile();
            //
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
                return tpl.render(tplId, json, helpersOrContext);
            }
            return that;
        }

        $.extend($.tw, {
            template: _template
        });
        //预加载全局模版
        $.tw.template('/communal/template.html');
    })($);

    /*************/
})(window, $);