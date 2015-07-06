/*
 $("#btn_add").click(function () {
                $.tw.photo.uploadImage({
                    single: true
                }).done(function (data) {
                    //用户选中了图片并点击确定按钮的之后
                    if (data && data.result && data.result.length) {
                        var image = data.result[0];
                        var url = image.tn;
                        $('#img_Url').attr('src', url);
                    } 
                });
            });

*/


; (function (window, $) {
    $(function ($) {
        $.tw = $.tw || {};
        $.tw.photo = $.tw.photo || {};
        if (typeof ($.tw.photo.uploadImage) == "function") { return; }
        var uploadImage = function (installOption) {
            var option = $.extend({
                border: [4, 0.1, '#000', true],
                title: "添加图片",
                //fix: false,
                //offset: ['120px', ''],
                area: ['800px', '400px'],
                position: 'center',
                single: false,
                singleUpload: false
            }, installOption || {});

            var $def = $.Deferred(), isSingle = option.single, isSingleUpload = option.singleUpload;

            delete option.single; delete option.singleUpload;
            var iframeSrc = "/admin/Photo/AlbumPopup.aspx?r=" + Math.random() + ((isSingle) ? "&s=1" : "") + ((isSingleUpload) ? "&su=1" : ""),
            lyIndex = $.layer($.extend({
                type: 2,
                iframe: { src: iframeSrc },
                end: function () {
                    if ($def.state() == "pending") {
                        $def.reject();
                    }
                },
                success: function ($layer) { }
            }, option));

            if (lyIndex <= 0) {
                $def.reject();
            }
            window.tw = window.tw || {};
            window.tw.AlbumPhotoPopup = {
                confirm: function (data) {
                    $def.resolve(data);
                }
            };
            return $def.promise();
        }
        $.extend($.tw.photo, { uploadImage: uploadImage });
    });

})(window, $);