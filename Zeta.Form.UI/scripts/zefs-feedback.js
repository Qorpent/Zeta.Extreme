/**
 * Виджет обратной связи
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var feedback = new root.security.Widget("feedback", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 80 });
    var b = $('<button class="btn btn-small btn-warning" data-original-title="Обратная связь" />').html('<i class="icon-envelope icon-white"></i>');
    b.tooltip({placement: 'bottom'});
    feedback.body = $('<div/>').append(b);
//    root.console.RegisterWidget(feedback);
}(window.jQuery);