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
        var self = this;
        this.execute = function(params) {
            params = params || self.getParameters();
            self.call(params);
        };
        self.call = function (params,onsuccess,onerror) {
            onsuccess = onsuccess || (function(result){
                if (!result) {
                   self.triggerOnError(result);
                    return;
                }
                if (self.wrap) result = self.wrap(result);
                self.triggerOnSuccess(result);
            });
            self.nativeCall(params,onsuccess, onerror);
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
        nativeCall : function(params,onsuccess,onerror,url,datatype){
            datatype = datatype || this.datatype;
            url = url || this.getUrl(this.datatype);
            $.ajax({
                url: url,
                type : !params ? "GET" : "POST",
                dataType: datatype,
                data : params || {}
            })
                .success(onsuccess)
                .error(onerror||function(error){console.log(error)});
        }
    });

})();