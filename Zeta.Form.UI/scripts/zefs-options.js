(function(){
var root = window.zefs = window.zefs || {};
var specification = root.api = root.api || {};
var Command = window.qweb.Command;

$.extend(specification,(function(){
	return {
		server : {
            start : function(){ this.ready.execute() },
            state : new Command({domain:"zefs",name:"server",title:"Статус сервера"}),
            restart : new Command({domain:"zefs",name:"restart",title:"Перезапуск сервера"}),
            ready : new Command({domain:"zefs",name:"ready",title:"Проверка доступности сервера", timeout:10000})
        },

        session : {
            start : $.extend (new Command({domain:"zefs", name:"start"}), {
                getParameters : function() { return specification.getParameters() },
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
                            result.cols.push(o);
                        }
                        if (o.type=="r") {
                            o.measure = o.measure || "тыс. руб.";
                            o.level = o.level || 0;
                            result.rows.push(o);
                        }
                    });
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
                getParameters : function() { return specification.getParameters() }
            }),
            // Команда проверки состояния сохранения
            savestate : new Command({domain: "zefs", name: "savestate"})
        },

        lock : {
            // Команда блокировки формы
            start : new Command({domain: "zefs", name: "lockform"}),
            // Команда получения статуса блокировки
            state : new Command({domain: "zefs", name: "currentlockstate"}),
            // Команда получения статуса возможности блокировки
            ispossible : new Command({domain: "zefs", name: "canlockstate"}),
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

        files : {
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
            add : $.extend(new Command({domain: "zefs", name: "attachfile"}), {
                call : function(params) {
                    params = params || {};
                    $.extend(params, {
                        formdata : new FormData(),
                        onsuccess : function() {},
                        onerror : function() {},
                        onprogress : function() {}
                    });
                    this.nativeCall(params.formdata, params.onsuccess, params.onerror, params.onprogress);
                },
                nativeCall: function(formdata, onsuccess, onerror, onprogress) {
                    $.ajax({
                        url: this.getUrl(),
                        type: "POST",
                        context: this,
                        dataType: "json",
                        xhr: function() {
                            var x = $.ajaxSettings.xhr();
                            if(x.upload) {
                                x.upload.addEventListener('progress', function(e) {onprogress(e)}, false);
                            }
                            return x;
                        },
                        data: formdata,
                        cache: false,
                        contentType: false,
                        processData: false
                    })
                        .success(function(r){onsuccess(r)})
                        .error(onerror||function(error){console.log(error)});
                }
            }),
            // команда скрытия/удаления файла
            delete : new Command({domain: "zefs", name: "deletefile"}),
            // команда загрузки файла
            download : $.extend(new Command({domain: "zefs", name: "downloadfile"}), {
                datatype: "filedesc"
            }),
            // команда получения возможных типов файлов
            gettypes : new Command({domain: "zefs", name: "getfiletypes"})
        },

        metadata : {
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
            getobjects : new Command({domain: "zeta", name: "getobjects"})
        },

        getParameters : function(){
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
        },

        dataType : "json"
	}
})())
})();
