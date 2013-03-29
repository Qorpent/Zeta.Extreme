/**
 * Виджет информационного меню
 */
!function($) {
    var information = new root.security.Widget("information", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 85 });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Информация"/>').html('<i class="icon-book"></i><span class="caret"></span>');
    var support = $('<a/>').css("background-color", "#FFE1E1").text("Где я, кто я?");
    support.click(function() { WhatTheHellAndWhoAmI() });
    var m = $('<div class="btn-group"/>').append(
        b, $('<ul class="dropdown-menu pull-right"/>').append(
            $('<li/>').append(support),
            $('<li/>').html('<a>Инструкция пользователя</a>'),
            $('<li/>').html('<a>Справка по форме</a>'),
            $('<li class="divider"/>'),
            $('<li/>').html('<a>О программе</a>')
        ));
    b.tooltip({placement: 'bottom'});
    var WhatTheHellAndWhoAmI = function() {
        var t = $('<table class="table table-bordered zefssessioninfo"/>').append(
            $('<tr/>').append($('<td/>').text("Сервер"), $('<td/>').text(location.host)),
            $('<tr/>').append($('<td/>').text("Логин"), $('<td/>').text(window.zeta.security.user.getLogonName())),
            $('<tr/>').append($('<td/>').text("Код формы"), $('<td/>').text(zefs.myform.currentSession.FormInfo.Code)),
            $('<tr/>').append($('<td/>').text("Код предприятия"), $('<td/>').text(zefs.myform.currentSession.ObjInfo.Id)),
            $('<tr/>').append($('<td/>').text("Номер периода"), $('<td/>').text(zefs.myform.currentSession.Period)),
            $('<tr/>').append($('<td/>').text("Год"), $('<td/>').text(zefs.myform.currentSession.Year))
        );
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Информация для службы поддержки",
            content: t
        });
    };
    information.body = $('<div/>').append(m);
    root.console.RegisterWidget(information);
}(window.jQuery);