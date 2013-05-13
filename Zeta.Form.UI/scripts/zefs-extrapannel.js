/**
 * Виджет дополнительной паннели
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var extrapannel = new root.Widget("zefsextrapannel", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    extrapannel.body = $('<div/>').hide();
    var ChangeObject = function(id) {
        location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + id);
        location.reload();
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function() {
        if (!$.isEmptyObject(window.zefs.myobjs)) {
            $('.zefsformheader').first().css("top", 60);
            $('.zefsalerter').first().css("top", 64);
            $.each(window.zefs.myobjs, function(i, obj) {
                var name = obj.shortname || obj.name || "";
                var l = $('<span class="label"/>');
                l.attr("value", obj.id);
                var r = name.replace(/УГМК-?/, "").match(/"([^"]+)"/);
                l.text(r != null ? r[1] : name);
                l.click(function() { ChangeObject(obj.id) });
                extrapannel.body.append(l);
                $.ajax({
                    url:"zefs/getcurratorlockstate.json.qweb",
                    data : { session : zefs.myform.sessionId, objid: obj.id }
                }).success(function(s) {
                    switch (s) {
                        case "0ISCHECKED" :
                            l.addClass("label-success");
                            break;
                        case "0ISOPEN" :
                            l.addClass("label-important");
                            break;
                        case "0ISBLOCK" :
                            l.addClass("label-warning");
                            break;
                    }
                });
            });
            extrapannel.body.show();
        }
    });
    root.console.RegisterWidget(extrapannel);
}(window.jQuery);