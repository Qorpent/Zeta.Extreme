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
            ////////////////////////////////////////////////
            if (!d) {
                options.timeout -= options.readydelay;
                if(options.timeout<=0){
                    FailStartServer();
                }
                window.setTimeout(StartForm, options.readydelay);
                return;
            }
            options.timeout = options.default_timeout;
            ///////////////////////////////////////////////
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
	
	 
	
	
	
	var Render = render.renderStructure; //вынес в рендер - отдельный скрипт
	var FillBatch = render.updateCells; //вынес в рендер - zefs-render.js
	
	var GetSessionParams = options.getSessionParameters; //перенес в спецификацию
	
	var FinishForm = function(session,batch){}; //какого хрена только тут таблица оживала - НАПОМНЮ "таблица должна быть доступной для правки сразу как пошли первые значения"
    


   
	StartForm();
})(jQuery);

})()