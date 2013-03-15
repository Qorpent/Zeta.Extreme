(function(){
var root = window.zefs = window.zefs || {};
var specification = root.api = root.api || {};
var options = root.options = root.api;
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
            saveready : new Command({domain: "zefs", name: "saveready"}),
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
        // команда инициализации сессии сохранения [от SESSIONID ] (true|false )
        saveready_command : "zefs/saveready.json.qweb",
		// команда инициализации сохранения [от SESSIONID ] (true|false )
		save_command : "zefs/save.json.qweb",
			savedata_param : "data",


		// команда проверки состояния сохранения [от SESSIONID ] (как asSaveState() ) должна быть еще и в DEBUG-меню
		savestate_command : "zefs/savestate.json.qweb",
			// 	Изначальное состояние
			savestage_none : "None",
			// 	Загрузка задачи сохранения
			savestage_load : "Load",
			// 	Проверка возможности сохранения по аспектам безопасности
			savestage_auth : "Authorize",
			// 	Подготовка входных данных - переработка справочников
			savestage_prepare : "Prepare",
			// 	Проверка целостности запрошенного сохранения
			savestage_validate : "Validate",
			// 	Проверка доступности соединений, хранимых процедур и проч
			savestage_test : "Test",
			// 	Собственно сохранение ячеек
			savestage_save : "Save",
			// 	Выполнение специальных процедур после выполнения сохранения, бизнес-тригеры
			savestage_aftersave : "AfterSave",
			// 	Успешное завершение
			savestage_finished : "Finished",
					
							
						
			/* УПРАВЛЕНИЕ БЛОКИРОВКАМИ И СТАТУСОМ ПО СОХРАНЕНИЮ*/
					// КОНСТАНТЫ ИМЕН СТАТУСОВ */
						// форма открыта
						open_lock_state : "0ISOPEN",
						// форма закрыта
						block_lock_state : "0ISBLOCK",
						// форма утверждена
						check_lock_state : "0ISCHECKED",
						// не определено (при сбоях)
						undef_lock_state : "UNDEFINED",
					// команда получения текущего статуса блокировки [от SESSIONID] (как asLockState() )
					currentlock_command : "zefs/currentlockstate.json.qweb",
					
					// команда получения статуса возможности блокировки [от SESSIONID] (как asLockState() )
					canlock_command : "zefs/canlockstate.json.qweb",
					// команда получения истории блокировок [от SESSIONID]
					locklist_command : "zefs/locklist.json.qweb",
					// команда блокировки формы [от SESSIONID]
					lockform_command : "zefs/lockform.json.qweb",


		    /* РАБОТА С ПРИСОЕДИНЕННЫМИ ФАЙЛАМИ */
					// команда получения списка прикрепленных к форме файлов [от SESSIONID]
					attachlist_command : "zefs/attachlist.json.qweb",
					// команда прекрепления или обновления файла к форме
					attachfile_command : "zefs/attachfile.json.qweb",
					// команда скрытия/удаления файла [от SESSIONID и UID]
                    deletefile_command : "zefs/deletefile.json.qweb",
					// команда загрузки файла [от SESSIONID и UID]
					downloadfile_command : "zefs/downloadfile.filedesc.qweb",
					// команда получения возможных типов файлов [от SESSIONID]
					getfiletypes_command : "zefs/getfiletypes.json.qweb",
		
		//КОНВЕРТИРУЕТ РЕЗУЛЬТАТЫ КОМАНД start_command, session_command в СТАНДАРТНЫЙ ОБЪЕКТ СЕССИИ
		asSession : function ( obj ) {
			$.extend(obj,{
				// SESSIONID
				getUid :	function(){return this.Uid;},
				// признак успешной инициализации сессии
				getIsStarted :	function(){return this.IsStarted;},
				// признак завершения процесса загрузки данных
				getIsFinished :	function(){return this.IsFinished;},
				// время создания сессии
				getCreateTime : function(){return this.Created;},
				// год шаблона сессии (может не совпадать с входным параметром)
				getYear : function(){return this.Year;},
				// период шаблона сессии (может не совпадать с входным параметром)
				getPeriod : function(){return this.Period;},
				// пользователь- владелец сессии
				getUsr : function(){return this.Usr;},
				// информация об объекте сессии
				getObjInfo : function(){return this.ObjInfo;},
				// дополнительная информация о шаблоне сессии
				getFormInfo : function(){return this.FormInfo;},
				// время, затраченное на подготовку
				getTimeToPrepare : function(){return this.TimeToPrepare;},
				// время, затраченное на структуру
				getTimeToStructure : function(){return this.TimeToStructure;},
				// время, затраченное на загрузку первичных данных
				getTimeToPrimary : function(){return this.TimeToPrimary;},
				// время, затраченное на загрузку всех данных
				getTimeToGetData : function(){return this.TimeToGetData;},
				// количество обслуженных запросов
				getQueryCount: function(){return this.QueriesCount;},
				// количество обслуженных первичных запросов
				getPrimaryCount: function(){return this.PrimaryCount},
				// количество полученных ячеек
				getDataCount: function(){return this.DataCount;},
				// признак необходимости вывода единицы измерения
                getNeedMeasure:function(){return !!this.NeedMeasure;},
				// признак завершения отрисовки сессии
                wasRendered : false,
				// контейнер для структуры
                structure : {},
				// контейнер для ячеек с данными
                data : []
			});
			$.extend(obj.getObjInfo(),{
				// идентификатор объекта
				getId : function(){return this.Id;},
				// код объекта
				getCode : function(){return this.Code;},
				// название объекта
				getName : function(){return this.Name;}
			});
			$.extend(obj.getFormInfo(),{
				// код/ид формы
				getId : function(){return this.Code;},
				// код/ид формы
				getCode : function(){return this.Code;},
				// имя формы формы
				getName : function(){return this.Name;}

			});
			return obj;
		},


		// КОНВЕРТИРУЕТ РЕЗУЛЬТАТ КОМАНДЫ savestate_command В СТАНДАРТНЫЙ ОБЪЕКТ СТАТУСА СОХРАНЕНИЯ
		asSaveState : function(obj){
			$.extend(obj,{
				// возвращает один из статусов стадии сохранения savestage_*
				getStage :  function(){return this.stage;},
				// текст ошибки
				getError :  function(){return this.error;},
				// признак наличия ошибки
				getIsError : function(){return !!this.getError();},
                // признак завершения комманды сохранения
                getIsFinished : function() { return ((this.getStage() == options.savestage_finished) || this.getIsError() ); },
                // признак завершения сохранения ячеек
                getCellsSaved : function() { return this.getStage() == options.savestage_finished || this.getStage()==options.savestage_aftersave; }
			});
			return obj;
		},

        dataType : "json"
	}
})())
})();
