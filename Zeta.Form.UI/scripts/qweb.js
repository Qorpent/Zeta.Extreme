(function(){
    var root = window.qweb = window.qweb || {};
    var siteroot = document.location.pathname.match(/([\\w\\d_\-]+)?/)[0];
    root.Command = function(sourceoptions){
        var options = sourceoptions || {};
        if (typeof(options)=="string"){
            options = {name:options};
        }
        $.extend(this,options);
        if(!this.url){
            var domain = this.domain || "_sys";
            this.url = domain+"/"+this.name+".{DATATYPE}.qweb";
        }
        if(!this.successEventName)this.successEventName=this.url+":success";
        if(!this.errorEventName)this.errorEventName=this.url+":error";
		this.repeatWait = false;
		if(options.timeout){
			this.basetimeout = options.timeout;
			this.currenttimeout = this.basetimeout;
			this.delay  = options.delay || 1000;
			this.repeatWait = true;
		}
    };

    $.extend(root.Command.prototype,{
        datatype : "json",
        getParameters : function() { return null },
        getUrl:function(datatype){
            datatype = datatype || self.dataType || "json";
                       return siteroot+this.url.replace('{DATATYPE}',datatype);
        },
        triggerOnSuccess : function(result){
            $(this).trigger(this.successEventName,result);
        },
        triggerOnError : function(result){
            $(this).trigger(this.errorEventName,result);
        },
        onSuccess: function( func ){
          $(this).on(this.successEventName, func);
        },
        onError: function( func ){
            $(this).on(this.errorEventName, func);
        },
		execute : function(params) {
            params = params || this.getParameters();
            this.call(params);
        },
		call : function (params,onsuccess,onerror) {
			if(!onsuccess){
				if(this.repeatWait){
					onsuccess = this._getRepeatWaitCallFunciton();
				}else{
					onsuccess = this._getUsualCallFunciton();
				}
			}
            this.nativeCall(params,onsuccess, onerror);
        },
		_getUsualCallFunciton : function(){
			var self = this;
			return (function(result){
                if (!result) {
                   self.triggerOnError(result);
                    return;
                }
                if (self.wrap) result = self.wrap(result);
                self.triggerOnSuccess(result);
            });
		},
		_getRepeatWaitCallFunciton : function(){
			var self = this;
			return (function(result,params){
                if(!result){
					self.currenttimeout -= self.delay;
					if(self.currenttimeout<=0){
						self.triggerOnError(null);
						return;
					}
					window.setTimeout(function(){self.execute(params)}, self.delay);
					return;
				}
				self.currenttimeout = self.basetimeout;
				self.triggerOnSuccess(result);
			});
		},
        nativeCall : function(params,onsuccess,onerror,url,datatype){
            datatype = datatype || this.datatype;
            url = url || this.getUrl(this.datatype);
            $.ajax({
                url: url,
                type : !params ? "GET" : "POST",
                dataType: datatype,
                data : params || {}
            })
                .success(function(r){onsuccess(r,params)})
                .error(onerror||function(error){console.log(error)});
        }
    });

})();