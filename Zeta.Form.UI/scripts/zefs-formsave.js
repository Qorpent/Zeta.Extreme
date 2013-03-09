/**
 * Виджет инструмента для сохранения формы
 */
!function($) {
    var zefsformsave = new root.security.Widget("zefsformsave", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 50 });
    var b = $('<button class="btn btn-small btn-primary" title="Сохранить форму" />').html('<i class="icon-ok icon-white"/>');
    b.click(function(e) {
        zefs.myform.save(window.zefs.myform.getChanges());
    });
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (!zefs.myform.lock) {
            b.attr("disabled", "disabled");
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_savestart, function() {
        b.attr("disabled", "disabled");
    });
    $(window.zefs).on(window.zefs.handlers.on_savefinished, function() {
        b.removeAttr("disabled");
    });
    zefsformsave.body = $('<div/>').append(b);
    b.tooltip({placement: 'bottom'});
    root.console.RegisterWidget(zefsformsave);
}(window.jQuery);