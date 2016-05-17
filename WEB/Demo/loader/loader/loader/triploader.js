 
(function ($) {
    $.trip = $.trip || {};
    $.trip.loader = $.trip.loader || {};
    var _loaderClass = "triploader";
    function centerLoader() {
        var winW = $(window).width();
        var winH = $(window).height();

        var spinnerW = $('.fl').outerWidth();
        var spinnerH = $('.fl').outerHeight();

        $('.fl').css({
            'position': 'absolute',
            'left': (winW / 2) - (spinnerW / 2),
            'top': (winH / 2) - (spinnerH / 2)
        });

    }
    $.trip.loader.show = function (time, no, options) {
        $.trip.loader.close();
        var settings = $.extend({
            time: time || 0, // Default Time to hide fakeLoader
            pos: 'fixed',// Default Position
            top: '0px',  // Default Top value
            left: '0px', // Default Left value
            width: '100%', // Default width 
            height: '100%', // Default Height
            zIndex: '999',  // Default zIndex 
            bgColor: '#000', // Default background color
            spinner: 'spinner' + (no || 2), // Default Spinner
            opacity: '0.5',
            imagePath: '' // Default Path custom image
        }, options);

        var tloader = '  <div class="' + _loaderClass + '"></div>';
        var spinner01 = '<div class="fl spinner1"><div class="double-bounce1"></div><div class="double-bounce2"></div></div>';
        var spinner02 = '<div class="fl spinner2"><div class="spinner-container container1"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div><div class="spinner-container container2"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div><div class="spinner-container container3"><div class="circle1"></div><div class="circle2"></div><div class="circle3"></div><div class="circle4"></div></div></div>';
        var spinner03 = '<div class="fl spinner3"><div class="dot1"></div><div class="dot2"></div></div>';
        var spinner04 = '<div class="fl spinner4"></div>';
        var spinner05 = '<div class="fl spinner5"><div class="cube1"></div><div class="cube2"></div></div>';
        var spinner06 = '<div class="fl spinner6"><div class="rect1"></div><div class="rect2"></div><div class="rect3"></div><div class="rect4"></div><div class="rect5"></div></div>';
        var spinner07 = '<div class="fl spinner7"><div class="circ1"></div><div class="circ2"></div><div class="circ3"></div><div class="circ4"></div></div>';

        //The target
        var $body = $("body");
        $body.append(tloader);
        var el = $("." + _loaderClass);

        //Init styles
        var initStyles = {
            'position': settings.pos,
            'width': settings.width,
            'height': settings.height,
            'top': settings.top,
            'left': settings.left,
            'opacity': settings.opacity
        };

        el.css(initStyles);

        el.each(function () {
            var a = settings.spinner;
            //console.log(a)
            switch (a) {
                case 'spinner1':
                    el.html(spinner01);
                    break;
                case 'spinner2':
                    el.html(spinner02);
                    break;
                case 'spinner3':
                    el.html(spinner03);
                    break;
                case 'spinner4':
                    el.html(spinner04);
                    break;
                case 'spinner5':
                    el.html(spinner05);
                    break;
                case 'spinner6':
                    el.html(spinner06);
                    break;
                case 'spinner7':
                    el.html(spinner07);
                    break;
                default:
                    el.html(spinner01);
            }

            //Add customized loader image

            if (settings.imagePath != '') {
                el.html('<div class="fl"><img src="' + settings.imagePath + '"></div>');
                centerLoader();
            }
        });
        if (time > 0) {
            setTimeout(function () {
                $(el).remove();
            }, (settings.time * 1000));
        }

        centerLoader();
        el.css({
            'backgroundColor': settings.bgColor,
            'zIndex': settings.zIndex
        });
        return el;

    }
    $.trip.loader.close = function () {
        $("." + _loaderClass).remove();
    }
    document.head.appendChild(function () {
        var js = document.scripts, script = js[js.length - 1], jsPath = script.src;
        var path = jsPath.substring(0, jsPath.lastIndexOf("/") + 1);
        var link = document.createElement('link');
        link.href = path + 'triploader.css';
        link.type = 'text/css';
        link.rel = 'styleSheet'
        link.id = 'layermcss';
        return link;
    }());
}(jQuery));




