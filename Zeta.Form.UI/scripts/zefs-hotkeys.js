/**
 * Виджет кнопки горячих клавиш
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefshotkeys = new root.Widget("zefshotkeys", root.console.layout.position.layoutHeader, "right", { priority: 87 });
    var hotkeys = $('<button class="btn btn-small" title="Горячие клавиши"/>').append('<i class="icon-edit"/>')
        .click(function() {
            zefs.api.wiki.getsync.execute({code: "/form/shortcuts"});
        });
    hotkeys.tooltip({placement: 'bottom'});
    zefshotkeys.body = $('<div/>').append(hotkeys);
    root.console.RegisterWidget(zefshotkeys);
}(window.jQuery);