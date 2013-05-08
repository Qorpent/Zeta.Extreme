/**
 * Виджет заголовка таблицы
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsformheader = new root.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true, priority: 100 });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        var s = window.zefs.myform.currentSession;
        $(h.find('span').first()).text(zefs.getperiodbyid(s.Period));
        if (!!s.FormInfo.HoldResponsibility) {
            var u = $('<span class="label label-success" style="display: none;"/>').text(s.FormInfo.HoldResponsibility).css("margin-left", 3);
            var k = $('<span class="label label-success kurator"/>').html('Куратор формы <i class="icon icon-white"/>');
            k.click(function() {
                u.trigger('click');
            });
            h.append(k);
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
    $(window.zefs).on(window.zefs.handlers.on_formusersload, function() {
        if (!!zefs.myform.users) {
            var b = $('<span class="label label-warning dropdown-toggle pull-right" data-toggle="dropdown"/>').text('Пользователи');
            var header = $('<tr/>').append(
                $('<th/>').text("Пользователь"),
                $('<th/>').text("Должность"),
                $('<th/>').text("Телефон")
            );
            if (zeta.user.getRealIsAdmin()) {
                header.append($('<th/>').text(""));
            }
            var content = $('<table class="table-bordered table formuserstable" />').append(
                $('<thead/>').append(header)
            );
            var body = $('<tbody/>');
            content.append(body);
            $.each(zefs.myform.users, function(i, e) {
                var u = $('<span class="label label-info"/>').text(e.Login);
                var tr = $('<tr/>').append(
                    $('<td class="username"/>').append(u),
                    $('<td class="dolzh"/>').text(e.Dolzh),
                    $('<td class="contacts"/>').text(e.Contact)
                );
                if (zeta.user.getRealIsAdmin()) {
                    var l = $('<button class="btn btn-mini"/>').text("Войти от");
                    tr.append($('<td/>').append(l));
                    l.click(function() {
                        zeta.api.security.impersonateall({Target: e.Login});
                    });
                }
                u.zetauser();
                body.append(tr);
            });
            b.click(function() {
                var params = { title: "Cписок пользователей данной формы", content: content, width: 800 };
                $(window.zeta).trigger(window.zeta.handlers.on_modal, params);
            });
            h.append(b);
        }
    });
    window.zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(window.zefs.periods)) {
            window.zefs.periods = result;
            $(window.zefs).trigger(window.zefs.handlers.on_periodsload);
        }
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);