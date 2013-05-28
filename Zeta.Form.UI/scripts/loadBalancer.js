var sa;

(function() {    
    var global = window.loadBalancer = window.psychosis = this; // let the name is psychosis
	
	this.pollApp = function(target, done, error) {
		$.ajax(
			{
				'url' : target.protocol + '://' + target.host + '/' + target.app + '/zefs/' + 'nodeload.json.qweb',
				'crossDomain' : true,
				'dataType' : 'json',
				'timeout' : 600,
				'xhrFields' : {
					'withCredentials' : true
				}
			}
		).done(done).error(error);
	},

	this.pollServer = function(target, callback) {
		var serverStat = {
			stat : new Object(),
			total : 0
		};
	
		for(app = 0; app < target.apps.length; app++) {
			(function(s, i) {
				global.pollApp(
					{
						protocol : target.protocol,
						host : target.host,
						app : target.apps[i]
					},
						
					function(json) {
						s.stat[target.apps[i]] = json.Availability;
						if(++s.total == target.apps.length) callback(s.stat);
					},
						
					function() {
						s.stat[target.apps[i]] = 0;
						if(++s.total == target.apps.length) callback(s.stat);
					}
				);
			})(serverStat, app);
		}
	},
	
	this.pollCloud = function(cloudMap, callback) {
		var cloudStats = {
			stat : new Array(),
			total : 0
		};

		for(srv = 0; srv < cloudMap.length; srv++) {
			(function(s, i) {
				global.pollServer(
					cloudMap[i],
					
					function(r) {
						s.stat[i] = r;
							
						if(++s.total == cloudMap.length) {
							callback(s.stat);
						}
					}
				);
			})(cloudStats, srv);
		}
	},
	
    this.getMostFreeServer = function(callback) {	
		global.pollCloud(serversMap, function(cs) {sa = cs});
    };
})();