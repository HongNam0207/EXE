// jquery-click-scroll
// by syamsul'isul' Arifin — fixed version by L?c

var sectionArray = [1, 2, 3, 4, 5];

$.each(sectionArray, function (index, value) {

    // Khi cu?n trang
    $(document).scroll(function () {
        var $section = $('#section_' + value);
        if ($section.length) { // ? ki?m tra t?n t?i
            var offsetSection = $section.offset().top - 75;
            var docScroll = $(document).scrollTop() + 1;

            if (docScroll >= offsetSection) {
                $('.navbar-nav .nav-item .nav-link').removeClass('active').addClass('inactive');
                $('.navbar-nav .nav-item .nav-link').eq(index).addClass('active').removeClass('inactive');
            }
        }
    });

    // Khi click vào menu
    $('.click-scroll').eq(index).click(function (e) {
        e.preventDefault();
        var $target = $('#section_' + value);
        if ($target.length) { // ? ki?m tra t?n t?i tr??c khi scroll
            var offsetClick = $target.offset().top - 75;
            $('html, body').animate({ scrollTop: offsetClick }, 300);
        }
    });
});

$(document).ready(function () {
    $('.navbar-nav .nav-item .nav-link').addClass('inactive');
    $('.navbar-nav .nav-item .nav-link').eq(0).addClass('active').removeClass('inactive');
});
