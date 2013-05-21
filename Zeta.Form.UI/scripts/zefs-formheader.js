/**
 * Виджет заголовка таблицы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformheader = new root.Widget("zefsformheader", root.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 100 });
    var formname = $('<h3 class="formname"/>');
    var formperiod = $('<h3/>');
    var formyear = $('<h3/>');
    var period = 999;
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
            formperiod.text(zefs.getperiodbyid(period) + " ");
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        if (null == zefs.myform.startError) {
            period = window.zefs.myform.currentSession.Period;
            formname.text(zefs.myform.currentSession.FormInfo.Name + " " + zefs.myform.currentSession.ObjInfo.Name + " за ");
            formyear.text(zefs.myform.currentSession.Year + " год");
        }
    });
    zefsformheader.body = $('<div/>').append(formname, formperiod, formyear);
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);