// WINDOW LOAD
$(window).load(function() {
  var w_w = $(window).width();
  var w_h = $(window).height();
  var box_h = $('#floating_box').outerHeight();
  var padding_h = (w_h - box_h)/2;
  var dots_h = w_h - padding_h;
  $('#index').height(w_h);
  $('#dots').css({
    'height': dots_h,
    'padding-top': padding_h
  });

  $(document).on('click','#signup_expand',function() {
    $('#login_form').hide();
    $('#signup_form').show();
    $(this).attr('id','login_expand');
    $(this).text('Log in');
  });

  $(document).on('click','#login_expand',function() {
    $('#login_form').show();
    $('#signup_form').hide();
    $(this).attr('id','signup_expand');
    $(this).text('Sign up');
  });

  $(window).resize(function(){
    w_w = $(window).width();
    w_h = $(window).height();
    box_h = $('#floating_box').outerHeight();
    padding_h = (w_h - box_h)/2;
    dots_h = w_h - padding_h;
    $('#index').height(w_h);
    $('#dots').css({
      'height': dots_h,
      'padding-top': padding_h
    });
  });
});