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
            onsuccess = onsuccess || (function(result){
                if (!result) {
                   this.triggerOnError(result);
                    return;
                }
                if (this.wrap) result = this.wrap(result);
                this.triggerOnSuccess(result);
            });
            this.nativeCall(params,onsuccess, onerror);
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