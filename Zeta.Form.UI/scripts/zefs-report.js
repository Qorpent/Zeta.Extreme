/**
 * Виджет дополнительной паннели
 */
!function($) {
    var zefsreport = new root.security.Widget("zefsreport", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 75 });
    var b = $('<button class="btn btn-small btn-warning" data-original-title="Стандартный отчет" />').html('<i class="icon-file icon-white"></i>');
    b.tooltip({placement: 'bottom'});
    b.click(function() {
        window.zefs.myform.openreport();
    });
    zefsreport.body = $('<div/>').append(b);
    root.console.RegisterWidget(zefsreport);
}(jQuery);