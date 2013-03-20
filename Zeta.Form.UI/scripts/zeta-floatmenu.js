(function ($) {
    var FloatMenu = function(element, options) {
        this.element = $(element); // btn-group element
        this.options = options;
        this.floatbutton = null;
        this.button = null; // dropdown-toggle element
        this.menu = null; // dropdown-menu element
        this.init();
    };

    FloatMenu.prototype.init = function () {
        this.button = this.element.find('.dropdown-toggle');
        this.menu = this.element.find('.dropdown-menu');
        this.float();
    };

    FloatMenu.prototype.float = function() {
        this.floatbutton = $('<div class="floatmode"/>');
        this.floatbutton.click($.proxy(function() {
            this.floatbutton.toggleClass("active");
            this.menu.toggleClass("floating");
            this.button.toggleClass("active");
            if (this.menu.hasClass("ui-draggable")) {
                this.menu.draggable('destroy');
                this.menu.css({"top": "", "left": ""});
            } else {
                this.menu.draggable();
            }
        }, this));
        this.menu.prepend(this.floatbutton);
    };

    FloatMenu.prototype.unfloat = function() {
        this.floatbutton.remove();
        this.menu.removeClass("floating");
        this.button.removeClass("active");
        if (this.menu.hasClass("ui-draggable")) {
            this.menu.draggable('destroy');
            this.menu.css({"top": "", "left": ""});
        }
    };

    $.fn.floatmenu = function (options) {
        return this.each(function () {
            if (!$.data(this, "floatmenu")) {
                $.data(this, "floatmenu", new FloatMenu(this, $.extend({}, $.fn.floatmenu.defaults,options)));
            }
            if (typeof options == "string") {
                if (options.toLowerCase() == "destroy") {
                    $.data(this, "floatmenu").unfloat();
                    $.removeData(this, "floatmenu");
                }
            }
        });
    };

    $.fn.floatmenu.defaults = {

    };
})(jQuery);