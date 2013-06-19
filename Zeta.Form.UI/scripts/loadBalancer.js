(function(config) {    
    var global = window.loadBalancer = window.psychosis = this;
	
	this.sysState = {
		bootstrapDone : false
	},
	
	this.groups = {
		db : new Object(),
		flow : new Array(),
		totalMigrations : 0,
		
		migrateGroup : function(group, newHandler) {
			if(global.groups.getHotMigrationProperty(group)) {
				global.groups.setHandler(group, newHandler);
			}
		},
		
		registerGroup : function(group, hotMigration) {
			global.groups.db[group] = {
				hotMigration : hotMigration,
				migrationsCount : 0
			};
		},
		
		initGroup : function(group, hotMigration, handler) {
			global.groups.registerGroup(group, hotMigration);
			global.groups.setHandler(group, handler);
		},
		
		createGroup : function(group, config) {			
			if(config.handler) {
				global.groups.initGroup(group, config.hotMigration, config.handler);
			} else {
				global.groups.registerGroup(group, config.hotMigration);
				global.groups.refreshHandler(group);
			}
		},
		
		setHandler : function(group, handler) {
			if(global.groups.checkGroupExists(group)) {
				global.groups.db[group].handler = handler;
			}
		},
		
		getCurrentGroupHandler : function(group) {
			if(global.groups.checkGroupExists(group)) {
				if(!global.groups.db[group].handler) {
					return undefined;
				} else {
					return global.groups.db[group].handler;
				}
			} else {
				return undefined;
			}
		},
		
		refreshHandler : function(group) {	
			global.pool.getHandler(
				function(handler) {
					if(global.groups.getCurrentGroupHandler(group) === undefined) {
						global.groups.setHandler(group, handler);
					} else {
						global.groups.migrateGroup(group, handler);
					}
				}
			);
		},
		
		getHotMigrationProperty : function(group) {
			if(global.groups.checkGroupExists(group)) {
				return global.groups.db[group].hotMigration;
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
		
		getGroupmigrationsCount : function(group) {
			if(global.groups.checkGroupExists(group)) {
				return global.groups.db[group].migrationsCount;
			} else {
				return -1;
			}
		},
		
		increaseMigrations : function(group) {
			if(global.groups.checkGroupExists(group)) {
				return global.groups.db[group].migrationsCount++;
			}
		},
		
		migrationControl : {
		
		},

		flowControl : {
			push : function(group, callback) {
				if(!global.groups.flow[group]) {
					global.groups.flow[group] = new Array();
				}
					
				global.groups.flow[group].push(callback);
			},
			
			shift : function(group) {
				if(!global.groups.flow[group]) {
					return undefined;
				}
				
				var callback = global.groups.flow[group].shift();
				
				if(global.groups.flow[group].length) {
					delete global.groups.flow[group];
				}
				
				return callback;
			},		
		
			handleGroup : function(group) {
				if(!global.groups.flow[group]) return;
			
				var groupHandler = global.groups.getCurrentGroupHandler(group);
			
				for(i = 0; i < global.groups.flow[group].length; i++) {
					global.groups.flow[group][i](groupHandler);
					delete global.groups.flow[group][i];
				}
				
				delete global.groups.flow[group];
			},
			
			removeGroup : function(group) {
				delete global.groups.flow[group];
			},
			
			checkGroupExists : function(group) {
				if(global.groups.flow[group]) {
					return true;
				} else {
					return false;
				}
			},
			
			handle : function() {
				var groups = Object.keys(global.groups.flow);
				
				for(i = 0; i < groups.length; i++) {
					global.groups.flowControl.handleGroup(groups[i]);
				}
			}
		},
		
		handle : function(group, callback) {
			global.groups.flowControl.push(group, callback);
		
			if(global.sysState.bootstrapDone) {
				global.groups.flowControl.handleGroup(group);
			}
		}
	},
	
	this.watchdog = {
		cloudStat : new Object(),
		
		heartbeat : function(callback) {
			global.poll.cloud(
				config.cloud.map,
				function(cloudStat) {
					global.watchdog.cloudStat = cloudStat;
					callback();
				}
			);
		},
		
		pulse : function() {
			global.watchdog.heartbeat(global.watchdog.migrate);
		},
		
		migrate : function() {
			var groups = Object.keys(global.groups.db);
			global.groups.totalMigrations++;
			
			for(i = 0; (i < groups.length) && (i < config.watchdog.migrateGroupsPerStep); i++) {
				var groupMigrations = global.groups.getGroupmigrationsCount(groups[i]);
				
				if((groupMigrations != -1) && (groupMigrations < global.groups.totalMigrations)) {
					global.groups.refreshHandler(groups[i]);
				} else {
					global.groups.increaseMigrations(groups[i]);
				}
			}
		}
	},
	
	this.poll = {
		app : function(target, done, error) {
			$.ajax(
				{
					url : target.protocol + '://' + target.host + '/' + target.app + '/' + config.polling.controller + '/' + config.polling.action,
					dataType : 'json',
					timeout : config.polling.timeout,
					xhrFields: {
						withCredentials : true
					},
                    crossDomain : true
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
			
			return list;
		},
		
		getHandler : function(callback) {
			var list = global.pool.select(
				{
					count : {
						servers : 1,
						apps : 1
					}
				},
				
				global.watchdog.cloudStat
			);
			
			callback(
				{
					protocol : 'https',
					host : list[0].server,
					app : list[0].apps[0]
				}
			);
		}
	},
	
	this.bootstrap = function(config) {
		global.watchdog.heartbeat(
			function() {
				if(config.groups) {
					$.each(
						config.groups,
						function(groupName, config) {
							global.groups.createGroup(groupName, config);
						}
					);
				}
			
				global.sysState.bootstrapDone = true;
				global.groups.flowControl.handle();
				
				setTimeout(global.watchdog.pulse, config.watchdog.pulseTime);
			}
		);
	};
	
	this.bootstrap(config);
})(
	{
		groups : {
			master : {
				hotMigration : true
			},
			
			margarita : {
				hotMigration : false
			},
			
			voland : {
				hotMigration : false,
				handler : {
					protocol : 'https',
					host : 'admin-assoi.ugmk.com',
					app : 'zefs27'
				}
			}
		},
		
		watchdog : {
			pulseTime : 120000,
			migrateGroupsPerStep : 5
		},
		
		polling : {
			timeout : 1200,
			controller : 'zefs',
			action : 'nodeload.json.qweb'
		},
		
		cloud : {
			map : serversMap
		}
	}
);