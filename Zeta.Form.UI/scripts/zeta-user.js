/*$.extend(window.zeta.handlers, {
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

*/

var ZetaUser = (function() {
    var instance;
    var zu = function(options) {
    };

    $.extend(zu.prototype, {
        defaultDetails: {
            Name: "",
            Login: "",
            Dolzh: "",
            Contact: "",
            Email: "",
            ObjName: "",
            ShortName: ""
        },

        getStorage: function() {
            return window.zeta.userinfostorage;
        },

        showDetails: function(input) {
            var details;
            if (typeof input == "string") {
                details = this.getDetails(input, $.proxy(function(result) {
                    this.renderDetails(result);
                }, this));
            }
            else if (typeof input == "object") {
                this.renderDetails(input);
            }
        },

        renderDetails: function(details) {
            var result = $('<table class="table table-bordered zetauserinfo"/>');
            result.append(
                $('<tr/>').append($('<td/>').text("Имя"), $('<td/>').text(details.Name)),
                $('<tr/>').append($('<td/>').text("Логин"), $('<td/>').text(details.Login)),
                $('<tr/>').append($('<td/>').text("Должность"), $('<td/>').text(details.Dolzh)),
                $('<tr/>').append($('<td/>').text("Контакты"), $('<td/>').text(details.Contact)),
                $('<tr/>').append($('<td/>').text("Email"), $('<td/>').text(details.Email)),
                $('<tr/>').append($('<td/>').text("Предприятие"), $('<td/>').text(details.ObjName))
            );
            var params = { title: "Информация о пользователе", content: result, width: 450 };
            var login = details.Login;
            if (zeta.user.getRealIsAdmin()) {
                params["customButton"] = {
                    class : "btn-warning",
                    text : "Вход от имени",
                    click : function() {
                        if (!!login && login != "") zeta.api.security.impersonateall({Target: details.login});
                    }
                }
            }
            $(window.zeta).trigger(window.zeta.handlers.on_modal, params);
        },

        getDetails: function(login, callback) {
            login = login.toLowerCase();
            var storage = this.getStorage().Get();
            if (!storage) return;
            var result = {};
            if (typeof login == "object") {
                // принимаем только массивы
                if (!!login.length) return;
                login = login.join(",");
            }
            // если логин не один
            if (login.indexOf(",") != -1) {
                logins = this.filterUniqueNotExistedLogins(login);
                if (logins == "") {
                    this.returnLogins(login, callback);
                } else {
                    this.loadDetails(logins, $.proxy(function() {
                        this.returnLogins(login, callback);
                    }, this));
                }
            } else {
                if (!!storage[login]) {
                    this.returnLogin(login, callback);
                } else {
                    this.loadDetails(login, $.proxy(function() {
                        this.returnLogin(login, callback);
                    }, this));
                }
            }            
        },

        returnLogins: function(logins, callback) {
            if (null == callback) return;
            var result = {};
            $.each(logins.split(","), $.proxy(function(i, l) {
                result[l] = this.getStorage().Get()[l];
            }, this));
            result.Get = $.proxy(function(login) {
                var result = this[login.toLowerCase()];
                return result;
            }, result);
            callback(result);
        },

        returnLogin: function(login, callback) {
            if (null == callback) return;
            var result = this.getStorage().Get()[login];
            result.Get = $.proxy(function() {
                return this;
            }, result);
            callback(result);
        },

        loadDetails: function(login, callback) {
            var ajax = {
                url: siteroot + window.zeta.api.metadata.userinfo.getUrl(), type: "POST", dataType: "json"
            };
            if (typeof login == "object") {
                ajax.data = { login : login.join(",") };
            }
            else if (typeof login == "string") {
                if (login.indexOf(",") != -1) {
                    login = this.filterUniqueNotExistedLogins(login);
                }
                ajax.data = { login : login };
            }
            $.ajax(ajax).success($.proxy(function(result, e) {
                var details = this.detailsSuccess(result);
                if (!!callback) callback(details);
            }, this));
        },

        filterUniqueNotExistedLogins: function(logins) {
            return $.grep($.unique(logins.split(",")), $.proxy(function(l, i) {
                return !this.isLoginExist(l);
            }, this)).join(",");
        },

        isLoginExist: function(login) {
            login = login.toLowerCase();
            var storage = this.getStorage().Get();
            return null != storage[login];
        },

        processDetails: function(details) {
            details = window.zeta.api.metadata.userinfo.wrap(details);
            if (!!this.getStorage().Get()) {
                var result = {};
                result[details.Login.toLowerCase()] = details;
                this.getStorage().AddOrUpdate(result);
            }
            return details;
        },

        detailsSuccess: function(details) { 
            if (!!details[0]) {
                var result = {};
                $.each(details, $.proxy(function(i, d) {
                    result[i] = this.processDetails(d);
                }, this));
                return result;
            } else {
                return this.processDetails(details);
            }
        }
    });

    return {
        getInstance: function(options) {
            if (!instance) {
                instance = new zu(options);
            }
            return instance;
        }
    }
})();

window.zeta = window.zeta || {};
zeta.zetauser = ZetaUser.getInstance();