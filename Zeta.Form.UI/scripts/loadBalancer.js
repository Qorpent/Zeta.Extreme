(function() {
    var serversState = new Object();    
    var global = window.loadBalancer = this;
    var serversHandled = 0;

    this.getMostFreeServer = function(callback) {
        this.getStatistics = function(
            protocol,
            host,
            application
        ) {         
            $.ajax(
                {
                    url : protocol + '://' + host + '/' + application + '/zefs' + '/nodeload.json.qweb',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain : true,
                    dataType : 'json',
                    success: function(json) {
                        serversState[host][application] = json.Availability;
                    },
                    timeout : 600
                }
            ).error(
                function() {
                    serversState[host][application] = 0;
                }
            );
        },
                    
        this.connect = function() {
            var maApps = new Array();
                        
            this.searchServerIndexInMap = function(server) {
                for(i = 0; i < serversMap.length; i++) {
                    if(serversMap[i].host == server) {
                        return i;
                    }
                }
            },
                        
            this.searchAppName = function(host, value) {
                srvIndex = searchServerIndexInMap(host);

                for(i = 0; i < serversMap[srvIndex].apps.length; i++) {
                    if(serversState[host][serversMap[srvIndex].apps[i]] == value) {
                        return serversMap[srvIndex].apps[i];
                    }
                }
            },
                            
            this.getInstanceByAvailability = function(availability) {   
                for(i = 0; i < serversMap.length; i++) {
                    for(k = 0; k < serversMap[i].apps.length; k++) {
                        if(serversState[serversMap[i].host][serversMap[i].apps[k]] == availability) {
                            return {'host' : serversMap[i].host, 'app' : serversMap[i].apps[k]};
                        }
                    }
                }
            };
                        
            for(i = 0; i < serversMap.length; i++) {
                var arr = Object.keys(
                    serversState[serversMap[i].host]
                ).map(
                    function (key) {
                        return serversState[serversMap[i].host][key];
                    }
                );

                maApps.push(Math.max.apply(null, arr));
            }

            callback(
                getInstanceByAvailability(
                    Math.max.apply(null, maApps)
                )
            );
        };
                
        for(i = 0; i < serversMap.length; i++) {
            serversHandled++;
            serversState[serversMap[i].host] = new Object();
                    
            for(k = 0; k < serversMap[i].apps.length; k++) {
                this.getStatistics(
                    serversMap[i].protocol,
                    serversMap[i].host,
                    serversMap[i].apps[k]
                );
            }
        }
                    
        setTimeout(this.connect, 2000);
    };
})();