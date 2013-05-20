/**
 * Виджет заголовка таблицы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformheader = new root.Widget("zefsformheader", root.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 100 });
    var h = $('<h3/>');
    var InsertPeriod = function() {
        if (null != zefs.myform.startError) return;
        var s = window.zefs.myform.currentSession || {};
        $(h.find('span').first()).text(zefs.getperiodbyid(s.Period));
        s = null;
    };
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        var zetaheader = $('#consolePageHeader');
        var lock =  window.zefs.myform.lock;
        if (lock != null) {
            zetaheader.className = "console-pageheader";
            if (lock.state == "0ISOPEN") zetaheader.addClass("isopen");
            else if (lock.state == "0ISBLOCK") zetaheader.addClass("isblock");
            else if (lock.state == "0ISCHECKED") zetaheader.addClass("ischecked");
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function() {
        if (null == zefs.myform.startError) {
            h.html(
                zefs.myform.currentSession.FormInfo.Name + " " +
                zefs.myform.currentSession.ObjInfo.Name + " за <span></span>, " +
                // zefs.myform.currentSession.getPeriod() + ", " +
                zefs.myform.currentSession.Year + " год"
            );
        }
        InsertPeriod();
    });
    zefsformheader.body = $('<div/>').append(h);
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);