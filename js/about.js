function checkWidth()
{
    if ($(window).width() >= 975 && !$("#main-menu").hasClass("affix"))
    {
        $('#main-menu').addClass('affix');
        $('#main-menu').affix({
            offset: {
                top: -1
            }
        });
    }
    else
        $('#main-menu').removeClass('affix');
}
$(document).ready(function() {
    checkWidth();
    $(window).resize(function() {
        checkWidth();
    });
});
var $body = $(document.body);
var navHeight = $('.navbar').outerHeight(true) + 10;
$body.scrollspy({
    target: '#my-nav',
    offset: navHeight
});