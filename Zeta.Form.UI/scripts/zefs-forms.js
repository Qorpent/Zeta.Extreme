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
        this.table.delegate('span.collapser', "click", $.proxy(function(e) {
            var $c = $(e.target);
            if (e.ctrlKey) {
                var l = $c.parents('tr').attr("level");
                $('tr[level="' + l + '"] .collapser').trigger('click');
            } else {
                this.toggleChildRows($c.parent().parent());
            }
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
        // Бредовое поведение detected
        if (!!ts.activecell && !!ts.pageoffset) {
            if (ts.pageoffset.session == zefs.myform.sessionId) {
                this.activateCell($('td.data[id="' + ts.activecell + '"]'));
            } else {
                this.nextCell();
            }
        }
        if (!!ts.pageoffset) {
            if (ts.pageoffset.session == zefs.myform.sessionId) {
                $(window).scrollTop(ts.pageoffset.offset);
            }
        }
    };

    // Эта функция пока не нужна, так как шапка сейчас фиксируется по-умолчанию
    Zefs.prototype.isHeadOutScreen = function($e) {
        if ($e.offset().top - window.pageYOffset - this.options.fixHeaderX < 0) return "top";
        else if (window.innerHeight + window.pageYOffset - $e.offset().top - $e.outerHeight() < 0) return "bottom";
        return "none";
    };

    // Пока эти две функции отличаются только тем, что учитывают высоту хидера
    Zefs.prototype.isCellOutScreen = function($e) {
        if ($e.offset().top - window.pageYOffset - this.options.fixHeaderX - this.header.height() < 0) return "top";
        else if (window.innerHeight + window.pageYOffset - $e.offset().top - $e.outerHeight() < 0) return "bottom";
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
            var f = this.getNumberFormat($cell);
            $cell.number($cell.text(), f.dl, f.ds, '');
        }
    };

    // сколько знаков после запятой
    Zefs.prototype.getNumberFormat = function($cell) {
        $cell = $($cell);
        var f = { gs: " ", ds: ".", dl: 0 };
        if (!!$cell.data("format")) {
            f = $cell.data("format");
        }
        return f;
    };

    /**
     * Применяет к ячейке специальный чистоловой формат вида 1 000.00
     * @param $cell
     */
    Zefs.prototype.applyNumberFormat = function ($cell) {
        $cell = $($cell);
        if ($cell.text() != "") {
            var f = this.getNumberFormat($cell);
            $cell.number($cell.text(), f.dl, f.ds, f.gs);
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
        var $input = $('<input class="dataedit"/>').css("width", $cell.width()).val($val);
        $input.attr("placeholder", $old);
        if ($cell.attr("pattern")) {
            $input.attr("pattern", $cell.attr("pattern"));
        }
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

            // валидизация введенных в ячейку данных 
            if (!$input.attr("pattern") || $input.get(0).checkValidity()) {
                // реальное значение (без форматов и т.д.) для сохранения в базу
                $cell.data("value", $val);
                $cell.removeClass("invalid");
            } else {
                $cell.addClass("invalid");
            }
            $cell.text($val);
            $input.remove();
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
        var $next = $cell.nextAll('.editable:visible:first');
        if ($next.length != 0) {
            this.activateCell($next);
        } else {
            var $index = this.inputs.index($cell) + 1;
            while (this.inputs.length > $index) {
                $next = $(this.inputs.get($index));
                if ($next.is(':visible')) {
                    this.activateCell($next);
                    return;
                } else {
                    $index++;
                }
            }
            return;
        }
    };

    Zefs.prototype.prevCell = function() {
        var $cell = this.getActiveCell();
        var $index = $cell != null ? this.inputs.index($cell) : 1;
        this.activateCell(this.inputs[($index - 1 < 0) ? this.inputs.length - 1 : $index - 1]);
    };

    Zefs.prototype.rightCell = function() {
        var $cell = this.getActiveCell();
        var c = $cell.nextAll('.editable:visible:first');
        if (c.length != 0) {
            this.activateCell(c);
            c = null;
        }
        else {
            this.nextCell();
        }
    };

    Zefs.prototype.leftCell = function() {
        var $cell = this.getActiveCell();
        var c = $cell.prevAll('.editable:visible:first');
        if (c.length != 0) {
            this.activateCell(c);
            c = null;
        }
        else {
            this.prevCell();
        }
    };

    Zefs.prototype.downCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $next = $cell.parent().nextAll(':visible:first');
        while (!$($next.children()[$colindex]).hasClass("editable")) {
            var c = $next.nextAll(':visible:first');
            if (c.length != 0) $next = c;
            else return;
        }
        this.activateCell($next.children()[$colindex]);
    };

    Zefs.prototype.upCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $prev = $cell.parent().prevAll(':visible:first');
        while (!$($prev.children()[$colindex]).hasClass("editable")) {
            var c = $prev.prevAll(':visible:first');
            if (c.length != 0) $prev = c;
            else return;
        }
        this.activateCell($prev.children()[$colindex]);
    };

    Zefs.prototype.upFirstCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = $(".zefsform tbody tr:visible").first();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            var c = $current.nextAll(':visible:first');
            if (c.length != 0) {
                $current = c;
            }
            else return;
        }
        this.activateCell($current.children()[$colindex]);
    };

    Zefs.prototype.downLastCell = function() {
        var $cell = this.getActiveCell();
        var $colindex = this.getColIndex($cell);
        var $current = $(".zefsform tbody tr:visible").last();
        while (!$($current.children()[$colindex]).hasClass("editable")) {
            var c = $current.prevAll(':visible:first');
            if (c.length != 0) {
                $current = c;
            }
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
                // Del button
                case 46 :
                    e.preventDefault();
                    $cell.text("");
                    $cell.data("previous", "");
                    $cell.data("value", 0);
                    if ($cell.text() != $cell.data("history")) $cell.addClass("changed");
                    break;
                default :
                    if (!printable || e.ctrlKey || e.altKey) return;
                    if ((k > 47 && k < 58) || (k > 95 && k < 106) || (k == 190 || k == 188 || k == 110 || k != 189)) {
                        if (!$cell.hasClass('editing')) {
                            this.inputCell("replace");
                        }
                    } else {
                        e.preventDefault();
                    }
            }
        },this));
    };

    Zefs.prototype.getChanges = function() {
        this.uninputCell();
        var obj = {};
        var div = $('<div/>');
        var invaliddata = $('table.data td.changed.invalid');
        if (invaliddata.length > 0) {
            obj.hasinvaliddata = true;
        }
        $.each($('table.data td.changed'), function(i,cell) {
            cell = $(cell);
            div.number(cell.text(), 0, '', '');
            obj[i] = {id:cell.attr("id"), value:cell.data("value") || div.text(), ri:cell.attr("ri")};
            if (cell.hasClass("invalid")) {
                obj[i].invalid = true;
                obj[i].validaterule = cell.attr("validaterule") || "";
            }
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
        fixHeaderX : 100 // Позиция по Х на которой фиксируется шапка
    };
})(jQuery);

