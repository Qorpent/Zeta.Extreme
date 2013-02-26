(function(){
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zefs = window.zefs || {};
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
    // Other handlers:
    on_periodsload : "periodsload",
    on_periodsfaild : "periodsfailed",
    on_objectsload : "objectsload",
    on_objectsfaild : "objectsfailed"
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
        lock : true
    };
    var options = window.zefs.options;
	var params = options.getParameters();
    var render = root.getRender();
    var StartForm = function() {
        $.ajax({
            url: siteroot+options.ready_command,
            context: this,
            dataType: 'json'
        }).success($.proxy(function(d) {
            //дожидаемся готовности сервера
            // serverstatus - это виджет на панели, отвечающий за отображения статуса сервера
            var serverstatus = $('div.zefsstatus>span.label').first();
            if (!d) {
                $(serverstatus).attr("class","label label-warning").text("Получение статуса сервера");
                options.timeout -= options.readydelay;
                if(options.timeout<=0){
                    $(serverstatus).attr("class","label label-important").text("Сервер не доступен");
                    FailStartServer();
                    return;
                }
                window.setTimeout(StartForm, options.readydelay);
                return;
            }
            $(serverstatus).attr("class","label label-success").text("Сервер доступен");
            options.timeout = options.default_timeout;
            $(root).trigger(root.handlers.on_zefsready);
            ExecuteSession();
            GetPeriods();
            GetObjects();
        }));
    };

    var FailStartServer = function(){};
    var ExecuteSession = $.proxy(function() {
        $.ajax({
            url: siteroot+options.start_command,
            context: this,
            type: "POST",
            dataType: "json",
            data: params
        }).success($.proxy(function(d) {
            var session = options.asSession(d);
            root.myform.currentSession = session;
            root.myform.sessionId = session.getUid();
            document.title = session.getFormInfo().getName();
            Structure(session);
            GetCurrentLock();
            $(root).trigger(root.handlers.on_sessionload);
            window.setTimeout(function(){Data(session,0)},options.datadelay); //первый запрос на данные
        }));
    }, this);

    var Structure = $.proxy(function(session) {
        var params = GetSessionParams(session);
        $.ajax({
            url: siteroot+options.struct_command,
            context: this,
            type: "POST",
            dataType: "json",
            data: params
        }).success($.proxy(function(d) {
            session.structure = options.asStruct(d);
            Render(session);
            Fill(session);
            $(root).trigger(root.handlers.on_structureload);
			$('table.data').zefs(); //нам сразу нужна живость!!!
        }));
    }, this);

    var Data = $.proxy(function(session,startIdx) {
        var params = GetSessionParams(session,startIdx);
        $.ajax({
            url: siteroot+options.data_command,
            context: this,
            type: "POST",
            dataType: "json",
            data: params
        }).success($.proxy(function(d) {
            var batch =  options.asDataBatch(d);
            session.data.push(batch);
            Fill(session);
            if(batch.getIsLast()){
                FinishForm(session,batch);
            } else {
                var s = session;
                var idx = batch.getNextIdx();
                window.setTimeout(function(){Data(s,idx)},options.datadelay);
            }
        }));
    }, this);

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

	var GetSessionParams = options.getSessionParameters; //перенес в спецификацию

	var FinishForm = function(session,batch){}; //какого хрена только тут таблица оживала - НАПОМНЮ "таблица должна быть доступной для правки сразу как пошли первые значения"

    var GetCurrentLock = function(){
        $.ajax({
            url: siteroot+options.currentlock_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: {session: root.myform.sessionId}
        }).error(function(d) {
                $(root).trigger(root.handlers.on_getlockfailed);
        }).success(function(d) {
            root.myform.lock = options.asLockState(d).getCanSave();
            $(root).trigger(root.handlers.on_getlockload);
        });
    };

    var Save = function(obj) {
        if ($.isEmptyObject(obj) || !root.myform.lock) return;
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

    var ReadySave = function() {
        $.ajax({
            url: siteroot+options.save_command,
            type: "POST",
            context: this,
            dataType: "json",
            data: {
                session: root.myform.sessionId
            }
        }).success(function(d) {
            // Вот тут, как я понял, начинается Save
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
                // вывести сообщение об ошибке
            }
            $(root).trigger(root.handlers.on_savefinished);
            ResetData();
        });
    };

    var ResetData = function() {
        $.ajax({
            url: siteroot+"zefs/resetdata.json.qweb",
            type: "POST",
            context: this,
            dataType: "json",
            data: {session: root.myform.sessionId}
        }).success(function(d) {
             root.myform.currentSession.data = [];
             Data(root.myform.currentSession,0);
        });
    };

    var SortObjectsByIdx = function(a, b) {
        return ((a.idx < b.idx) ? -1 : ((a.idx > b.idx) ? 1 : 0));
    }

    var GetObjects = function() {
        $.ajax({
            url: siteroot+options.getobject_command,
            context: this,
            dataType: "json"
        }).success(function(d) {
            $.each(d.divs, function(i,div) {
                root.divs.push(options.asDiv(div));
            });
            root.divs.sort(SortObjectsByIdx);
            $.each(d.objs, function(i,obj) {
                root.objects.push(options.asObject(obj));
            });
            $(root).trigger(root.handlers.on_objectsload);
        });
    };

    var GetPeriods = function() {
        $.ajax({
            url: siteroot+options.getperiods_command,
            context: this,
            dataType: "json"
        }).success(function(d) {
            $.each(d, function(i,p) {
                var period = options.asPeriod(p);
                if (!root.periods.hasOwnProperty(period.getType())) {
                    root.periods[period.getType()] = [];
                }
                root.periods[period.getType()].push(period);
            });
            $(root).trigger(root.handlers.on_periodsload);
        });
    };

    var GetPeriodName = function(id) {
        var name = "";
        $.each(root.periods, function(periodname, periodtype) {
            $.each(periodtype, function(i,p) {
                if (p.getId() == id) {
                    name = p.getName();
                    return false;
                }
            });
            if (name != "") return false;
        });
        return name;
    };

    $.extend(root, {
        getperiodbyid : GetPeriodName
    });

    $.extend(root.myform, {
        run : StartForm,
        save : Save
    });

    return root.myform;
});
})()