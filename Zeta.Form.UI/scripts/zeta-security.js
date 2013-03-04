var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zeta = window.zeta || {};
root.handlers = $.extend(root.handlers, {
    // Zeta handlers:
    on_zetaready : "zetaready",
    on_zetafailed : "zetafailed",
    // Login handlers:
    on_loginsuccess : "loginsuccess",
    on_loginfaild : "loginfaild",
    on_logout : "logount",
    on_impersonate : "impersonate",
    on_deimpersonate : "deimpersonate",
    on_getuserinfo : "getuserinfo"
});

root.security = root.security || $.extend(root.security, {
    user : null,
    auth : null
});

!function($) {
    var options = window.zeta.options;

    var Console = function() {

    };

    Console.prototype = {
        widgets : [],
        RegisterWidget : function(e) {
            this.widgets.push(e);
        },
        // Setup layout
        layout : {
            position : {
                layoutHeader : "header",
//              layoutBodyLeft : "body-left",
                layoutBodyMain : "body-main"
            },
            header : $('<div id="consoleHeader"/>').append(
                $('<div class="navbar"/>').append(
                    $('<div class="navbar-inner"/>')
                )
            ),
            body : $('<div id="consoleBody" class="console-body"/>'),
            /*.append($('<div/>').append($('<section/>'))
                $('<div class="span3 console-sidebar" />').append($('<ul class="nav nav-list"/>')),
                 $('<div class="span9" />').append($('<section/>'))
            ),*/
            footer : $('<div id="consoleFooter"/>'),
            add : function(e) {
                $(e.body).addClass(e.name)
                if (e.float != "none") $(e.body).addClass("pull-" + e.float)
                if (e.pos == this.position.layoutHeader) $('#consoleHeader > .navbar > .navbar-inner').append(e.body);
                //  if (e.pos == this.position.layoutBodyLeft) $('#consoleBody > .row > .span3 > .nav-list').append(e.body);
                if (e.pos == this.position.layoutBodyMain) $('#consoleBody').append(e.body);
            }
        },

        Setup : function() {
            $.ajax({
                url: siteroot+root.options.whoami_command,
                context: this,
                dataType: 'json'
            }).success($.proxy(function(d) {
                root.security.user = root.options.asUserInfo(d);
                $('body').append(this.layout.header,this.layout.body,this.layout.footer);
                $.each(this.widgets.sort(function(a,b) { return b.options.priority - a.options.priority }),
                    $.proxy(function(i, e) {
                        if ((root.security.user != null && root.security.user.getLogonName() != "" ) || !e.options.authonly) {
                            if (e.options.adminonly && root.security.user != null) {
                                if (!root.security.user.getIsAdmin()) return;
                            }
                            if (!this.widgets[i].installed) {
                                this.layout.add(e);
                                if (e.options.ready != null) e.options.ready();
                                this.widgets[i].installed = true;
                            }
                        }
                    }, this));
            }, this));
        },

        Uninstallwidgets: function() {
            $.each(this.widgets, function() {
                if (this.installed && this.options.authonly) {
                    this.body.remove();
                    this.installed = false;
                }
            }, this);
        }
    }

    Console.prototype.whoami = function() {
        $.ajax({
            url: siteroot+root.options.whoami_command,
            context: this,
            dataType: 'json'
        }).success($.proxy(function(d) {
            root.security.user = root.options.asUserInfo(d);
            $(root).trigger(root.handlers.on_getuserinfo);
        }, this));
    };

    Console.prototype.authorize = function(l,p) {
        $.ajax({
            url: siteroot+root.options.login_command,
            type: "POST",
            context: this,
            data: {
                _l_o_g_i_n_: l,
                _p_a_s_s_: p
            },
            dataType: 'json'
        }).success($.proxy(function(d) {
            var auth = root.options.asAuth(d);
            root.security.auth = root.options.asAuth(d);
            $(root).trigger(auth.getIsLogin() ? root.handlers.on_loginsuccess : root.handlers.on_loginfaild);
        }, this));
    };

    Console.prototype.unauthorize = function() {
        $.ajax({
            url: siteroot+root.options.logout_command,
            context: this,
            dataType: 'json'
        }).complete($.proxy(function(d) {
            $(root).trigger(root.handlers.on_logout);
        }, this));
    };

    Console.prototype.impersonate = function(l) {
        $.ajax({
            url: siteroot+root.options.impersonate_command,
            type: "POST",
            context: this,
            data: { Target : l},
            dataType: 'json'
        }).success($.proxy(function(d) {
            $(root).trigger(l != null ? root.handlers.on_impersonate : root.handlers.on_deimpersonate);
        }, this));
    };

    root.security.Widget = function(n, p, f, o) {
        this.name = n != null ? n : "widget";
        this.pos = p != null ? p : "none";
        this.float = f != null ? f : "none";
        this.options = $.extend({
            priority: 0,
            authonly: true,
            adminonly: false,
            // Функция которая вызывается после того как виджет добавлен
            ready: null
        }, o);
        this.body = null;
        this.installed = false;
    }

    root.console = new Console();
}(window.jQuery);

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

