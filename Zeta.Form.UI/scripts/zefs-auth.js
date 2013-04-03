/**
 * Виджет формы авторизации пользователя
 */
!function($) {
    var l = $('<input class="input-small" type="text" placeholder="Логин" autocomplete/>');
    var p = $('<input class="input-small" type="password" placeholder="Пароль"/>');
    var f = $('<form/>', { "class": "navbar-form login-form"})
        .submit(function(e) {
            e.preventDefault();
            authorize();
        })
        .append(l, p,
        $("<button/>", {
            "class" : "btn btn-small",
            "type" : "submit",
            "text" : "Войти"
        })
    ).hide();
    var authbtn = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Вход от имени"/>')
        .html('<i class="icon-user"></i><span class="caret"></span>');
    var implogin = $('<input class="input-small" type="text" placeholder="Войти от..."/>')
        .css("width", "120px")
        .change(function() {
            root.console.impersonate(implogin.val());
            $(document).trigger('click.dropdown.data-api');
        });
    var deimp = $('<li/>').append($('<button class="btn btn-mini"/>')
        .text("Вернуться в свой логин"))
        .click(function() {
            root.console.impersonate();
            implogin.val("");
        });
//  var imp = $('<li/>').append(implogin, $('<button class="btn btn-mini"/>').click(function() { root.console.impersonate(implogin.val()) }).text("Войти от..."));
    var menu = $('<ul class="dropdown-menu"/>').css("min-width", "100px").append(
        deimp.hide(), $('<li/>').append(implogin),
        $('<li/>').append($('<button class="btn btn-mini pull-right"/>').click(function() { root.console.unauthorize() }).text("Выйти из системы"))
    );
    var m = $('<div class="btn-group pull-right"/>').append(
        authbtn, menu).hide();

    var authorize = function() {
        root.console.authorize(l.val(), p.val());
    }

    $(window.zeta).on(window.zeta.handlers.on_loginsuccess, function() {
        window.zeta.console.whoami();
        f.hide();
        m.show();
    });

    $(window.zeta).on(window.zeta.handlers.on_deimpersonate, function() {
//        window.zeta.console.whoami();
//        deimp.hide();
//        implogin.show();
        location.reload();
    });

    $(window.zeta).on(window.zeta.handlers.on_impersonate, function() {
//        window.zeta.console.whoami();
//        implogin.hide();
//        deimp.show();
        location.reload();
    });

    $(window.zeta).on(window.zeta.handlers.on_logout, function() {
        location.reload();
    });

    $(document).on('click.dropdown.data-api', '.authorizer li', function (e) {
        e.stopPropagation();
    });

    var authorizer = new root.Widget("authorizer", root.console.layout.position.layoutHeader, "right", { authonly: false, priority: 100, ready: function() {
        if (window.zeta.user != null) {
            if (window.zeta.user.getLogonName() != "") {
                m.show();
                if (window.zeta.user.getImpersonation()) {
                    deimp.show();
                    implogin.hide();
                }
            } else {
                f.show();
            }
        }
    }});
    authorizer.body = $('<div/>').append(f,m);
    root.console.RegisterWidget(authorizer);
}(window.jQuery);