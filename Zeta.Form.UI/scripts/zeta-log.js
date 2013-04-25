var root = window.zeta = window.zeta || {};

!function($) {
    var Log = function(opts) {

    };

    Log.prototype.init = function() {
        this.messages = [];
    };

    Log.prototype.Get = function() {
        return this.messages;
    };

    Log.prototype.W = function(m) {
        if (this.messages.length > 199) {
            this.messages.splice(0, 100);
        }
        this.messages.push(m)
    };

    root.userinfostorage = new Log(null);
}(jQuery);