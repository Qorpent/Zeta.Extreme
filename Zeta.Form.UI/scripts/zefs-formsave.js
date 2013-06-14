/**
 * Виджет инструмента для сохранения формы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformsave = new root.Widget("zefsformsave", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 100 });
    var b = $('<button class="btn btn-small" title="Сохранить форму Ctrl+S" disabled/>').html('<i class="icon-ok"/>');
    var superb = $('<button class="btn btn-small" title="Принудительная прокачка данных Ctrl+Shift+S" disabled/>').html('<i class="icon-ok"/>');
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
    superb.click(function() {
        zefs.myform.forcesave();
    });
    var EnableBtn = function() {
        backdrop.hide();
        b.addClass("btn-primary");
        b.find("i").addClass("icon-white");
        b.removeAttr("disabled");
        superb.addClass("btn-danger");
        superb.find("i").addClass("icon-white");
        superb.removeAttr("disabled");
    };
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        var l = zefs.myform.lock;
        if (l.state == "0ISOPEN") {
            if (l.cansave || l.periodstateoverride) {
                EnableBtn();
            }
        }
        else {
            if (l.cansaveoverblock) {
                EnableBtn();
            }
        }
        $('#consoleBody').append(backdrop.hide());
    });
    $(window.zefs).on(window.zefs.handlers.on_savestart, function() {
        b.hide(); superb.hide(); preloader.show(); backdrop.show();
    });
    $(window.zefs).on(window.zefs.handlers.on_savefinished, function() {
        b.show(); superb.show(); preloader.hide(); backdrop.hide();
    });
    zefsformsave.body = $('<div/>').append(superb, b, preloader);
    b.tooltip({placement: 'bottom'});
    superb.tooltip({placement: 'bottom'});
    root.console.RegisterWidget(zefsformsave);
}(window.jQuery);