$(function () {
    $(".navbar-toggler").on("click", function (e) {
        var headerElement = $('header');
        var windowWidth = $(window).width();
        if (windowWidth >= 992) {
            if (!headerElement.hasClass('hidden')) {
                $(".custom-header").toggleClass("hidden");
                $('.custom-main').css('margin', '0 auto');

                $(".main-title-page").addClass("header-sidebar-off");//nnv title when sidebar is hidden
                $(".main-title-page").removeClass("header-sidebar-on");//nnv title when sidebar is hidden

                $('.custom-header').css('transition', 'all 0.3s ease');
            } else {
                $(".custom-header").removeClass("hidden");
                $('.custom-main').css('margin', '0 0 0 290px');

                $(".main-title-page").removeClass("header-sidebar-off");//nnv title when sidebar is show
                $(".main-title-page").addClass("header-sidebar-on");//nnv title when sidebar is show

                $('.custom-header').css('transition', 'all 0.3s ease');
            }
        } else {
            if (headerElement.hasClass('hidden')) {
                $(".custom-header").removeClass("hidden");
                $('.custom-header').css('transition', 'all 0.3s ease');
            }
            
            $(".main-title-page").addClass("header-sidebar-off");//nnv  title when windowWidth < 992px
            $(".main-title-page").removeClass("header-sidebar-on");//nnv  title when windowWidth < 992px

            $(".custom-header").toggleClass("show");

            
            
            e.stopPropagation();
        }
    });

    $("html").click(function (e) {
        var header = document.getElementById("custom-header");

        if (!header.contains(e.target)) {
            $(".custom-header").removeClass("show");
        }
    });

    $("#custom-nav .nav-link").click(function (e) {
        $(".custom-header").removeClass("show");
    });
});


$(document).ready(function () { //init title
   
    var windowWidth = $(window).width();
    if (windowWidth >= 992) {

        $(".main-title-page").addClass("header-sidebar-on");
        $(".main-title-page").removeClass("header-sidebar-off");
        
    } else { //title when windowWidth < 992px
        $(".main-title-page").addClass("header-sidebar-off");
        $(".main-title-page").removeClass("header-sidebar-on");
    }

});
