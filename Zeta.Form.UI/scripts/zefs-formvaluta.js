/**
 * Виджет nerublevoi valuti
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var formvaluta = new zeta.Widget("zefsformvaluta", zeta.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 80 });
    var l = $('<span class="label label-info"/>');
    $(root).on(root.handlers.on_structureload, function() {
        var s = root.myform.currentSession;
        if (!!s.ObjInfo.Currency) {
            if (s.ObjInfo.Currency != "RUB") {
                l.text("Валюта: " + s.ObjInfo.Currency + ", Курс для периода: " + s.ObjInfo.CurrencyRate);
                l.show();
            }
        }
    });
    formvaluta.body = $('<div/>').append(l.hide());
    zeta.console.RegisterWidget(formvaluta);
}(window.jQuery);