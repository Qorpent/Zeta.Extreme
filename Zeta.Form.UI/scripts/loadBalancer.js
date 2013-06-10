var cloudHandled;
(function(cloudMap) {    
    var global = window.loadBalancer = window.psychosis = this;
	
	this.groups = {
		db : new Object(),
	
		migrateGroup : function(group, newHandler) {
			if(!global.groups.getNoMigrationProperty(group)) {
				global.groups.setHandler(group, newHandler);
			}
		},
		
		registerGroup : function(group, noMigration) {
			global.groups.db[group] = {
				noMigration : noMigration
			};
		},
		
		setGroupHandler : function(group, handler) {
			if(global.groups.checkGroupExists(group)) {
				global.groups.db[group].handler = handler;
			}
		},
		
		initGroup : function(group, noMigration, handler) {
			global.groups.registerGroup(group, noMigration);
			global.groups.setHandler(group, handler);
		},
		
		getNoMigrationProperty : function(group) {
			if(global.groups.checkGroupExists(group)) {
				return global.groups.db[group].noMigration;
			} else {
				return undefined;
			}
		},
		
		checkGroupExists : function(group) {
			if(global.groups.db[group]) {
				return true;
			} else {
				return false;
			}
		},
		
		getCurrentGroupHandler : function(group) {
			if(global.groups.checkGroupExists(group)) {
				return global.groups.db[group].handler;
			} else {
				return undefined;
			}
		},
		
		refreshHandler : function(group) {
			if(global.groups.getCurrentGroupHandler(group) === undefined) {
				global.pool.getHandler(
					function(handler) {
						global.groups.migrateGroup(group, handler);
					}
				);
			} else {
				if(global.groups.getNoMigrationProperty(group)) {
					return global.groups.getCurrentGroupHandler(group);
				} else {
					global.pool.getHandler(
						function(handler) {
							global.groups.migrateGroup(group, handler);
						}
					);
				}
			}
		}
	},
	
	this.watchdog = {
		cloudMap : new Object(),
		
		pulse : function() {
			global.poll.cloud(
				cloudMap,
				function(cloudStat) {
					global.watchdog.cloudMap = cloudMap;
				}
			);
		},
		
		migrate : function() {

		}
	},
	
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
	
	this.ping = {
		app : function(target, callback) {
			global.poll.app(
				target,
				
				function(json) {
					callback(true);
				},
				
				function() {
					callback(false);
				}
			);
		}
	},
	
	this.pool = {
		select : function(query, source) {
			this.makeSources = function(targets, sources) {
				var r = new Object();
		
				if(targets === undefined) {
					r = sources;
				} else {
					$.each(
						targets,
						function(server, apps) {
							r[server] = new Object();
							
							if(apps.length == 0) {
								r[server] = sources[server];
							} else {
								for(i = 0; i < apps.length; i++) {
									r[server][apps[i]] = sources[server][apps[i]];
								}
							}
						}
					);
				}
				
				return r;
			};
		
			var list = new Array();
			var dirty = {
				servers : new Object(),
				leaders : new Object(),
				sources : this.makeSources(query.servers, source),
				count : {
					servers : 0,
					apps : new Object()
				},
				lista : list
			};

			$.each(
				dirty.sources,
				function(server, apps) {
					dirty.servers[server] = new Object();
					dirty.leaders[server] = 0;
					
					dirty.count.servers++;
					dirty.count.apps[server] = Object.keys(apps).length;
		
					for(a = 0; a < Object.keys(apps).length; a++) {				
						dirty.leaders[server] += dirty.sources[server][Object.keys(apps)[a]];
					}
					
					// relative availability
					dirty.leaders[server] = dirty.leaders[server] / Object.keys(apps).length;
				}
			);
			

			// build the list of most available servers(!), not apps.
			// we will use the relative availability
			for(l = 0; (l < Object.keys(dirty.leaders).length) && (l < query.count.servers); l++) {
				var r = window.math.objects.sort.max(dirty.leaders);
				
				// add the server to the top-list
				list.push(
					{
						server : r.key,
						availability : r.val,
						apps : new Array()
					}
				);
				
				delete dirty.leaders[r.key];

				for(a = 0; (a < dirty.count.apps[r.key]) && (a < query.count.apps); a++) {
					var b = window.math.objects.sort.max(dirty.sources[r.key]);
					
					list[l].apps.push(b.key);
					delete dirty.sources[r.key][b.key];
				}
			}
			
			cloudHandled = dirty;
			
			return list;
		},
		
		getHandler : function(callback) {
			global.handle(
				{
					count : {
						servers : 1,
						apps : 1
					}
				},
				
				function(list) {
					callback(
						{
							protocol : 'https',
							host : list[0].server,
							app : list[0].apps[0]
						}
					);
				}
			);
		}
	},
	
	this.handle = function(query, callback) {
		global.poll.cloud(
			cloudMap,
			function(cloudStat) {
				var list = global.pool.select(
					{
						servers : query.servers,
						count : {
							servers : query.count.servers,
							apps : query.count.apps
						}
					},
					
					cloudStat
				);
				
				callback(list);
			}
		);
	};
	
	setInterval(global.watchdog.pulse, 20000);
})(serversMap);