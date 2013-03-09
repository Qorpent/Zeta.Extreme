/**
 * Виджет списка периодов
 */
!function($) {
    var zefsperiodselector = new root.security.Widget("objselector", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Период"/>').html('<i class="icon-calendar"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    menu.append(
        $('<li class="dropdown-submenu" type="Month"/>').append($('<a/>').text('Месяцы')),
        $('<li class="dropdown-submenu" type="FromYearStartMain"/>').append($('<a/>').text('С начала года')),
        $('<li class="divider"/>'),
        $('<li class="dropdown-submenu" type="FromYearStartExt"/>').append($('<a/>').text('Промежуточные периоды')),
        $('<li class="dropdown-submenu" type="Plan"/>').append($('<a/>').text('Плановые периоды')),
        $('<li class="dropdown-submenu" type="MonthPlan"/>').append($('<a/>').text('Плановые (месячные) периоды')),
        $('<li class="dropdown-submenu" type="Corrective"/>').append($('<a/>').text('Коррективы плана')),
        $('<li class="dropdown-submenu" type="Awaited"/>').append($('<a/>').text('Ожидаемые периоды'))
        /*$('<li class="dropdown-submenu" type="Ext"/>').append($('<a/>').text('Дополнительные'))*/
    );
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});
    var ChangePeriod = function(e) {
        var type = $($(e.target).parents()[2]).attr("type");
        if (type.search("Plan") != -1) location.hash = location.hash.replace(/[ABC].in/gi,'B.in');
        else if (type == "Corrective") location.hash = location.hash.replace(/[ABC].in/gi,'C.in');
        else location.hash = location.hash.replace(/[ABC].in/gi,'A.in');
        location.hash = location.hash.replace(/period=\d+/gi,"period=" + $(e.target).attr("value"));
        location.reload();
    };
    var ChangeYear = function(e) {
        location.hash = location.hash.replace(/year=\d+/gi,"year=" + $(e.target).attr("value"));
        location.reload();
    };
    var yearmenu = $('<li class="dropdown-submenu" type="Years"/>').append($('<a/>').text('Года'));
    menu.append($('<li class="divider"/>'), yearmenu);
    var ul = $('<ul class="dropdown-menu"/>');
    $.each([2013,2012,2011,2010], function(i,y) {
        var a = $('<a/>').attr("value", y);
        a.click(function(e) {
            ChangeYear(e);
        });
        a.text(y);
        ul.append($('<li/>').append(a));
    });
    yearmenu.append(ul);
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function(e) {
        $.each(window.zefs.periods, function(periodname,periodtype) {
            var litype = menu.find('li[type=' + periodname + ']');
            if (litype.length != 0) {
                var ul = $('<ul class="dropdown-menu"/>');
                $.each(periodtype, function(i,period) {
                    var a = $('<a/>').attr("value", period.getId());
                    a.click(function(e) {
                        ChangePeriod(e);
                    });
                    a.text(period.getName());
                    ul.append($('<li/>').append(a));
                });
                litype.append(ul);
            }
        })
    });
    zefsperiodselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsperiodselector);
}(window.jQuery);