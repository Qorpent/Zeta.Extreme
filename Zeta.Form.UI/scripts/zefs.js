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
    on_reglamentload : "reglamentload", on_formusersload : "formusersload", on_detailsload: "detailsload",
    on_documentationload : "documentationload",
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

    var GetWiki = function(rowcode) {
        var wikicode = '/row/' + rowcode + '/default';
        api.wiki.getsync.execute({code: wikicode});
        var row = $.map(zefs.myform.currentSession.structure.rows, function(r) { if (r.code == rowcode && !!r.comment) return r; });
        if (row.length > 0) {
            var commentarticle = $('<div class="commentarticle"/>').attr("id", "commentarticle_" + rowcode);
            commentarticle.insertAfter($("#wikiarticle__row_" + rowcode + "_default"));
            commentarticle.append(
                $('<div class="wikititle"/>').text("Комментарий к строке"),
                $('<div class="wikitext"/>').html(row["0"].comment.replace(/\s*\r\n/g,'</br>'))
            );
        }
        api.metadata.getformuladependency.execute({code: rowcode});
    }

    var SaveWiki = function(code, text, params) {
        // с параметрами уточнить как слать
        if (zeta.user.getIsDocWriter()) {
            api.wiki.save.execute({code: code, text: text});
        }
    }

    var OpenForm = function(params, blank) {
        params = params || {};
        blank = blank || false;
        location.hash = location.hash.replace(/\|subobj=\d+/, '');
        var hashparams = api.getParameters();
        params = $.extend(hashparams, params);
        var loc = document.location.protocol + "//" + document.location.host + siteroot + "zefs-test.html#";
        if (!!params.form) {
            if (params.form.search(/[A|B]\.in/) == -1) {
                params.form += $(location.hash.match(/[A|B]\.in/)).get(0) || "";
            }
        }
        loc += "form=" + (params.form || "");
        loc += "|year=" + (params.year || "");
        loc += "|period=" + (params.period || "");
        loc += "|obj=" + (params.obj || "");
        if (!!params.subobj) loc += "|subobj=" + params.subobj;
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
	var FillOther = render.updateNullCells;
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
                    message = lockinfo.canblockresult.Reason.Message || lockinfo.canblockresult.Reason.ErrorMessage;
                    if (!!lockinfo.canblockresult.Reason.ReglamentCode) {
                        var reglament = $.map(zefs.reglament, function(r) { if (r.ReglamentCode == lockinfo.canblockresult.Reason.ReglamentCode) return r });
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
                    message = lockinfo.canopenresult.Reason.Message || lockinfo.canopenresult.Reason.ErrorMessage;
                    if (!!lockinfo.canopenresult.Reason.ReglamentCode) {
                        var reglament = $.map(zefs.reglament, function(r) { if (r.ReglamentCode == lockinfo.canopenresult.Reason.ReglamentCode) return r });
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
                    message = lockinfo.cancheckresult.Reason.Message || lockinfo.cancheckresult.Reason.ErrorMessage;
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
//            if (!$.isEmptyObject(root.myobjs)) $('table.data').zefs({ fixHeaderX : 100 }); else
             $('table.data').zefs();
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

    var ShowFormPreloader = function() {
        if (zefs.api.getParameters() != null && $('.zefsdatapreloader').length == 0) {
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "",
                name: "zefsdatapreloader",
                content: $('<div/>').append($('<div class="zefspreloader"/>'), $('<p/>').text("Идет загрузка формы...").append($('<span id="formLoadTime"/>').text("0"))),
                width: 300
            });
            window.formloadtimer = setInterval(function() {
                $('#formLoadTime').text(parseInt($('#formLoadTime').text())+1);
            }, 1000);
        }
    };

    var HideFormPreloader = function() {
        // Закрываем окно с прелоадером
        $('.zefsdatapreloader').modal('hide');
        clearInterval(window.formloadtimer);
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
        var rowcodes = $.map(zefs.myform.currentSession.structure.rows, function(r) { return '/row/' + r.code + '/default' }).join(',');
        api.wiki.exists.execute({ code: rowcodes });
    });

    api.server.ready.onSuccess(function(e, result) {
        if (!!result) {
            api.session.start.execute();
        }
        ShowFormPreloader();
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

    api.wiki.exists.onSuccess(function(e, result) {
        zefs.myform.documentation = result;
        $.each(result, function(i, c) {
            var code = c.Code.match(/row\/([^\/]+)/);
            var ifexist = $.map(zefs.myform.currentSession.structure.rows, function(r, i) { if (r.code == code[1] && !r.hasHelp) return i; });
            if (ifexist.length > 0) {
                zefs.myform.currentSession.structure.rows[ifexist[0]].hasHelp = true;
            }
        });
        $(root).trigger(root.handlers.on_documentationload);
        var withhelp = $.map(zefs.myform.currentSession.structure.rows, function(r) { if (r.hasHelp) return r; });
        $.each(withhelp, function(i, row) {
            var helpcode = '/row/' + row.code + '/default';
            var wikibtn = $('#wiki_' + helpcode.replace(/\//g, '_'));
            wikibtn.removeClass("notexist");
            wikibtn.click(function() {
                clearTimeout(window.wikitimer);
            });
            wikibtn.mouseenter(function() {
                window.wikitimer = setTimeout(function() {
                    var content = "";
                    if (!!row.comment && row.comment != "") {
                        content = '<p class="popover-comment">' + row.comment.replace(/\s*\r\n/g,'</br>') + '</p>';
                    }
                    var req = $.ajax({url : "wiki/get.json.qweb", data: {code: wikibtn.attr("code")}});
                    req.success(function(result) {
                        if (!!result[0]) {
                            content = wiky.process(result[0].Text) + content;
                        }
                        wikibtn.popover({selector: 'body', placement: "right", trigger: "hover", html: true, content: content });
                        wikibtn.popover("show");
                        content = null;
                    });
                }, 500);
            });
            wikibtn.mouseleave(function() {
                clearTimeout(window.wikitimer);
                if (!!wikibtn.data('popover')) {
                    wikibtn.popover('destroy');
                    wikibtn.removeAttr("data-original-title");
                    wikibtn.removeAttr("title");
                }
            });
        });
        if (!zeta.user.getIsDocWriter() && !zeta.user.getIsAdmin()) {
            $('.wikirowhelp.notexist').remove();
        }
        $('.wikirowhelp').show();
    });

    api.metadata.getformuladependency.onSuccess(function(e, result) {
        var code = result.code || "";
        var article = $('<div class="detailsarticle"/>').attr("id", "detailsarticle_" + code);
        article.insertAfter($("#wikiarticle__row_" + code + "_default"));
        var title = $('<div class="wikititle"/>').text("Зависимости формулы");
        article.append(title);
        var getReadableType = function(type) {
            switch (type) {
                case "formula" : return "Формула";
                case "ref" : return "Ссылка";
                case "exref" : return "Вн.ссылка";
                default : return type;
            }
        }
        var tbody = $('<tbody/>');
        var table = $('<table class="table table-bordered"/>').append(
            $('<thead/>').append($('<tr/>').append(
                $('<th/>').text("Код."),
                $('<th/>').text("Вн.код"),
                $('<th/>').text("Форма"),
                $('<th/>').text("Наименование"),
                $('<th/>').text("Зав-ть")
            )), tbody
        )
        $.each(result.dependency, function(i1, d1) {
            tbody.append($('<tr/>').append(
                $('<td/>').text(d1.outercode),
                $('<td/>').text(d1.code),
                $('<td/>').append(d1.form),
                $('<td/>').text(d1.name),
                $('<td/>').text(getReadableType(d1.type))
            ));
        });
        article.append(table);
        if (!!result.forms) {
            var formstitle = $('<div class="wikititle"/>').text("Формы, содержащие исходные данные");
            article.append(formstitle);
            $.each(result.forms, function(i, f) {
                if (zefs.myform.currentSession.FormInfo.Code.search(f.code) != -1) return;
                var a = $('<button class="btn-link"/>').text(f.name);
                if (f.allow) {
                    a.click(function() { OpenForm({form: f.code}, true) });
                }
                article.append(a);
            });
            if (formstitle.next().length == 0) formstitle.remove();
        }
    }),

    api.wiki.getsync.onSuccess(function(e, result) {
        if (!$.isEmptyObject(result)) {
            var content = $('<div/>');
            $.each(result, function(i, w) {
                var wikiarticle = $('<div class="wikiarticle"/>');
                wikiarticle.attr("id", "wikiarticle_" + w.Code.replace(/\//g, "_"));
                var wikititle = $('<div class="wikititle"/>').text(w.Title || w.Code);
                var wikitext = $('<div class="wikitext"/>').html(wiky.process(w.Text));
                var wikiinfo = $('<div class="wikiinfo"/>').text("Последняя правка: ");
                if (!!w.Date) {
                    wikiinfo.text(wikiinfo.text() + w.Date.format("dd.mm.yyyy HH:MM:ss"));
                }
                if (!!w.Editor) {
                    var user = $('<span class="label label-info"/>').text(w.Editor);
                    wikiinfo.append(user);
                    user.zetauser();
                }
                if (zeta.user.getIsDocWriter()) {
                    var wikiedit = $('<textarea class="wikiedit"/>').val(w.Text);
                    var wikititleedit = $('<input type="text" class="wikititleedit"/>').val(w.Title || w.Code);
                    wikiedit.hide();
                    wikititleedit.hide();
                    var wikicontrols = $('<div class="wikicontrols"/>');
                    var wikieditbtn = $('<button class="btn btn-mini"/>').text("Править");
                    var wikisavebtn = $('<button class="btn btn-mini btn-success"/>').html('<i class="icon-white icon-ok"/>');
                    wikisavebtn.hide();
                    wikicontrols.append(wikieditbtn, wikisavebtn);
                    wikiedit.keyup(function() {
                        wikitext.html(wiky.process(wikiedit.val()));
                    });
                    wikititleedit.keyup(function() {
                        wikititle.text(wikititleedit.val());
                    });
                    wikieditbtn.click(function() {
                        wikieditbtn.hide();
                        wikiedit.show();
                        wikititleedit.show();
                        wikisavebtn.show();
                    });
                    wikisavebtn.click(function() {
                        wikiedit.hide();
                        wikititleedit.hide();
                        wikisavebtn.hide();
                        var save = $.ajax({
                            url: "wiki/save.json.qweb",
                            type: "POST",
                            data: { code: w.Code, text: wikiedit.val(), title: wikititleedit.val() }
                        });
                        save.success(function(r) {
                            wikititle.text(r.Title || r.Code);
                            var v = eval(r.LastWriteTime.substring(2));
                            wikiinfo.text("Сохранено: " + v.format("dd.mm.yyyy HH:MM:ss"));
                            if (!!r.Editor) {
                                var user = $('<span class="label label-info"/>').text(r.Editor);
                                wikiinfo.append(user);
                                user.zetauser();
                            }
                        });
                        wikieditbtn.show();
                    });
                    wikiarticle.append(wikititleedit, wikiedit, wikicontrols);
                }
                wikiarticle.append(wikititle, wikitext, wikiinfo);
                content.append(wikiarticle);
            });
            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                title: "База знаний", width: 900,
                content: $('<div/>').append(content)
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
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Ошибка при старте приложения",
            text: root.myform.startError
        });
        HideFormPreloader();
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
        api.session.details.execute({form: root.myform.currentSession.FormInfo.CodeOnly});
        Render(root.myform.currentSession);
        Fill(root.myform.currentSession);
        $(root).trigger(root.handlers.on_structureload);
    });

    api.session.details.onSuccess(function(e, result) {
        root.myform.details = result;
        $(root).trigger(root.handlers.on_detailsload);
    });

    api.data.start.onSuccess(function(e, result) {
        root.myform.currentSession.data.push(result);
        Fill(root.myform.currentSession);
        if(result.state != "w"){
            HideFormPreloader();
            // Это штука для перерисовки шапки
            $(window).trigger("resize");
            $(root).trigger(root.handlers.on_dataload);
            FillOther();
            CheckConditions();
        } else {
            var idx = 0;
            if (!$.isEmptyObject(result)) {
                if (!!result.ei) {
                    idx = result.ei+1;
                }
                else if (!!result.si) {
                    idx = result.si;
                }
                else {
                    // если нет ни того ни другого, наверное, надо брать
                    // из последнего батча
                }
            };
//          var idx = !$.isEmptyObject(result.data) ? result.ei+1 : result.si;
            window.setTimeout(function(){api.data.start.execute({session: root.myform.sessionId,startidx: idx})},500);
        }
    });

    api.data.reset.onSuccess(function() {
        root.myform.currentSession.data = [];
        $('td.data').addClass("notloaded");
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
        if (!$.isEmptyObject(root.myform.adminchat)) {
            var notread = $.map(result, function(i) { if ($.map(zefs.myform.adminchat, function(j) { if(i.Id == j.Id) return j }).length == 0) return i });
            $.each(notread, function(i, o) {
                o.notread = true;
            });
        }
        root.myform.adminchat = result;
        $(root).trigger(root.handlers.on_adminchatlistload, result);
    });

    api.chat.haveread.onSuccess(function() {
        if (!$.isEmptyObject(root.myform.adminchat)) {
            var notread = $.map(root.myform.adminchat, function(m) { if (m.notread) return m });
            $.each(notread, function(i, o) {
                o.notread = false;
            });
        }
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
        chatupdateds: ChatUpdateOnce,
        wikiget: GetWiki,
        wikisave: SaveWiki
    });

    return root.myform;
});
})();