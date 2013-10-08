(function(){
    var root = window.qweb = window.qweb || {};
    var cache = root.cache= root.cache || {};
    var siteroot = document.location.pathname.match(/([\\w\\d_\-]+)?/)[0];

    var Log = function() {
        this.init();
    };

    Log.prototype.init = function() {
        this.messages = [];
    };

    Log.prototype.Get = function() {
        return this.messages;
    };

    Log.prototype.W = function(m) {
        if (this.messages.length > 199) {
            this.messages.splice(0, 100);
        }
        this.messages.push(m)
    };

    var log = window.qweb.log = window.qweb.log || new Log();

    var LogMessage = function(params) {
        params = $.extend({
            type : "error",
            code : 0,
            status : "",
            message : "",
            innerException : {}
        }, params);
        this.type = params.type;
        this.event = params.event;
        this.message = params.message;
        this.code = params.code;
        this.status = params.status;
        this.innerException = params.innerException;
    };

    root.Command = function(sourceoptions){
        var options = sourceoptions || {};
        if (typeof(options)=="string"){
            options = {name:options};
        }
        if (!!options.async) options.async = true;
        $.extend(this,options);
        if(!this.url){
            var domain = this.domain || "_sys";
            this.url = domain+"/"+this.name+".{DATATYPE}.qweb";
        }
        if(!this.startEventName)this.startEventName=this.url+":start";
        if(!this.successEventName)this.successEventName=this.url+":success";
        if(!this.errorEventName)this.errorEventName=this.url+":error";
        if(!this.progressEventName)this.progressEventName=this.url+":progress";
        if(!this.completeEventName)this.completeEventName=this.url+":complete";
        if(!this.finishEventName)this.finishEventName=this.url+":finish";
		this.repeatWait = false;
        this.useProgress = !!options.useProgress;
        this.cachekey = options.cachekey || "";
        this.wrap = options.wrap || function(r){return r;};
		if(options.timeout){
			this.basetimeout = options.timeout;
			this.currenttimeout = this.basetimeout;
			this.delay  = options.delay || 1000;
			this.repeatWait = true;
		}
    };

    $.extend(root.Command.prototype, {
        datatype : "json",
        group : "",
        getParameters : function() { return {} },
        getPrimaryServer : function() {
            return "";
        },
        getSecondaryServer : function() {
            return "";
        },
        getUrl:function(datatype, group) {
            datatype = datatype || this.datatype;
            return siteroot + this.url.replace('{DATATYPE}',datatype);
        },

        prepareResult: function(result) {
            if (Object.prototype.toString.call(result).indexOf("Array") != -1) {
                var r = {};
                $.each(result, function(i, e) {
                    r[i] = e;
                });
                result = r;
            }
            return result;
        },

        triggerOnSuccess : function(result){
            result = this.prepareResult(result);
            $(this).trigger(this.successEventName,result);
        },
        triggerOnError : function(result){
            result = this.prepareResult(result);
            $(this).trigger(this.errorEventName,result);
        },
        triggerOnProgress : function(result){
            result = this.prepareResult(result);
            $(this).trigger(this.progressEventName,result);
        },
        triggerOnComplete : function(result){
            result = this.prepareResult(result);
            $(this).trigger(this.completeEventName,result);
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
        onComplete: function(func) {
            $(this).on(this.completeEventName, func);
        },
        onFinished: function(func) {
            $(this).on(this.finishEventName, func);
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
                if (!result && result != 0) {
                   self.triggerOnError(result);
                   return;
                }

                result = typeof result == "object" ? self.wrap(result) : result;
                self.triggerOnSuccess(result);
            });
		},
		_getRepeatWaitCallFunciton : function(){
			var self = this;
			return (function(result,params){
                if(!result && result != 0){
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
            if (this.cachekey){
                if(root.embedStorage && root.embedStorage[this.cachekey] ){
                    this.triggerOnSuccess(this.wrap(root.embedStorage[this.cachekey]));
                    return;
                }
            }
            var self = this;
            var myoptions = options || {};
            $.extend(myoptions, {
                datatype : this.datatype,
                url : this.getUrl(this.datatype, this.group)
            });
            var method = "GET";
            if((params && JSON.stringify(params).length>200) || this.useProgress ){
                method = "POST";
            }
            var ajaxinfo = {
                url: myoptions.url,
                type : method,
                dataType: myoptions.datatype,
                data : params || {},
                crossDomain: true,
                ifModified: method=="GET"
//                cache: true
            };
            ajaxinfo.async = this.async;
            if (this.async) {
                ajaxinfo.xhrFields = { withCredentials: true };
            }
            if(this.useProgress){
                $.extend(ajaxinfo,{
                    xhr: function() {
                        var x = $.ajaxSettings.xhr();
                        if(x.upload) {
                            x.upload.addEventListener('progress', function(e) {self.triggerOnProgress(e)}, false);
                        }
                        return x;
                    },
                    cache: true,
                    contentType: false,
                    processData: false
                });
            }
            $.ajax(ajaxinfo)
                .complete(function(r) {
                    if (304 == r.status) {
                        myoptions.onsuccess(JSON.parse(sessionStorage.getItem(ajaxinfo.url+"?"+JSON.stringify(ajaxinfo.data))),params);
                    }
                    else if( 200 == r.status) {
                        if (!r.responseText.match(/^\s*</) && (r.responseText.indexOf("{") != -1 || r.responseText.indexOf("[") != -1)) {
                            if(ajaxinfo.type=="GET" && r.getResponseHeader("Last-Modified"))   {
                                sessionStorage.setItem(ajaxinfo.url+"?"+JSON.stringify(ajaxinfo.data), r.responseText);
                            }
                            myoptions.onsuccess(JSON.parse(r.responseText),params);
                        }else{
							myoptions.onsuccess(r.responseText,params);
						}
                    }
                    else {
                        self.triggerOnError(r);
                    }
                    self.triggerOnComplete(r);
                });
        }
    });

})();