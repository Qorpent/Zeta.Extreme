/**
 * Виджет сообщений
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsalerter = new root.security.Widget("zefsalerter", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    var container = $('<div/>');
    var ShowMessage = function(p) {
        p = $.extend({
            text: "",
            content: null,
            type: "",
            autohide: 0,
            fade: false,
            width: 0
        },p);
        var message = $('<div class="alert"/>');
        if (p.type != "") message.addClass(p.type);
        if (p.fade) message.addClass("fade in");
        message.append($('<a class="close" data-dismiss="alert"/>').html('&times;'));
        message.append(p.content || p.text);
        if (p.autohide != 0) {
            window.setTimeout(function() {
                message.remove();
            }, p.autohide);
        }
        if (p.width != 0) message.css("width", p.width);
        container.append(message);
    };
    $(window.zefs).on(window.zefs.handlers.on_message, function(e,params) {
        ShowMessage(params);
    });
    zefsalerter.body = $(container).append();
    root.console.RegisterWidget(zefsalerter);
}(window.jQuery);