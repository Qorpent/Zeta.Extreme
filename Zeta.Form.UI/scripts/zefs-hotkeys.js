/**
 * Виджет кнопки горячих клавиш
 */
!function($) {
    var zefshotkeys = new root.security.Widget("zefshotkeys", root.console.layout.position.layoutHeader, "right", { priority: 87 });
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
    };
    var restart = $('<button class="btn btn-small" title="Горячие клавиши"/>').append('<i class="icon-edit"/>')
        .click(function() {
            HotkeysInfo()
        });
    restart.tooltip({placement: 'bottom'});
    zefshotkeys.body = $('<div/>').append(restart);
    root.console.RegisterWidget(zefshotkeys);
}(window.jQuery);