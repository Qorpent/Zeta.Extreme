/**
 * Виджет кнопки рестарта сервера
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsfaq = new root.Widget("zefsfaq", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 89, adminonly: true });
    var faq = $('<button class="btn btn-small" title="Часто задаваемые вопросы"/>').append('<i class="icon-question-sign"/>')
        .click(function() {
            var iframe = $('<iframe/>').css("height", 340).attr("src", siteroot + "resources/faq.html");
            $(window.zeta).trigger(window.zeta.handlers.on_modal, { title: "Часто задаваемые вопросы", content: iframe, width: 700, height: 350});
        });
    faq.tooltip({placement: 'bottom'});
    zefsfaq.body = $('<div/>').append(faq);
    root.console.RegisterWidget(zefsfaq);
}(window.jQuery);