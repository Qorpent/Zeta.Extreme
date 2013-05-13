/**
 * Виджет списка бизнесс процессов
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsbizprocess = new root.Widget("zefsbizprocess", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 94 });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Список форм"/>').html('<i class="icon-list-alt"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.dropdownHover({delay: 100});
    b.tooltip({placement: 'bottom'});
    var ChangeForm = function(a, blank) {
        blank = blank || false;
        var form = $(a).attr("formcode");
        if (form.search(".in") == -1) {
            var hashparams = zefs.api.getParameters();
            if (!!hashparams) {
                if (!!hashparams.form) {
                    if (hashparams.form.search('.in') != -1) {
                        form += hashparams["form"].match(/[A|B]\.in/)[0];
                    }
                }
            }
        }
        if (form.search(".in") == -1) {
            form += "A.in";
        }
        zefs.myform.openform({form: form}, blank);
    };
    var GetReadableGroupName = function(code) {
        switch (code) {
            case "cost_grp"  : return "Контроллинг";
            case "eco_grp"   : return "Экономика";
            case "fin_grp"   : return "Финансы";
            case "analysisg" : return "Анализ";
            case "proiz_grp" : return "Производство";
            case "trud_grp"  : return "Персонал";
            case "sys"       : return "Системные";
            case "f7"        : return "Системные";
            default          : return "";
        }
    };
    var GetReadableParentName = function(code) {
        switch (code) {
            case "balans2011"       : return "Баланс";
            case "bankreportgroup"  : return "Отчетность для банков";
            case "biogroup"         : return "Экология";
            case "consreportgroup"  : return "Консолидированная отчетность";
            case "corpreportgroup"  : return "Корпоративная отчетность";
            case "director"         : return "Утвержденный директорат";
            case "draftdirector"    : return "Черновой директорат";
            case "finres"           : return "Финансовые результаты";
            case "free_active2011"  : return "Чистые активы";
            case "invest_group"     : return "Инвестиции";
            case "kovenant"         : return "Ковенанты";
            case "momeygroup"       : return "Денежные средства";
            case "osnpok"           : return "Основные показатели";
            case "prib2011"         : return "Прибыли и убытки";
            case "zatr"             : return "Затраты";
            case "nalog_group"      : return "Налоги";
            case "rasbal"           : return "Расшифровки баланса";
            case "energogroup"      : return "Энергетика";
            case "testgroup"        : return "Тестовые";
            case "calcgroup"        : return "Калькуляции";
            case "ras_dk"           : return "Дебиторы и кредиторы";
            default                 : return "";
        }
    };
    $(window.zefs).on(window.zefs.handlers.on_formsload, function() {
        $.each(zefs.forms, function(i,f) {
            var groupname = GetReadableGroupName(f.Group);
            var parentgroupname = GetReadableParentName(f.Parent);
            var ul = menu.find('ul[code="' + f.Group + '"]');
            if (ul.length == 0) {
                ul = $('<ul class="dropdown-menu"/>').attr("code", f.Group);
                if (groupname != ""){
                    menu.append($('<li class="dropdown-submenu"/>')
                        .append($('<a/>').text(groupname), ul));
                }
            }
            var a = $('<a/>').text(f.Name).attr("formcode", f.Code);
            var li = $('<li/>').append(a);
            if (!!f.Parent && parentgroupname != "") {
                var parent = menu.find('ul[code="' + f.Parent + '"]');
                if (parent.length == 0) {
                    parent = $('<ul class="dropdown-menu"/>').attr("code", f.Parent);
                    ul.append($('<li class="dropdown-submenu"/>')
                        .append($('<a/>').text(parentgroupname), parent));
                }
                parent.append(li);
                if (f.Parent == f.Code) parent.append($('<li class="divider"/>'));
            } else {
                ul.append(li);
            }
            a.click(function(e) {{
                ChangeForm(this, e.ctrlKey);
            }});
        });
        if (null == zefs.myform.startError) {
            var current = window.zefs.myform.currentSession.FormInfo.Code || "";
            $('a[formcode="' + current.replace(/[A|B].in/, "") + '"]').parents('li').addClass("current");
        }
    });
    zefsbizprocess.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsbizprocess);
}(window.jQuery);