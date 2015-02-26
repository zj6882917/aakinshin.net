function checkWidth(init)
{
    if ($(window).width() >= 975)
        $('#main-menu').addClass('affix');
    else if (!init)
        $('#main-menu').removeClass('affix');
}
$(document).ready(function() {
    checkWidth(true);
    $(window).resize(function() {
        checkWidth(false);
    });
});
$('#main-menu').affix({
  offset: {
    top: -1
  }
});
var $body   = $(document.body);
var navHeight = $('.navbar').outerHeight(true) + 10;
$body.scrollspy({
  target: '#my-nav',
  offset: navHeight
});