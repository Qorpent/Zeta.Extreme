/**
 * Виджет инструментов для отладки
 */
!function($) {
    var spec = window.zefs.specification;
    var zefsdebug = new root.security.Widget("zefsdebug", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 90, adminonly: true });
    var sid = "";
    var session = $('<a id="sessionInfo"/>')
        .click(function(e) { Debug("zefs/session.json.qweb?session=" + sid, "Данные о сессии") })
        .html('<i class="icon-globe"></i> Информация о сессии');
    var debuginfo = $('<a id="debugInfo"/>')
        .click(function(e) { Debug("zefs/debuginfo.json.qweb?session=" + sid, "Отладочные данные") })
        .html('<i class="icon-warning-sign"></i> Отладочная информация');
    var restart = $('<a id="restartInfo"/>')
        .click(function(e) { Debug(spec.server.restart) })
        .html('<i class="icon-repeat"></i> Рестарт');
    var lock = $('<a id="currentlockInfo"/>')
        .click(function(e) { Debug("zefs/currentlockstate.json.qweb?session=" + sid, "Статус блокировки") })
        .html('<i class="icon-lock"></i> Текущий статус');
    var serverstatus = $('<a id="serverInfo"/>')
        .click(function(e) { Debug(spec.server.state) })
        .html('<i class="icon-tasks"></i> Статус сервера');
    var canlock = $('<a id="canlockInfo"/>')
        .click(function(e) { Debug("zefs/canlockstate.json.qweb?session=" + sid, "Возможность блокировки") })
        .html('<i class="icon-lock"></i> Возможность блокровки');
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Отладка" data-toggle="dropdown"/>')
        .html('<i class="icon-eye-close"></i><span class="caret"></span>');
    var btngroup = $('<div class="btn-group pull-right"/>').append(
        b, $('<ul class="dropdown-menu"/>').append(
            $('<li/>').append(lock),$('<li/>').append(canlock),$('<li/>').append(session),$('<li/>').append(debuginfo),$('<li class="divider"/>'),
            $('<li/>').append(serverstatus),$('<li class="divider"/>'), $('<li/>').append(restart)
        ));
    b.tooltip({placement: 'bottom'});
    var Debug = function(command, title) {
        var url =  command;
        var title = title;
        if(typeof(command)=="object"){
            url = command.getUrl("json");
            title = command.title;
        }
        var iframe = $('<iframe id="debugResult"/>').css("height", 340).attr("src", url);
        $(window.zeta).trigger(window.zeta.handlers.on_modal, { title: title, content: iframe, width: 700, height: 350});
        iframe = null;
    }
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        sid = zefs.myform.sessionId;
    });
    zefsdebug.body = $('<div/>').append(btngroup);
    root.console.RegisterWidget(zefsdebug);
}(window.jQuery);