(function(){
var root = window.zefs = window.zefs || {};
window.zefs.handlers = $.extend(window.zefs.handlers, {
    on_renderfinished : "renderfinished"
});
$.extend(root,{
	getRender : function(){
		return {
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
			$.each(session.structure.rows, function(rowidx,row) {
				var tr = $("<tr/>").attr("level",row.level);
				if (row.iscaption) tr.addClass("istitle");
                tr.append($('<td class="number"/>').attr("title", row.code).text(row.number || ""));
				if (session.structure.rows.length > rowidx + 1) {
                    if (row.level < session.structure.rows[rowidx + 1].level) {
                        tr.addClass("haschild");
                    }
                }
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
                        if (col.format != null && col.format != "") td.data("format", col.format);
                        if (row.format != null && row.format != "") td.data("format", row.format);
                        tr.append(td);
                    });
                }
				body.append(tr);
			});
			table.append(body);

            $(root).trigger(window.zefs.handlers.on_renderfinished, table);

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
                if (!!b.iserror) $cell.addClass("erorrdata");
                if (!!b.c) $cell.data("cellid", b.c);
                $cell.attr("ri", b.ri);
                if (val.search(/\./) != -1 && val.search("error") == -1) {
                    $cell.addClass("rounded");
                    $cell.tooltip({title: val, placement: 'top', container: $('body')});
                }
			});
			batch.wasFilled = true;
			return session;
		}
		}
	}
	});
})();
