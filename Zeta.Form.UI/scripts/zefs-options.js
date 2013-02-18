(function(){
var root = window.zefs = window.zefs || {};
var options = root.options = root.options || {};
$.extend(options,(function(){
	return {
		form_hash_param : "form",
		year_hash_param : "year",
		period_hash_param : "period",
		obj_hash_param : "obj",
		
		form_param : "form",
		year_param : "year",
		period_param : "period",
		obj_param : "obj",
		
		session_param : "session",
		startidx_param : "startidx",
		
		server_command : "zefs/server.json.qweb",
		ready_command : "zefs/ready.json.qweb",
		start_command : "zefs/start.json.qweb",
		session_command : "zefs/session.json.qweb",
		struct_command : "zefs/struct.json.qweb",
		data_command : "zefs/data.json.qweb",
		debug_command : "zefs/debuginfo.json.qweb",

		finished_state : "f",
		inprocess_state : "w",
		error_state : "e",
		
		default_measure : "тыс. руб.",

        default_timeout : 5000,
        timeout : 5000,
		readydelay : 1000,
		datadelay : 800,
		
		getParameters : function(){
			// Парсим параметры из хэша
			var p = {};
			var result = {};
            if (location.hash == "") return null;
			$.each(location.hash.substring(1).split("|"), function(i,e) {
				p[e.split("=")[0]] = e.split("=")[1];
			});
			result[this.form_hash_param] = p[this.form_hash_param];
			result[this.obj_hash_param] = p[this.obj_hash_param];
			result[this.period_hash_param] = p[this.period_hash_param];
			result[this.year_hash_param] = p[this.year_hash_param];
			
			return result;
		},
		
		getSessionParameters  : function (session,startIdx) {
			var params = {};
			params[options.session_param] = session.getUid();
			if (!!startIdx) {
				params[options.startidx_param] = startIdx;
			}
			return params;
		},
		
		asSession : function ( obj ) {
			$.extend(obj,{
				getIsStarted :	function(){return this.IsStarted;},
				getIsFinished :	function(){return this.IsFinished;},
				getUid :	function(){return this.Uid;},
				getCreateTime : function(){return this.Created;},
				getYear : function(){return this.Year;},
				getPeriod : function(){return this.Period;},
				getUsr : function(){return this.Usr;},
				getObjInfo : function(){return this.ObjInfo;},
				getFormInfo : function(){return this.FormInfo;},
				getTimeToPrepare : function(){return this.TimeToPrepare;},
				getTimeToStructure : function(){return this.TimeToStructure;},
				getTimeToPrimary : function(){return this.TimeToPrimary;},
				getTimeToGetData : function(){return this.TimeToGetData;},
				getQueryCount: function(){return this.QueriesCount;},
				getPrimaryCount: function(){return this.PrimaryCount},
				getDataCount: function(){return this.DataCount;},
                getNeedMeasure:function(){return !!this.NeedMeasure;},
                wasRendered : false,
                structure : {},
                data : []
			});
			$.extend(obj.getObjInfo(),{
				getId : function(){return this.Id;},
				getCode : function(){return this.Code;},
				getName : function(){return this.Name;}
			});
			$.extend(obj.getFormInfo(),{
				getId : function(){return this.Code;},
				getCode : function(){return this.Code;},
				getName : function(){return this.Name;}
//                getNeedMeasure:function(){return !!this.NeedMeasure;} // Перенес выше в asSession
			});
			return obj;
		},
		
		asStruct : function (obj) {
			$.extend(obj,{
				rows : [],
				cols : [],
				prepare : function(){
                    var minlevel = 100;
                    for (var si in this){
                        var item=this[si];
                        if(item.type=="r"){
                            if(Number(item.level)<minlevel){
                                minlevel = Number(item.level);
                            }
                        }
                    }
					for ( var si in this ) {
						var item = this[si];
						if (item.type=="c") {
							this.cols.push(options.asColumn(item));
						}
						if (item.type=="r") {
							this.rows.push(options.asRow(item,minlevel));
						}
					}
				}
			});
			obj.prepare();
			return obj;
		},
		
		asRow : function(obj,minlevel){
			$.extend(obj,{
				getCode : function(){return this.code;},
				getIdx : function(){return this.idx;},
				getIsPrimary : function(){return !!this.isprimary;},
				getName: function(){return this.name;},
				getIsTitle : function(){return !!this.iscaption;},
				getLevel : function(){return (this.level- minlevel) || 0 ;}   ,
                getNumber : function(){return this.number || "" ;},  
                getMeasure : function(){return this.measure || options.default_measure;}
			});
			return obj;
		},	
		
		asColumn : function(obj){
			$.extend(obj,{
				getCode : function(){return this.code;},
				getIdx : function(){return this.idx;},
				getIsPrimary : function(){return !!this.isprimary;},
				getName: function(){return this.name;},
				getYear: function(){return this.year;},
				getPeriod: function(){return this.period;}
			});
			return obj;
		},
		
		asDataBatch : function(obj){
			$.extend(obj,{
				getFirstIdx : function(){return this.si;},
				getLastIdx : function(){return this.ei;},
				getState : function(){return this.state;},
				getIsError : function(){return this.getState()==options.error_state;},
				getError : function(){return this.e;},
				getIsLast : function(){return this.getState()!=options.inprocess_state;},
                getWasData : function(){
                  var data  = this.getData();
                    if(!data)return false;
                    if(0==data.length)return false;
                    return true;
                },
                getNextIdx : function(){
                    if(this.getWasData()){
                        return this.getLastIdx()+1;
                    }
                    return this.getFirstIdx();
                },
				getData : function(){ return this.data || {};} ,
                wasFilled : false
				});
			var data = obj.getData();
			for (var i in data) {
				options.asDataItem(data[i]);
			}
			return obj;
		},
		
		asDataItem : function(obj){
			obj.v = obj.v || null,
			$.extend(obj,{
				getIsCell : function(){return this.getCellId()!=0;},
				getCellId :  function(){return this.c || 0;},
				getId : function() {return this.i;},
				hasValue : function(){return null!=this.v;},
				getValue : function(){return this.v;}
			});
			var rc = obj.getId().split(":");
			obj.row = rc[0];
			obj.col = rc[1];
			return obj;
		}
	}
	
})())
})();
