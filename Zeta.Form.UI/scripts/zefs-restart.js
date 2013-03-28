/**
 * Виджет кнопки рестарта сервера
 */
!function($) {
    var spec = window.zefs.api;
    var zefsrestart = new root.security.Widget("zefsrestart", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 85, adminonly: true });
    var restart = $('<button class="btn btn-small"/>').append('<i class="icon-repeat"/>')
        .click(function() { window.zefs.myform.restart() });
    zefsrestart.body = $('<div/>').append(restart);
    root.console.RegisterWidget(zefsrestart);
}(window.jQuery);