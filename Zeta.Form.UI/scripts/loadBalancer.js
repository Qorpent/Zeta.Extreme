(function() {    
    var global = window.loadBalancer = window.psychosis = this;
	
	this.poll = {
		app : function(target, done, error) {
			$.ajax(
				{
					'url' : target.protocol + '://' + target.host + '/' + target.app + '/zefs/' + 'nodeload.json.qweb',
					'dataType' : 'json',
					'timeout' : 1200,
					'xhrFields': {
						'withCredentials' : true
					},
                    'crossDomain' : true
				}
			).done(done).error(error);
		},

		server : function(target, callback) {
			var serverStat = {
				stat : new Object(),
				total : 0
			};
		
			for(app = 0; app < target.apps.length; app++) {
				(function(s, i) {
					global.poll.app(
						{
							protocol : target.protocol,
							host : target.host,
							app : target.apps[i]
						},
							
						function(json) {
							s.stat[target.apps[i]] = json.Availability;
							
							if(++s.total == target.apps.length) {
								callback(s.stat);
							}
						},
							
						function() {
							s.stat[target.apps[i]] = 0;
							
							if(++s.total == target.apps.length) {
								callback(s.stat);
							}
						}
					);
				})(serverStat, app);
			}
		},
		
		cloud : function(cloudMap, callback) {
			var cloudStats = {
				stat : new Object(),
				total : 0
			};

			for(srv = 0; srv < cloudMap.length; srv++) {
				(function(s, i) {
					global.poll.server(
						cloudMap[i],
						
						function(r) {
							s.stat[cloudMap[i].host] = r;
							
							if(++s.total == cloudMap.length) {
								callback(s.stat);
							}
						}
					);
				})(cloudStats, srv);
			}
		}
	},
	
	this.sort = {
		getMostFreeApp : function(stat) {
			var appLeaders = new Object();
			var appLeader = {
				'host' : undefined,
				'app' : undefined,
				'av' : 0
			};
		
			$.each(
				stat,
				function(host, obj) {
					appLeaders[host] = {
						'av' : 0,
						'app' : undefined
					};
				
					$.each(
						obj,
						function(app, av) {
							if(appLeaders[host].av < av) {
								appLeaders[host].av = av;
								appLeaders[host].app = app;
							}
						}
					);
				}
			);
			
			$.each(
				appLeaders,
				function(host, obj) {
					if(appLeader.av < obj.av) {
						appLeader.av = obj.av;
						appLeader.host = host;
						appLeader.app = obj.app;
					}
				}
			);
			
			return appLeader;
		}
	},
	
    this.getMostFreeServer = function(callback) {	
		global.poll.cloud(
			serversMap,
			function(cs) {
				var i = global.sort.getMostFreeApp(cs);
				
				while(i.av == 0) {
					i = global.sort.getMostFreeApp(cs);
				}
			
				callback(i);
			}
		);
    };
})();