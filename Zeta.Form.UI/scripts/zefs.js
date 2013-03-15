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
    var options = window.zefs.options;
	var params = options.getParameters();
    var render = root.getRender();

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
        $.ajax({
            url: siteroot+options.attachfile_command,
            type: "POST",
            context: this,
            dataType: "json",
            xhr: function() {
                var x = $.ajaxSettings.xhr();
                if(x.upload) {
                    x.upload.addEventListener('progress', function(e) {
                        $(root).trigger(root.handlers.on_fileloadprocess, e);
                    }, false);
                }
                return x;
            },
            data: fd,
            cache: false,
            contentType: false,
            processData: false
        }).fail(function(error){
            $(root).trigger(root.handlers.on_fileloaderror, JSON.parse(error.responseText));
        }).success(function() {
            $(root).trigger(root.handlers.on_fileloadfinish);
            api.file.list.execute({session: root.myform.sessionId});
        });
    };

    var DeleteFile = function(uid) {
        $.ajax({
            url: siteroot+options.deletefile_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: {
                session: root.myform.sessionId,
                uid: uid
            }
        }).success(function() {
            api.file.list.execute({session: root.myform.sessionId});
        });
    };

    var DownloadFile = function(uid) {
        return siteroot+options.downloadfile_command + "?session=" +
            root.myform.sessionId + "&uid=" + uid;
        /* $.ajax({
             url: siteroot+options.downloadfile_command,
             type: "POST",
             context: this,
             dataType: "json",
             data: {
                 session: root.myform.sessionId,
                 uid: uid
             }
        })*/
    };

    var Lock = function() {
        if (root.myform.sessionId != null && root.myform.lock != null) {
            if (root.myform.lock.getCanLock()) {
                api.lock.start.execute({session: root.myform.sessionId});
            }
        }
    };

    var Save = function(obj) {
        $.each($('td.recalced'), function(i,e) {
            $(e).removeClass("recalced");
        });
        $.ajax({
            url: siteroot+options.save_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: {
                session: root.myform.sessionId,
                data: JSON.stringify(obj)
            }
        }).success(function(d) {
            $(root).trigger(root.handlers.on_savefinished);
            SaveState();
        });
    };

    var ReadySave = function(obj) {
        if ($.isEmptyObject(obj) || !root.myform.lock) return;
        $(root).trigger(root.handlers.on_savestart);
        $.ajax({
            url: siteroot+options.saveready_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: params
        }).success(function(d) {
            d = options.asSession(d);
            if(d.Uid != root.myform.sessionId) {
                root.myform.sessionId = d.Uid;
                root.myform.currentSession = d;
            }
            Save(obj);
        });
    };

    var SaveState = function() {
        $.ajax({
            url: siteroot+options.savestate_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: {session: root.myform.sessionId}
        }).success(function(d) {
            var state = options.asSaveState(d);
            if(!state.getIsFinished()) {
                window.setTimeout(SaveState, 1000);
                return;
            }
            if (state.getIsError()) {
                $(root).trigger(root.handlers.on_message, { text: state.getError(), autohide: 5000, type: "alert-error" });
            }
            $(root).trigger(root.handlers.on_message, { text: "Сохранение успешно завершено", autohide: 5000, type: "alert-success" });
            $(root).trigger(root.handlers.on_savefinished);
            api.session.reset.execute({session: root.myform.currentSession});
        });
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
        api.lock.history.execute(sessiondata);
        api.files.list.execute(sessiondata);
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
        api.session.data.execute({session: root.myform.currentSession, startidx: 0});
    });

    api.data.save.onSuccess(function() {
        $(root).trigger(root.handlers.on_savefinished);
        api.data.savestate.execute({session: root.myform.currentSession});
    });

    api.lock.start.onSuccess(function() {
        api.lock.start.execute({session: root.myform.sessionId});
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

    api.files.list.onSuccess(function(e, result) {
        if($.isEmptyObject(root.myform.attachment)) {
            root.myform.attachment = result;
        }
        $(root).trigger(root.handlers.on_attachmentload);
    });

    $.extend(root, {
        getperiodbyid : GetPeriodName
    });

    $.extend(root.myform, {
        execute : function(){api.server.start()},
        save : ReadySave,
        message: Message,
        lockform: Lock,
        attachfile: AttachFile,
        deletefile: DeleteFile,
        downloadfile: DownloadFile
    });

    return root.myform;
});
})()