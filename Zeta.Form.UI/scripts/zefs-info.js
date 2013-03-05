/**
 * Виджет информационного меню
 */
!function($) {
    var information = new root.security.Widget("information", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Информация"/>').html('<i class="icon-book"></i><span class="caret"></span>');
    var m = $('<div class="btn-group"/>').append(
        b, $('<ul class="dropdown-menu"/>').append(
            $('<li/>').html('<a>Инструкция пользователя</a>'),
            $('<li/>').html('<a>Справка по форме</a>'),
            $('<li class="divider"/>'),
            $('<li/>').html('<a>О программе</a>')
        ));
    b.tooltip({placement: 'bottom'});
    information.body = $('<div/>').append(m);
    root.console.RegisterWidget(information);
}(window.jQuery);