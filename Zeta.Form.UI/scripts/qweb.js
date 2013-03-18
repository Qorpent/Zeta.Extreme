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
        if(!this.progressEventName)this.progressEventName=this.url+":progress";
		this.repeatWait = false;
        this.useProgress = !!options.useProgress;

		if(options.timeout){
			this.basetimeout = options.timeout;
			this.currenttimeout = this.basetimeout;
			this.delay  = options.delay || 1000;
			this.repeatWait = true;
		}
    };

    $.extend(root.Command.prototype,{
        datatype : "json",
        getParameters : function() { return {} },
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
        triggerOnProgress : function(result){
            $(this).trigger(this.progressEventName,result);
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
        onProgress: function(func) {
            $(this).on(this.progressEventName, func);
        },
		execute : function(params,options) {
            params = params || {};
            $.extend(params, this.getParameters());
            this.call(params,options);
        },
		call : function (params,options) {
            options = options || {};
			if(!options.onsuccess){
                if(this.repeatWait){
                    options.onsuccess = this._getRepeatWaitCallFunciton();
                } else {
                    options.onsuccess = this._getUsualCallFunciton();
                }
            }
            this.nativeCall(params,options);
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
        nativeCall : function(params, options){
            var myoptions = options || {};
            $.extend(myoptions, {
                datatype : this.datatype,
                url : this.getUrl(this.datatype)
            });
            var ajaxinfo = {
                url: myoptions.url,
                type : !params ? "GET" : "POST",
                dataType: myoptions.datatype,
                data : params || {}
            };
            if(this.useProgress){
                var self = this;
                $.extend(ajaxinfo,{
                    xhr: function() {
                        var x = $.ajaxSettings.xhr();
                        if(x.upload) {
                            x.upload.addEventListener('progress', function(e) {self.triggerOnProgress(e)}, false);
                        }
                        return x;
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                });
            }
            $.ajax(ajaxinfo)
                .success(function(r){
                    myoptions.onsuccess(r,params);
                })
                .error(myoptions.onerror||function(error){console.log(error)});


        }
    });

})();