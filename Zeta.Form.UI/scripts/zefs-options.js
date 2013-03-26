(function(){
var root = window.zefs = window.zefs || {};
var api = root.api = root.api || {};
var Command = window.qweb.Command;

$.extend(api,(function(){
	return {
		server : {
            start : function(){ this.ready.execute() },
            state : new Command({domain:"zefs",name:"server",title:"Статус сервера"}),
            restart : new Command({domain:"zefs",name:"restart",title:"Перезапуск сервера"}),
            ready : new Command({domain:"zefs",name:"ready",title:"Проверка доступности сервера", timeout:10000})
        },

        session : {
            start : $.extend (new Command({domain:"zefs", name:"start"}), {
                getParameters : function() { return api.getParameters() },
                wrap : function(obj) {
                    $.extend(obj,{
                        // признак завершения отрисовки сессии
                        wasRendered : false,
                        // контейнер для структуры
                        structure : {},
                        // контейнер для ячеек с данными
                        data : []
                    });
                    return obj;
                }
            }),

            structure : $.extend(new Command({domain:"zefs", name:"struct"}), {
                wrap : function(obj) {
                    var result = {
                        // массив строк
                        rows : [],
                        // массив колонок
                        cols : []
                    };
                    $.each(obj, function(i,o) {
                        if (o.type=="c") {
                            o.exref = o.exref || false;
                            result.cols.push(o);
                        }
                        if (o.type=="r") {
                            o.measure = o.measure || "тыс. руб.";
                            o.level = o.level || 0;
                            o.exref = o.exref || false;
                            result.rows.push(o);
                        }
                    });
                    result.rootrow = $($.map(result.rows, function(e) { if (e.level == 0) return e.code })).get(0);
                    return result;
                }
            })
        },

        data : {
            // Комадна получения данных
            start : $.extend(new Command({domain: "zefs", name: "data"}), {
                 wrap : function(obj) {
                     obj.data = obj.data || [];
                     $.extend(obj, {
                         // признак применения батча к таблице
                         wasFilled : false
                     });
                     $.each(obj.data, function(i,o) {
                         $.extend(o, {
                             row : this.i.split(":")[0],
                             col : this.i.split(":")[1]
                         });
                     });
                     return obj;
                 }
            }),
            // Команда, сообщающая серверу о завешении получения данных
            loaded : new Command({domain: "zefs", name: "dataloaded"}),
            // Команда повторной закачки данных
            reset : new Command({domain: "zefs", name: "resetdata"}),
            // Команда сохранения измененных данных
            // на вход ждет объект вида: {id: (id ячейки), value: (новое значение) , ri: (id в базе)}
            save : new Command({domain: "zefs", name: "save"}),
            // Команда инициализации сессии сохранения
            saveready : $.extend(new Command({domain: "zefs", name: "saveready"}), {
                getParameters : function() { return api.getParameters() }
            }),
            // Команда проверки состояния сохранения
            savestate : new Command({domain: "zefs", name: "savestate"})
        },

        lock : {
            // Команда блокировки формы
            set : $.extend(new Command({domain: "zefs", name: "lockform"}), {
                getUrl: function() {
                    if (location.host.search('admin|corp|133|49') != -1 || location.port == '448' || location.port == '449') return '/ecot/form/setstate.rails';
                    return '/eco/form/setstate.rails';
                },
                getParameters: function() {
                    var s = root.myform.currentSession;
                    if ($.isEmptyObject(s)) return;
                    return {
                        object: s.ObjInfo.Id,
                        period: s.Period,
                        detail: 0,
                        year: s.Year,
                        tcode: s.FormInfo.Code
                    };
                }
            }),
            // Команда получения текущего статуса блокировки
            state : new Command({domain: "zefs", name: "currentlockstate"}),
            // Команда получения статуса возможности блокировки
            canlock : new Command({domain: "zefs", name: "canlockstate"}),
            // Команда получения списка блокировок
            history : $.extend(new Command({domain: "zefs", name: "locklist"}), {
                wrap: function(obj) {
                    if ($.isEmptyObject(obj)) return obj;
                    // Пишем сюда нормальную преобразованную дату
                    $.each(obj, function(i,o) {
                        o.Date = eval(o.Version.substring(2));
                    });
                    return obj;
                }
            })
        },

        file : {
            // команда получения списка прикрепленных к форме файлов
            list : $.extend(new Command({domain: "zefs", name: "attachlist"}), {
                wrap : function(obj) {
                    if ($.isEmptyObject(obj)) return obj;
                    $.each(obj, function(i,o) {
                        o.Date = eval(o.Version.substring(2));
                    });
                    return obj;
                }
            }),
            // команда прекрепления или обновления файла к форме
            add : new Command({domain: "zefs", name: "attachfile", useProgress:true}),
            // команда скрытия/удаления файла
            delete : new Command({domain: "zefs", name: "deletefile"}),
            // команда загрузки файла
            download : $.extend(new Command({domain: "zefs", name: "downloadfile"}), {
                datatype: "filedesc",
                getUrl:function(uid) {
                    return siteroot + this.url.replace('{DATATYPE}',this.datatype) + "?session=" + window.zefs.myform.sessionId + "&uid=" + uid;
                }
            }),
            // команда получения возможных типов файлов
            gettypes : new Command({domain: "zefs", name: "getfiletypes"})
        },

        metadata : {
            celldebug : new Command({ domain: "zefs", name: "evalstack" }),
            cellhistory : $.extend(new Command({domain: "zefs", name: "cellhistory"}), {
                wrap : function(obj) {
                    if ($.isEmptyObject(obj)) return obj;
                    obj.cell.Date = eval(obj.cell.version.substring(2));
                    $.each(obj.history, function(i,o) {
                        o.Date = eval(o.time.substring(2));
                    });
                    return obj;
                }
            }),
            //команда, возвращающая каталог периодов
            getperiods : $.extend(new Command({domain: "zeta", name: "getperiods"}), {
                // Ждем задачу ZC-404, которая изменит структуру результата команды
                wrap : function(obj) {
                    var years = {
                        100 : {
                            type : "Year",
                            periods : {}
                        }
                    }
                    $.each([2013,2012,2011,2010], function(i,y) {
                        years[100].periods[i] = { id: y, name: y, type: "Year"}
                    });
                    $.extend(obj, years);
                    return obj;
                }
            }),
            //команда, возвращающая список доступных предприятий
            getobjects : $.extend(new Command({domain: "zeta", name: "getobjects"}), {
                wrap : function(obj) {
                    var myobjs = [];
                    $.each(obj.objs, function(i, o) {
                        if (o.ismyobj) myobjs.push(o);
                    });
                    $.extend(obj, { my: myobjs});
                    return obj;
                }
            })
        },

        dataType : "json"
	}
})())
})();
