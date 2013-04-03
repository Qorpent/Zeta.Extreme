/**
 * Виджет списка предприятий
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsobjselector = new root.Widget("objselector", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Предприятие"/>').html('<i class="icon-map-marker"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});
    var ChangeObject = function(e) {
        location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + $(e.target).attr("objcode"));
        location.reload();
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function(e) {
        var divs = $.map(window.zefs.divs, function(d){ return d });
        var current = window.zefs.myform.currentSession.ObjInfo.Id || "";
        $.each(divs.sort(function(a,b) { return a.idx - b.idx }), function(i,div) {
            var ul = $('<ul class="dropdown-menu"/>').attr("code", div.code);
            menu.append($('<li class="dropdown-submenu"/>')
                .append($('<a/>').text(div.name), ul));
            var objs = $.map(window.zefs.objects, function(o) { if (o.div == div.code) return o });
            if (objs.length > 20) {
                ul.parent().css("position", "static");
                ul.css("margin-top", -1);
            }
            $.each(objs.sort(function(a,b) { return a.idx - b.idx }), $.proxy(function(i,obj) {
                var li = $('<li/>');
                if (this.length != 0) {
                    if (obj.ismyobj) li.addClass("primary");
                    var a = $('<a/>').attr("objcode", obj.id);
                    a.click(function(e) {
                        ChangeObject(e);
                    });
                    a.text(obj.name);
                    this.append(li.append(a));
                    li = null;
                }
            }, ul));
            ul = objs = null;
        });
        $('a[objcode="' + current + '"]').parents('li').addClass("current");
    });
    zefsobjselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsobjselector);
}(window.jQuery);