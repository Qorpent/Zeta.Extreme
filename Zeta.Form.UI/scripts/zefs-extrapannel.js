/**
 * Виджет дополнительной паннели
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var extrapannel = new root.Widget("zefsextrapannel", root.console.layout.position.layoutHeader, null, { authonly: true });
    extrapannel.body = $('<div/>').hide();
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
                var r = name.replace(/УГМК-?/, "").match(/"([^"]+)"/);
                l.text(r != null ? r[1] : name);
                l.click(function() { ChangeObject(obj.id) });
                extrapannel.body.append(l);
                l = null;
            });
            extrapannel.body.show();
        }
    });
    root.console.RegisterWidget(extrapannel);
}(window.jQuery);