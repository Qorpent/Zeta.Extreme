/**
 * Виджет менеджера колонок
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefscolmanager = new root.Widget("zefscolmanager", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 93 });
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
        StoreColVisible(n, false);
    };
    var ShowColumn = function(n) {
        $('table.data col[idx=' + n + ']').show();
        $('table.data th[idx=' + n + ']').show();
        $('table.data td[idx=' + n + ']').show();
        $(window).trigger("resize");
        StoreColVisible(n, true);
    };
    var StoreColVisible = function(col, visible) {
        var colvisiblelist = root.coloptionsstorage.Get();
        colvisiblelist.colsvisible = colvisiblelist.colsvisible || {};
        colvisiblelist.colsvisible[col] = visible;
        root.coloptionsstorage.AddOrUpdate(colvisiblelist);
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
        var colopts = root.coloptionsstorage.Get();
        colopts.colsvisible = colopts.colsvisible || {};
        $.each(colopts.colsvisible, function(col, visible) {
            if (!visible) menu.find('input[value="' + col + '"]').first().trigger("click");
        });
    });
    b.tooltip({placement: 'bottom'});
    zefscolmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefscolmanager);
}(window.jQuery);
