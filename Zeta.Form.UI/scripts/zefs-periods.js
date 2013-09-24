/**
 * Виджет списка периодов
 */
!function($) {
    var zefsperiodselector = new root.Widget("periodselector", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 96 });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Период"/>').html('<i class="icon-calendar"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});
    b.dropdownHover({delay: 100});
    var ChangePeriod = function(e) {
        var type = $($(e.target).parents()[1]).attr("type");
        var params = {
            period: $(e.target).attr("periodcode")
        };
        var hashparams = zefs.api.getParameters();
        if (hashparams != null) {
            if (hashparams.form != null) {
                var form = hashparams.form;
                if (form.search(".in") != -1) {
                    if (type.search("Plan") != -1) form = form.replace(/[ABC].in/gi,'B.in');
                    else if (type == "Corrective") form = form.replace(/[ABC].in/gi,'C.in');
                    else form = form.replace(/[ABC].in/gi,'A.in');
                } else {
                    form += "A.in";
                }
                params.form = form;
            }
        }
        zefs.myform.openform(params, e.ctrlKey);
    };

    var ChangeYear = function(e) {
        zefs.myform.openform({year: $(e.target).attr("periodcode")}, e.ctrlKey);
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

    $(zefs).on(zefs.handlers.on_periodsload, function() {
        $.each(zefs.periods, function(i,group) {
            if (group.type == "InYear") return;
            var li = $('<li class="dropdown-submenu"/>');
            var ul = $('<ul class="dropdown-menu"/>').attr("type", group.type);
            li.append($('<a/>').text(GetPeriodGroupName(group.type)), ul);
            $.each(group.periods, function(i, period) {
                var a = $('<a/>').attr("periodcode", period.id);
                a.click(function(e) {
                    period.type != "Year" ? ChangePeriod(e) : ChangeYear(e);
                });
                ul.append($('<li/>').append(a.text(period.name)));
            });
            menu.append(li);
        });
    });

    $(zefs).on(zefs.handlers.on_sessionload, function() {
        $('a[periodcode="' + window.zefs.myform.currentSession.Period + '"]').parents('li').addClass("current");
        $('a[periodcode="' + window.zefs.myform.currentSession.Year + '"]').parents('li').addClass("current");
    });

    zefsperiodselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsperiodselector);
}(window.jQuery);