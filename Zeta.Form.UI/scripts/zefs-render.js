(function(){
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
                "title": col.code + " " + col.year + " " + col.period
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
            if (row.childrens.length != 0) tr.addClass("haschild");
            var td = $('<td class="name"/>').text(row.name);
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
                    if (col.isprimary && row.isprimary) td.addClass("editable");
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
            var val = b.v || "";
            // сколько знаков после запятой
            var d = 0;
            if (!!$cell.data("format")) {
                var format = $cell.data("format");
                d = format.substring(format.indexOf('.') > 0 ? format.indexOf('.') + 1 : format.length).length;
            }
            $cell.number($cell.text(),d,'.','');
            if (parseFloat($cell.text()) != parseFloat(val).toFixed(d) && !!$cell.data("history")) {
                $cell.addClass("recalced");
            }
            if (val == "0") {
                if (b.c == undefined || !$cell.hasClass("editable")) val = "";
                $cell.text(val);
            } else {
                $cell.number(val,d,'.',' ');
            }
            $cell.removeClass("notloaded");
            $cell.data("history", val);
            $cell.data("previous", val);
            // реальное число без форматов, которое должно сохраняться в базу
            $cell.data("value", val);
            // если в ячейке произошла ошибка
            if (!!b.iserror) {
                $cell.text("");
                $cell.addClass("errordata");
            }
            if (!!b.c) $cell.data("cellid", b.c);
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