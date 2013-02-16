(function ($) {
    var Zefs = function(element, options) {
        this.table = $(element);
        this.inputs = this.table.find("td.editable");
        this.options = options;
        this.init();
        this.hotkeysConfigure();
        this.getColIndex = function(cell) {
            var cell = $(cell);
            return cell.parent().children().index(cell);
        }
        this.getActiveRow = function() {
            var cell = this.getActiveCell();
            return cell.parent();
        }
        this.isOutScreen = function(cell) {
            if (cell.offset().top - window.pageYOffset < 0) return "top";
            else if (window.innerHeight + window.pageYOffset - cell.offset().top - cell.outerHeight() < 0) return "bottom"
            return "none";
/*
            return (cell.offset().top >= window.pageYOffset &&
                cell.offset().left >= window.pageXOffset &&
                cell.offset().top + cell.height() <= window.innerHeight + window.pageYOffset &&
                cell.offset().left + cell.width() <= window.innerWidth + window.pageXOffset);
*/
        }
    }

    Zefs.prototype.init = function () {
        $(document).on('click', $.proxy(function(e) {
            this.deactivateCell();
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
        $(window).resize($.proxy(function(e) {
            if ($(this.table.find("thead")).hasClass("fixed")) {
                this.unfixHeader();
                this.fixHeader();
            }
        },this));
    };

    Zefs.prototype.fixHeader = function() {
        $.each(this.table.find('th'), $.proxy(function(i,th) {
            $(th).css("width", $(th).width());
//            $(this.table.find("col")[i]).css("width", $(th).outerWidth());
        },this));
        $(this.table.find("thead")).addClass("fixed");
    }

    Zefs.prototype.unfixHeader = function() {
        $.each(this.table.find('th'), $.proxy(function(i,th) {
            $(th).css("width", "");
        },this));
        $(this.table.find("thead")).removeClass("fixed");
    }

    Zefs.prototype.activateCell = function($cell) {
        var $cell = $($cell);
        if (!$cell.hasClass("editable")) return $cell;
        if (this.isOutScreen($cell) == "top") {
            $(window).scrollTop($cell.offset().top);
        }
        if (this.isOutScreen($cell) == "bottom") {
            $(window).scrollTop($cell.offset().top + $cell.outerHeight() - window.innerHeight);
        }
        var $colindex = this.getColIndex($cell);
        var $col = $(this.table.find("col")[$colindex]);
        this.deactivateCell();
        $cell.parent().css("height", $cell.height());
//        $col.css("width", $(this.table.find("th")[$colindex]).outerWidth());
        $cell.css("min-width", $cell.width());
        $cell.css("height", $cell.height());
        $cell.addClass("active");
        $cell.number($cell.text(), 0, '', '');
        return $cell;
    }

    Zefs.prototype.deactivateCell = function(e) {
        $.each(this.getActiveCell(), $.proxy(function(i,td) {
            var $cell = $(td);
            $cell.removeClass("active");
            $(this.table.find("col")[this.getColIndex($cell)]).css("width", "");
            $cell.css("min-width", "");
            $cell.css("height", "");
            $cell.parent().css("height", "");
            $cell.number($cell.text(),0,'.',' ');
            if (!!e && e == "esc") {
                $cell.text($cell.data("h"));
            } else {
                $cell.data("h", $cell.text());
            }
        }, this));
    }

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
    }

    Zefs.prototype.prevCell = function() {
        var $cell = this.getActiveCell();
        var $index = $cell != null ? this.inputs.index($cell) : 1;
        this.activateCell(this.inputs[($index - 1 < 0) ? this.inputs.length - 1 : $index - 1]);
    }

    Zefs.prototype.rightCell = function() {
        var $cell = this.getActiveCell();
        var $filter = this.options.jumpNoneditable ? '.editable' : '';
        if ($cell.next($filter).length != 0) this.activateCell($cell.next($filter));
        else {
            if ($filter != '') this.nextCell();
        }
    }

    Zefs.prototype.leftCell = function() {
        var $cell = this.getActiveCell();
        var $filter = this.options.jumpNoneditable ? '.editable' : '';
        if ($cell.prev($filter).length != 0) this.activateCell($cell.prev($filter));
        else {
            if ($filter != '') this.prevCell();
        }
    }

    Zefs.prototype.downCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $next = $cell.parent().next();
        while (!$($next.children()[$colindex]).hasClass("editable")) {
            if ($next.next().length != 0) $next = $next.next();
            else return;
        }
        this.activateCell($next.children()[$colindex]);
    }

    Zefs.prototype.upCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $prev = $cell.parent().prev();
        while (!$($prev.children()[$colindex]).hasClass("editable")) {
            if ($prev.prev().length != 0) $prev = $prev.prev();
            else return;
        }
        this.activateCell($prev.children()[$colindex]);
    }

    Zefs.prototype.hotkeysConfigure = function () {
        $(document).on('keydown', $.proxy(function(e) {
            var k = e.keyCode;
            var ctrl = e.ctrlKey;
            var printable =
                (k > 47 && k < 58)   || // number keys
                    k == 32 || //k == 13   ||  spacebar & return key(s) (if you want to allow carriage returns)
                    (k > 64 && k < 91)   || // letter keys
                    (k > 95 && k < 112)  || // numpad keys
                    (k > 185 && k < 193) || // ;=,-./` (in order)
                    (k > 218 && k < 223);   // [\]' (in order)
            switch (k) {
                case 37 :
                    e.preventDefault();
                    this.leftCell();
                    break;
                case 38 :
                    e.preventDefault();
                    this.upCell();
                    break;
                case 39 :
                    e.preventDefault();
                    this.rightCell();
                    break;
                case 40 :
                    e.preventDefault();
                    this.downCell();
                    break;
                // Tab button
                case 9 :
                    e.preventDefault();
                    this.nextCell();
                    break;
                // Enter button
                case 13 :
                    e.preventDefault();
                    this.nextCell();
                    break;
                // Escape button
                case 27 :
                    e.preventDefault();
                    this.deactivateCell("esc");
                    break;
                default :
                    if (!printable) return;
                    var active = this.table.find("td.active");
                    active.text(active.text() + String.fromCharCode(k));
            }
        },this));
    };

    $.fn.zefs = function (options) {
        return this.each(function () {
            if (!$.data(this, "zefs")) {
                $.data(this, "zefs", new Zefs(this, $.extend({}, $.fn.zefs.defaults,options)));
            }
        });
    }

    $.fn.zefs.defaults = {
        jumpNoneditable : true // Перепрыгивать через нередактируемые ячейки по нажатию на UP, DOWN, LEFT, RIGHT
    };
})(jQuery);

$(function () {
    $.each($("table"), function(i, table) {
        $(table).zefs();
    });
});