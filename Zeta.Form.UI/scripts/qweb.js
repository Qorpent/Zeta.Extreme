(function(){
    var root = window.qweb = window.qweb || {};
    var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
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
        if(!this.onsuccess)this.onsuccess=this.url+":success";
        if(!this.onsuccess)this.onsuccess=this.url+":error";
        var self = this;
        this.execute = function() {
            self.call();
        }

        self.call = function (params,onsuccess,onerror) {

            var onsuccess = onsuccess || (function(result){
                if (!result) {
                    $(root).trigger(self.onerror, result);
                    return;
                }
                $(root).trigger(self.onsuccess, result);
            });
            self.nativeCall(params,onsuccess, onerror) ;
        }
    };

    $.extend(root.Command.prototype,{
        datatype : "json",
        getUrl:function(datatype){
            var datatype = datatype || self.dataType || "json";
                       return siteroot+this.url.replace('{DATATYPE}',datatype);
        },
        nativeCall : function(params,onsuccess,onerror,url,datatype){
            var datatype = datatype || this.datatype || "json";
            var url =url || this.getUrl(this.datatype);
            $.ajax({
                url:url ,
                dataType: datatype,
                data : params || {}
            })
                .success(onsuccess)
                .error(onerror||function(error){console.log(error)});
        }
    });

})();