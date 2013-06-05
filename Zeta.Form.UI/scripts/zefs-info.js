/**
 * Виджет информационного меню
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var information = new root.Widget("information", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 0 });
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Помощь"/>').html('<i class="icon-question-sign"></i><span class="caret"></span>');
    var m = $('<div class="btn-group pull-left"/>').append(b, qwiki.getMENU('/form/helpmenu'));
    m.css("margin-left", 3);
    b.tooltip({placement: 'bottom'});
    b.dropdownHover({delay: 100});
    /*var whoami = $('<a/>').css("background-color", "#FFE1E1").text("Где я, кто я?");
    var hotkeys = $('<a/>').text("Горячие клавиши");
    var faq = $('<a/>').text("Часто задаваемые вопросы");
    var instructions = $('<a/>').text("Инструкция пользователя");
    var about = $('<a/>').text("О программе");
    whoami.click(function() { WhatTheHellAndWhoAmI() });
    hotkeys.click(function() { zefs.api.wiki.getsync.execute({code: "/form/shortcuts"}); });
    faq.click(function() { zefs.api.wiki.getsync.execute({code: "/common/faq"}); });
    instructions.click(function() { zefs.api.wiki.getsync.execute({code: "/common/instructions"}); });
    about.click(function() { zefs.api.wiki.getsync.execute({code: "/common/about"}); });
    var m = $('<div class="btn-group"/>').append(
        b, $('<ul class="dropdown-menu pull-right"/>').append(
            $('<li/>').append(whoami),
            $('<li/>').append(hotkeys),
            $('<li/>').append(faq),
            $('<li/>').append(instructions),
            $('<li class="divider"/>'),
            $('<li/>').append(about)
        ));
    b.tooltip({placement: 'bottom'});
    b.dropdownHover({delay: 100});

    var WhatTheHellAndWhoAmI = function() {*/
    $.extend(zefs.myform, {
        sendsupport : function() {
            var email = "support.assoi@ugmk.com";
            var subject = "Проблема при работе в АССОИ";
            var body =  "Сервер: " + location.host + "\n" +
                "Логин: " + window.zeta.user.getLogonName() + "\n" +
                "Код формы: " + zefs.myform.currentSession.FormInfo.Code + "\n" +
                "Код предприятия: " + zefs.myform.currentSession.ObjInfo.Id + "\n" +
                "Номер периода: " + zefs.myform.currentSession.Period + "\n" +
                "Год: " + zefs.myform.currentSession.Year + "\n" +
                "Блокировки: " + JSON.stringify(zefs.myform.lock) + "\n" +
                "Ссылка на форму: " + location.href;
            window.open("mailto:" + email + "?subject=" + subject + "&body=" + encodeURIComponent(body), "_blank");
        }
    });
    information.body = $('<div/>').append(m);
    root.console.RegisterWidget(information);
}(jQuery);