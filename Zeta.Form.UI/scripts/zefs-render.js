(function(){
var root = window.zefs = window.zefs || {};
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
				var th = $('<th class="data"/>').attr("idx",col.idx).text(col.name);
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
				var tr = $("<tr/>").attr("level",row.getLevel());
				tr.append($('<td class="number"/>').attr("title", row.code).text(row.number || ""));
				if (session.structure.rows.length > rowidx + 1) {
                    if (row.getLevel() < session.structure.rows[rowidx + 1].getLevel()) {
                        tr.addClass("haschild");
                    }
                }
                var td = $('<td class="name"/>').text(row.name);
				if (row.iscaption) {
					tr.append(td.attr("colspan", "100"));
				} else {
					tr.append(td);
					if (session.NeedMeasure) {
						tr.append($('<td class="measure"/>').text(row.getMeasure()));
					}
                    $.each(session.structure.cols, function(i,col) {
                        var td = $('<td class="data notloaded"/>').attr({
                            "id": row.idx + ":" + col.idx,
                            "idx": col.idx,
                            "visible": "visible"
                        });
                        if (col.isprimary && row.isprimary) td.addClass("editable");
                        if (col.controlpoint && row.controlpoint) td.addClass("control");
                        tr.append(td);
                    });
                }
				body.append(tr);
			});
			table.append(body);
			session.wasRendered = true;
			return session;
		},

		updateCells : function(session,batch){
			var tbody = $(session.table).find("tbody").first();
            var div = $('<div/>'); // Это контейнер для форматирования чисел :)
			$.each(batch.getData(), function(i,b) {
                var $cell = $("td[id='" + b.i +  "']");
                var val = b.getValue() || "";
                $cell.number($cell.text(),0,'','');
                if ($cell.text() != val && !$.isEmptyObject($cell.data())) {
                    $cell.addClass("recalced");
                }
                if (val == "0") {
                    if (b.getCellId() == 0 || !$cell.hasClass("editable")) val = "";
                    $cell.text(val);
                } else {
                    $cell.number(val,0,'.',' ');
                }
                $cell.removeClass("notloaded");
                $cell.data("history", val);
                $cell.data("previous", val);
                $cell.attr("ri", b.getRealId());
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
