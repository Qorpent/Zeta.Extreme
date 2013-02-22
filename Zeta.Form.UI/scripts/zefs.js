(function(){
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zefs = window.zefs || {};
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
                }
                window.setTimeout(StartForm, options.readydelay);
                return;
            }
            $(serverstatus).attr("class","label label-success").text("Сервер доступен");
            options.timeout = options.default_timeout;
            ExecuteSession();
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
            $(root).trigger("session_load");
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
            $('#zefsFormHeader').text(session.getFormInfo().getName()
                + " " + session.getObjInfo().getName() + " за "
                + session.getPeriod() + ", " + session.getYear() + " год");
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
        }).success(function(d) {
                root.myform.lock = options.asLockState(d).getCanSave();
                $(root).trigger("formstatus_load");
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
            $(root).trigger("savestage_started");
            SaveState();
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
            ResetData();
            $(root).trigger("savestage_finished");
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
             Data(root.myform.currentSession,0);
        });
    };

    $.extend(root.myform, {
        run : StartForm,
        save : Save
    });

    return root.myform;
});
})()