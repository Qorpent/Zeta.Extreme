﻿(function(){
var root = window.zefs = window.zefs || {};
root.handlers = $.extend(root.handlers, {
    on_renderfinished : "renderfinished"
});
root.render = root.render || {};
$.extend(root.render, {
	renderStructure : function(session) {
		var table = $('table.data');
        if (table.length != 0) {
            table = $($('table.data').first());
        } else {
            table = $('<table class="data"/>');
            $('body').append(table);
        }
		session.table = table;
		var colgroup = $('<colgroup/>').append(
			$("<col/>").addClass("number"),
			$("<col/>").addClass("name")
		);
		if (session.NeedMeasure) {
			colgroup.append($("<col/>").addClass("measure"));
		}
		var thead = $('<thead/>').append($("<tr/>").append(
			$('<th class="number"/>').text("№"),
			$('<th class="name"/>').text("Наименование")
		));
		if (session.NeedMeasure) {
			$(thead.find("tr").first()).append($('<th class="measure"/>').text("Ед. изм."));
		}
		$.each(session.structure.cols, function(i,col) {
            colgroup.append($('<col class="data"/>').attr("idx",col.idx));
            var th = $('<th class="data"/>').attr({
                "idx": col.idx,
                "title": col.code + "," + col.period + "," + col.year
            }).text(col.name);
            if (col.isprimary) {
                th.addClass("primary");
            }
            thead.find("tr").append(th);
		});
		table.append(colgroup);
		table.append(thead);
	    //table.find("thead tr").first().append($("<th/>").text(col.name).data("src_column",col))
			var body = $("<tbody/>");
		if (session.NeedMeasure) {
			thead.find("thead").append($('<th class="measure"/>').text("Ед. изм."));
		}
		$.each(session.structure.rows, function(i,row) {
            var tr = $("<tr/>").attr("level",row.level);
            if (row.iscaption) tr.addClass("istitle");
            tr.append($('<td class="number"/>').attr("title", row.code).text(row.number || ""));
            var td = $('<td class="name"/>');
            if (row.haschilds) {
                tr.addClass("haschilds");
                td.html('<span class="collapser"/>');
            }
            td.html(td.html() + row.name);

            // Zefs Wiki button
            var wikibtn = $('<span class="wikirowhelp notexist"/>').hide();
            var wikicode = '/row/' + row.code + '/default';
            wikibtn.click(function() {
                zefs.myform.wikiget(row.code);
            });
            wikibtn.attr("id", 'wiki__row_' + row.code + '_default');
            wikibtn.attr("code", wikicode);
            td.append(wikibtn);


            if (row.iscaption) {
                tr.append(td.attr("colspan", "100"));
            } else {
                tr.append(td);
                if (session.NeedMeasure) {
                    tr.append($('<td class="measure"/>').text(row.measure));
                }
                $.each(session.structure.cols, function(i,col) {
                    var td = $('<td class="data notloaded"/>').attr({
                        "id": row.idx + ":" + col.idx,
                        "idx": col.idx,
                        "visible": "visible"
                    });
                    if (col.controlpoint && row.controlpoint) td.addClass("control");
                    if (col.isprimary && row.isprimary) {
                        td.addClass("editable");
                        if (!!col.validate) {
                            td.attr("pattern", col.validate);
                            td.attr("validaterule", col.validateReadable);
                        }
                    }
                    if (col.exref && row.exref) td.removeClass("editable");
                    if (!$.isEmptyObject(row.activecols)) {
                        if ($.map(row.activecols, function(e) { if (e == col.code) return e }).length == 0) {
                            td.removeClass("editable");
                        }
                    }
                    if (col.format != null && col.format != "") td.data("format", col.format);
                    if (row.format != null && row.format != "") td.data("format", row.format);
                    tr.append(td);
                });
            }
			body.append(tr);
		});

		table.append(body);

        $(root).trigger(root.handlers.on_renderfinished, table);

		session.wasRendered = true;
		return session;
	},

	updateCells : function(session,batch){
		var tbody = $(session.table).find("tbody").first();
        var div = $('<div/>'); // Это контейнер для форматирования чисел :)
		$.each(batch.data, function(i,b) {
            var $cell = $("td[id='" + b.i +  "']");
            if (!!b.c) $cell.data("cellid", b.c);
            var val = b.v || "";
            root.render.updateData($cell, val);
            // если в ячейке произошла ошибка
            if (!!b.iserror) {
                $cell.text("");
                $cell.addClass("errordata");
            }
            $cell.attr("ri", b.ri);
            if (val.search(/\./) != -1 && val.search("error") == -1) {
                $cell.addClass("rounded");
                $cell.tooltip({title: val, placement: 'top', container: $('body')});
            }
            if (val != "") {
                $cell.addClass("hasdata");
            }
		});
		batch.wasFilled = true;
		return session;
	},

    updateData : function(cell, val) {
        var $cell = $(cell);
        // сколько знаков после запятой
        var f = { gs: " ", ds: ".", dl: 0 };
        if (!!$cell.data) {
            if (!!$cell.data("format")) {
                f = $cell.data("format");
            }
        }
        $cell.number($cell.text(), f.dl, f.ds, '');
        if (parseFloat($cell.text()) != parseFloat(val).toFixed(f.dl) && !!$cell.data("history")) {
            $cell.addClass("recalced");
        }
        $cell.number(val, f.dl, f.ds, '');
        if (val == "0") {
            if (!$cell.data("cellid") || !$cell.hasClass("editable")) val = "";
            $cell.text(val);
        } else {
            $cell.number(val, f.dl, f.ds, f.gs);
        }
        $cell.data("history", val);
        $cell.data("previous", val);
        // реальное число без форматов, которое должно сохраняться в базу
        $cell.data("value", val);

        $cell.removeClass("notloaded");
    },

    updateNullCells : function() {
        var notloaded = $('td.notloaded');
        $.each(notloaded, function(i,cell) {
            root.render.updateData(cell, "0");
        });
    },

    checkValue : function(rule, value) {
        switch (rule.Action) {
            case "<>":
                return value != rule.Value;
            case "!=":
                return value != rule.Value;
            case "=":
                return value == rule.Value;
            case "==":
                return value == rule.Value;

            case "|<>|":
                return Math.abs(value) != Math.abs(rule.Value);
            case "|!=|":
                return Math.abs(value) != Math.abs(rule.Value);
            case "|=|":
                return Math.abs(value) == Math.abs(rule.Value);
            case "|==|":
                return Math.abs(value) == Math.abs(rule.Value);

            case "~<>":
                return Math.abs((((rule.Value - value)/rule.Value))*100) > 5;
            case "~!=":
                return Math.abs((((rule.Value - value)/rule.Value))*100) > 5;
            case "~=":
                return Math.abs((((rule.Value - value) / rule.Value)) * 100) <= 5;
            case "~==":
                return Math.abs((((rule.Value - value) / rule.Value)) * 100) <= 5;

            case "~|<>|":
                return Math.abs((((Math.abs(rule.Value) - Math.abs(value)) / Math.abs(rule.Value))) * 100) > 5;
            case "~|!=|":
                return Math.abs((((Math.abs(rule.Value) - Math.abs(value)) / Math.abs(rule.Value))) * 100) > 5;
            case "~|=|":
                return Math.abs((((Math.abs(rule.Value) - Math.abs(value)) / Math.abs(rule.Value))) * 100) <= 5;
            case "~|==|":
                return Math.abs((((Math.abs(rule.Value) - Math.abs(value)) / Math.abs(rule.Value))) * 100) <= 5;

            case ">=":
                return value >= rule.Value;
            case ">":
                return value > rule.Value;
            case "<=":
                return value <= rule.Value;
            case "<":
                return value < rule.Value;

            case "|>=|":
                return Math.abs(value) >= Math.abs(rule.Value);
            case "|>|":
                return Math.abs(value) > Math.abs(rule.Value);
            case "|<|":
                return Math.abs(value) < Math.abs(rule.Value);
            case "|<=|":
                return Math.abs(value) <= Math.abs(rule.Value);

            case "<->":
                return value > rule.Value && value < rule.Value2;
            case "<=->":
                return value >= rule.Value && value < rule.Value2;
            case "<=-=>":
                return value >= rule.Value && value <= rule.Value2;
            case "<-=>":
                return value > rule.Value && value <= rule.Value2;

            case "|<->|":
                return Math.abs(value) > Math.abs(rule.Value) && Math.abs(value) < Math.abs(rule.Value2);
            case "|<=->|":
                return Math.abs(value) >= Math.abs(rule.Value) && Math.abs(value) < Math.abs(rule.Value2);
            case "|<=-=>|":
                return Math.abs(value) >= Math.abs(rule.Value) && Math.abs(value) <= Math.abs(rule.Value2);
            case "|<-=>|":
                return Math.abs(value) > Math.abs(rule.Value) && Math.abs(value) <= Math.abs(rule.Value2);
        }
        return false;
    },

    checkConditions : function() {
//      $.map(zefs.myform.currentSession.structure.cols, function(e) { if (e.idx == 3) return e })
        var cols = zefs.myform.currentSession.structure.cols;
        $.each(cols, function(i, col) {
            if (!$.isEmptyObject(col.rules)) {
                $.each($('td[idx="' + col.idx + '"].hasdata'), function(i,cell) {
                    var $c = $(cell);
                    $.each(col.rules, function(i, rule) {
                        if (root.render.checkValue(rule, $c.text())) {
                            if (!!rule.CellStyle) $c.attr("style", rule.CellStyle);
                            if (!!rule.RowStyle) $c.parent().attr("style", rule.RowStyle);
                        }
                    });
                });
            }
        });
    }
	});
})();