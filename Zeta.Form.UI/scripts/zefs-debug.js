/**
 * Виджет инструментов для отладки
 */
!function($) {
    var zefsdebug = new root.security.Widget("zefsdebug", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 90, adminonly: true });
    var session = $('<a id="sessionInfo"/>')
        .click(function(e) { debug("zefs/session.json.qweb?session=" + $(this).attr("uid"), "Данные о сессии") })
        .html('<i class="icon-globe"></i> Информация о сессии');
    var debuginfo = $('<a id="debugInfo"/>')
        .click(function(e) { debug("zefs/debuginfo.json.qweb?session=" + $(this).attr("uid"), "Отладочные данные") })
        .html('<i class="icon-warning-sign"></i> Отладочная информация');
    var restart = $('<a id="restartInfo"/>')
        .click(function(e) { debug("zefs/restart.json.qweb", "Рестарт приложения") })
        .html('<i class="icon-repeat"></i> Рестарт');
    var lock = $('<a id="currentlockInfo"/>')
        .click(function(e) { debug("zefs/currentlockstate.json.qweb?session=" + $(this).attr("uid"), "Статус блокировки") })
        .html('<i class="icon-lock"></i> Текущий статус');
    var serverstatus = $('<a id="serverInfo"/>')
        .click(function(e) { debug("zefs/server.json.qweb", "Статус сервера") })
        .html('<i class="icon-tasks"></i> Статус сервера');
    var canlock = $('<a id="canlockInfo"/>')
        .click(function(e) { debug("zefs/canlockstate.json.qweb?session=" + $(this).attr("uid"), "Возможность блокировки") })
        .html('<i class="icon-lock"></i> Возможность блокровки');

    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Отладка" data-toggle="dropdown"/>')
        .html('<i class="icon-eye-close"></i><span class="caret"></span>');
    var btngroup = $('<div class="btn-group pull-right"/>').append(
        b, $('<ul class="dropdown-menu"/>').append(
            $('<li/>').append(lock),$('<li/>').append(canlock),$('<li/>').append(session),$('<li/>').append(debuginfo),$('<li class="divider"/>'),
            $('<li/>').append(serverstatus),$('<li class="divider"/>'), $('<li/>').append(restart)
        ));
    b.tooltip({placement: 'bottom'});
    var debug = function(command, title) {
        var modal = $('<div class="modal" role="dialog" />');
        var iframe = $('<iframe id="debugResult"/>').attr("src", command);
        var modalheader = $("<div/>", {"class":"modal-header"}).append(
            $('<button type="button" class="close" data-dismiss="modal" aria-hidden="true" />').html("&times;"),
            $('<h3/>').text(title));
        var modalbody = $('<div class="modal-body" />').append(iframe);
        var modalfooter = $('<div class="modal-footer"/>').append(
            $('<a href="#" class="btn btn-primary" data-dismiss="modal" />')
                .html("Закрыть"));
        modal.append(modalheader, modalbody, modalfooter);
        $(modal).modal({backdrop: false});

        // Убиваем окно после его закрытия
        $(modal).on('hidden', function(e) { $(this).remove() });
        $(modal).draggable();
    }
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        session.attr("uid",zefs.myform.sessionId);
        debuginfo.attr("uid",zefs.myform.sessionId);
        lock.attr("uid",zefs.myform.sessionId);
        canlock.attr("uid",zefs.myform.sessionId);
    });
    zefsdebug.body = $('<div/>').append(btngroup);
    root.console.RegisterWidget(zefsdebug);
}(window.jQuery);