/**
 * Виджет формы авторизации пользователя
 */
!function($) {
    var l = $('<input class="input-small" type="text" placeholder="Логин" autocomplete/>');
    var p = $('<input class="input-small" type="password" placeholder="Пароль"/>');
    var f = $('<form/>', { "class": "navbar-form login-form"})
        .submit(function(e) {
            e.preventDefault();
            authorize();
        })
        .append(l, p,
        $("<button/>", {
            "class" : "btn btn-small",
            "type" : "submit",
            "text" : "Войти"
        })
    ).hide();
    var authbtn = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Вход от имени"/>')
        .html('<i class="icon-user"></i><span class="caret"></span>');
    var implogin = $('<input class="input-small" type="text"/>');
    var deimp = $('<li/>').append($('<button class="btn"/>').click(function() { root.console.impersonate() }).text("Вернуться в свой логин"));
    var imp = $('<li/>').append(implogin, $('<button class="btn"/>').click(function() { root.console.impersonate(implogin.val()) }).text("Войти от..."));
    var menu = $('<ul class="dropdown-menu"/>').append(
        deimp.hide(), imp,
        $('<li/>').append($('<button class="btn "/>').click(function() { root.console.unauthorize() }).text("Выход из системы"))
    );
    var m = $('<div class="btn-group pull-right"/>').append(
        authbtn, menu).hide();

    var authorize = function() {
        root.console.authorize(l.val(), p.val());
    }

    $(window.zeta).on(window.zeta.handlers.on_loginsuccess, function(e) {
        window.zeta.console.Setup();
        f.hide();
        m.show();
    });

    $(window.zeta).on(window.zeta.handlers.on_deimpersonate, function(e) {
        deimp.hide();
        imp.show();
    });

    $(window.zeta).on(window.zeta.handlers.on_impersonate, function(e) {
        imp.hide();
        deimp.show();
    });

    $(window.zeta).on(window.zeta.handlers.on_logout, function(e) {
        location.reload();
    });

    $(document).on('click.dropdown.data-api', '.authorizer li', function (e) {
        e.stopPropagation();
    });

    var authorizer = new root.security.Widget("authorizer", root.console.layout.position.layoutHeader, "right", { authonly: false, priority: 100, ready: function() {
        if (window.zeta.security.user.getLogonName() != "") {
            m.show();
            if (window.zeta.security.user.getImpersonation() != "") {
                deimp.show();
                imp.hide();
            }
        } else {
            f.show();
        }
    }});
    authorizer.body = $('<div/>').append(f,m);
    root.console.RegisterWidget(authorizer);
}(window.jQuery);

/**
 * Виджет информации о текущем польвателе
 */
!function($) {
    var l = $('<span class="login-user label label-inverse" />');
    var ConfigurePermissions = function() {
        if (window.zeta.security.user != null) {
            if (window.zeta.security.user.getLogonName() != "") {
                l.text(window.zeta.security.user.getLogonName());
                var a = window.zeta.security.user;
                var t = $('<div/>').append($('<ul class="login-permissions"/>').css({
                    "list-style-type" : "none",
                    "margin": "0",
                    "text-align": "left"
                }).append(
                    $("<li/>").html(a.getIsAdmin() ? 'Administrator<span>YES</span>' : 'Administrator NO'),
                    $("<li/>").html(a.getIsDeveloper() ? 'Developer<span>YES</span>' : 'Developer NO'),
                    $("<li/>").html(a.getIsDataMaster() ? 'Datamaster<span>YES</span>' : 'Datamaster NO')
                ));
            }
        }
        l.tooltip({title:t.html(), placement: 'bottom', html: true});
    }
    $(window.zeta).on(window.zeta.handlers.on_getuserinfo, function() {
        ConfigurePermissions();
    });
    var logininfo = new root.security.Widget("logininfo", root.console.layout.position.layoutHeader, "right", { authonly: false, adminonly: true, ready: function() {
        ConfigurePermissions();
    }});
    logininfo.body = $('<div/>').append(l);
    root.console.RegisterWidget(logininfo);
}(window.jQuery);

