<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CutDemo.aspx.vb" Inherits="CutDemo" %>

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
        jQuery(function ($) {
            var jcrop_api,
                boundx,
                boundy,
                $preview = $('#preview-pane'),
                $pcnt = $('#preview-pane .preview-container'),
                $pimg = $('#preview-pane .preview-container img'),
                xsize = $pcnt.width(),
                ysize = $pcnt.height();

            $('#target').Jcrop({
                onChange: updatePreview,
                onSelect: updatePreview,
                aspectRatio: xsize / ysize
            }, function () {
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];
                jcrop_api = this;
                jcrop_api.animateTo([0, 0, 240, 240]);
                $preview.appendTo(jcrop_api.ui.holder);
            });

            function updatePreview(c) {
                if (parseInt(c.w) > 0) {
                    var rx = xsize / c.w;
                    var ry = ysize / c.h;

                    $pimg.css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }
            };

        });

    </script>
    <style type="text/css">
        .jc-box .tg { display: block; position: absolute; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        .jc-box #preview-pane { display: block; position: absolute; z-index: 2000; top: 10px; right: -280px; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        #preview-pane .preview-container { width: 120px; height: 120px; overflow: hidden; }
    </style>

</head>
<body>
    <div class="jc-box">
        <div class="tg">
            <img src="images/001.jpg" id="target" alt="[Jcrop Example]" style="width: 500px; height: 500px;" />
        </div>
        <div class="jcrop-holder" style=" ">
            <div id="preview-pane">
                <div class="preview-container">
                    <img src="images/001.jpg" class="jcrop-preview" alt="Preview" />
                </div>
            </div>
        </div>

    </div>
</body>
</html>

