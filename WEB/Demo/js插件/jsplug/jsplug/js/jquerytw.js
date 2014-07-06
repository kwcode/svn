(function (window, $, undefined) {
    // 所有我们自己写的jQuery插件都挂载在 $.tw 下
    $.tw = $.tw || {};
    //自定义提示框 
    $.tw.alert = function (title, msg, ico) {
        var ii = layer.load('加载中');
        alert(msg);
    }
    window.tw = {
        AA: function () { alert("dd"); }
    }
    twAA = {
        BB: function () { alert("bb"); },
        twBB: function () { alert("cc"); }
    }
    $.tw.pralert = function () {
        var pr = " <div class='tw_alert'><div class='tw_a_t'>  <span>标题</span> </div>";
        pr += " <div class='tw_a_body'><div class='tw_a_content'> <span>内容</span></div>";
        pr += "<div class='tw_a_fun'> <input type='button' value='Ok' /><input type='button' value='Cancel' /></div> </div></div>";
        $("BODY").append(pr);
      
    }


})(window, $);