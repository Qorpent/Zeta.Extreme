(function ($) {
    var Zefs = function(element, options) {
        this.table = $(element);
        this.header = $($('table.zefsform thead'));
        this.inputs = $("table.zefsform td.editable");
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
        this.getActiveCol = function() {
            var cell = this.getActiveCell();
            var colindex = this.getColIndex(cell);
            return $($("table.zefsform th")[colindex]);
        };
        var self = this;
        $.extend(zefs, {
            getChanges: function() { return self.getChanges(); },
            hidechildrows: function() { return self.toggleChildRows(); },
            restorelaststate: function() { self.restoreLastState(); }
        });
        $(zefs).on(zefs.handlers.on_savefinished, $.proxy(function() {
            this.applyChanges();
        },this));
        this.table.delegate('td.name', "click", $.proxy(function(e) {
            this.toggleChildRows($(e.target).parent());
        }, this));
    };

    Zefs.prototype.init = function () {
        $(document).on('click', $.proxy(function() {
            if (this.getActiveCell().hasClass('editing')) {
                this.uninputCell();
            }
        }, this));
        this.table.delegate('td.editable','click', $.proxy(function(e) {
            if (e.ctrlKey) {
                window.zefs.myform.cellhistory(e.target);
            }
            e.stopPropagation();
            this.activateCell(e.target);
        }, this));
        this.table.delegate('td.data','click', $.proxy(function(e) {
            if (!$(e.target).hasClass("editable")) {
                if (e.ctrlKey) {
                    window.zefs.myform.celldebug(e.target);
                }
            }
        }, this));

        // Фиксируем шапку сразу
        this.fixHeader();
        var timer;
        $(window).on('scroll', $.proxy(function () {
            clearTimeout(timer);
            timer = setTimeout(function() {
                var ts = zeta.temporarystorage.Get();
                ts["pageoffset"] = { offset: window.pageYOffset, session: zefs.myform.sessionId };
                zeta.temporarystorage.AddOrUpdate(ts);
            }, 1000);
        }, this));
        $(window).resize($.proxy(function() {
            if (this.header.hasClass("fixed")) {
                this.unfixHeader();
                this.fixHeader();
            }
        },this));
    };

    Zefs.prototype.restoreLastState = function() {
        var ts = zeta.temporarystorage.Get();
        if (!!ts["activecell"]) {
            if (ts.pageoffset.session == zefs.myform.sessionId) {
                this.activateCell($('td.data[id="' + ts.activecell + '"]'));
            } else {
                this.nextCell();
            }
        }
        if (!!ts["pageoffset"]) {
            if (ts.pageoffset.session == zefs.myform.sessionId) {
                $(window).scrollTop(ts.pageoffset.offset);
            }
        }
    };

    // Эта функция пока не нужна, так как шапка сейчас фиксируется по-умолчанию
    Zefs.prototype.isHeadOutScreen = function($e) {
        if ($e.offset().top - window.pageYOffset - this.options.fixHeaderX < 0) return "top";
        else if (window.innerHeight + window.pageYOffset - $e.offset().top - $e.outerHeight() < 0) return "bottom"
        return "none";
    };

    // Пока эти две функции отличаются только тем, что учитывают высоту хидера
    Zefs.prototype.isCellOutScreen = function($e) {
        if ($e.offset().top - window.pageYOffset - this.options.fixHeaderX - this.header.height() < 0) return "top";
        else if (window.innerHeight + window.pageYOffset - $e.offset().top - $e.outerHeight() < 0) return "bottom"
        return "none";
    };

    Zefs.prototype.fixHeader = function() {
        $.each($('table.zefsform th'), $.proxy(function(i,th) {
            $(th).css("width", $(th).width());
//          $($("table.zefsform col")[i]).css("width", $(th).outerWidth());
        },this));
        this.header.addClass("fixed");
        this.header.css("top", this.options.fixHeaderX);
        this.table.css("margin-top", this.options.fixHeaderX + this.header.height());
    };

    Zefs.prototype.unfixHeader = function() {
        $.each($('table.zefsform th'), $.proxy(function(i,th) {
            $(th).css("width", "");
        },this));
        this.header.removeClass("fixed");
        this.header.css("top", "");
        this.table.css("margin-top", this.options.fixHeaderX);
    };

    Zefs.prototype.clearNumberFormat = function($cell) {
        $cell = $($cell);
        if ($cell.text() != "") {
            $cell.number($cell.text(), this.getNumberFormat($cell), '.', '');
        }
    };

    // сколько знаков после запятой
    Zefs.prototype.getNumberFormat = function($cell) {
        $cell = $($cell);
        var d = 0;
        if (!!$cell.data("format")) {
            var format = $cell.data("format");
            d = format.substring(format.indexOf('.') > 0 ? format.indexOf('.') + 1 : format.length).length;
        }
        return d;
    };

    /**
     * Применяет к ячейке специальный чистоловой формат вида 1 000.00
     * @param $cell
     */
    Zefs.prototype.applyNumberFormat = function ($cell) {
        $cell = $($cell);
        if ($cell.text() != "") {
            $cell.number($cell.text(), this.getNumberFormat($cell), '.', ' ');
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
        var isoutofview = this.isCellOutScreen($cell);
        if (isoutofview == "top") {
            $(window).scrollTop($cell.offset().top - this.options.fixHeaderX - this.header.height());
        }
        else if (isoutofview == "bottom") {
            $(window).scrollTop($cell.offset().top + $cell.outerHeight() - window.innerHeight);
        }

        this.deactivateCell(null);
        $cell.parent().css("height", $cell.height());
//      $col.css("width", $($("table.zefsform th")[$colindex]).outerWidth());
        $cell.css("min-width", $cell.width());
        $cell.css("height", $cell.height());
        $cell.addClass("active");
        this.clearNumberFormat($cell);

        // store current active cell
        var ts = zeta.temporarystorage.Get();
        ts["activecell"] = $cell.attr("id");
        zeta.temporarystorage.AddOrUpdate(ts);

        this.rowcolHighlight();

        return $cell;
    };

    Zefs.prototype.rowcolHighlight = function() {
        $($("table.zefsform th.active")).removeClass("active");
        $($("table.zefsform tr.active")).removeClass("active");
        $($("table.zefsform tr.preactive")).removeClass("preactive");
        this.getActiveCol().addClass("active");
        this.getActiveRow().addClass("active");
        // небольшой изврат чтобы изменить цвет нижнего бордера у предыдущей tr
        if (this.getActiveRow().prev('tr').length != 0) {
            this.getActiveRow().prev('tr').addClass("preactive");
        }
    };

    Zefs.prototype.deactivateCell = function(e) {
        var $cell = this.getActiveCell();
        if (e != "esc" && $cell.hasClass('editing')) {
            this.uninputCell();
        }
        $cell.removeClass("active");
        $($("table.zefsform col")[this.getColIndex($cell)]).css("width", "");
        $cell.css("min-width", "");
        $cell.css("height", "");
        $cell.parent().css("height", "");
        this.applyNumberFormat($cell);
    };

    Zefs.prototype.inputCell = function($mode) {
        var $cell = this.getActiveCell();
        this.clearNumberFormat($cell);
        var $val = "";
        var $old = $cell.text();
        if ($mode == "edit") {
            $val = $old;
        }
        $cell.text("");
        var $input = $('<input class="dataedit"/>').attr("placeholder", $old).css("width", $cell.width()).val($val);
        $cell.append($input);
        $input.focus();
        $cell.addClass("editing");
        return $cell;
    };

    Zefs.prototype.uninputCell = function(e) {
        var $cell = this.getActiveCell();
        var $input = $($cell.find('input').first());
        if ($input.length != 0){
            // преобразование запятой в точку
            var $val = $input.val().replace(",",".");
            $input.remove();
            $cell.text($val);
            // реальное значение (без форматов и т.д.) для сохранения в базу
            $cell.data("value", $val);
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
        return $("td.active");
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
        if ($cell.nextAll('.editable:visible:first').length != 0) this.activateCell($cell.nextAll('.editable:visible:first'));
        else {
            this.nextCell();
        }
    };

    Zefs.prototype.leftCell = function() {
        var $cell = this.getActiveCell();
        if ($cell.prevAll('.editable:visible:first').length != 0) this.activateCell($cell.prevAll('.editable:visible:first'));
        else {
            this.prevCell();
        }
    };

    Zefs.prototype.downCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $next = $cell.parent().nextAll(':visible:first');
        while (!$($next.children()[$colindex]).hasClass("editable")) {
            if ($next.nextAll(':visible:first').length != 0) $next = $next.nextAll(':visible:first');
            else return;
        }
        this.activateCell($next.children()[$colindex]);
    };

    Zefs.prototype.upCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $prev = $cell.parent().prevAll(':visible:first');
        while (!$($prev.children()[$colindex]).hasClass("editable")) {
            if ($prev.prevAll(':visible:first').length != 0) $prev = $prev.prevAll(':visible:first');
            else return;
        }
        this.activateCell($prev.children()[$colindex]);
    };

    Zefs.prototype.upFirstCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = $("table.zefsform tbody>tr").first();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            if ($current.next().length != 0) $current = $current.next();
            else return;
        }
        this.activateCell($current.children()[$colindex]);
    };

    Zefs.prototype.downLastCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = $("table.zefsform tbody>tr").last();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            if ($current.prev().length != 0) $current = $current.prev();
            else return;
        }
        this.activateCell($current.children()[$colindex]);
    };

    Zefs.prototype.hotkeysConfigure = function () {
        $(document).on('keydown', $.proxy(function(e) {
            if ($(':focus').length != 0) {
                if (!$(':focus').hasClass("dataedit")) return;
            }
            var $cell = $(this.getActiveCell());
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
                    if ($cell.hasClass('editing')) return;
                    e.preventDefault();
                    this.leftCell();
                    break;
                // UP button
                case 38 :
                    if (e.ctrlKey) {
                        $(window).scrollTop(0);
                        this.upFirstCell();
                        return;
                    }
                    e.preventDefault();
                    this.upCell();
                    break;
                case 39 :
                    if ($cell.hasClass('editing')) return;
                    e.preventDefault();
                    this.rightCell();
                    break;
                case 40 :
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
                    if (e.ctrlKey && !$cell.hasClass('editing')) {
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
                // Del button
                case 46 :
                    e.preventDefault();
                    $cell.text("");
                    $cell.data("previous", "");
                    if ($cell.text() != $cell.data("history")) $cell.addClass("changed");
                // S button
                case 83 :
                    e.preventDefault();
                    this.uninputCell();
                    if (e.ctrlKey && e.shiftKey) {
                        zefs.myform.forcesave();
                    }
                    else if (e.ctrlKey) {
                        zefs.myform.save();
                    }
                    break;
                default :
                    if (!printable || e.ctrlKey || e.altKey) return;
                    if (!$cell.hasClass('editing')) {
                        this.inputCell("replace");
                    }
            }
        },this));
    };

    Zefs.prototype.getChanges = function() {
        this.uninputCell();
        var obj = {};
        var div = $('<div/>');
        $.each($('table.data td.changed'), function(i,cell) {
            cell = $(cell);
            div.number(cell.text(), 0, '', '');
            obj[i] = {id:cell.attr("id"), value:cell.data("value") || div.text(), ri:cell.attr("ri")};
        });
        return obj;
    };

    Zefs.prototype.getChildRows = function(e) {
        var $e = $(e);
        var $next = $(e).nextAll('tr').filter(function(){ return  $(this).attr("level") <= $e.attr("level") }).first();
        if ($next.length == 0) return $(e).nextAll('tr');
        var $result = $e.nextAll('tr:lt(' + ($next.index() - $e.index() - 1) + ')');
        if ($result.length == 0) {
            if ($next.attr("level") >= $e.attr("level")) return null;
            else return $e.nextAll('tr');
        }
        return $result;
    };

    Zefs.prototype.toggleChildRows = function(e) {
        var $childs = this.getChildRows($(e));
        if (null == $childs) return;
        var $collapsed = $childs.filter(function() { return $(this).hasClass("collapsed") });
        if ($collapsed.length != 0) {
            for (var i in $collapsed.toArray()) {
                $childs.splice($childs.index($collapsed[i]) + 1, this.getChildRows($collapsed[i]).length);
            }
        }
        $childs.toggle();
        $(e).toggleClass("collapsed");
        $(window).trigger("resize");
    };

    Zefs.prototype.applyChanges = function() {
        var div = $('<div/>');
        $.each($('table.data td.changed'), function(i,td) {
            td = $(td);
            div.number(td.text(), 0, '', '');
            td.data("history", div.text());
            td.data("previous", div.text());
            td.removeClass("changed");
        });
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
        fixHeaderX : 77 // Позиция по Х на которой фиксируется шапка
    };
})(jQuery);

/*
$(function () {
    $.each($("table"), function(i, table) {
        $(table).zefs();
    });
});
*/
