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

    Storage.prototype.tryGetItem = function(code) {
        var result = {};
        if (this.support()) {
            try {
                result = JSON.parse(this.options.storage.getItem(code));
            } catch(e) {
                this.options.storage.removeItem(code);
                return result;
            }
        }
        return result;
    };

    Storage.prototype.tryGetItemProp = function(code, name) {
        var result = this.tryGetItem(code);
        if (result != null && !$.isEmptyObject(result)) {
            return result[name] || {};
        }
        return null;
    };

    Storage.prototype.tryUpdateItem = function(code, newitem) {
        if (this.support()) {
            var item = this.tryGetItem(code) || {};
            $.extend(item, newitem);
            this.options.storage.setItem(code, JSON.stringify(item));
        }
    };

    /** ХРАНЕНИЕ ИНФОРМАЦИИ О ПОЛЬЗОВАТЕЛЯХ */

    Storage.prototype.getUsers = function() {
        return this.tryGetItem("ZetaUsers");
    };

    Storage.prototype.getUser = function(login) {
        return this.tryGetItemProp("ZetaUsers", login);
    };

    Storage.prototype.updateUser = function(newuser) {
        this.tryUpdateItem("ZetaUsers", newuser);
    };

    /** ХРАНЕНИЕ НАСТРОЕК ФОРМЫ */

    Storage.prototype.getFormOptions = function(code) {

    };

    Storage.prototype.updateFormOptions = function(newform) {
        this.tryUpdateItem("ZefsForms", newform);
    };

    root.localstorage = new Storage({ storage: window.localStorage });
    root.sessionstorage = new Storage();
}(window.jQuery);