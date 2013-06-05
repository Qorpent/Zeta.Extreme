/**
 * Виджет Zefs-формы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsform = new root.Widget("zefsform", root.console.layout.position.layoutBodyMain, null, { authonly: true, ready: function() {
        zefs.init(jQuery);
        zefs.myform.execute()
    } });
    zefsform.body = $('<table class="data" id="zefsForm"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (!zefs.myform.lock.getCanSave()) {
            zefsform.body.addClass("isblocked");
        }
    });
    root.console.RegisterWidget(zefsform);
}(window.jQuery);