var root = window.zeta = window.zeta || {};

!function($) {
    var Storage = function(opts) {
        // Определение текущего хранилища sessionStorage || localStorage
        this.location = window.sessionStorage;
        this.dependency = [];
        if (!$.isEmptyObject(opts)) {
            $.extend(this, opts);
        }
    };

    Storage.prototype.init = function() {

    };

    Storage.prototype.support = function() {
        try {
            return this.options.storage !== null;
        } catch (e) {
            return false;
        }
    };

    /**
     * Функция возвращает набор параметров (для перекрывания)
     * @returns {{}}
     */
    Storage.prototype.getParameters = function() {
        return {};
    };

    Storage.prototype.getCode = function() {
        var params = this.getParameters();
        var code = this.name;
        if (!$.isEmptyObject(params)) {
            code += $.map(this.dependency, function(type) { return "_"+type+":"+params[type] }).join('');
        }
        return code;
    };

    Storage.prototype.Get = function() {
        var result = {};
        var code = this.getCode();
        try {
            result = JSON.parse(this.location.getItem(code)) || {};
        } catch(e) {
            this.location.removeItem(this.name);
        }
        return result;
    };

    Storage.prototype.AddOrUpdate = function(newitem) {
        var item = this.Get();
        $.extend(item, newitem);
        this.location.setItem(this.getCode(), JSON.stringify(item));
    };

    Storage.prototype.Delete = function() {
        var item = this.Get();
        this.location.removeItem(item);
    };

    root.userinfostorage = new Storage({name: "__zeta_users"});
    root.chatoptionsstorage = new Storage({name: "__zeta_chatoptions"});
    root.temporarystorage = new Storage({name: "__zeta_temporary"});
    root.formoptionsstorage = $.extend(new Storage({name: "__zeta_formoptions", dependency: ["form"]}), {
        getParameters : function() {
            return { form: zefs.myform.currentSession.FormInfo.Code || "undefined" }
        }
    });
    root.coloptionsstorage = $.extend(new Storage({name: "__zeta_coloptions", dependency: ["period", "form"]}), {
        getParameters : function() {
            return {
                form: zefs.myform.currentSession.FormInfo.Code || "undefined",
                period: zefs.myform.currentSession.Period || 0
            }
        }
    });
}(jQuery);