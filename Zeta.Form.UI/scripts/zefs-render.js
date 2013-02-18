(function(){
var root = window.zefs = window.zefs || {};
$.extend(root,{
	getRender : function(){
		return {
		renderStructure : function(session) {
			var table = $('<table class="data"/>');
			session.table = table;
			var colgroup = $('<colgroup/>').append(
				$("<col/>").addClass("number"),
				$("<col/>").addClass("name")
			);
			if (session.getNeedMeasure()) {
				colgroup.append($("<col/>").addClass("measure"));
			}
			var thead = $('<thead/>').append($("<tr/>").append(
				$('<th class="number"/>').text("№"),
				$('<th class="name"/>').text("Наименование")
			));
			if (session.getNeedMeasure()) {
				thead.find("thead").append($('<th class="measure"/>').text("Ед. изм."));
			}
			$.each(session.structure.cols, function(i,col) {
				colgroup.append($('<col class="data"/>'));
				var th = $('<th class="data"/>').text(col.getName());
				thead.find("tr").append(th);
			});
			table.append(colgroup);
			table.append(thead);
		    //table.find("thead tr").first().append($("<th/>").text(col.getName()).data("src_column",col))
				var body = $("<tbody/>");
			if (session.getNeedMeasure()) {
				thead.find("thead").append($('<th class="measure"/>').text("Ед. изм."));
			}
			$.each(session.structure.rows, function(rowidx,row) {
				var tr = $("<tr/>").attr("level",row.getLevel());
				tr.append($('<td class="number"/>').attr("title", row.code).text(row.getNumber()));
				var td = $('<td class="name"/>').text(row.getName());
				if (row.getIsTitle()) {
					tr.append(td.attr("colspan", "100"));
				} else {
					tr.append(td);
					if (session.getNeedMeasure()) {
						tr.append($('<td class="measure"/>').text(row.getMeasure()));
					}
                    $.each(session.structure.cols, function(i,col) {
                        var td = $('<td class="data notloaded"/>').attr("id", row.getIdx() + ":" + col.getIdx());
                        if (col.getIsPrimary() && row.getIsPrimary()) td.addClass("editable");
                        tr.append(td);
                    });
                }
				body.append(tr);
			});
			table.append(body);
			$("body").append(table);
			session.wasRendered = true;
			return session;
		},

		updateCells : function(session,batch){
			var tbody = $(session.table).find("tbody").first();
				$.each(batch.getData(), function(i,b) {
                    var $cell = $("td[id='" + b.i +  "']")
                    var val = b.getValue() | "";
                    if (val == 0) {
                        if (b.getCellId() == 0 || !$cell.hasClass("editable")) val = "";
                    }
                    if (val != "") $cell.number(val,0,'.',' ');
                    $cell.removeClass("notloaded");
                    $cell.data("history", val);
				});
				batch.wasFilled = true;
				return session;
			}
		}
	}
	});
})();
