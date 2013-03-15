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
        if(!this.startEventName)this.startEventName=this.url+":start";
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
        getUrl:function(datatype) {
            datatype = datatype || this.datatype;
            return siteroot + this.url.replace('{DATATYPE}',datatype);
        },
        triggerOnSuccess : function(result){
            $(this).trigger(this.successEventName,result);
        },
        triggerOnError : function(result){
            $(this).trigger(this.errorEventName,result);
        },
        onStart: function(func) {
          $(this).on(this.startEventName, func);
        },
        onSuccess: function(func) {
          $(this).on(this.successEventName, func);
        },
        onError: function(func) {
            $(this).on(this.errorEventName, func);
        },
		execute : function(params) {
            params = params || this.getParameters() || {};
            params.options = params.options || {};
            this.call(params);
        },
		call : function (params) {
			if(!params.options.onsuccess){
                if(this.repeatWait){
                    params.options.onsuccess = this._getRepeatWaitCallFunciton();
                } else {
                    params.options.onsuccess = this._getUsualCallFunciton();
                }
            }
            this.nativeCall(params);
        },
		_getUsualCallFunciton : function(){
			var self = this;
			return (function(result){
                if (!result) {
                   self.triggerOnError(result);
                   return;
                }
                if (!!self.wrap) result = self.wrap(result);
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
        nativeCall : function(params){
            var options = params.options;
            $.extend(options, {
                datatype : this.datatype,
                url : this.getUrl(this.datatype)
            });
            $.ajax({
                url: options.url,
                type : !params ? "GET" : "POST",
                dataType: options.datatype,
                data : params || {}
            })
                .success(function(r){options.onsuccess(r,params)})
                .error(options.onerror||function(error){console.log(error)});
        }
    });

})();