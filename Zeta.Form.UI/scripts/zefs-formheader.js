/**
 * Виджет заголовка таблицы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformheader = new root.Widget("zefsformheader", root.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 100 });
    var formname = $('<h3 class="formname"/>');
    var formperiod = $('<h3/>');
    var formyear = $('<h3/>');
    var period = null;
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
            formperiod.text(zefs.getperiodbyid(period));
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        if (null == zefs.myform.startError) {
            var s = zefs.myform.currentSession;
            if (period == null) period = s.Period;
            var periodtype = "";
            if (s.FormInfo.Name.search(/(план|факт)/) == -1) {
                if (s.FormInfo.Code.search(/A\.in/) != -1) periodtype = " (факт)";
                else if (s.FormInfo.Code.search(/B\.in/) != -1) periodtype = " (план)";
            }
            formname.text(s.FormInfo.Name + " " + s.ObjInfo.Name + periodtype + " за");
            formyear.text(s.Year + " год");
        }
    });
    zefsformheader.body = $('<div/>').append(formname, formperiod, formyear);
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);