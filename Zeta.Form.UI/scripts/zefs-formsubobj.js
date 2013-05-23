/**
 * Виджет даже не знаю как это назвать
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var subobj = new zeta.Widget("zefsformsubobj", zeta.console.layout.position.layoutPageHeader, "left", { authonly: true, priority: 90 });
    $(root).on(root.handlers.on_sessionload, function() {
        var s = root.myform.currentSession;
        if (!!s.SplitObjInfo) {
            var bg = $('<div class="btn-group"/>');
            var b = $('<button class="btn btn-mini dropdown-toggle"/>');
            var list = $('<ul class="dropdown-menu"/>');
            b.html('<strong>' + s.SubObjInfo.Name + '</strong><span class="caret"/>');
            $.each(s.SplitObjInfo, function(i, o) {
                var li = $('<li/>');
                var a = $('<a/>').text(o.Name);
                a.click(function() {
                    ChangeSubobj(o.Id);
                });
                if (s.SubObjInfo.Id == o.Id) {
                    li.addClass("current");
                }
                list.append(li.append(a));
            });
            bg.append(b,list);
            subobj.body.append(bg);
            b.dropdownHover({delay: 100});
            b.show();
        }
    });
    var ChangeSubobj = function(sobj) {
        zefs.myform.openform({subobj: sobj});
    };
    subobj.body = $('<div/>');
    zeta.console.RegisterWidget(subobj);
}(window.jQuery);