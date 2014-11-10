var isload;
var isSmall;
(function (window, $) {
    isload = false;
    LoadJS();
})(window, jQuery);

var bindJcrop = function bindJcrop() {

    $preview = $('#preview-pane');
    $pcnt = $('#preview-pane .preview-container');
    $pimg = $('#preview-pane .preview-container img');
    ysize = $pcnt.height();
    xsize = $pcnt.width();
    if (isSmall) {
        sm_$preview = $('#preview-pane_small');
        sm_$pcnt = $('#preview-pane_small .preview-container_small');
        sm_$pimg = $('#preview-pane_small .preview-container_small img');
        sm_ysize = sm_$pcnt.height();
        sm_xsize = sm_$pcnt.width();
    }
    $('#target').Jcrop({
        onChange: updatePreview,
        onSelect: updatePreview,
        aspectRatio: xsize / ysize
    }, function () {
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        console.log(boundx);
        console.log(boundy);
        jcrop_api = this;
        jcrop_api.animateTo([0, 0, xsize, ysize]);
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
            if (isSmall) {
                var sm_rx = sm_xsize / c.w;
                var sm_ry = sm_ysize / c.h;
                sm_$pimg.css({
                    width: Math.round(sm_rx * boundx) + 'px',
                    height: Math.round(sm_ry * boundy) + 'px',
                    marginLeft: '-' + Math.round(sm_rx * c.x) + 'px',
                    marginTop: '-' + Math.round(sm_ry * c.y) + 'px'
                });
            }
        }
    };

};
//layer.closeAll = function () { var layerObj = $('.xubox_layer'); $.each(layerObj, function () { var i = $(thus).attr('times'); layer.close(i); }); };
//上传图片
/*divID:点击的按钮ID
imgID ：暂时的图片ID
oldsrc:旧图片
filetype：文件类型;{
       Authentication = 0,  //身份认证
        OperatingLicense = 1,  //运营许可证
        DrivingLicense = 2,  //驾驶证
        TravelLicense = 3, //行驶证
        GuideCertification = 4,  //导游认证
        Cooperation = 5,   //代理合作
        UploadWorks = 6, //上传作品
        UploadImg = 7, //上传图片
        Picture = 8, //用户头像
        Advertising = 9, //广告横幅的枚举
        Blogroll = 10, //友情链接的枚举
        OutlinkImg = 11, //外链图片的自动保存
        ThemesImg = 12, //主题图片
        LeaderPhoto = 13, //主题图片
        TempPicture = 99,//临时图片 
}
swidth:需要保存的小图片 宽
sheight:需要保存的小图片 高
action:特殊的要求 如用户头像需要后台处理 默认 /communal/ActionImage.aspx 只做保存图片 返回一张小图路径
isSmall：界面是否暂时小图片 默认24*24 ，如需修改更改样式
isRef：是否刷新
*/
function upload(divID, imgID, hc_oldsrc, filetype, swidth, sheight, up_action, up_isSmall, isRef) {

    clear();
    var oldsrc = hc_oldsrc;
    isSmall = up_isSmall;
    if (isload == false) {
        LoadJS();
        return;
    }
    if ($("#img-upload").length > 0) {
        $("#img-upload").remove();
    }
    var html = '<div id="img-upload" class="img-upload" style="display: none;"> <div class="wraper"> <div class="btn-wraper"> <input type="button" class="btn_file button" value="选择文件..." id="browse" /> </div> </div> <div class="jc-box"></div><div class="toolbtn"> <input type="button" value="确定提交" class=" button acta4" style="margin-left: 20px; margin: 6px 8px 5px 0; " id="imgsave" /><input type="button" value="取消修改" class=" button acta4" style="margin-left: 20px; margin: 6px 8px 5px 0; " id="btn_cancel" /> </div> </div>';
    $("body").append(html);

    if (oldsrc != "") {
        $(".jc-box").html('<div class="jc-main-box"> <div class="tg" id="file-list"> </div></div> <div id="preview-pane">  </div>');
        img64 = oldsrc;
        $('.tg').html('<img id="target"  src="' + oldsrc + '" />');
        $("#preview-pane").html('<div class="preview-container"> <img id="pre_img"   src="' + oldsrc + '"   class="jcrop-preview" alt="  " /> </div> ');
        if (isSmall) {
            $(".jc-box").append('<div id="preview-pane_small">  </div> ');
            $("#preview-pane_small").html('<div class="preview-container_small"> <img id="small_img"  src="' + oldsrc + '"   class="jcrop-preview_small" alt="  " /> </div> ');
        }
        bindJcrop();//绑定剪切  
    }

    console.log("isSmall=" + isSmall);
    var _layer = $.layer({
        type: 1,
        title: '选择图片',
        page: { dom: '.img-upload' },
        area: ['750px', '650px'],
        offset: ['100px', ''],
        position: 'center',
        success: function ($layer) {
            Init();
            $($layer).on("click", "#imgsave", function () {
                if (typeof (img64) == "undefined" || img64 == "") {
                    alert("请选择图片！");
                    return;
                }
                console.log(img64);
                console.log(jcrop_api.tellScaled())//获取选框的值（实际尺寸）。

                var x = jcropc.x, y = jcropc.y, w = jcropc.w, h = jcropc.h, path = JSON.toString(img64);
                var params = "action=saveimg&x=" + x + "&y=" + y + "&w=" + w + "&h=" + h + "&path=" + encodeURIComponent(img64) + "&filetype=" + filetype + "&swidth=" + swidth + "&sheight=" + sheight;
                var action = "/communal/ActionImage.aspx";
                if (up_action != "") {
                    action = up_action;
                }
                $.ajax({
                    type: "POST",
                    url: action,
                    data: params,
                    dataType: "json",
                    success: function (result) {
                        if (result.res == 1) {
                            layer.close(_layer);
                            $("#img-upload").remove();
                            $("." + imgID).attr("src", result.desc).val(result.desc);
                            alert("提交成功！").done(function () {
                                if (isRef) {
                                    location.reload();
                                } 
                                //$("#img-upload").remove();
                                //$(".xubox_layer").remove();  
                            }); 
                        }
                        else {
                            alert("提交失败，请刷新重试！错误代码 0");
                        }
                    }
                }).fail(function () {
                    alert("提交失败，请刷新重试！错误代码 -1")
                })
            })
            $($layer).on("click", "#btn_cancel", function () {
                //$("#img-upload").remove();
                //$(".xubox_layer").remove();
                layer.close(_layer);
                $("#img-upload").remove();
            });
        }
    });
}
// 缓存字典
var jcrop_api,
       boundx,
       boundy,
       $preview,
       $pcnt,
       $pimg,
       xsize,
       ysize,
       img64,
       jcropc;
