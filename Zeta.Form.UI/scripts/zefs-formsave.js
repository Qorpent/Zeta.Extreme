/**
 * Виджет инструмента для сохранения формы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformsave = new root.Widget("zefsformsave", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 100 });
    var b = $('<button class="btn btn-small btn-primary" title="Сохранить форму" />').html('<i class="icon-ok icon-white"/>').hide();
    var preloader = $('<div/>').css("padding", "1px 7px").append($('<img src="images/300.gif"/>')).hide();
    b.click(function(e) {
        if (e.ctrlKey && e.shiftKey) {
            zefs.myform.forcesave();
        }
        else {
            zefs.myform.save();
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (zefs.myform.lock.cansave || zefs.myform.lock.cansaveoverblock) {
            b.show();
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_savestart, function() {
        b.hide(); preloader.show();
    });
    $(window.zefs).on(window.zefs.handlers.on_savefinished, function() {
        b.show(); preloader.hide();
    });
    zefsformsave.body = $('<div/>').append(b, preloader);
    b.tooltip({placement: 'bottom'});
    root.console.RegisterWidget(zefsformsave);
}(window.jQuery);