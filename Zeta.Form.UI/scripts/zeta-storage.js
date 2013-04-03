var root = window.zeta = window.zeta || {};

!function($) {
    var Storage = function(opts) {
        this.init();
        if(!$.isEmptyObject(opts)) {
            $.extend(this.options, opts);
        }
    };

    Storage.prototype.init = function() {
        this.options = this.options || {
            storage : window.sessionStorage
        };
    };

    Storage.prototype.support = function() {
        try {
            return this.options.storage !== null;
        } catch (e) {
            return false;
        }
    };

    Storage.prototype.tryGetItem = function(item) {
        var result = {};
        if (this.support()) {
            try {
                result = JSON.parse(this.options.storage.getItem(item));
            } catch(e) {
                this.options.storage.removeItem(item);
                return result;
            }
        }
        return null;
    };

    Storage.prototype.getUsers = function() {
        return this.tryGetItem("ZetaUsers");
    };

    Storage.prototype.getUser = function(login) {
        var users = this.getUsers();
        if (!!users[login.toLowerCase()]) {
            return users[login.toLowerCase()];
        }
    };

    Storage.prototype.addUser = function(user) {
        if (this.support()) {
            var users = this.getUsers();
            users[user.Login.toLowerCase()] = users[user.Login.toLowerCase()] || {};
            $.extend(users[user.Login.toLowerCase()], user);
            this.options.storage.setItem("ZetaUsers", JSON.stringify(users));
            users = null;
        }
    };

    root.localstorage = new Storage({ storage: window.localStorage });
    root.sessionstorage = new Storage();
}(window.jQuery);