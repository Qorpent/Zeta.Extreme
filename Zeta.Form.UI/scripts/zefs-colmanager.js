/**
 * Виджет менеджера колонок
 */
!function($) {
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
    $(document).on('click.dropdown.data-api', '.zefscolmanager li', function (e) {
        e.stopPropagation();
        var input = null;
        if (e.target.tagName == "INPUT") {
            input = $(e.target);
        } else {
            return;
        }
        if (input.is(":checked")) {
            ShowColumn(input.val());
        } else {
            HideColumn(input.val());
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_structureload, function(e) {
        $.each(($('table.data>thead>tr').first()).children(), function(i,col) {
            var li = $('<li/>');
            if ($(col).hasClass("primary")) li.addClass("primary");
            var input = $('<input type="checkbox" checked/>').attr("value", $(col).attr("idx") || "");
            if (/number|name|measure/.test(col.className)) {
                li.addClass("disabled");
                input.attr("disabled","disabled");
            }
            menu.append(li.append($('<a/>').append($('<label/>').append(input, $(col).text()))));
        });
    });
    b.tooltip({placement: 'bottom'});
    zefscolmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefscolmanager);
}(window.jQuery);