(function(form) {
$.extend(form, {
    init: function() {
        form.params = form.params || {};
        $.extend(form.params, this.defaults, this.getFormParameters());
    },

    structure : {},

    defaults: {
        form: "",
        obj: "",
        period: "",
        year: ""
    },

    isParamsValid: function(p) {
        p = p || this.params;
        if (!p.form || p.form == "") return false;
        else if (!p.obj || p.obj.toString() == "") return false;
        else if (!p.period || p.period.toString() == "") return false;
        else if (!p.year || p.year.toString() == "") return false;
        return true;
    },

    getFormParameters : function() {
        // Парсим параметры из хэша
        var h = location.hash;
        var p = {};
        var result = {};
        if (h == "") return null;
        if (!h.match(/^#form/)) return null;
        $.each(h.substring(1).split("|"), function(i,e) {
            p[e.split("=")[0]] = e.split("=")[1];
        });
        result["form"] = p["form"];
        result["obj"] = p["obj"];
        result["period"] = p["period"];
        result["year"] = p["year"];
        if (!!p.subobj) result["subobj"] = p["subobj"];
        return result;
    },

    open : function(params, blank) {
        params = $.extend(this.defaults, form.getFormParameters(), params);
        var hash = "";

        // контролим код формы. Выставляем А|B в зависимости от периода
        params.form = params.form.replace(/[A|B]\.in/, "");
        if (params.period) {
            var period = zefs.getperiodbyid(params.period);
            if (!!period) {
                params.form += period.type.match(/^Plan/) ? "B.in" : "A.in";
            }
        }
        $.each(params, function(k, v) { hash += k + "=" + (v || "") + "|" });
        hash = hash.substring(0, hash.length - 1);
        if (this.isParamsValid(params)) {
            if (blank) window.open("#" + hash, "_blank");
            else {
                location.hash = hash;
                location.reload();
            }
        } else {
            location.hash = hash;
        }
    }
});
})(window.form = window.form || {});

$(document).ready(function() {
    form.init();
});
