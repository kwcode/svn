<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index2.aspx.cs" Inherits="index2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>头像设置</title>
    <script src="js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type="text/javascript" src="js/jquery.fileupload.js"></script>

    <script type="text/javascript" src="js/jquery.Jcrop.js"></script>
    <link href="css/jquery.Jcrop.css" rel="stylesheet" />
    <link href="css/jquery.fileupload.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var picFile = "";
        var picHeight = 0;
        var picWidth = 0;
        function toCrop() {
            var x = $('#x').val();
            var y = $('#y').val();
            var w = $('#w').val();
            var h = $('#h').val();
            if ($.trim(x) == "" || $.trim(y) == "" || $.trim(w) == "" || $.trim(h) == "") {
                //console.log("数据不能为空!");
                return;
            }
            var params = "action=saveimg&x=" + x + "&y=" + y + "&w=" + w + "&h=" + h + "&path=" + picFile;
            $.ajax({
                type: "POST",
                url: "ActionUploadImage.aspx",
                data: params,
                success: function (html) {
                    //console.log(html);
                    //$("#result").html('<img src="' + html + '"/>');
                    $('#portrait').attr('src', html);
                    alert("ok");
                }
            });
        }
        $(function () {
            function storeCoords(c) {
                $('#x').val(c.x);
                $('#y').val(c.y);
                $('#w').val(c.w);
                $('#h').val(c.h);
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
            $('#Cancel').click(function () {
                $('#operation-box').trigger('close');
            });
            var jcrop_api;
            function bindJcrop(picPath) {
                picHeight = $('#picresult').height();
                picWidth = $('#picresult').width();
                $('#preview').attr('src', picPath);
                if ($("#preview").is(":visible") == false) {
                    $('#preview').show();
                }
                $('#picresult').Jcrop({
                    onChange: storeCoords,
                    onSelect: storeCoords,
                    aspectRatio: 1
                }, function () {
                    jcrop_api = this,
                    jcrop_api.setSelect([10, 10, 240, 240]);
                });
                jcrop_api.animateTo([0, 0, 240, 240]);
                $('#oper').html('<input type="button" value="保存头像" class="WhiteButton" onclick="toCrop()"/>');
            }
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
                    $.each(data.result, function (index, file) {
                        $('#result').html();
                        picFile = file;
                        $('#result').html('<img src="' + picFile + '" id="picresult"/>');
                        if ($.browser.msie) {
                            bindJcrop(picFile);
                        } else {
                            if ($('#picresult').load(function () {
                                bindJcrop(picFile);
                            }));
                        }
                        $('#picresult').load(function () {
                            //alert('111');

                        });

                    });
                }

            });
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
        .WhiteButton { -moz-box-shadow: inset 0px 1px 0px 0px #ffffff; -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff; box-shadow: inset 0px 1px 0px 0px #ffffff; background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #ffffff), color-stop(1, #f6f6f6)); background: -moz-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -webkit-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -o-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: -ms-linear-gradient(top, #ffffff 5%, #f6f6f6 100%); background: linear-gradient(to bottom, #ffffff 5%, #f6f6f6 100%); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f6f6f6',GradientType=0); background-color: #ffffff; -moz-border-radius: 6px; -webkit-border-radius: 6px; border-radius: 6px; border: 1px solid #dcdcdc; display: inline-block; color: #666666; font-family: arial; font-size: 15px; font-weight: bold; padding: 6px 24px; text-decoration: none; text-shadow: 0px 1px 0px #ffffff; }
        .WhiteButton:hover { background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f6f6f6), color-stop(1, #ffffff)); background: -moz-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -webkit-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -o-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: -ms-linear-gradient(top, #f6f6f6 5%, #ffffff 100%); background: linear-gradient(to bottom, #f6f6f6 5%, #ffffff 100%); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f6f6f6', endColorstr='#ffffff',GradientType=0); background-color: #f6f6f6; }
        .WhiteButton:active { position: relative; top: 1px; }
        * { font-size: 12px; font-family: 微软雅黑; }
        li img { padding: 3px; border: 1px solid #ccc; }
        ul { list-style: none; padding: 0; margin: 0; }
        li { margin: 2px 0; }
        .clear { clear: both; }
        li #portrait { width: 150px; height: 150px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <p>
                <ul>
                    <li><b>当前头像</b></li>
                    <li>
                        <img src="images/3X0kBykp.jpg" id="portrait" alt="" /></li>
                </ul>

            </p>
        </div>

        <div id="operation-box">
            <div id="header">
                <table>
                    <tr>
                        <td>
                            <div class="fileinput-button">
                                <span>
                                    <input type="button" value="上传图片" class="WhiteButton" id="UploadFile" /></span>
                                <input id="fileupload" type="file" name="file" value="shang" />
                            </div>

                        </td>
                        <td>
                            <input type="button" value="取消" class="WhiteButton" id="Cancel" />
                        </td>
                    </tr>

                </table>
            </div>
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
        </div>
    </form>
</body>
</html>
