/**
 * Виджет Zefs-формы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsform = new root.Widget("zefsform", root.console.layout.position.layoutBodyMain, null, { authonly: true, ready: function() {
        zefs.myform.execute()
    } });
    zefsform.body = $('<table class="data isblocked"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (zefs.myform.lock.cansave || zefs.myform.lock.cansaveoverblock) {
            zefsform.body.removeClass("isblocked");
        } else {
            zefsform.body.addClass("isblocked");
        }
    });
    root.console.RegisterWidget(zefsform);
}(window.jQuery);