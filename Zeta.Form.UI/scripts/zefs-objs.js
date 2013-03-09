/**
 * Виджет списка предприятий
 */
!function($) {
    var zefsobjselector = new root.security.Widget("objselector", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Предприятие"/>').html('<i class="icon-map-marker"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});
    var ChangeObject = function(e) {
        location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + $(e.target).attr("value"));
        location.reload();
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function(e) {
        $.each(window.zefs.divs, function(i,div) {
            menu.append($('<li class="dropdown-submenu"/>')
                .append($('<a/>').text(div.getName()), $('<ul class="dropdown-menu"/>').attr("code", div.getCode())));
        });
        $.each(window.zefs.objects, function(i,obj) {
            var ul = menu.find('ul[code=' + obj.getDivCode() + ']');
            if (ul.length != 0) {
                var a = $('<a/>').attr("value", obj.getId());
                a.click(function(e) {
                    ChangeObject(e);
                });
                a.text(obj.getName());
                ul.append($('<li/>').append(a));
            }
        });
    });
    zefsobjselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsobjselector);
}(window.jQuery);