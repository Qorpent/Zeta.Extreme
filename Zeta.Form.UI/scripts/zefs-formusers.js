/**
 * Виджет nerublevoi valuti
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var formusers = new zeta.Widget("zefsformusers", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 90 });
    var b = $('<span class="label label-warning dropdown-toggle pull-right" data-toggle="dropdown"/>').text('Пользователи');
    $(window.zefs).on(window.zefs.handlers.on_formusersload, function() {
        if (!!zefs.myform.users) {
            b.show();
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
        }
    });
    formusers.body = $('<div/>').append(b.hide());
    zeta.console.RegisterWidget(formusers);
}(window.jQuery);