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
								colgroup.append($("<col/>"));
								var th = $('<th/>').text(col.getName());
								thead.find("tr").append(th);
							});
							table.append(colgroup);
							table.append(thead);
					//        table.find("thead tr").first().append($("<th/>").text(col.getName()).data("src_column",col))

							var body = $("<tbody/>");
							if (session.getNeedMeasure()) {
								thead.find("thead").append($('<th class="measure"/>').text("Ед. изм."));
							}
							$.each(session.structure.rows, function(rowidx,row) {
								var tr = $("<tr/>").attr("level",row.getLevel());
								tr.append($('<td class="number"/>').text(row.getNumber()));

								var td = $('<td class="name"/>').text(row.getName());
								if (row.getIsTitle()) {
									if (session.getNeedMeasure()) {
										tr.append(td);
										tr.append($('<td class="measure"/>').text(row.getMeasure()));
										tr.append($('<td colspan="100"/>').text("И че сюда?"));
										return;
									}
								} else {
									tr.append(td);
									if (session.getNeedMeasure()) {
										tr.append($('<td class="measure"/>').text(row.getMeasure()));
									}
								}
								$.each(session.structure.cols, function(i,col) {
									var td = $("<td/>").attr("id", row.getIdx() + ":" + col.getIdx());
									if (col.getIsPrimary() && row.getIsPrimary()) td.addClass("editable");
									tr.append(td);
								});
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
							$("td[id='" + b.i +  "']").text(b.getValue() | "");
						});
						batch.wasFilled = true;
						return session;
					},
			
			}		
		}
	});
})();
