$.extend(window.zeta.handlers, {
    on_zetagetuserdetails : "getuserdetails"
});

(function ($) {
    var ZetaUser = function(element, options) {
        this.element = $(element);
        this.options = options;
        this.login = this.element.text();
        this.details = null;
        this.init();
        this.element.click($.proxy(function(e) {
            this.showDetails();
        },this));
    };

    ZetaUser.prototype.init = function () {
        if (!!window.zeta.storage) {
            this.details = window.zeta.storage.getUser(this.login);
        }
        if (!this.details) {
            this.getDetails();
        } else {
            this.loginToName();
        }
    };

    ZetaUser.prototype.loginToName = function() {
        this.element.text(this.details.getShortName());
    };

    ZetaUser.prototype.showDetails = function() {
        var details = $('<table class="table table-bordered zetauserinfo"/>');
        details.append(
            $('<tr/>').append($('<td/>').text("Имя"), $('<td/>').text(this.details.getName())),
            $('<tr/>').append($('<td/>').text("Должность"), $('<td/>').text(this.details.getJob())),
            $('<tr/>').append($('<td/>').text("Контакты"), $('<td/>').text(this.details.getContact())),
            $('<tr/>').append($('<td/>').text("Email"), $('<td/>').text(this.details.getEmail())),
            $('<tr/>').append($('<td/>').text("Предприятие"), $('<td/>').text(this.details.getObjName()))
        );
        $(window.zeta).trigger(window.zeta.handlers.on_modal, { title: "Информация о пользователе", content: details, width: 450 });
    };

    ZetaUser.prototype.getDetails = function() {
        $.ajax({
            url: siteroot+window.zeta.options.getuserinfo_command,
            context: this,
            type: "POST",
            dataType: "json",
            data: { login : this.login }
        }).success($.proxy(function(d) {
            this.details = window.zeta.options.asUserInfo(d);
            this.loginToName();
            if (!!window.zeta.storage) {
                window.zeta.storage.addUser(this.details);
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