(function(){
var root = window.zefs = window.zefs || {};
var api = root.api = root.api || {};
var Command = window.qweb.Command;

api.siterootold = function(){
    if (location.host.search('admin|corp|133|49') != -1 || location.port == '448' || location.port == '449') return '/ecot/';
    //      else if (location.host.search('assoi') == 0 || location.port == '447') return '/eco/';
    return '/eco/';
};

api.getParameters = function(){
    // Парсим параметры из хэша
    var p = {};
    var result = {};
    if (location.hash == "") return null;
    $.each(location.hash.substring(1).split("|"), function(i,e) {
        p[e.split("=")[0]] = e.split("=")[1];
    });
    result["form"] = p["form"];
    result["obj"] = p["obj"];
    result["period"] = p["period"];
    result["year"] = p["year"];
    return result;
};

$.extend(api,(function(){
	return {
		server : {
            start : function(){ this.ready.execute() },
            state : new Command({domain:"zefs",name:"server",title:"Статус сервера"}),
            restart : new Command({domain:"zefs",name:"restart",title:"Перезапуск сервера"}),
            restartall : function() {
                $.ajax({ url: "zefs/restart.qweb" });
                var apps = ["zefs","zefs1","zefs2","zefs3","zefs4"];
                var status = {
                    success : [],
                    faild : [],
                    count : 0
                };
                for (var i in apps) {
                    var url = "/" + apps[i] + "/zefs/restart.qweb";
                    var ajax = $.ajax({ url: url });
                    ajax.success(function() {
                        status.success.push(apps[i]);
                    });
                    ajax.error(function() {
                        status.faild.push(apps[i]);
                    });
                    ajax.complete(function() {
                        status.count++;
                        if (status.count == apps.length) {
                            var content = status.faild.length == 0
                                ? "<p>Все сервера успешно перезапущены</p>"
                                : "<p>С перезагрузкой некоторых серверов возникли проблемы</p></br>";
                            for (var s in status.faild) {
                                content += '<strong>' + status.faild[s] + '</strong></br>';
                            }
                            $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                                title: "Перезапуск серверов zefs",
                                content: $("<p/>").html(content)
                            });
                        }
                    });
                }
            },
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
                    var currentRow = null;
                    $.each(obj, function(i,o) {
                        if (o.type=="c") {
                            o.exref = o.exref || false;
                            result.cols.push(o);
                            if (!!o.validate) {
                                switch (o.validate) {
                                    case "validate-int-cell-number" :
                                        o.validate = "-?[0-9]+"; break;
                                    default :
                                        break;
                                }
                            }
                        }
                        if (o.type=="r") {
                            o.measure = o.measure || "тыс. руб.";
                            o.level = o.level || 0;
                            o.exref = o.exref || false;
                            o.childrens = o.childrens || [];
                            /*if (!!currentRow) {
                                if (o.level > currentRow.level) {
                                    o.parent = currentRow;
                                    currentRow.childrens.push(o);
                                    currentRow = o;
                                } else {
                                    currentRow.parent.childrens.push(o);
                                    currentRow = currentRow.parent;
                                }
                            } else {
                                currentRow = o;
                                o.parent = result.rows;
                            }*/
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
            setstateold : $.extend(new Command({domain: "zefs", name: "lockform"}), {
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
            // Команда новый блокировки формы
            set : $.extend(new Command({domain: "zefs", name: "setstate"}), {
                getParameters: function() {
                    var s = root.myform.currentSession;
                    if ($.isEmptyObject(s)) return;
                    return {
                        session: root.myform.sessionId,
                        obj: s.ObjInfo.Id,
                        period: s.Period,
                        year: s.Year,
                        form: s.FormInfo.Code
                    };
                }
            }),
            // Единая команда для получения статуса блокировок
            state : new Command({domain: "zefs", name: "getlockstate"}),
            // Команда получения текущего статуса блокировки
//            state : new Command({domain: "zefs", name: "currentlockstate"}),
            // Команда получения статуса возможности блокировки
//            canlock : new Command({domain: "zefs", name: "canlockstate"}),
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
            }),
            // Команда запроса статуса по предприятиям куратора
            curatorstate : new Command({domain: "zefs", name: "getcurratorlockstate"})
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

        chat : {
            list : $.extend(new Command({ domain: "zefs", name: "chatlist" }), {
                getParameters: function() {
                    var s = root.myform.currentSession;
                    if ($.isEmptyObject(s)) return;
                    return {
                        session: root.myform.sessionId,
                        obj: s.ObjInfo.Id,
                        period: s.Period,
                        year: s.Year,
                        form: s.FormInfo.Code
                    };
                },

                wrap : function(obj) {
                    if ($.isEmptyObject(obj)) return obj;
                    $.each(obj, function(i,o) {
                        o.Date = eval(o.Time.substring(2));
                        switch (o.Type) {
                            case "default" :
                                o.ReadableType = "Лента";
                                break;
                            case "formcurrator" :
                                o.ReadableType = "Куратору формы";
                                break;
                            case "objcurrator" :
                                o.ReadableType = "Куратору предприятия";
                                break;
                            case "support" :
                                o.ReadableType = "Поддержка";
                                break;
                            case "locks" :
                                o.ReadableType = "По блокировкам";
                                break;
                        }
                    });
                    return obj;
                }
            }),
            add : $.extend(new Command({domain: "zefs", name: "chatadd" }), {
                getParameters: function() {
                    var s = root.myform.currentSession;
                    if ($.isEmptyObject(s)) return;
                    return {
                        session: root.myform.sessionId,
                        obj: s.ObjInfo.Id,
                        period: s.Period,
                        year: s.Year,
                        form: s.FormInfo.Code
                    };
                }
            }),
            // возвращает список сообщений
            // from : date (DEFAULT LAST 30 DAY), showarchived : bool (DEFAULT false), typefilter : string (DEFAULT NULL)
            get : $.extend(new Command({domain: "zecl", name: "get"}), {
                wrap : function(obj) {
                    if ($.isEmptyObject(obj)) return obj;
                    $.each(obj, function(i,o) {
                        o.Date = eval(o.Time.substring(2));
                        switch (o.Type) {
                            case "default" :
                                o.ReadableType = "Лента";
                                break;
                            case "formcurrator" :
                                o.ReadableType = "Куратору формы";
                                break;
                            case "objcurrator" :
                                o.ReadableType = "Куратору предприятия";
                                break;
                            case "support" :
                                o.ReadableType = "Поддержка";
                                break;
                        }
                    });
                    return obj;
                }
            }),
            // пометить конкретное сообщение как прочитанное (архивированное)
            archive : new Command({domain: "zecl", name: "archive"}),
            // вовращает boolean - true - если есть обновления после lastread
            updatecount : new Command({domain: "zecl", name: "updatecount"}),
            // получить дату последней отметки zecl/haveread
            getlastread : new Command({domain: "zecl", name: "getlastread"}),
            // устанавливает внутреннюю переменную lastread
            haveread : new Command({domain: "zecl", name: "haveread"})
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
            getperiods : $.extend(new Command({domain: "zeta", name: "getperiods",cachekey:"zeta__getperiods"}), {
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
            //команда, возвращающая список доступных форм
            getforms : $.extend(new Command({domain: "zefs", name: "bizprocesslist",cachekey:"zefs__bizprocesslist"}), {
                wrap : function(obj) {
                    obj.groups = [];
                    obj.parents = [];
                    $.each(obj, function(i,g) {
                        if ($.inArray(g.Group,obj.groups) == -1) {
                            obj.groups.push(g.Group);
                        }
                        if ($.inArray(g.Parent,obj.parents) == -1) {
                            obj.parents.push(g.Parent);
                        }
                    });
                    return obj;
                }
            }),
            //команда, возвращающая список доступных предприятий
            getobjects : $.extend(new Command({domain: "zeta", name: "getobjects",cachekey:"zeta__getobjects"}), {
                wrap : function(obj) {
                    var myobjs = [];
                    $.each(obj.objs, function(i, o) {
                        if (o.ismyobj) myobjs.push(o);
                    });
                    $.extend(obj, { my: myobjs});
                    return obj;
                }
            }),
            //команда которая возвращающает регламент по блокировкам
            getreglament : new Command({domain: "zefs", name: "getreglament"}),
            //команда, позволяющая получить список пользователей предприятия, имеющих доступ к данной форме
            getformusers : new Command({domain: "zefs", name: "responsibleusers"}),
            getnews : $.extend(new Command({domain: "message", name: "getnews"}), {
                url : location.protocol + "//" + location.host + api.siterootold() + "message/getnews.{DATATYPE}.qweb",
                wrap : function(obj) {
                    $.each(obj, function(i,o) {
                        o.Date = eval(o.Version.substring(2));
                    });
                    return obj;
                }
            }),
            archivenews : $.extend(new Command({domain: "message", name: "getnews"}), {
                url : location.protocol + "//" + location.host + api.siterootold() + "message/archive.rails"
            })
        },

        dataType : "json"
	}
})())
})();
