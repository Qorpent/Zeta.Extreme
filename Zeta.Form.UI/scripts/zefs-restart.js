/**
 * Виджет кнопки рестарта сервера
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsrestart = new root.security.Widget("zefsrestart", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 85, adminonly: true });
    var restart = $('<button class="btn btn-small" title="Перезапуск сервера"/>').append('<i class="icon-repeat"/>')
        .click(function() { window.zefs.myform.restart() });
    restart.tooltip({placement: 'bottom'});
    zefsrestart.body = $('<div/>').append(restart);
    root.console.RegisterWidget(zefsrestart);
}(window.jQuery);