/**
 * Виджет дополнительной паннели
 */
!function($) {
    var extrapannel = new root.security.Widget("zefsextrapannel", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    extrapannel.body = $('<div/>');
    var InsertPeriod = function() {

    };
    var ChangeObject = function(e) {
        location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + $(e.target).attr("value"));
        location.reload();
    };
    $(window.zefs).on(window.zefs.handlers.on_objectsload, function() {
        if (!$.isEmptyObject(window.zefs.myobjs)) {
            $('.zefsformheader').first().css("top", 62);
            $.each(window.zefs.myobjs, function(i, obj) {
                var name = obj.shorname || obj.name || "";
                extrapannel.body.append($('<span class="label"/>').text(name.match(/"([^"]+)"/)[1]));
            });
        }
    });
    root.console.RegisterWidget(extrapannel);
}(window.jQuery);