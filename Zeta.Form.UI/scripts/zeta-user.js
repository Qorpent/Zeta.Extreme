$.extend(window.zeta.handlers, {
    on_zetagetuserdetails : "getuserdetails"
});

(function ($) {
    var ZetaUser = function(element, options) {
        this.element = $(element);
        this.options = options;
        this.login = this.element.text().replace('/', '\\');
        this.details = {};
        this.init();
        this.element.click($.proxy(function() {
            this.showDetails();
        },this));
    };

    ZetaUser.prototype.init = function () {
        if (!!window.zeta.userinfostorage) {
            this.details = window.zeta.userinfostorage.Get()[this.login.toLowerCase()];
        }
        if (!this.details || $.isEmptyObject(this.details)) {
            this.getDetails();
        } else {
            this.loginToName();
        }
    };

    ZetaUser.prototype.loginToName = function() {
        if (this.details.ShortName !== "NOT REGISTERED IN DB") {
            this.element.text(this.details.ShortName);
        }
    };

    ZetaUser.prototype.showDetails = function() {
        var details = $('<table class="table table-bordered zetauserinfo"/>');
        details.append(
            $('<tr/>').append($('<td/>').text("Имя"), $('<td/>').text(this.details.Name)),
            $('<tr/>').append($('<td/>').text("Логин"), $('<td/>').text(this.details.Login)),
            $('<tr/>').append($('<td/>').text("Должность"), $('<td/>').text(this.details.Dolzh)),
            $('<tr/>').append($('<td/>').text("Контакты"), $('<td/>').text(this.details.Contact)),
            $('<tr/>').append($('<td/>').text("Email"), $('<td/>').text(this.details.Email)),
            $('<tr/>').append($('<td/>').text("Предприятие"), $('<td/>').text(this.details.ObjName))
        );
        var params = { title: "Информация о пользователе", content: details, width: 450 };
        var login = this.details.Login;
        if (zeta.user.getRealIsAdmin()) {
            params["customButton"] = {
                class : "btn-warning",
                text : "Вход от имени",
                click : function() {
                    if (!!login && login != "") zeta.api.security.impersonateall({Target: login});
                }
            }
        }
        $(window.zeta).trigger(window.zeta.handlers.on_modal, params);
    };

    ZetaUser.prototype.getDetails = function() {
        $.ajax({
            url: siteroot+window.zeta.api.metadata.userinfo.getUrl(),
            context: this,
            type: "POST",
            dataType: "json",
            data: { login : this.login }
        }).success($.proxy(function(d) {
            this.details = window.zeta.api.metadata.userinfo.wrap(d);
            this.loginToName();
            if (!!window.zeta.userinfostorage) {
                var user = {};
                user[this.details.Login.toLowerCase()] = this.details;
                window.zeta.userinfostorage.AddOrUpdate(user);
            }
        }, this));
    };

    $.fn.zetauser = function (options) {
        return this.each(function () {
            if (!$.data(this, "zetauser")) {
                $.data(this, "zetauser", new ZetaUser(this, $.extend({}, $.fn.zetauser.defaults,options)));
            }
        });
    };

    $.fn.zetauser.defaults = {

    };
})(jQuery);