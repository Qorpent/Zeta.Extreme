/**
 * Виджет заголовка таблицы
 */
!function($) {
    var zefsformheader = new root.security.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        $(h.find('span').first()).text(zefs.getperiodbyid(window.zefs.options.getParameters().period));
    }
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        h.html(
            zefs.myform.currentSession.getFormInfo().getName() + " " +
                zefs.myform.currentSession.getObjInfo().getName() + " за <span></span>, " +
                // zefs.myform.currentSession.getPeriod() + ", " +
                zefs.myform.currentSession.getYear() + " год"
        );
        if (window.zefs._periods_loaded){
            InsertPeriod();
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function() {
        window.zefs._periods_loaded = true;
        InsertPeriod();
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);