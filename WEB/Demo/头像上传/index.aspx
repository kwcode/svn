<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.js"></script>
    <script src="js/jquery.imgareaselect.js"></script>
    <script src="js/jquery.imgareaselect.pack.js"></script>
    <link href="css/imgareaselect-default.css" rel="stylesheet" />
    <link href="css/imgareaselect-animated.css" rel="stylesheet" />
    <link href="css/imgareaselect-deprecated.css" rel="stylesheet" />
    <script>
        function preview(img, selection) {
            var scaleX = 100 / (selection.width || 1);
            var scaleY = 100 / (selection.height || 1);

            $('#ferret + div > img').css({
                width: Math.round(scaleX * 400) + 'px',
                height: Math.round(scaleY * 300) + 'px',
                marginLeft: '-' + Math.round(scaleX * selection.x1) + 'px',
                marginTop: '-' + Math.round(scaleY * selection.y1) + 'px'
            });
        }

        $(document).ready(function () {
            $('<div><img src="/images/psb.jpg" style="position: relative;" /><div>')
                .css({
                    float: 'left',
                    position: 'relative',
                    overflow: 'hidden',
                    width: '100px',
                    height: '100px'
                })
                .insertAfter($('#ferret'));

            $('#ferret').imgAreaSelect({ aspectRatio: '1:1', onSelectChange: preview });

            $("input[type='file']").change(function () {
                var fileName = $(this).val();
                var seat = fileName.lastIndexOf(".");
                var extension = fileName.substring(seat).toLowerCase();
                var allowed = [".jpg", ".gif", ".png", ".bmp", ".jpeg"];
                for (var i = 0; i < allowed.length; i++) {
                    if (!(allowed[i] != extension)) {
                        //$(img).attr("src", fileName);
                        /*ajax上传图片*/
                        $.ajax("ActionUploadImage.aspx", { data: {
                            action: "uploadImage",
                            filename: fileName 
                        }, type: "POST"
                        }).success(function (result) {
                            alert(result);
                        });
                        /*END*/
                        return true;
                    }
                }
                $(this).val(null);
                alert("不支持" + extension + "格式");
                return false;
            });
        });
        function onCheckType(fileOjb, img) {
            var fileName = $(fileOjb).val();
            var seat = fileName.lastIndexOf(".");
            var extension = fileName.substring(seat).toLowerCase();
            var allowed = [".jpg", ".gif", ".png", ".bmp", ".jpeg"];
            for (var i = 0; i < allowed.length; i++) {
                if (!(allowed[i] != extension)) {
                    $(img).attr("src", fileName);
                    return true;
                }
            }
            $(fileOjb).val(null);

            alert("不支持" + extension + "格式");
            return false;
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="file" value="选择图片" name="file" id="btn_selimg" />
    <div>
        <img id="ferret" src="/images/psb.jpg" style="float: left; width: 500px; height: 500px;
            margin-right: 10px;" />
    </div>
    </form>
</body>
</html>
