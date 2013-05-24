(function(){
    var root = window.qweb = window.qweb || {};
    var siteroot = document.location.pathname.match(/([\\w\\d_\-]+)?/)[0];

	if(window.loadBalancer) {
		window.loadBalancer.getMostFreeServer(
			function(s) {
				siteroot = 'https://' + s.host + '/' + s.app + '/';
				global.initQweb();
			}
		);
	} else {
		global.initQweb();
	}
	
	function initQweb() {
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
			getParameters : function() { return {} },
			getPrimaryServer : function() {
				return "";
			},
			getSecondaryServer : function() {
				return "";
			},
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
			triggerOnComplete : function(result){
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
					result = self.wrap(result);
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
					url : this.getUrl(this.datatype)
				});
				var ajaxinfo = {
					url: myoptions.url,
					type : !params ? "GET" : "POST",
					dataType: myoptions.datatype,
					data : params || {}
				};
				if(this.useProgress){
					$.extend(ajaxinfo,{
						xhr: function() {
							var x = $.ajaxSettings.xhr();
							if(x.upload) {
								x.upload.addEventListener('progress', function(e) {self.triggerOnProgress(e)}, false);
							}
							return x;
						},
						/*xhrFields: { withCredentials: true },
						crossDomain: true,*/
						cache: false,
						contentType: false,
						processData: false
					});
				}
				$.ajax(ajaxinfo)
					.success(function(r){
						myoptions.onsuccess(r,params);
					})
					.error(myoptions.onerror/* ||
					function(error) {
						log.W(new LogMessage({
							code : error.status,
							status : error.statusText,
							innerException : JSON.parse(error.responseText)
						}));
					}*/)
					.complete(function(r) {
						self.triggerOnComplete(r)
					});

			}
		});
	}
})();