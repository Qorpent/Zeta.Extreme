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
            $('<li/>').html('<a>Горячие клавиши</a>').click(function() { HotkeysInfo() }),
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
    }
    var HotkeysInfo = function() {
        var t = $('<table class="table table-bordered zefshotkeysinfo"/>').append(
            $('<tr/>').append($('<td/>').text("Ctrl + ↑"), $('<td/>').html("Перемещение табличного курсора в самую <strong>верхнюю ячейку</stron> текущего столбца")),
            $('<tr/>').append($('<td/>').text("Ctrl + ↓"), $('<td/>').html("Перемещение табличного курсора в самую <strong>нижнюю ячейку</stron> текущего столбца")),
            $('<tr/>').append($('<td/>').text("Ctrl + Backspace"), $('<td/>').text("Удаление введенного значения с возвратом к сохраненному значению")),
            $('<tr/>').append($('<td/>').text("Ctrl + S"), $('<td/>').text("Сохранение внесенных изменений (Аналог синей кнопка с галочкой на верхней панели)")),
            $('<tr/>').append($('<td/>').text("Ctrl + Shift + S"), $('<td/>').text("Принудительный вызов процедуры сохранения при отсутствии внесенных изменений (или синяя кнопка с галочкой на верхней панели при нажатых Ctrl + Shift)")),
            $('<tr/>').append($('<td/>').text("F2"), $('<td/>').text("Перейти в режим редактирования текущей ячейки")),
            $('<tr/>').append($('<td/>').text("Del"), $('<td/>').text("Очистка текущей ячейки")),
            $('<tr/>').append($('<td/>').text("F5"), $('<td/>').text("Обновление страницы (может использоваться для очистки введенных значений)")),
            $('<tr/>').append($('<td/>').text("Ctrl + F5"), $('<td/>').text("Обновить программные модули на локальном компьютере (данная операция выполняется в случае, если поведение программы отличается от ожидаемого или имеется информация, что в программе добавлены и/или изменены функциональные возможности)"))
        );
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            width: 800,
            title: "Горячие клавиши",
            content: t
        });
    }
    information.body = $('<div/>').append(m);
    root.console.RegisterWidget(information);
}(window.jQuery);