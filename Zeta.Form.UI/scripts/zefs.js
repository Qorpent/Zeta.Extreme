(function(){
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zefs = window.zefs || {};
root.init = root.init ||
(function ($) {
    var params = {};
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
                $(serverstatus).attr("class","label label-warning").text("Полчение статуса сервера");
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
    }
    var FailStartServer = function(){};
    var ExecuteSession = $.proxy(function() {
        $.ajax({
            url: siteroot+options.start_command,
            context: this,
            dataType: "json",
            data: params
        }).success($.proxy(function(d) {
            var session = options.asSession(d);
            document.title = session.getFormInfo().getName();
            $('#sessionInfo').attr("uid", d.getUid());
            $('#debugInfo').attr("uid", d.getUid());

            $('#currentlockInfo').attr("uid", d.getUid());

            $('#canlockInfo').attr("uid", d.getUid());
            Structure(session);
            window.setTimeout(function(){Data(session,0)},options.datadelay);    //первый запрос на данные
        }));
    }, this);

    var Structure = $.proxy(function(session) {
        var params = GetSessionParams(session);
        $.ajax({
            url: siteroot+options.struct_command,
            context: this,
            dataType: "json",
            data: params
        }).success($.proxy(function(d) {
            session.structure = options.asStruct(d);
            Render(session);
            Fill(session);
            $('#zefsFormHeader').text(session.getFormInfo().getName()
                + " " + session.getFormInfo().getName() + " за "
                + session.getPeriod() + ", " + session.getYear() + " год");
			$('table.data').zefs(); //нам сразу нужна живость!!!
        }));
    }, this);

    var Data = $.proxy(function(session,startIdx) {
        var params = GetSessionParams(session,startIdx);
        $.ajax({
            url: siteroot+options.data_command,
            context: this,
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
    }
	
	 
	var RenderSession = function(session) {
        var html = $('<p/>');
        $.each(session, function(k,v) {
            html.append($('<span/>').html('<strong>' + k + ':</strong>' + v), $('<br/>'));
        });
        var modal = $('<div class="modal fade" role="dialog" />');
        var modalheader = $("<div/>", {"class":"modal-header"}).append(
            $('<button type="button" class="close" data-dismiss="modal" aria-hidden="true" />')
                .html("&times;"),
            $('<h3/>').text("Текущее состояние сессии")
        );
        var modalbody = $('<div class="modal-body" />').html("<strong>UID:</strong>" + d.getUid());
        var modalfooter = $('<div class="modal-footer"/>').append(
            $('<a href="#" class="btn btn-primary" data-dismiss="modal" />')
                .html("Закрыть")
        );
        modal.append(modalheader, modalbody, modalfooter);
    }
	
	
	var Render = render.renderStructure; //вынес в рендер - отдельный скрипт
	var FillBatch = render.updateCells; //вынес в рендер - zefs-render.js
	
	var GetSessionParams = options.getSessionParameters; //перенес в спецификацию
	
	var FinishForm = function(session,batch){}; //какого хрена только тут таблица оживала - НАПОМНЮ "таблица должна быть доступной для правки сразу как пошли первые значения"

    if (params != null){
        StartForm();
    }
});

})()