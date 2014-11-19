var do_signup = function () {
    $('#external_signup_form').hide();
    $('#login_form').hide();
    $('#signup_form').show();
    $('#expand').text('Log in').attr('data-value', 'login');
};

var do_login = function () {
    $('#external_signup_form').hide();
    $('#signup_form').hide();
    $('#login_form').show();
    $('#expand').text('Sign up').attr('data-value', 'signup');
};

var do_externalsignup = function () {
    $('#signup_form').hide();
    $('#login_form').hide();
    $('#external_signup_form').show();
    $('#expand').text('Log in').attr('data-value', 'login');
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

    var loc_ch = function () {
        switch (location.hash) {
            case "#login": do_login(); break;
            case "#signup": do_signup(); break;
            case '#externalsignup': do_externalsignup(); break;
            default: break;
        }
    }

    $(window).on('hashchange', function () {
        loc_ch();
    });

    loc_ch();

    $('#expand').on('click', function () { location.hash = '#' + $(this).attr('data-value'); });

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
});

