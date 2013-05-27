!function ($) {
    "use strict";
    $.fn.editable = function (method) {
        var methods = {
            init:function () {
                this.each(function () {
                    var $source = $(this);
                    var $oldtext = $source.text();
                    $source.css('white-space','pre');
                    var $input = $('<input/>');
                    var $save = $('<div class="editable-f save"/>');
                    $save.click($.proxy(function(e) {
                        $source.trigger('change');
                        $source.editable('destroy');
                    }, this));
                    // Это чтобы по высоте подмена была не заметна
                    $input.css('height', $source.height());
                    $($input).attr('class', $($source).attr('class'));
                    $source.hide();
                    $source.after($input);
                    $input.val($source.text());
                    var autowidth = function() {
                        $source.text($input.val());
                        $input.css("width",$source.width());
                    }
                    $input.after($save);
                    autowidth();
                    $input.keydown(function(e) {
                        var k = e.keyCode;
                        var printable =
                            (k > 47 && k < 58)   || // number keys
                            k == 32 || //k == 13   ||  spacebar & return key(s) (if you want to allow carriage returns)
                            (k > 64 && k < 91)   || // letter keys
                            (k > 95 && k < 112)  || // numpad keys
                            (k > 185 && k < 193) || // ;=,-./` (in order)
                            (k > 218 && k < 223);   // [\]' (in order)
                        if (printable) $input.css("width",$source.width()+10);
                    });
                    $input.keyup(function(e) {
                        if (e.keyCode == 27) {
                            $input.val($oldtext);
                            $source.val($oldtext);
                            $input.blur();
                            $source.editable('destroy');
                        }
                        if (e.keyCode == 13) {
                            $input.blur();
                            $source.editable('apply');
                        }
                        autowidth();
                        if ($source.text() != $oldtext) {
                            $input.addClass("changed");
                        } else {
                            $input.removeClass("changed");
                        }
                    });
                    $input.focusin(function(e) {
                        $input.addClass('focused');
                    });
                    $input.focusout(function(e) {
                        $input.removeClass('focused');
                    });
                    $input.trigger('focus');
                });
            },
            destroy:function () {
                this.each(function () {
                    var $source = $(this);
                    var $input = $source.next('.editable');
                    var $apply = $input.next('.save');

                    $input.remove();
                    $apply.remove();
                    $source.show();
                });
            }
        };

        if (methods[method])
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        else if (typeof method === 'object' || !method)
            return methods.init.apply(this, arguments);
    };
}(jQuery);
