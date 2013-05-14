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
    var ChangeState = function(element, state) {
        element = $(element);
        switch (state) {
            case "0ISCHECKED" :
                element.addClass("label-success");
                break;
            case "0ISOPEN" :
                element.addClass("label-important");
                break;
            case "0ISBLOCK" :
                element.addClass("label-warning");
                break;
        }
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function() {
        if (!$.isEmptyObject(window.zefs.myobjs)) {
            $('.zefsformheader').first().css("top", 60);
            $('.zefsalerter').first().css("top", 64);
            var currentObj = zefs.api.getParameters()["obj"] || -1;
            $.each(window.zefs.myobjs, function(i, obj) {
                var name = obj.shortname || obj.name || "";
                var l = $('<span class="label"/>');
                l.attr("value", obj.id);
                if (obj.id == currentObj) {
                    l.addClass("currentobj");
                }
                var r = name.replace(/УГМК-?/, "").match(/"([^"]+)"/);
                l.text(r != null ? r[1] : name);
                l.click(function() { ChangeObject(obj.id) });
                extrapannel.body.append(l);
                $.ajax({
                    url:"zefs/getcurratorlockstate.json.qweb",
                    data : { session : zefs.myform.sessionId, objid: obj.id }
                }).success(function(s) {
                    ChangeState(l, s);
                });
            });
            extrapannel.body.show();
        }
    });
    $(zefs).on(zefs.handlers.on_getlockload, function() {
        var current = $("span.currentobj");
        if (current.length > 0) {
            ChangeState(current.first(), zefs.myform.lock.state);
        }
    });
    root.console.RegisterWidget(extrapannel);
}(window.jQuery);