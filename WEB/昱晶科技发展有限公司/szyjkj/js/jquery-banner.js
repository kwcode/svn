(function (window, $, undefined) {
    var _times = 3000;
    var t, size, n = 0;
    $(function () {
        size = $(".banner-list ul li").size();
        init();
        t = setInterval(showbanner, _times); //3秒切换  
        function init() {
            if (size > 0) {
                var html = '<div class="banner-no"><ul>';
                for (var i = 1; i < size + 1; i++) {
                    html += '<li data-index="' + i + '">' + i + '</li>';
                }
                html += '</ul><div>';
                $(".banner").append(html);
            }
        }
        function showbanner() {
            n = n >= (size - 1) ? 0 : ++n;
            $(".banner-list ul li").eq(n).trigger('click');
        }

        $(".banner-list li").click(function () {
            var i = $(this).index();
            i = i >= (size - 1) ? 0 : ++i;
            $(".banner-list ul li").fadeOut(500).parent().children().eq(i).fadeIn(1000);
            $(".banner-no ul li").each(function () {
                var $that = $(this);
                if ($that.index() == i) {
                    $that.css("background", "red");
                } else { $that.css("background", "rgb(255, 153, 0)"); }
            });
        });
        //点击
        $(".banner-no li").click(function () {
            var i = $(this).index();
            $(".banner-list ul li").fadeOut(500).parent().children().eq(i).fadeIn(1000);

            $(".banner-no ul li").each(function () {
                var $that = $(this);
                if ($that.index() == i) {
                    $that.css("background", "red");
                } else { $that.css("background", "rgb(255, 153, 0)"); }
            });
        });
        $(".banner-list").mouseleave(function () {
            t = setInterval(showbanner, _times); //3秒切换 
            $(".btn_qt").val("移开");
        });
        $(".banner-list").mousemove(function () {
            clearInterval(t);
            $(".btn_qt").val("停靠");
        });

        $(".bannertitle").mousemove(function () {
            clearInterval(t);
            $(".btn_qt").val("停靠");
        });
        $(".bannertitle").mouseleave(function () {
            clearInterval(t);
            $(".btn_qt").val("移开");
        });

        $(".banner-no").mousemove(function () {
            clearInterval(t);
            $(".btn_qt").val("停靠");
        });
        $(".banner-no").mouseleave(function () {
            clearInterval(t);
            $(".btn_qt").val("移开");
        });


    });

})(window, $);