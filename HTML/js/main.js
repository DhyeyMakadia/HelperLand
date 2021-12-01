$(window).scroll(function() {     
    var scroll = $(window).scrollTop();
    if (scroll > 0) {
        $("#navbar").addClass("active");
    }
    else {
        $("#navbar").removeClass("active");
    }
});