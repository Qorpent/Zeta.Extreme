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
            var dataType = self.dataType || "json";
            var url = self.url;
            var onsuccess = onsuccess || (function(result){
                if (!result) {
                    $(root).trigger(self.onerror, result);
                    return;
                }
                $(root).trigger(self.onsuccess, result);
            });
            self.nativeCall(url,dataType,params,onsuccess, onerror) ;
        }
    };

    $.extend(root.Command.prototype,{
        nativeCall : function(url,datatype,params,onsuccess,onerror){
            $.ajax({
                url: siteroot+url.replace('{DATATYPE}',datatype),
                dataType: datatype,
                data : params || {}
            })
                .success(onsuccess)
                .error(onerror||function(error){console.log(error)});
        }
    });

})();