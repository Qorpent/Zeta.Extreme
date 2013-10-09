/**
 * Виджет nerublevoi valuti
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var formkurator = new zeta.Widget("zefsformkurator", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 100 });
    var k = $('<span class="label label-success"/>');
    $(root).on(root.handlers.on_sessionload, function() {
        var s = root.myform.currentSession;
        if (!!s.FormInfo.HoldResponsibility) {
            var u = $('<span class="label label-success" style="display: none;"/>');
            k.html('Куратор формы <i class="icon icon-white"/>');
            k.click(function() {
                u.trigger('click');
            });
            zeta.zetauser.getDetails(s.FormInfo.HoldResponsibility, function(result) { 
                u.click(function() { zeta.zetauser.renderDetails(result) });
            });
            k.show();
        }
    });
    formkurator.body = $('<div/>').append(k.hide());
    zeta.console.RegisterWidget(formkurator);
}(window.jQuery);