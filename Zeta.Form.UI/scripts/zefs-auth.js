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

    var authorize = function() {
        root.console.authorize(l.val(), p.val());
    }

    $(window.zeta).on(window.zeta.handlers.on_loginsuccess, function() {
        window.zeta.console.whoami();
        f.hide();
    });

    $(window.zeta).on(window.zeta.handlers.on_deimpersonate, function() {
        location.reload();
    });

    $(window.zeta).on(window.zeta.handlers.on_impersonate, function() {
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
            if (zeta.user.getLogonName() == "") {
                f.show();
            }
        }
    }});
    authorizer.body = $('<div/>').append(f);
    root.console.RegisterWidget(authorizer);
}(window.jQuery);