/**
 * Виджет даже не знаю как это назвать
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var subobj = new zeta.Widget("zefsformsubobj", zeta.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 90 });
    var obj = $('<button class="btn btn-mini"/>');
    $(root).on(root.handlers.on_sessionload, function() {
        var s = root.myform.currentSession;
        if (!!s.SplitObjInfo) {
            obj.html(s.SubObjInfo.Name + '<span class="caret"/>');
            obj.show();
        }
    });
    subobj.body = $('<div/>').append(obj.hide());
    zeta.console.RegisterWidget(subobj);
}(window.jQuery);