/**
 * Виджет заголовка таблицы
 */
!function($) {
    var zefsformheader = new root.security.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true, priority: 100 });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        var s = window.zefs.myform.currentSession;
        $(h.find('span').first()).text(zefs.getperiodbyid(s.Period));
        if (!!s.FormInfo.HoldResponsibility) {
            var u = $('<span class="label label-success" style="display: none;"/>').text(s.FormInfo.HoldResponsibility).css("margin-left", 3);
            h.append($('<span class="label label-success kurator"/>').html('Куратор формы <i class="icon icon-white"/>').click(function() {u.trigger('click')}),u);
            u.zetauser();
        }
        if (!!s.ObjInfo.Currency) {
            if (s.ObjInfo.Currency != "RUB") {
                h.append($('<span class="label label-info"/>').text("Валюта: " + s.ObjInfo.Currency + ", Курс для периода: " + s.ObjInfo.CurrencyRate));
            }
        }
        s = null;
    };
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        var lock =  window.zefs.myform.lock;
        if (lock != null) {
            zefsformheader.body.get(0).className = "zefsformheader";
            if (lock.state == "0ISOPEN") zefsformheader.body.addClass("isopen");
            else if (lock.state == "0ISBLOCK") zefsformheader.body.addClass("isblock");
            else if (lock.state == "0ISCHECKED") zefsformheader.body.addClass("ischecked");
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function() {
        h.html(
            zefs.myform.currentSession.FormInfo.Name + " " +
            zefs.myform.currentSession.ObjInfo.Name + " за <span></span>, " +
            // zefs.myform.currentSession.getPeriod() + ", " +
            zefs.myform.currentSession.Year + " год"
        );
        InsertPeriod();
    });
    window.zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(window.zefs.periods)) {
            window.zefs.periods = result;
            $(window.zefs).trigger(window.zefs.handlers.on_periodsload);
        }
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);