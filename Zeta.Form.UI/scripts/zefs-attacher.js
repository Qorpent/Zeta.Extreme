/**
 * Виджет менеджера прикрепленных файлов
 */
!function($) {
    var attacher = new root.security.Widget("attacher", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 30 });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown"/>')
        .html('<i class="icon-file"></i><span class="caret"></span>');
    var filelist = $('<ul class="dropdown-menu"/>');
    b.tooltip({title: "Прикрепленные файлы", placement: 'bottom'});
    attacher.body = $('<div/>').append($('<div class="btn-group"/>').append(b,filelist));
    root.console.RegisterWidget(attacher);
}(window.jQuery);