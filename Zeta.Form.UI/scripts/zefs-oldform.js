/**
 * Виджет кнопки вызова старой формы ввода
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsoldform = new root.Widget("zefsoldform", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 79 });
    var b = $('<button class="btn btn-small btn-inverse" data-original-title="Старая форма ввода" />').html('<i class="icon-th-large icon-white"></i>');
    b.tooltip({placement: 'bottom'});
    b.click(function() {
        window.zefs.myform.openoldform();
    });
    zefsoldform.body = $('<div/>').append(b);
    root.console.RegisterWidget(zefsoldform);
}(jQuery);