/**
 * Виджет индикатора состояния сервера.
 * Временно отключен, так как планируется передалать на Алерты, выпадающие сверху таблицы
 */
!function($) {
    var zefsstatus = new root.security.Widget("zefsstatus", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var status = $('<span class="label"/>').text("Статус сервера");
    zefsstatus.body = $('<div/>').append(status);
    //root.console.registerWidget(zefsstatus);
}(window.jQuery);

/**
 * Виджет инструмента для сохранения формы
 */
!function($) {
    var zefsformsave = new root.security.Widget("zefsformsave", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var b = $('<button class="btn btn-small btn-primary" title="Сохранить форму" />').html('<i class="icon-ok icon-white"/>');
    b.click(function(e) {
       zefs.myform.save(window.zefs.myform.getChanges());
    });
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (!zefs.myform.lock) {
            b.attr("disabled", "disabled");
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_savestart, function() {
        b.attr("disabled", "disabled");
    });
    $(window.zefs).on(window.zefs.handlers.on_savefinished, function() {
        b.removeAttr("disabled");
    });
    zefsformsave.body = $('<div/>').append(b);
    b.tooltip({placement: 'bottom'});
    root.console.RegisterWidget(zefsformsave);
}(window.jQuery);

/**
 * Виджет обратной связи
 */
!function($) {
    var feedback = new root.security.Widget("feedback", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var b = $('<button class="btn btn-small btn-warning" data-original-title="Обратная связь" />').html('<i class="icon-envelope icon-white"></i>');
    b.tooltip({placement: 'bottom'});
    feedback.body = $('<div/>').append(b);
    root.console.RegisterWidget(feedback);
}(window.jQuery);

/**
 * Виджет информационного меню
 */
!function($) {
    var information = new root.security.Widget("information", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Информация"/>').html('<i class="icon-book"></i><span class="caret"></span>');
    var m = $('<div class="btn-group"/>').append(
        b, $('<ul class="dropdown-menu"/>').append(
            $('<li/>').html('<a>Инструкция пользователя</a>'),
            $('<li/>').html('<a>Справка по форме</a>'),
            $('<li class="divider"/>'),
            $('<li/>').html('<a>О программе</a>')
        ));
    b.tooltip({placement: 'bottom'});
    information.body = $('<div/>').append(m);
    root.console.RegisterWidget(information);
}(window.jQuery);

/**
 * Виджет заголовка таблицы
 */
!function($) {
    var zefsformheader = new root.security.Widget("zefsformheader", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    var h = $('<h3/>');
    zefsformheader.body = $('<div/>').append(h);
    var InsertPeriod = function() {
        $(h.find('span').first()).text(zefs.getperiodbyid(window.zefs.options.getParameters().period));
    }
    $(window.zefs).on(window.zefs.handlers.on_sessionload, function() {
        h.html(
           zefs.myform.currentSession.getFormInfo().getName() + " " +
           zefs.myform.currentSession.getObjInfo().getName() + " за <span></span>, " +
        // zefs.myform.currentSession.getPeriod() + ", " +
           zefs.myform.currentSession.getYear() + " год"
        );
        if (window.zefs._periods_loaded){
            InsertPeriod();
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_periodsload, function() {
        window.zefs._periods_loaded = true;
        InsertPeriod();
    });
    root.console.RegisterWidget(zefsformheader);
}(window.jQuery);

/**
 * Виджет Zefs-формы
 */
!function($) {
    var zefsform = new root.security.Widget("zefsform", root.console.layout.position.layoutBodyMain, null, { authonly: true, ready: function() {
       zefs.init(jQuery);
       zefs.myform.run();
    } });
    zefsform.body = $('<table class="data" id="zefsForm"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        if (!zefs.myform.lock) {
            zefsform.body.addClass("isblocked");
        }
    });
    root.console.RegisterWidget(zefsform);
}(window.jQuery);

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
/*        $('<ul class="dropdown-menu"/>').append(
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Месяцы'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="11">Январь</a>'),
                    $('<li/>').html('<a value="12">Февраль</a>'),
                    $('<li/>').html('<a value="13">Март</a>'),
                    $('<li/>').html('<a value="14">Апрель</a>'),
                    $('<li/>').html('<a value="15">Май</a>'),
                    $('<li/>').html('<a value="16">Июнь</a>'),
                    $('<li/>').html('<a value="17">Июль</a>'),
                    $('<li/>').html('<a value="18">Август</a>'),
                    $('<li/>').html('<a value="19">Сентябрь</a>'),
                    $('<li/>').html('<a value="110">Октябрь</a>'),
                    $('<li/>').html('<a value="111">Ноябрь</a>'),
                    $('<li/>').html('<a value="112">Декабрь</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('С начала года'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="1">3 мес.</a>'),
                    $('<li/>').html('<a value="2">6 мес.</a>'),
                    $('<li/>').html('<a value="3">9 мес.</a>'),
                    $('<li/>').html('<a value="4">12 мес.</a>'),
                    $('<li/>').html('<a value="42">II квартал</a>'),
                    $('<li/>').html('<a value="43">III квартал</a>'),
                    $('<li/>').html('<a value="44">IV квартал</a>'),
                    $('<li/>').html('<a value="46">II п/г</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Промежуточные периоды'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="22">2 мес.</a>'),
                    $('<li/>').html('<a value="24">4 мес.</a>'),
                    $('<li/>').html('<a value="25">5 мес.</a>'),
                    $('<li/>').html('<a value="27">7 мес.</a>'),
                    $('<li/>').html('<a value="28">8 мес.</a>'),
                    $('<li/>').html('<a value="210">10 мес.</a>'),
                    $('<li/>').html('<a value="211">11 мес.</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Плановые периоды'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="301">ТПФП (действующий)</a>'),
                    $('<li/>').html('<a value="3512">ТПФП, на директорат</a>'),
                    $('<li/>').html('<a value="303">План I кв.</a>'),
                    $('<li/>').html('<a value="306">План I пг.</a>'),
                    $('<li/>').html('<a value="309">План 9 мес.</a>'),
                    $('<li/>').html('<a value="642">План II кв.</a>'),
                    $('<li/>').html('<a value="643">План III кв.</a>'),
                    $('<li/>').html('<a value="644">План IV кв.</a>'),
                    $('<li/>').html('<a value="646">План II пг.</a>'),
                    $('<li/>').html('<a value="251">ТПФП, версия 1</a>'),
                    $('<li/>').html('<a value="252">ТПФП, версия 2</a>'),
                    $('<li/>').html('<a value="253">ТПФП, версия 3</a>'),
                    $('<li/>').html('<a value="254">ТПФП, версия 4</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Коррективы плана'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="31">Корректив 1</a>'),
                    $('<li/>').html('<a value="32">Корректив 2</a>'),
                    $('<li/>').html('<a value="33">Корректив 3</a>'),
                    $('<li/>').html('<a value="34">Корректив 4</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Ожидаемые периоды'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="403">Ожидаемое за I кв.</a>'),
                    $('<li/>').html('<a value="416">Ожидаемое за июнь</a>'),
                    $('<li/>').html('<a value="406">Ожидаемое за I пг.</a>'),
                    $('<li/>').html('<a value="409">Ожидаемое за 9 мес.</a>'),
                    $('<li/>').html('<a value="401">Ожидаемое за год</a>')
                )
            ),
            $('<li class="dropdown-submenu"/>').append(
                $('<a/>').text('Дополнительно'),
                $('<ul class="dropdown-menu"/>').append(
                    $('<li/>').html('<a value="302">Общий план</a>'),
                    $('<li/>').html('<a value="-1">Начало года</a>'),
                    $('<li/>').html('<a value="1000">Конец года</a>'),
                    $('<li/>').html('<a value="-99">Некорректный период</a>')
                )
            )
        )*/
    zefsperiodselector.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsperiodselector);
}(window.jQuery);

/**
 * Виджет списка предприятий
 */
!function($) {
   var zefsobjselector = new root.security.Widget("objselector", root.console.layout.position.layoutHeader, "left", { authonly: true });
   var list = $('<div class="btn-group"/>');
   var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Предприятие"/>').html('<i class="icon-map-marker"></i><span class="caret"/>');
   var menu = $('<ul class="dropdown-menu"/>');
   list.append(b,menu);
   b.tooltip({placement: 'bottom'});
   var ChangeObject = function(e) {
       location.hash = location.hash.replace(/obj=\d+/gi,"obj=" + $(e.target).attr("value"));
       location.reload();
   };
   $(window.zefs).on(window.zefs.handlers.on_objectsload, function(e) {
       $.each(window.zefs.divs, function(i,div) {
           menu.append($('<li class="dropdown-submenu"/>')
               .append($('<a/>').text(div.getName()), $('<ul class="dropdown-menu"/>').attr("code", div.getCode())));
       });
       $.each(window.zefs.objects, function(i,obj) {
           var ul = menu.find('ul[code=' + obj.getDivCode() + ']');
           if (ul.length != 0) {
               var a = $('<a/>').attr("value", obj.getId());
               a.click(function(e) {
                   ChangeObject(e);
               });
               a.text(obj.getName());
               ul.append($('<li/>').append(a));
           }
       });
   });
   zefsobjselector.body = $('<div/>').append(list);
   root.console.RegisterWidget(zefsobjselector);
}(window.jQuery);

/**
 * Виджет менеджера колонок
 */
!function($) {
    var zefscolmanager = new root.security.Widget("zefscolmanager", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Управление колонками"/>')
        .html('<i class="icon-list"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    list.append(b,menu);
    var HideColumn = function(n) {
        $('table.data col[idx=' + n + ']').hide();
        $('table.data th[idx=' + n + ']').hide();
        $('table.data td[idx=' + n + ']').hide();
        $(window).trigger("resize");
    };
    var ShowColumn = function(n) {
        $('table.data col[idx=' + n + ']').show();
        $('table.data th[idx=' + n + ']').show();
        $('table.data td[idx=' + n + ']').show();
        $(window).trigger("resize");
    };
    $(document).on('click.dropdown.data-api', '.zefscolmanager li', function (e) {
        e.stopPropagation();
        var input = null;
        if (e.target.tagName == "INPUT") {
            input = $(e.target);
        } else {
            return;
        }
        if (input.is(":checked")) {
            ShowColumn(input.val());
        } else {
            HideColumn(input.val());
        }
    });
    $(window.zefs).on(window.zefs.handlers.on_structureload, function(e) {
        $.each(($('table.data>thead>tr').first()).children(), function(i,col) {
            var li = $('<li/>');
            if ($(col).hasClass("primary")) li.addClass("primary");
            var input = $('<input type="checkbox" checked/>').attr("value", $(col).attr("idx") || "");
            if (/number|name|measure/.test(col.className)) {
                li.addClass("disabled");
                input.attr("disabled","disabled");
            }
            menu.append(li.append($('<a/>').append($('<label/>').append(input, $(col).text()))));
        });
    });
    b.tooltip({placement: 'bottom'});
    zefscolmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefscolmanager);
}(window.jQuery);

/**
 * Виджет менеджера блокировок
 */
!function($) {
    var zefsblockmanager = new root.security.Widget("zefsblockmanager", root.console.layout.position.layoutHeader, "left", { authonly: true });
    var list = $('<div class="btn-group"/>');
    var b1 = $('<button class="btn btn-success"/>').text("Утв.");
    var b2 = $('<button class="btn btn-warning"/>').text("Заблок.");
    var b3 = $('<button class="btn btn-danger"/>').text("Разблок.");
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Управление блокировками"/>')
        .html('<i class="icon-lock"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    menu.append($('<li/>').append($('<div class="btn-group" data-toggle="buttons-radio"/>').append(
        b1,b2,b3
    )));
    var history = $('<table class="table table-striped"/>');
    menu.append($('<li/>').append(history));
    history.append(
        $('<thead/>').append($('<tr/>').append($('<th/>').text("История блокировок"))),
        $('<tbody/>').append($('<tr/>').append($('<td/>').text("Форма не блокировалась")))
    );
    history.css({"margin":"0 5px", "width": 210});
    list.append(b,menu);
    $(document).on('click.dropdown.data-api', '.zefsblockmanager li', function (e) {
        e.stopPropagation();
    });
    b.tooltip({placement: 'bottom'});
    zefsblockmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsblockmanager);
}(window.jQuery);

/**
 * Виджет окна с сообщениями
 */
!function($) {
    var zefsalerter = new root.security.Widget("zefsalerter", root.console.layout.position.layoutBodyMain, null, { authonly: true });
    var container = $('<div/>');
    var ShowMessage = function(p) {
        p = $.extend({
            text: "",
            content: null,
            type: "",
            autohide: 0,
            fade: false,
            width: 0
        },p);
        var message = $('<div class="alert"/>');
        if (p.type != "") message.addClass(p.type);
        if (p.fade) message.addClass("fade in");
        message.append($('<a class="close" data-dismiss="alert"/>').html('&times;'));
        message.append(p.content || p.text);
        if (p.autohide != 0) {
            window.setTimeout(function() {
                message.remove();
            }, p.autohide);
        }
        if (p.width != 0) message.css("width", p.width);
        container.append(message);
    };
    $(window.zefs).on(window.zefs.handlers.on_message, function(e,params) {
        ShowMessage(params);
    });
    zefsalerter.body = $(container).append();
    root.console.RegisterWidget(zefsalerter);
}(window.jQuery);


!function($) {
    window.zeta.console.Setup();
}(window.jQuery);
