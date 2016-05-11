<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="demoPlupload.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.8.3.min.js"></script>
    <script src="js/plupload/plupload.full.min.js"></script>
    <style>
        .trip-uploader .webuploader-container { float: left; position: relative; width: 20%; display: block; line-height: 1.4; background: #fff; border: 1px dashed #D2D1D6; border-radius: 6px; color: #ccc; padding: 15px 0; font-size: 13px; text-align: center; margin: 4px; cursor: pointer; }
        .webuploader-pick { width: 100%; display: block; cursor: pointer; overflow: hidden; }
        .trip-uploader .webuploader-container .icon-plus { width: 32px; height: 32px; display: block; margin: 10px auto; background: url(/upimagedefault.png) no-repeat; background-size: 32px; }
        .upload-btn { position: absolute; top: 0px; left: 0px; width: 100%; height: 98%; overflow: hidden; z-index: 0; }
        .file-item { width: 120px; height: 120px; float: left; position: relative; margin: 0 0 10px; padding: 4px; padding: 4px; line-height: 1.42857143; background-color: #fff; border: 1px solid #ddd; border-radius: 4px; -webkit-transition: border .2s ease-in-out; -o-transition: border .2s ease-in-out; transition: border .2s ease-in-out; }
        .fancybox { display: block; overflow: hidden; background: #eee; height: 120px; }
        .file-item img { height: 110px; }
        .file-item .progress { position: absolute; right: 4px; bottom: 4px; left: 4px; height: 4px; overflow: hidden; z-index: 15; margin: 0; padding: 0; border-radius: 0; background: 0 0; }
        .file-item .progress span { display: block; overflow: hidden; width: 0; height: 100%; background: url(progress.png) repeat-x #06BD01; -webit-transition: width .2s linear; -moz-transition: width .2s linear; -o-transition: width .2s linear; -ms-transition: width .2s linear; transition: width .2s linear; -webkit-animation: progressmove 2s linear infinite; -moz-animation: progressmove 2s linear infinite; -o-animation: progressmove 2s linear infinite; -ms-animation: progressmove 2s linear infinite; animation: progressmove 2s linear infinite; -webkit-transform: translateZ(0); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="trip-uploader" style="height: 160px;">
                <div class="uploader-images-list">
                </div>
                <div class="webuploader-container">
                    <div id="coverPicker" class="webuploader-pick webuploader-pick-hover" style="position: relative;">
                        <i class="icon icon-plus"></i>上传主题图片<br />
                        (最多1张)
                    </div>
                    <div id="imgupload" class="upload-btn"></div>
                </div>
            </div>
        </div>
        <script>
            var $list = $(".uploader-images-list");
            var uploader = new plupload.Uploader({ //实例化一个plupload上 传对象
                browse_button: 'imgupload',
                runtimes: 'html5,flash,silverlight,html4',
                url: '/Core/UploadHandler.ashx',
                flash_swf_url: '/js/plupload/Moxie.swf',
                silverlight_xap_url: '/js/plupload/Moxie.xap',
                filters: {
                    mime_types: [ //只允许上传图片文件
                      { title: "图片文件", extensions: "jpg,gif,png" }
                    ]
                }
                , prevent_duplicates: !1
                , max_file_size: '10mb'
                , chunk_size: '1mb'//小片上传一定要注意压缩的大小
                //, resize: { width: 320, height: 240, quality: 90 }
                , init:
                {
                    PostInit: function (a) {
                        console.log("初始化完毕");
                    },
                    FilesAdded: function (uder, files) {
                        console.log("添加进队列");
                        for (var i = 0; i < files.length; i++) {
                            var file = files[i];
                            appendimg(file.id);
                        }
                        uder.start();
                    },
                    BeforeUpload: function (uder, files) {
                        console.log("开始上传");
                    },
                    UploadProgress: function (uder, file) {
                        console.log("进度：[百分比:" + file.percent + "，状态：" + file.status + ",原始大小：" + file.origSize + ",已传：" + file.loaded + "]");
                        progress(file.id, file.percent);
                    },
                    UploadFile: function (uder) {
                        console.log(uder.id + "开始上传");
                    },
                    FileUploaded: function (uder, file, resObject) {
                        var result = resObject.response;
                        console.log("上传完成" + result);
                        var $fileitem = $("." + file.id)
                        $fileitem.find("img").attr("src", JSON.parse(result).data);
                        //移除进度条
                        $fileitem.find(".progress").remove();
                    },
                    ChunkUploaded: function (a, b, c) {
                        console.log("小片上传完成后");
                    },
                    UploadComplete: function (uder, files) {
                        alert("上传完毕");
                    },
                    Error: function () {
                        alert("ERROR");
                    }

                }

            });
            uploader.init(); //初始化

            function appendimg(id, imgurl) {
                var html = ' <div  class="' + id + ' file-item"><a class="fancybox"> <img /> </a> </div>';
                $(".uploader-images-list").append(html);
            }
            function progress(id, percent) {
                //var html = '<p class="progress" style=""><span style=" width: 24%; "></span></p>';
                var c = $list.find("." + id);
                var d = c.find(".progress span");
                d.length || (d = $('<p class="progress"><span></span></p>').appendTo(c).find("span"));
                d.css("width", 100 * percent + "%")
            }
            ////绑定文件添加进队列事件
            //uploader.bind('FilesAdded', function (uploader, files) {
            //    for (var i = 0, len = files.length; i < len; i++) {
            //        var file_name = files[i].name; //文件名
            //        //构造html来更新UI
            //        var html = '<li id="file-' + files[i].id + '"><p class="file-name">' + file_name + '</p><p class="progress"></p></li>';
            //        $(html).appendTo('#file-list');
            //        !function (i) {
            //            previewImage(files[i], function (imgsrc) {
            //                $('#file-' + files[i].id).append('<img src="' + imgsrc + '" />');
            //            })
            //        }(i);
            //    }
            //});


            //function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
            //    if (!file || !/image\//.test(file.type)) return; //确保文件是图片
            //    if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
            //        var fr = new mOxie.FileReader();
            //        fr.onload = function () {
            //            callback(fr.result);
            //            fr.destroy();
            //            fr = null;
            //        }
            //        fr.readAsDataURL(file.getSource());
            //    } else {
            //        var preloader = new mOxie.Image();
            //        preloader.onload = function () {
            //            preloader.downsize(300, 300);//先压缩一下要预览的图片,宽300，高300
            //            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            //            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            //            preloader.destroy();
            //            preloader = null;
            //        };
            //        preloader.load(file.getSource());
            //    }
            //}

        </script>
    </form>
</body>
</html>
