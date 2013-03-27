var root = window.zeta = window.zeta || {};

!function($) {
    var options = window.zeta.options;

    var Storage = function() {

    };

    Storage.prototype.init = function() {

    };

    Storage.prototype.support = function() {
        try {
            return 'localStorage' in window && window['localStorage'] !== null;
        } catch (e) {
            return false;
        }
    };

    Storage.prototype.getUsers = function() {
        if (this.support()) {
            return JSON.parse(window.localStorage.getItem("ZetaUsers")) || {};
        }
        return null;
    };

    Storage.prototype.getUser = function(login) {
        if (this.support()) {
            var users = this.getUsers();
            if (!!users[login.toLowerCase()]) {
                return options.asUserInfo(users[login.toLowerCase()]);
            }
        }
        return null;
    };

    Storage.prototype.addUser = function(user) {
        if (this.support()) {
            var users = this.getUsers();
            users[user.getLogin().toLowerCase()] = users[user.getLogin().toLowerCase()] || {};
            $.extend(users[user.getLogin().toLowerCase()], user);
            window.localStorage.setItem("ZetaUsers", JSON.stringify(users));
            users = null;
        }
    };

    Storage.prototype.clear = function() {
        if (this.support()) {
            window.localStorage.setItem("ZetaUsers", {});
        }
    };

    root.storage = new Storage();
}(window.jQuery);