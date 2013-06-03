/**
 * Виджет инструментов для отладки
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var spec = window.zefs.api;
    var zefsdebug = new root.Widget("zefsdebug", root.console.layout.position.layoutHeader, "right", { authonly: true, priority: 90, adminonly: true });
    var sid = "";
    var session = $('<a id="sessionInfo"/>')
        .click(function() { Debug("zefs/session.json.qweb?session=" + sid, "Данные о сессии") })
        .html('<i class="icon-globe"></i> Информация о сессии');
    var formuladebuger = $('<a/>')
        .click(function() { window.zefs.myform.openformuladebuger() })
        .html('<i class="icon-pencil"></i> Отладка формул');
    var setup = $('<a/>')
        .click(function() { window.zefs.myform.setupform() })
        .html('<i class="icon-wrench"></i> Настройка формы');
    var debuginfo = $('<a id="debugInfo"/>')
        .click(function() { Debug("zefs/debuginfo.json.qweb?session=" + sid, "Отладочные данные") })
        .html('<i class="icon-warning-sign"></i> Отладочная информация');
    var lock = $('<a id="currentlockInfo"/>')
        .click(function() { Debug("zefs/currentlockstate.json.qweb?session=" + sid, "Статус блокировки") })
        .html('<i class="icon-lock"></i> Текущий статус');
    var serverstatus = $('<a id="serverInfo"/>')
        .click(function() { Debug(spec.server.state) })
        .html('<i class="icon-tasks"></i> Статус сервера');
    var canlock = $('<a id="canlockInfo"/>')
        .click(function() { Debug("zefs/canlockstate.json.qweb?session=" + sid, "Возможность блокировки") })
        .html('<i class="icon-lock"></i> Возможность блокровки');
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Отладка"/>')
        .html('<i class="icon-eye-close"></i><span class="caret"></span>');
    var list = $('<ul class="dropdown-menu"/>').append(
        $('<li/>').append(setup),$('<li/>').append(formuladebuger),$('<li class="divider"/>'),
        $('<li/>').append(lock),$('<li/>').append(canlock),$('<li/>').append(session),$('<li/>').append(debuginfo),$('<li class="divider"/>'),
        $('<li/>').append(serverstatus)
    );




    var implogin = $('<input class="input-small" type="text" placeholder="Войти от..."/>')
        .css({"width": 120, "margin": "2px 40px"})
        .change(function() {
            root.console.impersonate({Target: implogin.val()});
            $(document).trigger('click.dropdown.data-api');
        });
    var deimp = $('<li/>').append($('<button class="btn btn-mini"/>')
            .text("Вернуться в свой логин"))
        .css("margin", "2px 40px")
        .click(function() {
            root.console.impersonate();
            implogin.val("");
        }).hide();

    $(zeta).on(window.zeta.handlers.on_getuserinfo, function() {
        if (zeta.user.getLogonName() != "") {
            if (zeta.user.getImpersonation()) {
                deimp.show();
                implogin.hide();
            }
        }
    });



    list.append($('<li class="divider"/>'));
    list.append($('<li/>').append(implogin));
    list.append($('<li/>').append(deimp));
    list.append($('<li/>').append(
        $('<button class="btn btn-mini"/>').click(function() { root.console.unauthorize() })
            .css("margin", "2px 40px")
            .text("Выйти из системы")
    ));
    var btngroup = $('<div class="btn-group pull-right"/>').append(b, list);
    b.tooltip({placement: 'bottom'});
    b.dropdownHover({delay: 100});
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
    };
    $(window.zeta).on(root.handlers.on_getuserinfo, function() {
        if (zeta.user.getIsBudget()) {
            var oldform = $('<a/>').html('<i class="icon-th-large"></i> Открыть старую форму');
            list.prepend($('<li/>').append(oldform));
            oldform.click(function() {
                window.zefs.myform.openoldform();
            });
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        sid = zefs.myform.sessionId;
    });
    zefsdebug.body = $('<div/>').append(btngroup);
    root.console.RegisterWidget(zefsdebug);
}(window.jQuery);