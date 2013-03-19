/**
 * Виджет дополнительной паннели
 */
!function($) {
    var extrapannel = new root.security.Widget("zefsextrapannel", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    extrapannel.body = $('<div/>');
    var InsertPeriod = function() {

    };
    var ChangeObject = function(id) {
        location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + id);
        location.reload();
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function() {
        if (!$.isEmptyObject(window.zefs.myobjs)) {
            $('.zefsformheader').first().css("top", 60);
            $.each(window.zefs.myobjs, function(i, obj) {
                var name = obj.shortname || obj.name || "";
                var l = $('<span class="label"/>');
                l.attr("value", obj.id);
                l.text(name.match(/"([^"]+)"/)[1]);
                l.click(function() { ChangeObject(obj.id) });
                extrapannel.body.append(l);
                l = null;
            });
        }
    });
    root.console.RegisterWidget(extrapannel);
}(window.jQuery);