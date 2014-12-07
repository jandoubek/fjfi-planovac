
var do_signup = function () {
    $('#external_signup_form').hide();
    $('#login_form').hide();
    $('#signup_form').show();
    $('#expand').text('Log in').attr('data-value', 'Login');
};

var do_login = function () {
    $('#external_signup_form').hide();
    $('#signup_form').hide();
    $('#login_form').show();
    $('#expand').text('Sign up').attr('data-value', 'Register');
};

var do_externalsignup = function () {
    $('#signup_form').hide();
    $('#login_form').hide();
    $('#external_signup_form').show();
    $('#expand').text('Log in').attr('data-value', 'Login');
}

var hide_header = function (w_h) {
    $(document).on('click', '#hide_header', function () {
        var hc = $('#header_content');
        if (hc.hasClass('active')) {
            hc.slideUp("slow",error_image_h(w_h,1)).removeClass('active');
            $(this).addClass('small');
        } else {
            hc.slideDown("slow",error_image_h(w_h,0)).addClass('active');
            $(this).removeClass('small');
        }
    });
}

var error_image_h = function(w_h,scroll) {
    var content_h = scroll ? parseInt(w_h) - 255 : parseInt(w_h) - 355;
    if(content_h<400) content_h = 400;
    var percent = parseFloat(content_h/820),
        margin_top = (parseInt(content_h) - parseInt($('#error_404 .left').outerHeight()))/2 - 20;
    $('#error_404 img').height(content_h+'px').width((percent*451)+'px');
    $('#error_404 .left').css({'margin-top': margin_top+'px'});
    console.log(content_h+' '+$('#error_404').outerHeight());
}

var changeTab = function (arg) {
    switch (arg) {
        case "Login": 
            do_login();
            break;
        case "Register":
            do_signup();
            break;
        case 'ExternalLogin':
        case 'ExternalLoginConfirmation':
            do_externalsignup();
            break;
        default:
            break;
    }
}

// WINDOW LOAD
$(window).load(function () {

    var w_w = $(window).width();
    var w_h = $(window).height();
    var box_h = $('#floating_box').outerHeight();
    var padding_h = (w_h - box_h) / 2;
    var dots_h = w_h - padding_h;
    $('#index').height(w_h);
    $('#dots').css({
        'height': dots_h,
        'padding-top': padding_h
    });
    error_image_h(w_h);

    $(window).resize(function () {
        w_w = $(window).width();
        w_h = $(window).height();
        box_h = $('#floating_box').outerHeight();
        padding_h = (w_h - box_h) / 2;
        dots_h = w_h - padding_h;
        $('#index').height(w_h);
        $('#dots').css({
            'height': dots_h,
            'padding-top': padding_h
        });
        error_image_h(w_h);
    });

    $('#expand').on('click', function () { changeTab($(this).attr('data-value')); });
    hide_header(w_h);
});

