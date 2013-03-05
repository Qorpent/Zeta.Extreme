/**
 * Виджет информации о текущем польвателе
 */
!function($) {
    var login = $('<span class="login-user label label-inverse" />');
    var loginas = $('<span class="login-user label label-warning" />');
    var ConfigurePermissions = function() {
        var user = window.zeta.security.user;
        if (user != null) {
            if (user.getLogonName()) {
                login.text(user.getLogonName());
                var t1 = $('<div/>').append($('<ul class="login-permissions"/>').append(
                    $("<li/>").html("Администратор<span>" + (user.getIsAdmin() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Разработчик<span>" + (user.getIsDeveloper() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Датамастер<span>" + (user.getIsDataMaster() ? "ДА" : "НЕТ") + "</span>")
                ));
                login.tooltip({title:t1.html(), placement: 'bottom', html: true});
            }
            if (user.getImpersonation()) {
                loginas.show();
                loginas.text(user.getImpersonation());
                var t2 = $('<div/>').append($('<ul class="login-permissions"/>').append(
                    $("<li/>").html("Администратор<span>" + (user.getIsImpAdmin() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Разработчик<span>" + (user.getIsImpDeveloper() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Датамастер<span>" + (user.getIsImpDataMaster() ? "ДА" : "НЕТ") + "</span>")
                ));
                loginas.tooltip({title:t2.html(), placement: 'bottom', html: true});
            } else {
                loginas.hide();
            }
        }
        user = null;
    }
    $(window.zeta).on(window.zeta.handlers.on_getuserinfo, function() {
        ConfigurePermissions();
    });
    var logininfo = new root.security.Widget("logininfo", root.console.layout.position.layoutHeader, "right", { authonly: false, priority: 70, adminonly: true, ready: function() {
        ConfigurePermissions();
    }});
    logininfo.body = $('<div/>').append(login, loginas.hide());
    root.console.RegisterWidget(logininfo);
}(window.jQuery);