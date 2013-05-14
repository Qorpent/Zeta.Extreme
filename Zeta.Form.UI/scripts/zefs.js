(function(){
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
window.zefs.handlers = $.extend(window.zefs.handlers, {
    // Zefs handlers:
    on_zefsready : "zefsready", on_zefsstarting : "zefsstarting", on_zefsfailed : "zefsfailed",
    // Session handlers:
    on_sessionload : "sessionload", on_structureload : "structureload",
    // Form handlers:
    on_dataload : "dataload", on_formready : "forrmready", on_statusload : "statusload",
    on_statusfailed : "statusfaild", on_savestart : "savestart", on_savefailed : "savefaild",
    on_savefinished : "savefinished", on_getcanlockload : "getcanlockload", on_getlockfailed : "getlockfinished",
    on_getlockload : "getlockload", on_getlockhistoryload : "getlockhistory", on_lockform : "lockform",
    // Other handlers:
    on_formsload : "formsload", on_periodsload : "periodsload", on_periodsfaild : "periodsfailed",
    on_objectsload : "objectsload", on_objectsfaild : "objectsfailed", on_attachmentload : "attachmentload",
    on_reglamentload : "reglamentload", on_formusersload : "formusersload",
    // Message handlers:
    on_message : "message",
    // File handlers:
    on_fileloadstart: "fileloadstart", on_fileloadfinish: "fileloadfinish",
    on_fileloaderror: "fileloaderror", on_fileloadprocess: "fileloadprocess",
    // Chat handlers:
    on_chatlistload : "chatlistload", on_adminchatlistload : "adminchatlistload",
    on_adminchatcountload : "adminchatcountload"
});
var root = window.zefs = window.zefs || {};
var api = root.api;
root.periods =  root.periods || {};
root.divs =  root.divs || [];
root.objects =  root.objects || {};
root.init = root.init ||
(function ($) {
    if (root.myform) return root.myform;
    root.myform = root.myform ||  {
        sessionId : null,
        currentSession : null,
        startError : null,
        lock : null,
        lockhistory : null,
        attachment : null,
        users : null
    };
    var render = root.render;

    var ArchiveNews = function(code) {

    };

    var GetNewCount = function() {

    };

    var OpenForm = function(params, blank) {
        params = params || {};
        blank = blank || false;
        var hashparams = api.getParameters();
        params = $.extend(hashparams, params);
        var loc = document.location.protocol + "//" + document.location.host + siteroot + "zefs-test.html#";
        loc += "form=" + (params.form || "");
        loc += "|year=" + (params.year || "");
        loc += "|period=" + (params.period || "");
        loc += "|obj=" + (params.obj || "");
        if (!blank) document.location.href = loc;
        if ((typeof params.form == "undefined" || params.form == "") ||
            (typeof params.year == "undefined" || params.year == "") ||
            (typeof params.period  == "undefined" || params.period == "") ||
            (typeof params.obj == "undefined" || params.obj == "")) {
            return;
        }
        if (!blank) {
            document.location.reload();
        } else {
            window.open(loc, "_blank");
        }
    };

    var Fill = function(session) {
        if(!session.wasRendered) { //вот тут чо за хрень была? он в итоге дважды рендировал
            return;
        }
        for(var batchidx in session.data){
            var batch = session.data[batchidx];
            if(!batch.wasFilled){
                FillBatch(session,batch);
            }
        }
        return session;
    };

	var Render = render.renderStructure; //вынес в рендер - отдельный скрипт
	var FillBatch = render.updateCells; //вынес в рендер - zefs-render.js
    var CheckConditions = render.checkConditions;

    var AttachFile = function(form) {
        var fd = new FormData();
        fd.append("type", form.find('select[name="type"]').val());
        fd.append("filename", form.find('input[name="filename"]').val());
        fd.append("datafile", form.find('input[name="datafile"]').get(0).files[0]);
        fd.append("uid", form.find('input[name="uid"]').val());
        fd.append("session", root.myform.sessionId);
        $(root).trigger(root.handlers.on_fileloadstart);
        api.file.add.execute(fd);
    };

    var DeleteFile = function(uid) {
        api.file.delete.execute({
            session: root.myform.sessionId,
            uid: uid
        });
    };

    var DownloadFile = function(uid) {
        api.file.download.getUrl(uid);
    };

    var ChatList = function() {
        api.chat.list.execute({session: root.myform.sessionId});
        if (zeta.user.getIsAdmin()) {
            api.chat.get.execute(zeta.chatoptionsstorage.Get());
        }
    };

    var ChatAdd = function(m, t) {
        api.chat.add.execute({session: root.myform.sessionId, text: m, type: t});
    };

    var ChatArchive = function(id) {
        api.chat.archive.execute({id: id});
    };

    var ChatRead = function() {
        if (zeta.user.getImpersonation() == null) {
            api.chat.haveread.execute();
        }
    };

    var ChatUpdate = function() {
        api.chat.updatecount.execute();
        window.setTimeout(function(){
            ChatUpdate();
        },60000);
    };

    var ChatUpdateOnce = function() {
        api.chat.updatecount.execute();
    };

    var LockForm = function() {
        var lockinfo = root.myform.lock;
        if (root.myform.sessionId != null && lockinfo != null) {
            if (lockinfo.canblock) {
                if (lockinfo.newstates) {
                    api.lock.set.execute({newstate: "Closed"});
                } else {
                    api.lock.setstateold.execute({state: "0ISBLOCK"});
                }
            } else {
                var message = "";
                if (!!lockinfo.canblockresult) {
                    message = lockinfo.canblockresult.Reason.Message;
                    if (!!lockinfo.canblockresult.Reason.ReglamentCode) {
                        var reglament = $.map(zefs.reglament, function(r) { if (r.ReglamentCode == lockinfo.cancheckresult.Reason.ReglamentCode) return r });
                        if (reglament.length > 0) {
                            message += '<br/><h4>' + reglament[0].Message + '</h4>' + '<p>' + reglament[0].ReglamentDescription + '</p>';
                        }
                    }
                }
                if (lockinfo.noattachedfiles) {
                    message += "<p>Для блокировки формы необходимо прикрепить файлы.</p>"
                }
                if (!!lockinfo.message) {
                    if (lockinfo.message == "cpavoid") {
                        message += "<p>Контрольные точки не сходятся</p>";
                    }
                }
                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                    title: "Форма не может быть заблокирована",
                    content: $("<p/>").html(message)
                });
            }
        }
        lockinfo = null;
    };

    var UnlockForm = function() {
        var lockinfo = root.myform.lock;
        if (root.myform.sessionId != null && lockinfo != null) {
            if (lockinfo.canopen) {
                if (lockinfo.newstates) {
                    api.lock.set.execute({newstate: "Open"});
                } else {
                    api.lock.setstateold.execute({state: "0ISOPEN"});
                }
            } else {
                var message = "";
                if (!!lockinfo.canopenresult) {
                    message = lockinfo.canopenresult.Reason.Message;
                    if (!!lockinfo.canopenresult.Reason.ReglamentCode) {
                        var reglament = $.map(zefs.reglament, function(r) { if (r.ReglamentCode == lockinfo.cancheckresult.Reason.ReglamentCode) return r });
                        if (reglament.length > 0) {
                            message += '<br/><h4>' + reglament[0].Message + '</h4>' + '<p>' + reglament[0].ReglamentDescription + '</p>';
                        }
                    }
                }
                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                    title: "Форма не может быть открыта",
                    content: $("<p/>").html(message)
                });
            }
        }
        lockinfo = null;
    };

    var CheckForm = function() {
        var lockinfo = root.myform.lock;
        if (root.myform.sessionId != null && lockinfo != null) {
            if (lockinfo.cancheck) {
                if (lockinfo.newstates) {
                    api.lock.set.execute({newstate: "Checked"});
                } else {
                    api.lock.setstateold.execute({state: "0ISCHECKED"});
                }
            } else {
                var message = "";
                if (!!lockinfo.cancheckresult) {
                    message = lockinfo.cancheckresult.Reason.Message;
                    if (!!lockinfo.cancheckresult.Reason.ReglamentCode && lockinfo.cancheckresult.Reason.ReglamentCode != "") {
                        var reglament = $.map(zefs.reglament, function(r) { if (r.ReglamentCode == lockinfo.cancheckresult.Reason.ReglamentCode) return r });
                        if (reglament.length > 0) {
                            message += '<br/><h4>' + reglament[0].Message + '</h4>' + '<p>' + reglament[0].ReglamentDescription + '</p>';
                        }
                    }
                }
                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                    title: "Форма не может быть утверждена",
                    content: $("<p/>").html(message)
                });
            }
        }
        lockinfo = null;
    };

    var CellHistory = function(cell) {
        cell = $(cell);
        if (!!cell.data("cellid")) {
            api.metadata.cellhistory.execute({session: root.myform.sessionId, cellid: cell.data("cellid")});
        }
    };

    var CellDebug = function(cell) {
        cell = $(cell);
        if (!!cell.attr("id")) {
            api.metadata.celldebug.execute({session: root.myform.sessionId, key: cell.attr("id")});
        }
    };

    var Save = function() {
        if (!root.myform.lock.cansave
            && !root.myform.lock.cansaveoverblock) {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "Не удалось сохранить форму",
                text: "Форма заблокирована"
            });
            return;
        }
        var obj = window.zefs.getChanges();
        if ($.isEmptyObject(obj)) return;
        root.myform.datatosave = obj;
        $(root).trigger(root.handlers.on_savestart);
        api.data.saveready.execute();
    };

    var ForceSave = function() {
        if (!root.myform.lock.cansave
            && !root.myform.lock.cansaveoverblock) {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "Не удалось сохранить форму",
                text: "Форма заблокирована"
            });
            return;
        }
        root.myform.datatosave = "FORCE";
        $(root).trigger(root.handlers.on_savestart);
        api.data.saveready.execute();
    };

    var Message = function(obj) {
        $(root).trigger(root.handlers.on_message, obj);
    };

    var ZefsIt = function(table) {
        if (!$.isEmptyObject(root.objects))  {
            if (!$.isEmptyObject(root.myobjs)) $('table.data').zefs({ fixHeaderX : 100 });
            else $('table.data').zefs();
        } else {
            window.setTimeout(function(){ ZefsIt(table) },100);
        }
    };

    var GetPeriodName = function(id) {
        var name = "";
        $.each(root.periods, function(i, periodtype) {
            $.each(periodtype.periods, function(i,p) {
                if (p.id == id) {
                    name = p.name;
                    return false;
                }
            });
            if (name != "") return false;
        });
        return name;
    };

    var OpenReport = function() {
        if (!!root.myform.currentSession) {
            var s = root.myform.currentSession;
            window.open(api.siterootold() + "report/render.rails?notemplate=1&tcode=" + s.FormInfo.Code.replace('.in','b.out') +
                "&tp.currentObject=" + s.ObjInfo.Id + "&tp.currentDetail=&tp.year=" + s.Year +
                "&tp.period=" + s.Period, '_blank');
            s = null;
        }
    };

    var OpenOldForm = function() {
        if (!!root.myform.currentSession) {
            var s = root.myform.currentSession;
            window.open(api.siterootold() + "form/fill.rails?template=" + s.FormInfo.Code +
                "&object=" + s.ObjInfo.Id + "&detail=&year=" + s.Year +
                "&period=" + s.Period, '_blank');
            s = null;
        }
    };

    var OpenFormulaDebuger = function() {
        window.open(api.siterootold() + "zeta/debug/index.rails?asworkspace=1", '_blank');
    };

    var SetupForm = function() {
        if (!!root.myform.currentSession) {
            window.open(api.siterootold() + "row/index.rails?root=" + root.myform.currentSession.structure.rootrow, '_blank');
        }
    };

    var Restart = function() {
        api.server.restartall();
    };

    // Обработчики событий
    $(window.zefs).on(window.zefs.handlers.on_renderfinished, function(e, table) {
        ZefsIt(table);
    });

    api.server.ready.onSuccess(function(e, result) {
        if (!!result) {
            api.session.start.execute();
        }
    });

    api.metadata.getobjects.onSuccess(function(e, result) {
        root.divs = result.divs;
        root.objects = result.objs || {};
        root.myobjs = result.my || {};
        $(root).trigger(root.handlers.on_objectsload);
    });

    api.metadata.getreglament.onSuccess(function(e, result) {
        root.reglament = result || {};
        $(root).trigger(root.handlers.on_reglamentload);
    });

    api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(root.periods)) {
            root.periods = result;
            $(root).trigger(root.handlers.on_periodsload);
        }
    });

    api.metadata.getforms.onSuccess(function(e, result) {
        if($.isEmptyObject(root.forms)) {
            root.forms = result;
            $(root).trigger(root.handlers.on_formsload);
        }
    });

    api.metadata.celldebug.onSuccess(function(e, result) {
        var htmlresult = window.zeta.jsformat.jsonObjToHTML(result);
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            width: 900,
            title: "Отладка ячейки",
            content: $(htmlresult).children().first()
        });
        $('.rootKvov').click(function(e) {
            window.zeta.jsformat.generalClick(e);
        });
    });

    api.metadata.getformusers.onSuccess(function(e, result) {
        root.myform.users = result;
        $(root).trigger(root.handlers.on_formusersload);
    });

    api.metadata.cellhistory.onSuccess(function(e, result) {
        if(!$.isEmptyObject(result)) {
            var cellinfotoggle = $('<button class="btn-link"/>').text("Показать/Спрятать полную информацию о ячейке");
            cellinfotoggle.css("padding", "5px 0");
            var cellinfo = $('<table class="table table-bordered"/>').append(
                $('<tr/>').append($('<td/>').text("ID"), $('<td/>').text(result.cell.id)),
                $('<tr/>').append($('<td/>').text("Объект"), $('<td/>').text("(" + result.cell.objid + ") " + result.cell.objname)),
                $('<tr/>').append($('<td/>').text("Колонка"), $('<td/>').text("(" + result.cell.colcode + ") " + result.cell.colname)),
                $('<tr/>').append($('<td/>').text("Строка"), $('<td/>').text("(" + result.cell.rowcode + ") " + result.cell.rowname)),
                $('<tr/>').append($('<td/>').text("Период"), $('<td/>').text("(" + result.cell.period + ") " + window.zefs.getperiodbyid(result.cell.period)))
            ).hide();
            if (!!result.cell.currency) {
                cellinfo.append($('<tr/>').append($('<td/>').text("Валюта"), $('<td/>').text(result.cell.currency)));
            }
            cellinfotoggle.click(function() { cellinfo.toggle() });
            var cellhistory = $('<table class="table table-bordered table-striped"/>');
            var insertvalue = function(value) {
                var zefsform = $('.zefsform').data("zefs");
                zefsform.inputCell();
                $($('.zefsform td.active input').first()).val(value);
                zefsform.uninputCell();
            }
            cellhistory.append(
                $('<thead/>').append(
                    $('<tr/>').append($('<th/>').text("Время"),$('<th/>').text("Пользователь"),$('<th/>').text("Значение"), $('<th/>'))
                ), $('<tbody/>')
            );
            var u = $('<span class="label label-inverse"/>').text(result.cell.user);
            cellhistory.find('tbody').append(
                $('<tr/>').append(
                    $('<td/>').text(result.cell.Date.format("dd.mm.yyyy HH:MM:ss")), $('<td/>').append(u), $('<td/>').text(result.cell.value), $('<td/>')
                )
            );
            u.zetauser();
            if (!$.isEmptyObject(result.history)) {
                $.each(result.history, function(i, e) {
                    var user = $('<span class="label label-inverse"/>').text(e.user);
                    var iv = $('<i class="icon icon-arrow-down"/>');
                    cellhistory.find('tbody').append(
                        $('<tr/>').append(
                            $('<td/>').text(e.Date.format("dd.mm.yyyy HH:MM:ss")), $('<td/>').append(user), $('<td/>').append(e.value), $('<td/>').append(iv)
                        )
                    );
                    iv.click(function() { insertvalue(e.value) });
                    user.zetauser();
                });
            }
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "История ячейки",
                content: $('<div/>').append(cellinfotoggle, cellinfo, cellhistory)
            });
        }
    });

    api.server.restart.onSuccess(function() {
        $(window.zeta).trigger(window.zeta.handlers.on_modal, { title: "Сервер был перезапущен" });
    });

    api.session.start.onSuccess(function(e, result) {
        root.myform.currentSession = result;
        root.myform.sessionId = result.Uid;
        var sessiondata = {session: root.myform.sessionId};
        document.title = result.FormInfo.Name;
        api.session.structure.execute(sessiondata);
        api.metadata.getformusers.execute(sessiondata);
        // получаем ленту сообщений формы
        api.chat.list.execute({session: root.myform.sessionId});
        // если админы, то получаем все ленты сообщений
        api.chat.get.execute(zeta.chatoptionsstorage.Get());
        api.lock.state.execute(sessiondata);
        api.lock.history.execute(sessiondata);
        api.file.list.execute(sessiondata);
        window.setTimeout(function(){
            root.myform.currentSession.data = [];
            api.data.start.execute($.extend(sessiondata, {startidx: 0}))}
        ,1000); //первый запрос на данные
        $(root).trigger(root.handlers.on_sessionload);
    });

    api.session.start.onError(function(e, result) {
        root.myform.startError = JSON.parse(result.responseText);
    });

    api.session.start.onComplete(function() {
        api.metadata.getobjects.execute();
        api.metadata.getperiods.execute();
        api.metadata.getforms.execute();
        api.metadata.getreglament.execute();
        api.metadata.getnews.execute();
    });

    api.session.structure.onSuccess(function(e, result) {
        root.myform.currentSession.structure = result;
        Render(root.myform.currentSession);
        Fill(root.myform.currentSession);
        $(root).trigger(root.handlers.on_structureload);
    });

    api.data.start.onSuccess(function(e, result) {
        root.myform.currentSession.data.push(result);
        Fill(root.myform.currentSession);
        if(result.state != "w"){
            // Это штука для перерисовки шапки
<<<<<<< HEAD
            var notloaded = $('td.notloaded');
            notloaded.removeClass("notloaded");
            notloaded.data({"history": "", "previous": "", "value": ""});
=======
            $('td.notloaded').removeClass("notloaded");
>>>>>>> cae43a77a8760a1520548bb08aa2092266c6e2cb
            $(window).trigger("resize");
            $(root).trigger(root.handlers.on_dataload);
            CheckConditions();
        } else {
            var idx = !$.isEmptyObject(result.data) ? result.ei+1 : result.si;
            window.setTimeout(function(){api.data.start.execute({session: root.myform.sessionId,startidx: idx})},500);
        }
    });

    api.data.reset.onSuccess(function() {
        root.myform.currentSession.data = [];
        api.data.start.execute({session: root.myform.sessionId, startidx: 0});
    });

    api.data.saveready.onSuccess(function(e, result) {
        root.myform.sessionId = result.Uid;
        if ($.isEmptyObject(root.myform.datatosave)) return;
        $.each($('td.recalced'), function(i,e) {
            $(e).removeClass("recalced");
        });
        if (typeof root.myform.datatosave == "object") {
            root.myform.datatosave = JSON.stringify(root.myform.datatosave);
        }
        $(root).trigger(root.handlers.on_message, {
            text: "Сохранение данных формы", autohide: 5000, type: "alert"
        });
        api.data.save.execute({
            session: root.myform.sessionId,
            data: root.myform.datatosave
        });
    });

    api.data.saveready.onError(function() {
        $(root).trigger(root.handlers.on_savefinished);
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Сохранение формы не возможно",
            text: "Обратитесь за помощью в службу поддержки"
        });
    });

    api.data.save.onSuccess(function() {
        root.myform.datatosave = {};
        api.data.savestate.execute({session: root.myform.sessionId});
    });

    api.data.save.onComplete(function(e, result) {
        if (result.status != "200") {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "Во время сохранения формы произошла ошибка",
                text: JSON.stringify(result.responseText)
            });
        }
    });

    api.data.savestate.onSuccess(function(e, result) {
        if(result.stage != "Finished" && result.error == null) {
            window.setTimeout(function(){api.data.savestate.execute({session: root.myform.sessionId})}, 1000);
            return;
        }
        $(root).trigger(root.handlers.on_savefinished);
        if (!!result.error) {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "Во время сохранения формы произошла ошибка",
                text: JSON.stringify(result.error)
            });
            return;
        }
        $(root).trigger(root.handlers.on_message, { text: "Сохранение данных успешно завершено", autohide: 5000, type: "alert-success" });
        api.data.reset.execute({session: root.myform.sessionId});
        api.lock.state.execute({session: root.myform.sessionId});
    });

    api.data.savestate.onError(function(e, result) {
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Во время сохранения формы произошла ошибка",
            text: JSON.stringify(result)
        });
    });

    api.lock.set.onSuccess(function(e, result) {
        api.lock.state.execute({session: root.myform.sessionId});
        api.lock.history.execute({session: root.myform.sessionId});
    });

    api.lock.set.onError(function(e, result) {
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Во время блокировки произошла ошибка",
            content: $('<div/>').html(result.responseText),
            width: 800
        });
    });

    api.lock.setstateold.onComplete(function(e, result) {
        if (result.status == 200) {
            api.lock.state.execute({session: root.myform.sessionId});
            api.lock.history.execute({session: root.myform.sessionId});
        } else {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "Во время блокировки произошла ошибка",
                content: $('<div/>').html(result.responseText),
                width: 800
            });
        }
    });

    api.metadata.getnews.onSuccess(function(e, result) {
        var content = $('.zefsnews');
        if (!$.isEmptyObject(result)) {
            var exist = false;
            if (content.length > 0) {
                content.empty();
                exist = true;
            } else {
                content = $('<div class="zefsnews"/>');
            }
            $.each(result, function(i, m) {
                var n = $('<div class="news-header"/>');
                var u = $('<span class="label label-info"/>').text(m.Sender);
                var archivebtn = $('<button class="btn btn-mini btn-success pull-right"/>')
                    .attr("code", m.Code)
                    .html('<i class="icon-white icon-ok"></i>Прочитать');
                var v = eval(m.Version.substring(2));
                n.append(u, $('<span class="label"/>').text(v.format("dd.mm.yyyy HH:MM:ss")), archivebtn);
                content.append($('<div/>').attr("code", m.Code).append(n, $('<div class="news-content"/>').html(m.Text)));
                u.zetauser();
                archivebtn.click(function(e) {
                    api.metadata.archivenews.execute({ code : $(e.target).attr("code") });
                });
            });
            var allowclose = zeta.user.getIsAdmin() || false;
            if (!exist) {
                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                    title: "Непрочитанные новости",
                    content: content, width: 820, height: 500, closebutton: allowclose, backdrop: true
                });
            }
        } else {
            if (content.length > 0) {
                $(content).modal("hide");
            }
        }
    });

    api.metadata.archivenews.onSuccess(function(e, result) {
        api.metadata.getnews.execute();
    });

    api.metadata.archivenews.onComplete(function(e, result) {
        api.metadata.getnews.execute();
    });

    api.lock.state.onSuccess(function(e, result) {
        root.myform.lock = result;
        $(root).trigger(root.handlers.on_getlockload);
    });

    api.lock.history.onSuccess(function(e, result) {
        if($.isEmptyObject(root.lockhistory)) {
            root.lockhistory = result;
        }
    });

    api.file.list.onSuccess(function(e, result) {
        root.myform.attachment = result;
        $(root).trigger(root.handlers.on_attachmentload);
    });

    api.file.delete.onSuccess(function() {
        api.file.list.execute({session: root.myform.sessionId});
    });

    api.file.add.onSuccess(function() {
        $(root).trigger(root.handlers.on_fileloadfinish);
        api.file.list.execute({session: root.myform.sessionId});
    });

    api.file.add.onError(function(e, error) {
        $(root).trigger(root.handlers.on_fileloaderror, JSON.parse(error.responseText));
    });

    api.file.add.onProgress(function(e, result) {
        $(root).trigger(root.handlers.on_fileloadprocess, result);
    });

    api.chat.add.onSuccess(function() {
        root.myform.chatlist()
    });

    api.chat.list.onSuccess(function(e, result) {
        $(root).trigger(root.handlers.on_chatlistload, result);
    });

    api.chat.list.onError(function(e, result) {
        $(root).trigger(root.handlers.on_chatlistload);
    });

    api.chat.get.onSuccess(function(e, result) {
        $(root).trigger(root.handlers.on_adminchatlistload, result);
    });

    api.chat.archive.onSuccess(function(e, result) {
        root.myform.chatlist();
    });

    api.chat.updatecount.onComplete(function(e, result) {
        $(root).trigger(root.handlers.on_adminchatcountload, result.responseText);
    });

    $.extend(root, {
        getperiodbyid : GetPeriodName
    });

    $.extend(root.myform, {
        execute : function(){api.server.start()},
        openform: OpenForm,
        save : Save,
        restart : Restart,
        forcesave : ForceSave,
        message: Message,
        lockform: LockForm,
        unlockform: UnlockForm,
        checkform: CheckForm,
        attachfile: AttachFile,
        deletefile: DeleteFile,
        downloadfile: DownloadFile,
        openreport: OpenReport,
        openoldform: OpenOldForm,
        setupform: SetupForm,
        cellhistory: CellHistory,
        celldebug: CellDebug,
        openformuladebuger: OpenFormulaDebuger,
        chatlist: ChatList,
        chatadd: ChatAdd,
        chatarchive: ChatArchive,
        chatread: ChatRead,
        chatupdateds: ChatUpdateOnce
    });

    return root.myform;
});
})();