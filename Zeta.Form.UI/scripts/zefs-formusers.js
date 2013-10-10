/**
 * Виджет nerublevoi valuti
 */
!function($) {
    var formusers = new zeta.Widget("zefsformusers", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 90 });
    var b = $('<span class="label label-warning dropdown-toggle pull-right" data-toggle="dropdown"/>').text('Пользователи');
    $(window.zefs).on(window.zefs.handlers.on_formusersload, function() {
        if (!!zefs.myform.users) {
            b.show();
            // внимание! тут используется метод returnLogins() потому что эти пользователи гарантированно есть в сторадже
            // мы их туда пихаем в Zefs.js 
            zeta.zetauser.returnLogins($.map(zefs.myform.users, function(u) { return u.Login.toLowerCase() }).join(","), function(users) {
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
                    if (!!zefs.myform.currentSession) {
                        if (e.Login == zefs.myform.currentSession.FormInfo.ObjectResponsibility) {
                            tr.addClass("responsible");
                            u.removeClass("label-info");
                            u.addClass("label-important");
                        }
                    }
                    if (zeta.user.getRealIsAdmin()) {
                        var l = $('<button class="btn btn-mini"/>').text("Войти от");
                        tr.append($('<td/>').append(l));
                        l.click(function() {
                            zeta.api.security.impersonateall({Target: e.Login});
                        });
                    }
                    u.text(users[e.Login.toLowerCase()].ShortName);
                    u.click(function() {
                        zeta.zetauser.showDetails(users[e.Login.toLowerCase()]);
                    });
                    body.append(tr);
                });
                b.click(function() {
                    var params = { title: "Cписок пользователей данной формы", content: content, width: 800 };
                    $(window.zeta).trigger(window.zeta.handlers.on_modal, params);
                });
            });
        }
    });
    formusers.body = $('<div/>').append(b.hide());
    zeta.console.RegisterWidget(formusers);
}(window.jQuery);