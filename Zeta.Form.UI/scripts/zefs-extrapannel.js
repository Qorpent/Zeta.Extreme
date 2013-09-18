/**
 * Виджет дополнительной паннели
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var extrapannel = new root.Widget("zefsextrapannel", root.console.layout.position.layoutTools, null, { authonly: true });
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

    var pannelInnit = function() {
        if (zefs.api.getParameters() != null) {
            var currentObj = zefs.api.getParameters()["obj"] || -1;
        }
        $.each(window.zefs.myform.currentSession.OverrideObjects || window.zefs.myobjs, function(i, obj) {
            var name = obj.shortname || obj.name || obj.Name || "";
            var l = $('<span class="label kuratorobj"/>');
            l.attr("value", obj.id || obj.Id);
            if (obj.id == currentObj || obj.Id == currentObj) {
                l.addClass("currentobj");
            }
            var r = name.replace(/УГМК-?/, "").match(/"([^"]+)"/);
            l.text(r != null ? r[1] : name);
            l.click(function() { ChangeObject(obj.id || obj.Id) });
            extrapannel.body.append(l);
        });
        extrapannel.body.show();

        $.each($('.kuratorobj'), function(i, kuratorobj) {
            $.ajax({
                url:"zefs/getcurratorlockstate.json.qweb",
                data : { session : zefs.myform.sessionId, objid: $(kuratorobj).attr("value") }
            }).success(function(s) {
                ChangeState($(kuratorobj), s);
            });
        });
    };

    $(window.zefs).on(window.zefs.handlers.on_objectsload, function() {
        if (!!window.zefs.currentSession) pannelInnit();
        else {
            $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
                pannelInnit();
            });
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