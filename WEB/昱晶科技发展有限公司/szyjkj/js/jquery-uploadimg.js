
//使用方式
//点击按钮
// <div class="u-button" style="float: left; width: 330px;"></div>
//呈现图片
//<div class="e-item u-imgaddress" style="margin-left: 150px; margin-top: 10px;"></div>
(function (window, $, undefined) {
    $.tw = $.tw || {};
    $(document).load(init());
    function init() {
        loadjscssfile("/js/ajaxfileupload.js", "js");
        //第一次加载 
        $(document).on("change", "#ufile", function () {
            var _layer = $.layer({ type: 3 });
            /*Start*/
            $.ajaxFileUpload({
                url: '/admin/action/actionupdateimage.aspx?action=ajaxuploadfile', //用于文件上传的服务器端请求地址 
                secureuri: false,//是否需要安全协议，一般设置为false
                fileElementId: 'ufile', //文件上传域的ID
                dataType: 'json', //返回值类型 一般设置为json 
                type: "POST",//当要提交自定义参数时，这个参数要设置成post
                success: function (result) {
                    /*提交成功*/
                    if (result.res == 1) {
                        $("#img-adres").attr("src", result.desc);
                        $("#img-adres").data().type = 64;
                    }
                    else {
                        alert(result.desc);
                    }
                    layer.close(_layer);
                    /*提交成功END*/
                },//提交成功后自动执行的处理函数，参数data就是服务器返回的数据。
                error: function () { alert("提交失败！"); layer.close(_layer); }//  提交失败自动执行的处理函数。
            });

            /*END*/

        });

    }
    //window.onload = loadhtml;
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

    function loadhtml() {
        var ub = '<span class="inpbbut1" style="margin-left: 10px; width: 180px;">选择图片</span> <input type="file" class="u-button" name="ecfile" id="ufile" style="z-index: 1; position: absolute; filter: alpha(opacity=0); opacity: 0; width: 200px; font: 19px monospace; cursor: pointer; margin-left: -200px;" />';
        $(".u-button").html(ub);
        var uimg = '<img id="img-adres" data-type="1" src="/images/d-banner.jpg" onerror="this.onerror=null;this.src="/images/nophoto1.jpg"" alt="昱晶科技发展有限公司" title="昱晶科技发展有限公司" />';
        $(".u-imgaddress").html(uimg);
    }
    var _loadimg = function (img) {
        loadhtml();
        $("#img-adres").attr("src", img);
    }
    $.tw.getimgaddress = function () {
        return encodeURIComponent($("#img-adres").attr("src"));
    }
    $.tw.getimgtype = function () {
        return $("#img-adres").data("type");
    }


    $.extend($.tw, {
        loadimg: _loadimg
    });
})(window, $);