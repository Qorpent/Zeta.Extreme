(function ($) {
    var User = function(element, options) {
        this.element = $(element);
        this.options = options;
        this.login = this.element.text();
    };

    User.prototype.init = function () {

    };

    User.prototype.getDetails = function() {
        $.ajax({
            url: siteroot+window.zeta.options.getuserinfo_command,
            context: this,
            type: "POST",
            dataType: "json",
            data: { login : this.login }
        }).success($.proxy(function(d) {

        }));
    };

    $.fn.user = function (options) {
        return this.each(function () {
            if (!$.data(this, "user")) {
                $.data(this, "user", new User(this, $.extend({}, $.fn.user.defaults,options)));
            }
        });
    };

    $.fn.user.defaults = {

    };
})(jQuery);