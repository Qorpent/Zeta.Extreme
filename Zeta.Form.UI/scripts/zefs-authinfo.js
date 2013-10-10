/**
 * Виджет информации об авторизации польвателя
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var login = $('<span class="login-user label label-info"/>');
    var loginas = $('<span class="login-user label label-warning" />');
    var ConfigurePermissions = function() {
        var user = window.zeta.user;
        if (user != null) {
            if (user.getRealLogonName()) {
                zeta.zetauser.getDetails(user.getRealLogonName(), function(result) { login.text(result.ShortName) });
                var t1 = $('<div/>').append($('<ul class="login-permissions"/>').append(
                    $("<li/>").html("Администратор<span>" + (user.getRealIsAdmin() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Разработчик<span>" + (user.getRealIsDeveloper() ? "ДА" : "НЕТ") + "</span>"),
                    $("<li/>").html("Датамастер<span>" + (user.getRealIsDataMaster() ? "ДА" : "НЕТ") + "</span>")
                ));
                login.tooltip({title:t1.html(), placement: 'bottom', html: true});
            }
            if (user.getImpersonation()) {
                loginas.show();
                zeta.zetauser.getDetails(user.getImpersonation(), function(result) { loginas.text(result.ShortName) });
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
    };
    $(window.zeta).on(window.zeta.handlers.on_getuserinfo, function() {
        ConfigurePermissions();
    });
    var logininfo = new root.Widget("logininfo", root.console.layout.position.layoutHeader, "right", { authonly: false, priority: 70, ready: function() {
        ConfigurePermissions();
    }});
    logininfo.body = $('<div/>').append(login, loginas.hide());
    root.console.RegisterWidget(logininfo);
}(window.jQuery);