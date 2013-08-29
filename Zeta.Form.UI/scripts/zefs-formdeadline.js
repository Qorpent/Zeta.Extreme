/**
 * Виджет дедлайна формы
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var formdeadline = new zeta.Widget("formdeadline", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 100 });
    var k = $('<span class="label label-important"/>');
    $(root).on(root.handlers.on_sessionload, function() {
        var ps = root.myform.currentSession.FormInfo.PeriodState;
        if (!!ps) {
            if (!ps.DeadLine || parseInt(ps.DeadLine.getFullYear()) < 2010) return;
            k.text("закрыть до " + ps.DeadLine.format("dd.mm.yyyy"));
            k.show();
        }
    });
    formdeadline.body = $('<div/>').append(k.hide());
    zeta.console.RegisterWidget(formdeadline);
}(window.jQuery);