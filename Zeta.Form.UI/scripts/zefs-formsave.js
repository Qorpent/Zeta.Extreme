/**
 * Виджет инструмента для сохранения формы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformsave = new root.Widget("zefsformsave", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 100 });
    var b = $('<button class="btn btn-small" title="Сохранить форму" disabled/>').html('<i class="icon-ok"/>');
    var preloader = $('<div/>').css("padding", "1px 7px").append($('<img src="images/300.gif"/>')).hide();
    var backdrop = $('<div class="zefsbackdrop"/>');
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
            backdrop.hide();
            b.addClass("btn-primary");
            b.find("i").addClass("icon-white");
            b.removeAttr("disabled");
        }
        $('#consoleBody').append(backdrop.hide());
    });
    $(window.zefs).on(window.zefs.handlers.on_savestart, function() {
        b.hide(); preloader.show(); backdrop.show();
    });
    $(window.zefs).on(window.zefs.handlers.on_savefinished, function() {
        b.show(); preloader.hide(); backdrop.hide();
    });
    zefsformsave.body = $('<div/>').append(b, preloader);
    b.tooltip({placement: 'bottom'});
    root.console.RegisterWidget(zefsformsave);
}(window.jQuery);