var sm_$preview, sm_$pcnt, sm_$pcnt, sm_ysize, sm_xsize;


//初始化
var Init = function Init() {
    //  console.log(isSmall);
    var uploader = new plupload.Uploader({ //实例化一个plupload上传对象
        browse_button: 'browse',
        url: '/communal/ActionImage.aspx',  // url: 'upload.html',
        flash_swf_url: '/js/picupload/Moxie.swf',
        silverlight_xap_url: '/js/picupload/Moxie.xap',
        filters: {
            mime_types: [ //只允许上传图片文件
              { title: "图片文件", extensions: "jpg,gif,png" }
            ]
        }
        // , prevent_duplicates: true //不允许选取重复文件

    });
    uploader.init(); //初始化 
    //绑定文件添加进队列事件
    uploader.bind('FilesAdded', function (uploader, files) {
        console.log("FileAdd添加文件");
        for (var i = 0, len = files.length; i < len; i++) {
            var file_name = files[i].name; //文件名    
            !function (i) {
                previewImage(files[i], function (imgsrc) {
                    $(".jc-box").html('<div class="jc-main-box"> <div class="tg" id="file-list"> </div></div> <div id="preview-pane">  </div>');
                    img64 = imgsrc;
                    $('.tg').html('<img id="target"  src="' + imgsrc + '" />');
                    $("#preview-pane").html('<div class="preview-container"> <img id="pre_img"   src="' + imgsrc + '"   class="jcrop-preview" alt="  " /> </div> ');
                    if (isSmall) {
                        $(".jc-box").append('<div id="preview-pane_small">  </div> ');
                        $("#preview-pane_small").html('<div class="preview-container_small"> <img id="small_img"  src="' + imgsrc + '"   class="jcrop-preview_small" alt="  " /> </div> ');
                    }
                    bindJcrop();//绑定剪切
                })
            }(i);
        }
    });
    function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
        console.log("previewImage");
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
                preloader.downsize(500, 450);//先压缩一下要预览的图片,宽300，高300
                var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
                callback && callback(imgsrc); //callback传入的参数为预览图片的url
                preloader.destroy();
                preloader = null;
            };
            preloader.load(file.getSource());
        }
    }


    return true;
}


function LoadJS() {
    if (!isload) {
        loadjscssfile("/style/style.css", "css");
        loadjscssfile("/style/css.cs", "css");
        loadjscssfile("/js/picupload/jquery.Jcrop.js", "js");
        loadjscssfile("/js/picupload/plupload.full.min.js", "js");
        loadjscssfile("/js/jquery.layer.min.js", "js");

        loadjscssfile("/style/ucss/picupload.css", "css");
        loadjscssfile("/style/ucss/jquery.Jcrop.css", "css");
        //loadjscssfile("/js/jqueryExt.js", "js");
        isload = true;
        console.log("加载JS");
    }

}
function loadjscssfile(filename, filetype) {

    if (filetype == "js") {
        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", filename);
    } else if (filetype == "css") {

        var fileref = document.createElement('link');
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", filename);
    }
    if (typeof fileref != "undefined") {
        document.getElementsByTagName("head")[0].appendChild(fileref);
    }

}
function clear() {
    img64 = "";
}
