/**
 * Виджет инструментов для отладки
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var nullrowsmanager = new root.Widget("zefsnullrows", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 92 });
    var b = $('<button class="btn btn-small" data-original-title="Показать/скрыть нулевые строки"/>')
        .html('<i class="icon-filter"></i>');
    b.tooltip({placement: 'bottom'});
    b.click(function() {
        if (b.hasClass("active")) {
            b.removeClass("active");
            root.temporarystorage.AddOrUpdate({ hidenullrows: false });
            ShowNullRows();
        } else  {
            b.addClass("active");
            root.temporarystorage.AddOrUpdate({ hidenullrows: true });
            HideNullRows();
        }
    });
    // Hotkey для скрытия нулевых строк
    $(document).on('keydown', function(e) {
        if (e.keyCode == 49 && e.altKey) {
            b.trigger('click');
            e.stopPropagation();
        }
    });
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
    $(window.zefs).on(window.zefs.handlers.on_dataload, function() {
        var formopts = root.temporarystorage.Get();
        if (formopts == null || $.isEmptyObject(formopts)) {
            root.temporarystorage.AddOrUpdate({ hidenullrows: false });
        } else {
            if (formopts.hidenullrows) {
                if (!b.hasClass('active')) {
                    b.trigger("click");
                } else {
                    ShowNullRows();
                    HideNullRows();
                }
            }
        }
        window.zefs.restorelaststate();
    });
    nullrowsmanager.body = $('<div/>').append(b);
    root.console.RegisterWidget(nullrowsmanager);
}(window.jQuery);