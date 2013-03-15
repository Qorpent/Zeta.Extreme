/**
 * Виджет заголовка таблицы
 */
!function($) {
    var zefsformheader = new root.security.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        $(h.find('span').first()).text(zefs.getperiodbyid(window.zefs.myform.currentSession.Period));
    }
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        h.html(
            zefs.myform.currentSession.FormInfo.Name + " " +
                zefs.myform.currentSession.ObjInfo.Name + " за <span></span>, " +
                // zefs.myform.currentSession.getPeriod() + ", " +
                zefs.myform.currentSession.Year + " год"
        );
        if (window.zefs._periods_loaded){
            InsertPeriod();
        }
    });
    window.zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(window.zefs.periods)) {
            window.zefs.periods = result;
        }
        window.zefs._periods_loaded = true;
        InsertPeriod();
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);