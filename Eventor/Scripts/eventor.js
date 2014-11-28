
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

var hide_header = function () {
    $(document).on('click', '#hide_header', function () {
        var hc = $('#header_content');
        if (hc.hasClass('active')) {
            hc.slideUp("slow").removeClass('active');
            $(this).addClass('small');
        } else {
            hc.slideDown("slow").addClass('active');
            $(this).removeClass('small');
        }
    });
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
    });

    $('#expand').on('click', function () { changeTab($(this).attr('data-value')); });
    hide_header();
});

