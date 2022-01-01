$(document).ready(function() {
    $(".navbar-toggler").click(function() {
        var display1 = $("#toggle-btn").attr("aria-expanded");
        if(display1){
            $('#navbar').attr('style', 'background-color: #525252 !important');
        }
        if(display1 == 'true'){
            $('#navbar').attr('style', 'background-color: transparent');
        }
        
    });
});