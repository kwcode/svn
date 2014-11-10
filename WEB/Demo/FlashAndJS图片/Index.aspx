<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Index.aspx.vb" Inherits="Index" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>上传，裁剪</title>
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <link href="css/jquery.Jcrop.css" rel="stylesheet" />
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="js/picupload/plupload.full.min.js"></script>
    <script src="js/picupload/jquery.Jcrop.min.js"></script>
    <script type="text/javascript">
        //上传 
        jQuery(function ($) {
            var img64;
            var uploader = new plupload.Uploader({ //实例化一个plupload上传对象
                browse_button: 'browse',
                url: 'upload.html',
                flash_swf_url: 'js/Moxie.swf',
                silverlight_xap_url: 'js/Moxie.xap',
                filters: {
                    mime_types: [ //只允许上传图片文件
                      { title: "图片文件", extensions: "jpg,gif,png" }
                    ]
                }
                //   , resize: { width: 320, height: 240, quality: 80 }
                //, prevent_duplicates: true //不允许选取重复文件

            });
            uploader.init(); //初始化

            //绑定文件添加进队列事件
            uploader.bind('FilesAdded', function (uploader, files) {
                for (var i = 0, len = files.length; i < len; i++) {
                    var file_name = files[i].name; //文件名   
                    $(".jc-box").html("");
                    var html = '<div class="jc-main-box"> <div class="tg" id="file-list"> </div></div>';
                    $(".jc-box").html(html);
                    !function (i) {
                        previewImage(files[i], function (imgsrc) {
                            img64 = imgsrc;
                            $('.tg').append('<img id="target" src="' + imgsrc + '" />');
                            $(".jc-box").append('<div id="preview-pane"> <div class="preview-container"> <img src="' + imgsrc + '" class="jcrop-preview" alt="  " /> </div>  </div> ');
                            $(".jc-box").append('<div id="preview-pane_small"> <div class="preview-container_small"> <img src="' + imgsrc + '" class="jcrop-preview_small" alt="  " /> </div>  </div> ');
                            bindJcrop();//绑定剪切
                        })
                    }(i);
                }
            });
            function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
                if (!file || !/image\//.test(file.type)) return; //确保文件是图片
                if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
                    var fr = new mOxie.FileReader();
                    fr.onload = function () {
                        callback(fr.result);
                        fr.destroy();
                        fr = null;
                    }
                    fr.readAsDataURL(file.getSource());
                } else {
                    var preloader = new mOxie.Image();
                    preloader.onload = function () {
                        preloader.downsize(400, 300);//先压缩一下要预览的图片,宽300，高300
                        var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
                        callback && callback(imgsrc); //callback传入的参数为预览图片的url
                        preloader.destroy();
                        preloader = null;
                    };
                    preloader.load(file.getSource());
                }
            }

            //裁剪
            var jcrop_api,
                   boundx,
                   boundy,
                   $preview,
                   $pcnt,
                   $pimg,
                   xsize,
                   ysize,
                   jcropc;
            var sm_$preview, sm_$pcnt, sm_$pcnt, sm_ysize, sm_xsize;
            function bindJcrop() {

                $preview = $('#preview-pane'),
                 $pcnt = $('#preview-pane .preview-container'),
                $pimg = $('#preview-pane .preview-container img'),
                  ysize = $pcnt.height(),
                xsize = $pcnt.width();
                sm_$preview = $('#preview-pane_small'),
               sm_$pcnt = $('#preview-pane_small .preview-container_small'),
               sm_$pimg = $('#preview-pane_small .preview-container_small img'),
                    sm_ysize = sm_$pcnt.height(),
                sm_xsize = sm_$pcnt.width();
                $('#target').Jcrop({
                    onChange: updatePreview,
                    onSelect: updatePreview,
                    aspectRatio: xsize / ysize,
                }, function () {
                    var bounds = this.getBounds();
                    boundx = bounds[0];
                    boundy = bounds[1];
                    console.log(boundx);
                    console.log(boundy);
                    jcrop_api = this;
                    jcrop_api.animateTo([0, 0, 240, 240]);
                    //$preview.appendTo(jcrop_api.ui.holder);
                    //界面绑定

                });

                function updatePreview(c) {
                    jcropc = c;
                    if (parseInt(c.w) > 0) {
                        var rx = xsize / c.w;
                        var ry = ysize / c.h;

                        $pimg.css({
                            width: Math.round(rx * boundx) + 'px',
                            height: Math.round(ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(rx * c.x) + 'px',
                            marginTop: '-' + Math.round(ry * c.y) + 'px'
                        });
                        var sm_rx = sm_xsize / c.w;
                        var sm_ry = sm_ysize / c.h;
                        sm_$pimg.css({
                            width: Math.round(sm_rx * boundx) + 'px',
                            height: Math.round(sm_ry * boundy) + 'px',
                            marginLeft: '-' + Math.round(sm_rx * c.x) + 'px',
                            marginTop: '-' + Math.round(sm_ry * c.y) + 'px'
                        });
                    }
                };

            };

            $("#imgsave").on("click", function () {
                console.log(img64);
                console.log(jcrop_api.tellScaled())//获取选框的值（实际尺寸）。

                var x = jcropc.x, y = jcropc.y, w = jcropc.w, h = jcropc.h, path = JSON.toString(img64);
                var params = "action=saveimg&x=" + x + "&y=" + y + "&w=" + w + "&h=" + h + "&path=" + encodeURIComponent(img64);
                //$.ajax("ActionUploadImage.aspx", {
                //    data: params,
                //}).success(function (result) {
                //    alert("保存成功");
                //});
                $.ajax({
                    type: "POST",
                    url: "ActionImage.aspx",
                    data: params,
                    dataType: "json",
                    success: function (result) {
                        alert("保存成功");
                    }
                }).fail(function () {
                    alert("sb")
                })
            })
        });
    </script>
    <style type="text/css">
        /*上传*/
        body { font-size: 12px; }
        body, p, div { padding: 0; margin: 0; }
        .wraper { padding: 30px 0; }
        .btn-wraper { text-align: center; }
        .btn-wraper input { margin: 0 10px; }
        #file-list { margin: 20px auto; }
        #file-list li { margin-bottom: 10px; }
        .file-name { line-height: 30px; }
        .progress { height: 4px; font-size: 0; line-height: 4px; background: orange; width: 0; }
        .tip1 { text-align: center; font-size: 14px; padding-top: 10px; }
        .tip2 { text-align: center; font-size: 12px; padding-top: 10px; color: #b00; }
        .catalogue { position: fixed; _position: absolute; _width: 200px; left: 0; top: 0; border: 1px solid #ccc; padding: 10px; background: #eee; }
        .catalogue a { line-height: 30px; color: #0c0; }
        .catalogue li { padding: 0; margin: 0; list-style: none; }

        /*裁剪*/
        .jc-box .jc-main-box { width: 400px; display: block; position: absolute; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        .jc-box #preview-pane { display: block; position: absolute; z-index: 2000; top: 110px; left: 430px; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        #preview-pane .preview-container { width: 120px; height: 120px; overflow: hidden; }
        .jcrop-holder { width: 400px; }

        .btn_file { background-color: #F93; border-radius: 5px; padding: 5px; border: 0px; }
        .button { }
        .jc-box { background-color: #F4F0EA; width: 500px; height: 500px; }

        .jc-box #preview-pane_small { display: block; position: absolute; z-index: 2000; top: 250px; left: 480px; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        #preview-pane_small .preview-container_small { width: 16px; height: 16px; overflow: hidden; }
    </style>

</head>
<body>
    <div class="wraper">
        <div class="btn-wraper">
            <input type="button" class="btn_file button" value="选择文件..." id="browse" />
        </div>
    </div>
    <div class="jc-box">
    </div>
    <div class="toolbtn" style="position: absolute;">
        <input type="button" value="保存" class="WhiteButton" id="imgsave" />
    </div>
</body>
</html>
