/**
 * Виджет списка бизнесс процессов
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsbizprocess = new root.security.Widget("zefsbizprocess", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Список форм"/>').html('<i class="icon-list-alt"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    b.tooltip({placement: 'bottom'});
    var ChangeForm = function(a) {
        location.hash = location.hash.replace(/(form=)(\w+)(\w\.in)/,'$1' + $(a).attr("code") + '$3');
        location.reload();
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
            default                 : return "";
        }
    };
    $(window.zefs).on(window.zefs.handlers.on_formsload, function() {
        var forms = window.zefs.forms;
        $.each(forms, function(i,f) {
            var groupname = GetReadableGroupName(f.Group);
            var parentgroupname = GetReadableParentName(f.Parent);
            var ul = menu.find('ul.' + f.Group);
            if (ul.length == 0) {
                ul = $('<ul class="dropdown-menu"/>').addClass(f.Group);
                if (groupname != ""){
                    menu.append($('<li class="dropdown-submenu"/>')
                        .append($('<a/>').text(groupname), ul));
                }
            }
            var a = $('<a/>').text(f.Name).attr("code", f.Code);
            var li = $('<li/>').append(a);
            if (!!f.Parent) {
                var parent = menu.find('ul.' + f.Parent);
                if (parent.length == 0) {
                    parent = $('<ul class="dropdown-menu"/>').addClass(f.Parent);
                    if (parentgroupname != ""){
                        ul.append($('<li class="dropdown-submenu"/>')
                            .append($('<a/>').text(parentgroupname), parent));
                    }
                }
                parent.append(li);
                if (f.Parent == f.Code) parent.append($('<li class="divider"/>'));
            } else {
                ul.append(li);
            }
            a.click(function() { ChangeForm(this); });
        });
    });
    zefsbizprocess.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsbizprocess);
}(window.jQuery);