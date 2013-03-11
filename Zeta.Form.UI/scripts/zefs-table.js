/**
 * Виджет Zefs-формы
 */
!function($) {
    var zefsform = new root.security.Widget("zefsform", root.console.layout.position.layoutBodyMain, null, { authonly: true, ready: function() {
        zefs.init(jQuery);
        zefs.myform.run();
    } });
    zefsform.body = $('<table class="data"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (!zefs.myform.lock.getCanSave()) {
            zefsform.body.addClass("isblocked");
        }
    });
    root.console.RegisterWidget(zefsform);
}(window.jQuery);