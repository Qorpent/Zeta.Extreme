(function ($) {
    var Zefs = function(element, options) {
        this.table = $(element);
        this.header = $(this.table.find('thead'));
        this.inputs = this.table.find("td.editable");
        this.options = options;
        this.init();
        this.hotkeysConfigure();
        this.getColIndex = function($cell) {
            $cell = $($cell);
            return $cell.parent().children().index($cell);
        };
        this.getActiveRow = function() {
            var cell = this.getActiveCell();
            return cell.parent();
        };

        this.nextCell();
    };

    Zefs.prototype.init = function () {
        $(document).on('click', $.proxy(function(e) {
            if (this.getActiveCell().hasClass('editing')) {
                this.uninputCell();
            }
        }, this));
        this.table.delegate('td.editable','click', $.proxy(function(e) {
            e.stopPropagation();
            this.activateCell(e.target);
        }, this));
        $(window).scroll($.proxy(function(e) {
            var theadScreenOut = this.isOutScreen(this.table);
            if (theadScreenOut == "top") {
                if (!$(this.table.find("thead")).hasClass("fixed")) this.fixHeader();
            }
            else this.unfixHeader();
        },this));
        $(window).resize($.proxy(function() {
            if ($(this.table.find("thead")).hasClass("fixed")) {
                this.unfixHeader();
                this.fixHeader();
            }
        },this));
    };

    Zefs.prototype.isOutScreen = function($e) {
        if ($e.offset().top - window.pageYOffset - this.header.height() < 0) return "top";
        else if (window.innerHeight + window.pageYOffset - $e.offset().top - $e.outerHeight() < 0) return "bottom"
        return "none";
        /*return (cell.offset().top >= window.pageYOffset &&
         cell.offset().left >= window.pageXOffset &&
         cell.offset().top + cell.height() <= window.innerHeight + window.pageYOffset &&
         cell.offset().left + cell.width() <= window.innerWidth + window.pageXOffset);*/
    };

    Zefs.prototype.fixHeader = function() {
        $.each(this.table.find('th'), $.proxy(function(i,th) {
            $(th).css("width", $(th).width());
//            $(this.table.find("col")[i]).css("width", $(th).outerWidth());
        },this));
        $(this.table.find("thead")).addClass("fixed");

    };

    Zefs.prototype.unfixHeader = function() {
        $.each(this.table.find('th'), $.proxy(function(i,th) {
            $(th).css("width", "");
        },this));
        $(this.table.find("thead")).removeClass("fixed");
    };

    Zefs.prototype.clearNumberFormat = function($cell) {
        $cell = $($cell);
        if ($cell.text() != "") {
            $cell.number($cell.text(), 0, '', '');
        }
    };

    /**
     * Применяет к ячейке специальный чистоловой формат вида 1 000.00
     * @param $cell
     */
    Zefs.prototype.applyNumberFormat = function ($cell) {
        $cell = $($cell);
        if ($cell.text() != "") {
            $cell.number($cell.text(), 0, '.', ' ');
        }
    };

    Zefs.prototype.rollbackValue = function() {
        var $cell = this.getActiveCell();
        this.clearNumberFormat($cell);
        $cell.data("previous",$cell.text());
        if ($cell.data("history") != "") {
            $cell.text($cell.data("history"));
            this.applyNumberFormat($cell);
        } else {
            $cell.text("");
        }
        $cell.removeClass("changed");
        return $cell;
    };

    Zefs.prototype.activateCell = function($cell) {
        var $cell = $($cell);
        if (!$cell.hasClass("editable")) return $cell;
        if ($cell.hasClass("active")) return $cell;
        var isoutofview = this.isOutScreen($cell);
        if (isoutofview == "top") {
            $(window).scrollTop($cell.offset().top - this.header.height());
        }
        else if (isoutofview == "bottom") {
            $(window).scrollTop($cell.offset().top + $cell.outerHeight() - window.innerHeight);
        }
        var $colindex = this.getColIndex($cell);
        var $col = $(this.table.find("col")[$colindex]);
        this.deactivateCell(null);
        $cell.parent().css("height", $cell.height());
//      $col.css("width", $(this.table.find("th")[$colindex]).outerWidth());
        $cell.css("min-width", $cell.width());
        $cell.css("height", $cell.height());
        $cell.addClass("active");
        this.clearNumberFormat($cell);
        return $cell;
    };

    Zefs.prototype.deactivateCell = function(e) {
        $.each(this.getActiveCell(), $.proxy(function(i,td) {
            var $cell = $(td);
            if (e != "esc" && $cell.hasClass('editing')) {
                this.uninputCell();
            }
            $cell.removeClass("active");
            $(this.table.find("col")[this.getColIndex($cell)]).css("width", "");
            $cell.css("min-width", "");
            $cell.css("height", "");
            $cell.parent().css("height", "");
            this.applyNumberFormat($cell);
        }, this));
    };

    Zefs.prototype.inputCell = function($mode) {
        var $cell = this.getActiveCell();
        this.clearNumberFormat($cell);
        var $val = "";
        if ($mode == "edit") {
            $val = $cell.text();
        }
        $cell.text("");
        var $input = $('<input/>').css("width", $cell.width()).val($val);
        $cell.append($input);
        $input.focus();
        $cell.addClass("editing");
        return $cell;
    };

    Zefs.prototype.uninputCell = function(e) {
        var $cell = this.getActiveCell();
        var $input = $($cell.find('input').first());
        if ($input.length != 0){
            var $val = $input.val();
            $input.remove();
            $cell.text($val);
        }
        if (!!e && e == "esc") {
            $cell.text($cell.data("previous"));
        } else {
            $cell.data("previous", $cell.text());
        }
        $cell.removeClass("editing");
        this.clearNumberFormat($cell);
        if ($cell.text() != $cell.data("history")) $cell.addClass("changed");
        else $cell.removeClass("changed");
        return $cell;
    };

    Zefs.prototype.getActiveCell = function() {
        return $(this.table.find("td.active"));
    };

    Zefs.prototype.nextCell = function() {
        var $cell = this.getActiveCell();
        var $index = -1;
        if ($cell.length != 0) {
            $index = this.inputs.index($cell);
            if ($index == -1) {
                $index = this.inputs.index($cell.nextAll('.editable').first()) - 1;
            }
        }
        this.activateCell(this.inputs[($index + 1 == this.inputs.length) ? 0 : $index + 1]);
    };

    Zefs.prototype.prevCell = function() {
        var $cell = this.getActiveCell();
        var $index = $cell != null ? this.inputs.index($cell) : 1;
        this.activateCell(this.inputs[($index - 1 < 0) ? this.inputs.length - 1 : $index - 1]);
    };

    Zefs.prototype.rightCell = function() {
        var $cell = this.getActiveCell();
        var $filter = this.options.jumpNoneditable ? '.editable' : '';
        if ($cell.next($filter).length != 0) this.activateCell($cell.next($filter));
        else {
            if ($filter != '') this.nextCell();
        }
    };

    Zefs.prototype.leftCell = function() {
        var $cell = this.getActiveCell();
        var $filter = this.options.jumpNoneditable ? '.editable' : '';
        if ($cell.prev($filter).length != 0) this.activateCell($cell.prev($filter));
        else {
            if ($filter != '') this.prevCell();
        }
    };

    Zefs.prototype.downCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $next = $cell.parent().next();
        while (!$($next.children()[$colindex]).hasClass("editable")) {
            if ($next.next().length != 0) $next = $next.next();
            else return;
        }
        this.activateCell($next.children()[$colindex]);
    };

    Zefs.prototype.upCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $prev = $cell.parent().prev();
        while (!$($prev.children()[$colindex]).hasClass("editable")) {
            if ($prev.prev().length != 0) $prev = $prev.prev();
            else return;
        }
        this.activateCell($prev.children()[$colindex]);
    };

    Zefs.prototype.upFirstCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = this.table.find("tbody>tr").first();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            if ($current.next().length != 0) $current = $current.next();
            else return;
        }
        this.activateCell($current.children()[$colindex]);
    };

    Zefs.prototype.downLastCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = this.table.find("tbody>tr").last();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            if ($current.prev().length != 0) $current = $current.prev();
            else return;
        }
        this.activateCell($current.children()[$colindex]);
    };

    Zefs.prototype.hotkeysConfigure = function () {
        $(document).on('keydown', $.proxy(function(e) {
            var k = e.keyCode;
            var printable =
                (k > 47 && k < 58)   || // number keys
                    k == 32 || //k == 13   ||  spacebar & return key(s) (if you want to allow carriage returns)
                    (k > 64 && k < 91)   || // letter keys
                    (k > 95 && k < 112)  || // numpad keys
                    (k > 185 && k < 193) || // ;=,-./` (in order)
                    (k > 218 && k < 223);   // [\]' (in order)
            switch (k) {
                case 37 :
                    if ($(this.getActiveCell()).hasClass('editing')) return;
                    e.preventDefault();
                    this.leftCell();
                    break;
                // UP button
                case 38 :
                    if ($(this.getActiveCell()).hasClass('editing')) return;
                    if (e.ctrlKey) {
                        $(window).scrollTop(0);
                        this.upFirstCell();
                        return;
                    }
                    e.preventDefault();
                    this.upCell();
                    break;
                case 39 :
                    if ($(this.getActiveCell()).hasClass('editing')) return;
                    e.preventDefault();
                    this.rightCell();
                    break;
                case 40 :
                    if ($(this.getActiveCell()).hasClass('editing')) return;
                    if (e.ctrlKey) {
                        $(window).scrollTop(this.table.offset().top + this.table.height());
                        this.downLastCell();
                        return;
                    }
                    e.preventDefault();
                    this.downCell();
                    break;
                // F2 button
                case 113 :
                    e.preventDefault();
                    this.inputCell("edit");
                    break;
                // Backspace button
                case 8 :
                    if (e.ctrlKey && !$(this.getActiveCell()).hasClass('editing')) {
                        this.rollbackValue();
                    }
                    break;
                // Tab button
                case 9 :
                    e.preventDefault();
                    this.nextCell();
                    break;
                // Enter button
                case 13 :
                    e.preventDefault();
                    this.downCell();
                    break;
                // Escape button
                case 27 :
                    e.preventDefault();
                    this.uninputCell("esc");
                    break;
                default :
                    if (!printable) return;
                    if (!$(this.getActiveCell()).hasClass('editing')) {
                        this.inputCell("replace");
                    }
            }
        },this));
    };

    $.fn.zefs = function (options) {
        return this.each(function () {
            if (!$.data(this, "zefs")) {
                $.data(this, "zefs", new Zefs(this, $.extend({}, $.fn.zefs.defaults,options)));
            }
        });
    };

    $.fn.zefs.defaults = {
        jumpNoneditable : true, // Перепрыгивать через нередактируемые ячейки по нажатию на UP, DOWN, LEFT, RIGHT
        fixHeaderX : 50 // Позиция по Х на которой фиксируется шапка
    };
})(jQuery);

/*
$(function () {
    $.each($("table"), function(i, table) {
        $(table).zefs();
    });
});
*/
