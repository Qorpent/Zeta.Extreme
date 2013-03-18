(function(){
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zefs = window.zefs || {};
var api = root.api;
root.handlers = $.extend(root.handlers, {
    // Zefs handlers:
    on_zefsready : "zefsready",
    on_zefsstarting : "zefsstarting",
    on_zefsfailed : "zefsfailed",
    // Session handlers:
    on_sessionload : "sessionload",
    on_structureload : "structureload",
    // Form handlers:
    on_formready : "forrmready",
    on_statusload : "statusload",
    on_statusfailed : "statusfaild",
    on_savestart : "savestart",
    on_savefailed : "savefaild",
    on_savefinished : "savefinished",
    on_getlockfailed : "getlockfinished",
    on_getlockload : "getlockload",
    on_getlockhistoryload : "getlockhistory",
    on_lockform : "lockform",
    // Other handlers:
    on_periodsload : "periodsload",
    on_periodsfaild : "periodsfailed",
    on_objectsload : "objectsload",
    on_objectsfaild : "objectsfailed",
    on_attachmentload : "attachmentload",
    // Message handlers:
    on_message : "message",
    // File handlers:
    on_fileloadstart: "fileloadstart",
    on_fileloadfinish: "fileloadfinish",
    on_fileloaderror: "fileloaderror",
    on_fileloadprocess: "fileloadprocess"
});
root.periods =  root.periods || {};
root.divs =  root.divs || [];
root.objects =  root.objects || [];
root.init = root.init ||
(function ($) {
    if (root.myform) return root.myform;
    root.myform = root.myform ||  {
        sessionId : null,
        currentSession : null,
        lock : null,
        lockhistory : null,
        attachment : null
    };
    var render = root.getRender();
    api.getParameters = function(){
        // Парсим параметры из хэша
        var p = {};
        var result = {};
        if (location.hash == "") return null;
        $.each(location.hash.substring(1).split("|"), function(i,e) {
            p[e.split("=")[0]] = e.split("=")[1];
        });
        result["form"] = p["form"];
        result["obj"] = p["obj"];
        result["period"] = p["period"];
        result["year"] = p["year"];
        return result;
    };
    api.siterootold = function(){
        if (location.host.search('admin|corp|133|49') || location.port == '448' || location.port == '449') return '/ecot/';
//      else if (location.host.search('assoi') == 0 || location.port == '447') return '/eco/';
        return /eco/;
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

    var Lock = function() {
        if (root.myform.sessionId != null && root.myform.lock != null) {
            if (root.myform.lockinfo.canblock) {
                api.lock.set.execute({state: "0ISBLOCK"});
            }
        }
    };

    var Unlock = function() {
        if (root.myform.sessionId != null && root.myform.lock != null) {
            if (!root.myform.lockinfo.isopen) {
                api.lock.set.execute({state: "0ISOPEN"});
            }
        }
    };

    var Save = function(obj) {
        if (!$.isEmptyObject(obj) && !root.myform.lock) return;
        root.myform.datatosave = obj;
        $(root).trigger(root.handlers.on_savestart);
        api.data.saveready.execute();
    };

    var Message = function(obj) {
        $(root).trigger(root.handlers.on_message, obj);
    }

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

    // Обработчики событий
    api.server.ready.onSuccess(function(e, result) {
        if (!!result) {
            api.session.start.execute();
        }
        api.metadata.getobjects.execute();
        api.metadata.getperiods.execute();
    });

    api.metadata.getobjects.onSuccess(function(e, result) {
        root.divs = result.divs;
        root.objects = result.objs;
        $(root).trigger(root.handlers.on_objectsload);
    });

    api.metadata.getperiods.onSuccess(function(e, result) {
        if($.isEmptyObject(root.periods)) {
            root.periods = result;
        }
    });

    api.session.start.onSuccess(function(e, result) {
        root.myform.currentSession = result;
        root.myform.sessionId = result.Uid;
        var sessiondata = {session: root.myform.sessionId};
        document.title = result.FormInfo.Name;
        api.session.structure.execute(sessiondata);
        api.lock.state.execute(sessiondata);
        api.lock.info.execute(sessiondata);
        api.lock.history.execute(sessiondata);
        api.file.list.execute(sessiondata);
        window.setTimeout(function(){
                root.myform.currentSession.data = [];
                api.data.start.execute($.extend(sessiondata, {startidx: 0}))}
        ,1000); //первый запрос на данные
        $(root).trigger(root.handlers.on_sessionload);
    });

    api.session.structure.onSuccess(function(e, result) {
        root.myform.currentSession.structure = result;
        Render(root.myform.currentSession);
        Fill(root.myform.currentSession);
        $(root).trigger(root.handlers.on_structureload);
        $('table.data').zefs();
    });

    api.data.start.onSuccess(function(e, result) {
        root.myform.currentSession.data.push(result);
        Fill(root.myform.currentSession);
        if(result.state != "w"){
            // Это штука для перерисовки шапки
            $(window).trigger("resize");
        } else {
            var idx = $.isEmptyObject(result.data) ? result.ei+1 : result.si;
            window.setTimeout(function(){api.data.start.execute({session: root.myform.sessionId,startidx: idx})},500);
        }
    });

    api.data.reset.onSuccess(function() {
        root.myform.currentSession.data = [];
        api.data.start.execute({session: root.myform.sessionId, startidx: 0});
    });

    api.data.saveready.onSuccess(function(e, result) {
        if(result.Uid != root.myform.sessionId) {
            root.myform.sessionId = result.Uid;
            root.myform.currentSession = result;
        }
        if ($.isEmptyObject(root.myform.datatosave)) return;
        $.each($('td.recalced'), function(i,e) {
            $(e).removeClass("recalced");
        });
        api.data.save.execute({
            session: root.myform.sessionId,
            data: JSON.stringify(root.myform.datatosave)
        });
    });

    api.data.save.onSuccess(function() {
        root.myform.datatosave = {};
        $(root).trigger(root.handlers.on_savefinished);
        api.data.savestate.execute({session: root.myform.sessionId});
    });

    api.data.savestate.onSuccess(function(e, result) {
        if(result.stage != "Finished") {
            window.setTimeout(api.data.savestate.execute({session: root.myform.sessionId}), 1000);
            return;
        }
        if (null != result.error) {
            $(root).trigger(root.handlers.on_message, { text: result.error, autohide: 5000, type: "alert-error" });
        }
        $(root).trigger(root.handlers.on_message, { text: "Сохранение успешно завершено", autohide: 5000, type: "alert-success" });
        $(root).trigger(root.handlers.on_savefinished);
        api.data.reset.execute({session: root.myform.sessionId});
    });

    api.lock.set.onSuccess(function() {
        api.lock.state.execute({session: root.myform.sessionId});
    });

    api.lock.set.onError(function(e, error) {
        $(root).trigger(root.handlers.on_modal, {
            title: "", // заголовок ошибки
            text: "" // текст ошибки
        });
    });

    api.lock.state.onSuccess(function(e, result) {
        root.myform.lock = result;
        $(root).trigger(root.handlers.on_getlockload);
    });

    api.lock.info.onSuccess(function(e, result) {
        root.myform.lockinfo = result;
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

    $.extend(root, {
        getperiodbyid : GetPeriodName
    });

    $.extend(root.myform, {
        execute : function(){api.server.start()},
        save : Save,
        message: Message,
        lockform: Lock,
        unlockform: Unlock,
        attachfile: AttachFile,
        deletefile: DeleteFile,
        downloadfile: DownloadFile
    });

    return root.myform;
});
})()