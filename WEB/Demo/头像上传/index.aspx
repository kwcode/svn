<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js"></script>

    <script src="js/jquery-ui.js"></script>
    <script src="js/jquery.fileupload.js"></script>

    <%-- <script src="js/jquery.iframe-transport.js"></script>
    <script src="js/jquery.Jcrop.js"></script>--%>

    <link href="css/jquery.Jcrop.css" rel="stylesheet" />
    <script src="js/jquery.Jcrop.js"></script>


    <%--  <script src="js/jquery.imgareaselect.js"></script> 
    <link href="css/imgareaselect-default.css" rel="stylesheet" />--%>


    <link href="css/jquery.fileupload.css" rel="stylesheet" />
    <%-- <link href="css/jquery.Jcrop.css" rel="stylesheet" />--%>
    <script>
        jQuery(function ($) {

            // Create variables (in this scope) to hold the API and image size
            var jcrop_api,
                picWidth,
                picHeight
            //    imgpath,
            //    // Grab some information about the preview pane
            //    $preview = $('#preview-pane'),
            //    $pcnt = $('#preview-pane .preview-container'),
            //    $pimg = $('#preview-pane .preview-container img'),

            //    xsize = $pcnt.width(),
            //    ysize = $pcnt.height();

            //console.log('init', [xsize, ysize]);
            //$('#target').Jcrop({
            //    onChange: updatePreview,
            //    onSelect: updatePreview,
            //    aspectRatio: xsize / ysize
            //}, function () {
            //    // Use the API to get the real image size
            //    var bounds = this.getBounds();
            //    boundx = bounds[0];
            //    boundy = bounds[1];
            //    // Store the API in the jcrop_api variable
            //    jcrop_api = this;
            //    //jcrop_api.setSelect([10, 10, 240, 240]);
            //    jcrop_api.animateTo([0, 0, 240, 240]);
            //    // Move the preview into the jcrop container for css positioning
            //    $preview.appendTo(jcrop_api.ui.holder);
            //});


            /*上传图片显示*/
            $('#fileupload').fileupload({
                replaceFileInput: false,
                dataType: 'json',
                url: '<%=ResolveUrl("UploadHandler.ashx") %>',
                add: function (e, data) {
                    var re = /^.+\.((jpg)|(png))$/i;
                    $.each(data.files, function (index, file) {
                        if (re.test(file.name)) {
                            data.submit();
                        }
                    });
                },
                done: function (e, data) {
                    imgpath = data.result.name;
                    $('#result').html('<img src="' + imgpath + '" id="picresult"/>');
                    if ($.browser.msie) {
                        bindJcrop(imgpath);
                    } else {
                        if ($('#picresult').load(function () {
                            bindJcrop(imgpath);
                        }));
                    }
                    $('#picresult').load(function () {
                        //alert('111');

                    });
                    //jcrop_api.setImage(imgpath);
                    //jcrop_api.setOptions({ bgOpacity: .6 });
                    ////jcrop_api.setSelect([10, 10, 240, 240]);
                    //jcrop_api.animateTo([0, 0, 240, 240]);
                    //$(".jcrop-preview").attr("src", data.result.name);
                }
            });
            /*END*/
            /*绑定剪切*/
            function bindJcrop(picPath) {
                picWidth = $('#picresult').height();
                picHeight = $('#picresult').width();
                $('#preview').attr('src', picPath);
                if ($("#preview").is(":visible") == false) {
                    $('#preview').show();
                }
                $('#picresult').Jcrop({
                    onChange: storeCoords,
                    onSelect: storeCoords,
                    aspectRatio: 1
                });
                // $('#oper').html('<input type="button" value="修剪头像" class="WhiteButton" onclick="toCrop()"/>');
            }
            //var jcrop_api,
            //    boundx,
            //    boundy,
            //    imgpath,
            //    // Grab some information about the preview pane
            //    $preview = $('#preview-pane'),
            //    $pcnt = $('#preview-pane .preview-container'),
            //    $pimg = $('#preview-pane .preview-container img'),

            //    xsize = $pcnt.width(),
            //    ysize = $pcnt.height();

            //console.log('init', [xsize, ysize]);
            //$('#target').Jcrop({
            //    onChange: updatePreview,
            //    onSelect: updatePreview,
            //    aspectRatio: xsize / ysize
            //}, function () {
            //    // Use the API to get the real image size
            //    var bounds = this.getBounds();
            //    boundx = bounds[0];
            //    boundy = bounds[1];
            //    // Store the API in the jcrop_api variable
            //    jcrop_api = this;
            //    //jcrop_api.setSelect([10, 10, 240, 240]);
            //    jcrop_api.animateTo([0, 0, 240, 240]);
            //    // Move the preview into the jcrop container for css positioning
            //    $preview.appendTo(jcrop_api.ui.holder);
            //});
            //function updatePreview(c) {
            //    if (parseInt(c.w) > 0) {
            //        var rx = 150 / c.w;
            //        var ry = 150 / c.h;

            //        $('#preview').css({
            //            width: Math.round(rx * boundx) + 'px',
            //            height: Math.round(ry * boundy) + 'px',
            //            marginLeft: '-' + Math.round(rx * c.x) + 'px',
            //            marginTop: '-' + Math.round(ry * c.y) + 'px'
            //        });
            //        var jzb = $("#jcrop-zb").data();
            //        jzb.x = Math.floor(c.x);
            //        jzb.y = Math.floor(c.y);
            //        jzb.x2 = Math.floor(c.x2);
            //        jzb.y2 = Math.floor(c.y2);
            //        jzb.w = Math.floor(c.w);
            //        jzb.h = Math.floor(c.h);
            //    }
            //};
            function storeCoords(c) {
                //$('#x').val(c.x);
                //$('#y').val(c.y);
                //$('#w').val(c.w);
                //$('#h').val(c.h);
                var rx = 150 / c.w;
                var ry = 150 / c.h;
                var x, y, w, h;
                w = Math.round(rx * picWidth);
                h = Math.round(ry * picHeight);
                x = Math.round(rx * c.x);
                y = Math.round(ry * c.y);
                $('#preview').css({
                    width: w + 'px',
                    height: h + 'px',
                    marginLeft: '-' + x + 'px',
                    marginTop: '-' + y + 'px'
                });

            };
            /*END*/
            /*保存图片*/
            $("#btn_save").click(function () {
                var jzb = $("#jcrop-zb").data();
                var x = jzb.x;
                var y = jzb.y;
                var w = jzb.w;
                var h = jzb.h;

                if ($.trim(x) == "" || $.trim(y) == "" || $.trim(w) == "" || $.trim(h) == "") {
                    console.log("数据不能为空!");
                    return;
                }
                var params = "action=saveimg&x=" + x + "&y=" + y + "&w=" + w + "&h=" + h + "&path=" + imgpath;
                $.ajax("ActionUploadImage.aspx", {
                    data: params,
                }).success(function (result) {
                    alert("保存成功");
                });
            });
            /*END*/
        });
    </script>
    <style type="text/css">
        /* Apply these styles only when #preview-pane has
   been placed within the Jcrop widget */
        .preview-pane { display: block; position: absolute; z-index: 2000; top: 10px; right: -280px; padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 6px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); }
        .bigview-base { padding: 6px; border: 1px rgba(0,0,0,.4) solid; background-color: white; -webkit-border-radius: 6px; -moz-border-radius: 6px; border-radius: 5px; -webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); -moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2); max-width: 500px; }
        /* The Javascript code will set the aspect ratio of the crop
   area based on the size of the thumbnail preview,
   specified here */

        #preview-pane .preview-container { width: 120px; height: 120px; overflow: hidden; }

        .WhiteButton { -moz-box-shadow: inset 0px 1px 0px 0px #ffffff; -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff; box-shadow: inset 0px 1px 0px 0px #ffffff; background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #ffffff), color-stop(1, #f6f6f6)); background: -moz-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -webkit-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -o-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -ms-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: linear-gradient(to bottom, #ffffff 5%, #f6f6f6 100%); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f6f6f6',GradientType=0); background-color: #ffffff; -moz-border-radius: 6px; -webkit-border-radius: 6px; border-radius: 6px; border: 1px solid #dcdcdc; display: inline-block; color: #666666; font-family: arial; font-size: 15px; font-weight: bold; padding: 6px 24px; text-decoration: none; text-shadow: 0px 1px 0px #ffffff; }
        .WhiteButton:hover { background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f6f6f6), color-stop(1, #ffffff)); background: -moz-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -webkit-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -o-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -ms-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: linear-gradient(to bottom, #f6f6f6 5%, #ffffff 100%); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f6f6f6', endColorstr='#ffffff',GradientType=0); background-color: #f6f6f6; }
        .WhiteButton:active { position: relative; top: 1px; }
        * { font-size: 12px; font-family: 微软雅黑; }
        li img { padding: 3px; border: 1px solid #ccc; }
        ul { list-style: none; padding: 0; margin: 0; }
        li { margin: 2px 0; }
        #operation-box { display: none; background: white; width: 700px; height: 600px; }
        #operation-box #header { padding: 10px; border-bottom: 1px solid #dddddd; }
        #operation-box #content { margin: 10px; }

        .clear { clear: both; }
        li #portrait { width: 150px; height: 150px; }
        .fileinput-button { position: relative; overflow: hidden; }
        .fileinput-button input[type=file] { position: absolute; top: 0; right: 0; margin: 0; opacity: 0; -ms-filter: 'alpha(opacity=0)'; font-size: 200px; direction: ltr; cursor: pointer; }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="fileinput-button">
            <span>
                <input type="button" value="上传图片" class="WhiteButton" /></span>
            <input id="fileupload" type="file" name="file" value="shang" />
        </div>
        <input type="button" id="btn_save" value="保存" />

        <%--<div id="result" style="float: left"></div>

        <div style="width: 150px; height: 150px; overflow: hidden; margin-left: -50px; float:right;">
            <img src="" id="preview" style="display: none;" />
        </div>
        <div id="jcrop-zb" hidden="hidden" data-x="10" data-y="10" data-x2="240" data-y2="240" data-w="120"
            data-h="120">
        </div>--%>

        <div id="content">
            <table>
                <tr>
                    <td id="result"></td>
                    <td valign="top">
                        <div style="width: 150px; height: 150px; overflow: hidden; margin-left: 5px;">
                            <img src="" id="preview" style="display: none;" />
                        </div>
                        <input type="hidden" id="x" />
                        <input type="hidden" id="y" />
                        <input type="hidden" id="w" />
                        <input type="hidden" id="h" />
                        <div id="oper" style="margin-top: 10px;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
