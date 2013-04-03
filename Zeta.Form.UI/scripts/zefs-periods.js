/**
 * Виджет списка периодов
 */
!function($) {
    var zefsperiodselector = new root.security.Widget("periodselector", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Период"/>').html('<i class="icon-calendar"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});

    var ChangePeriod = function(e) {
        var type = $($(e.target).parents()[1]).attr("type");
        if (type.search("Plan") != -1) location.hash = location.hash.replace(/[ABC].in/gi,'B.in');
        else if (type == "Corrective") location.hash = location.hash.replace(/[ABC].in/gi,'C.in');
        else location.hash = location.hash.replace(/[ABC].in/gi,'A.in');
        location.hash = location.hash.replace(/period=\d+/gi,"period=" + $(e.target).attr("periodcode"));
        location.reload();
    };

    var ChangeYear = function(e) {
        location.hash = location.hash.replace(/year=\d+/gi,"year=" + $(e.target).attr("periodcode"));
        location.reload();
    };

    var GetPeriodGroupName = function(code) {
         switch (code) {
             case "Month" : return "Месяцы";
             case "FromYearStartMain" : return "С начала года";
             case "FromYearStartExt" : return "Промежуточные периодв";
             case "Plan" : return "Плановые периоды";
             case "MonthPlan" : return "Плановые (месячные) периоды";
             case "Corrective" : return "Коррективы плана";
             case "Awaited" : return "Ожидаемые периоды";
             case "Year" : return "Года";
             default : return "Неизвесная группа периодов";
         }
    };

    window.zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        var currentPeriod = window.zefs.myform.currentSession.Period || "";
        var currentYear = window.zefs.myform.currentSession.Year || "";
        $.each(result, function(i,group) {
            if (group.type == "InYear") return;
            var li = $('<li class="dropdown-submenu"/>');
            var ul = $('<ul class="dropdown-menu"/>').attr("type", group.type);
            li.append($('<a/>').text(GetPeriodGroupName(group.type)), ul);
            $.each(group.periods, function(i,period) {
                var a = $('<a/>').attr("periodcode", period.id);
                a.click(function(e) {
                    period.type != "Year" ? ChangePeriod(e) : ChangeYear(e);
                });
                ul.append($('<li/>').append(a.text(period.name)));
            });
            menu.append(li);
        });
        $('a[periodcode="' + currentPeriod + '"]').parents('li').addClass("current");
        $('a[periodcode="' + currentYear + '"]').parents('li').addClass("current");
    });

    zefsperiodselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsperiodselector);
}(window.jQuery);