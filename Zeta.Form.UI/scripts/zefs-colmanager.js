/**
 * Виджет менеджера колонок
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefscolmanager = new root.security.Widget("zefscolmanager", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Управление колонками"/>')
        .html('<i class="icon-list"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    var HideColumn = function(n) {
        $('table.data col[idx=' + n + ']').hide();
        $('table.data th[idx=' + n + ']').hide();
        $('table.data td[idx=' + n + ']').hide();
        $(window).trigger("resize");
    };
    var ShowColumn = function(n) {
        $('table.data col[idx=' + n + ']').show();
        $('table.data th[idx=' + n + ']').show();
        $('table.data td[idx=' + n + ']').show();
        $(window).trigger("resize");
    };
    var HideNullRows = function() {
        $.each($(".zefsform tbody tr"), function(i,tr) {
            if ($(tr).hasClass("istitle")) return;
            var isempty = true;
            $.each($(tr).find("td.data"), function(j,td) {
                if (isempty && ($(td).data("history") != "" && $(td).data("history") != "0")) {
                    isempty = false;
                }
            });
            if(isempty) $(tr).addClass("isempty");
        });
        $("tr.isempty").hide();
    };
    var ShowNullRows = function() {
        $("tr.isempty").removeClass("isempty").show();
    };
    $(document).on('click.dropdown.data-api', '.zefscolmanager li', function (e) {
        e.stopPropagation();
    });
    $(window.zefs).on(window.zefs.handlers.on_structureload, function() {
        $.each(($('table.data>thead>tr').first()).children(), function(i,col) {
            var li = $('<li/>');
            if ($(col).hasClass("primary")) li.addClass("primary");
            var input = $('<input type="checkbox" checked/>').attr("value", $(col).attr("idx") || "");
            input.click(function(e) {
                if ($(e.target).is(":checked")) {
                    ShowColumn($(e.target).val());
                } else {
                    HideColumn($(e.target).val());
                }
            });
            if (/number|name|measure/.test(col.className)) {
                li.addClass("disabled");
                input.attr("disabled","disabled");
            }
            menu.append(li.append($('<a/>').append($('<label/>').append(input, $(col).text()))));
        });
        var input = $('<input type="checkbox" checked/>');
        input.click(function() {
            input.is(":checked") ? ShowNullRows() : HideNullRows();
        });
        menu.append($('<li class="divider"/>'), $('<li/>').append($('<a/>').append($('<label/>').append(input, "Нулевые строки"))));
    });
    b.tooltip({placement: 'bottom'});
    zefscolmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefscolmanager);
}(window.jQuery);
