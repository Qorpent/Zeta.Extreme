/**
 * Виджет заголовка таблицы
 */
!function($) {
    var zefsformheader = new root.security.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true, priority: 100 });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        var s = window.zefs.myform.currentSession;
        $(h.find('span').first()).text(zefs.getperiodbyid(s.Period));
        if (!!s.FormInfo.HoldResponsibility) {
            var u = $('<span class="label label-success" style="display: none;"/>').text(s.FormInfo.HoldResponsibility).css("margin-left", 3);
            h.append($('<span class="label label-success"/>').html('Куратор формы <i class="icon icon-white"/>').click(function() {u.trigger('click')}),u);
            u.zetauser();
        }
        s = null;
    }
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function() {
        h.html(
            zefs.myform.currentSession.FormInfo.Name + " " +
            zefs.myform.currentSession.ObjInfo.Name + " за <span></span>, " +
            // zefs.myform.currentSession.getPeriod() + ", " +
            zefs.myform.currentSession.Year + " год"
        );
        InsertPeriod();
    });
    window.zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(window.zefs.periods)) {
            window.zefs.periods = result;
            $(window.zefs).trigger(window.zefs.handlers.on_periodsload);
        }
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);