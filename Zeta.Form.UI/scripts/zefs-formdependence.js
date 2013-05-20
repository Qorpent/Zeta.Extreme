/**
 * Виджет nerublevoi valuti
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var formdependence = new zeta.Widget("zefsformdependence", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 80 });
    var t = $('<span class="label-tree"/>');
    $(root).on(root.handlers.on_structureload, function() {
        t.click(function() {
            var iframe = $('<iframe/>').css("height", 340).attr("src", "zefs-formdetails.html#" + zefs.myform.currentSession.FormInfo.CodeOnly);
            $(zeta).trigger(window.zeta.handlers.on_modal, { title: "Дерево зависимостей формы", content: iframe, width: 700, height: 350});
            iframe = null;
        });
    });
    formdependence.body = $('<div/>').append(t);
    zeta.console.RegisterWidget(formdependence);
}(window.jQuery);