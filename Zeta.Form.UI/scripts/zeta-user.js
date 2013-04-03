$.extend(window.zeta.handlers, {
    on_zetagetuserdetails : "getuserdetails"
});

(function ($) {
    var ZetaUser = function(element, options) {
        this.element = $(element);
        this.options = options;
        this.login = this.element.text();
        this.details = {};
        this.init();
        this.element.click($.proxy(function() {
            this.showDetails();
        },this));
    };

    ZetaUser.prototype.init = function () {
        if (!!window.zeta.sessionstorage) {
            this.details = window.zeta.sessionstorage.getUser(this.login);
        }
        if (!this.details || $.isEmptyObject(this.details)) {
            this.getDetails();
        } else {
            this.loginToName();
        }
    };

    ZetaUser.prototype.loginToName = function() {
        this.element.text(this.details.ShortName);
    };

    ZetaUser.prototype.showDetails = function() {
        var details = $('<table class="table table-bordered zetauserinfo"/>');
        details.append(
            $('<tr/>').append($('<td/>').text("Имя"), $('<td/>').text(this.details.Name)),
            $('<tr/>').append($('<td/>').text("Должность"), $('<td/>').text(this.details.Dolzh)),
            $('<tr/>').append($('<td/>').text("Контакты"), $('<td/>').text(this.details.Contact)),
            $('<tr/>').append($('<td/>').text("Email"), $('<td/>').text(this.details.Email)),
            $('<tr/>').append($('<td/>').text("Предприятие"), $('<td/>').text(this.details.ObjName))
        );
        $(window.zeta).trigger(window.zeta.handlers.on_modal, { title: "Информация о пользователе", content: details, width: 450 });
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
            if (!!window.zeta.sessionstorage) {
                var user = {};
                user[this.details.Login.toLowerCase()] = this.details;
                window.zeta.sessionstorage.updateUser(user);
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