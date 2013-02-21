(function(){
var root = window.zefs = window.zefs || {};
var options = root.options = root.options || {};
$.extend(options,(function(){
	return {
		/* ПАРАМЕТРЫ ДЛЯ ОБРАБОТКИ ИЗ ХЭША (зарезервированные) */
			//параметр кода формы в хэше вызова HTML, может но не обязан включать в себя .in
			form_hash_param : "form",

			//параметр года формы в хэше вызова HTML
			year_hash_param : "year",
			
			//параметр периода формы в хэше вызова HTML
			period_hash_param : "period",
			
			//параметр идентификатора объекта в хэше вызова HTML
			obj_hash_param : "obj",
	
	
		/* КОМАНДЫ И КОНСТАНТЫ РАБОТЫ С САМИМ СЕРВЕРОМ ФОРМ */
			// команда получения статуса сервера (прямой JSON)
			server_command : "zefs/server.json.qweb",
			
			// команда перезапуска сервера (true|false)
			restart_command : "zefs/restart.json.qweb",
			
			// команда проверки готовности сервера форм к обслуживанию запросов (true|false)
			ready_command : "zefs/ready.json.qweb",
				// общий таймаут ожидания 
				default_timeout : 30000,
				// текущий таймаут ожидания
				timeout : 30000,
				// период опроса ожидания
				readydelay : 1000,
				
		/* КОМАНДЫ ЗАГРУЗКИ ДОПОЛНИТЕЛЬНЫХ МЕТАДАННЫХ */
			//команда, возвращающая каталог периодов [] ( []->each ( asPeriod() ) ) упорядоченный по типам и индексам
			getperiods_command : "zefs/getperiods.json.qweb" ,
				//типы периодов
				periodtype_none:"None",
				// Месяц
				periodtype_month:"Month",
				// Основные периоды с начала года
				periodtype_ysmain:"FromYearStartMain",
				// Дополнительные периоды с начала года
				periodtype_ysext:"FromYearStartExt",
				// Плановые периоды
				periodtype_plan:"Plan",
				// Плановые периоды (месячные)
				periodtype_mplan:"MonthPlan",
				// Коррективы
				periodtype_corr:"Corrective",
				// Ожидаемые периоды
				periodtype_wait:"Awaited",
				// Период в середине года
				periodtype_inyear:"InYear",
				// Дополнительные периоды
				periodtype_ext:"Ext",
				
			//команда, возвращающая список доступных предприятий ( применить asObjectList())
			getobject_command : "zefs/getobjects.json.qweb",
				
			
		
		/* РАБОТА С СЕССИЯМИ */
			//параметр номера сессии для всех команд, связанных с сессией
			session_param : "session",
		
			/* СОЗДАНИЕ И ОБЩИЙ КОНТРОЛЬ */

					// команда вызова сессии формы (как  asSession() )
					start_command : "zefs/start.json.qweb",
										
							//параметр кода формы в команде start, может но не обязан включать в себя .in
							form_param : "form",
							
							//параметр года формы в команде start
							year_param : "year",
							
							//параметр периода формы в команде start
							period_param : "period",
							
							//параметр идентификтаора объекта формы в команде start
							obj_param : "obj",
					
					// команда получения данных о сессии [от SESSIONID] (как asSession() )
					session_command : "zefs/session.json.qweb",

					// команда получения отладочной таблицы [от SESSIONID] (прямой JSON)
					debug_command : "zefs/debuginfo.json.qweb",
					
					
			/* РАБОТА С ДАННЫМИ */
					// команда получения структуры таблицы [от SESSIONID] (как asStruct() )
					struct_command : "zefs/struct.json.qweb",
						// единица измерения по умолчанию
						default_measure : "тыс. руб.",
						// тип элемента структуры - строка
						row_struct_type : "r",
						// тип элемента структуры - колонка
						col_struct_type : "c",
					
					// команда получения батча данных [от SESSIONID ] (как asDataBatch() )
					data_command : "zefs/data.json.qweb",
						//параметр начальноно индекса для команды data
						startidx_param : "startidx",
						//стандартная величина задержки между батчами
						datadelay : 800,
						// КОНСТАНТЫ СТАТУСА ЗАКАЧКИ ДАННЫХ
							//завешено
							finished_state : "f",
							//в процессе
							inprocess_state : "w",
							//ошибка
							error_state : "e",
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
		
		
		// КОНВЕРТИРУЕТ ХЭШ ПАРАМЕТРЫ В ПАРАМЕТРЫ ВЫЗОВА ФОРМЫ ДЛЯ КОМАНДЫ START
		getParameters : function(){
			// Парсим параметры из хэша
			var p = {};
			var result = {};
            if (location.hash == "") return null;
			$.each(location.hash.substring(1).split("|"), function(i,e) {
				p[e.split("=")[0]] = e.split("=")[1];
			});
			result[this.form_hash_param] = p[this.form_hash_param];
			result[this.obj_hash_param] = p[this.obj_hash_param];
			result[this.period_hash_param] = p[this.period_hash_param];
			result[this.year_hash_param] = p[this.year_hash_param];
			return result;
		},
		
		// ФОРМИРУЕТ ПАРАМЕТРЫ AJAX ДЛЯ КОМАНД, ОПИРАЮЩИХСЯ НА СЕССИЮ
		getSessionParameters  : function (session,startIdx) {
			var params = {};
			params[options.session_param] = session.getUid();
			if (!!startIdx) {
				params[options.startidx_param] = startIdx;
			}
			return params;
		},
		
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
		
		// КОНВЕРТИРУЕТ РЕЗУЛЬТАТЫ КОМАНД canlock_command, currentlock_command В СТАНДАРТНЫЙ ОБЪЕКТ СТАТУСА БЛОКИРОВКИ
		asLockState : function( obj ) {
			$.extend(obj,{
				// признак возможности сохранения в БД - если такой возможности нет ячейки следует отрисовывать не зеленым, а ЖЕЛТЫМ ЦВЕТОМ, но оставлять доступными для правки
				getCanSave : function() {return !!this.cansave || false;},
				// текущий статус формы, один из *_lock_state
				getState : function() {return this.state || options.undef_lock_state;},
				// признак возможности заблокировать форму (видимость и/или доступность кнопки "блокировать")
				getCanLock : function(){return !!this.canblock || false;},
				// сообщение ошибки при невозможности блокировки
				getErrorMessage : function(){return this.message||"";},
				// резервный признак открытости формы (в терминах открытости периода и неустарелости формы), пока резерв
				getIsOpen : function(){return !!this.isopen || false;}
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
		// КОНВЕРТИРУЕТ РЕЗУЛЬТАТ КОМАНДЫ getperiods_command В СТАНДАРТНЫЙ ОБЪЕКТ ПЕРИОДА
		asPeriod : function(obj){
			$.extend(obj, {
				//идентификатор периода в формах
				getId : function(){return this.id;},
				// название периода
				getName : function(){return this.name;},
				// тип периода, один из periodtype_*
				getType : function(){return this.type;},
				// порядок периода (в рамках типа)
				getIdx : function(){return this.idx;}
			});		
			return obj;
		},
		
		
		// КОНВЕРТИРУЕТ РЕЗУЛЬТАТ КОМАНДЫ struct_command в унифицированную структуру строк и колонок
		asStruct : function (obj) {
			$.extend(obj,{
				// массив строк
				rows : [],
				// массив колонок
				cols : [],
				prepare : function(){
					for ( var si in this ) {
						var item = this[si];
						if (item.type==options.col_struct_type) {
							this.cols.push(options.asColumn(item));
						}
						if (item.type==options.row_struct_type) {
							this.rows.push(options.asRow(item));
						}
					}
				}
			});
			obj.prepare();
			return obj;
		},
		
		// КОНВЕРТИРУЕТ ЭЛЕМЕНТ СТРУКТУРЫ "строка" в стандартный объект строки
		asRow : function(obj){
			$.extend(obj,{
				// код строки
				getCode : function(){return this.code;},
				// ID строки для ключа ячейки
				getIdx : function(){return this.idx;},
				// признак первичной строки
				getIsPrimary : function(){return !!this.isprimary;},
				// название строки
				getName: function(){return this.name;},
				// признак заголовочной строки
				getIsTitle : function(){return !!this.iscaption;},
				// уровень вложенности строки
				getLevel : function(){return (this.level) || 0 ;}   ,
				// номер строки (для колонки №)
                getNumber : function(){return this.number || "" ;},  
				// единица измерения строки
                getMeasure : function(){return this.measure || options.default_measure;},
				// признак контрольной точки
                getIsControlPoint : function(){return !!this.controlpoint || false;}
			});
			return obj;
		},	
		
		// КОНВЕРТИРУЕТ ЭЛЕМЕНТ СТРУКТУРЫ "колонка" в стандартный объект колонки
		asColumn : function(obj){
			$.extend(obj,{
				// код колонки
				getCode : function(){return this.code;},
				// ID строки для ключа ячейки
				getIdx : function(){return this.idx;},
				// признак первичной колонки
				getIsPrimary : function(){return !!this.isprimary;},
				// название колонки
				getName: function(){return this.name;},
				// год колонки
				getYear: function(){return this.year;},
				// период колонки
				getPeriod: function(){return this.period;},
				// признак контрольной точки
                getIsControlPoint : function(){return !!this.controlpoint || false;}
			});
			return obj;
		},
		
		// КОНВЕРТИРУЕТ РЕЗУЛЬТАТ команды data_command в стандартный объект БАТЧА
		asDataBatch : function(obj){
			$.extend(obj,{
				// первый индекс ячейки батча
				getFirstIdx : function(){return this.si;},
				// последний индекс ячейки батча
				getLastIdx : function(){return this.ei;},
				// статус закачки finished_state,error_state, inprocess_state
				getState : function(){return this.state;},
				// признак наличия ошибки в процессе загрузки
				getIsError : function(){return this.getState()==options.error_state;},
				// текст ошибки в процессе загрузки
				getError : function(){return this.e;},
				// признак завершения процесса загрузки ( по окончанию данных или ошибке)
				getIsLast : function(){return this.getState()!=options.inprocess_state;},
				// признак наличия реальных данных в батче
                getWasData : function(){
                  var data  = this.getData();
                    if(!data)return false;
                    if(0==data.length)return false;
                    return true;
                },
				// индекс для стартового индекса следующей прокачки
                getNextIdx : function(){
                    if(this.getWasData()){
                        return this.getLastIdx()+1;
                    }
                    return this.getFirstIdx();
                },
				// массив ячеек в батче (собственно данные)
				getData : function(){ return this.data || {};} ,
				// признак применения батча к таблице
                wasFilled : false
				});
			var data = obj.getData();
			for (var i in data) {
				// приводим элементы данных к стандартному виду
				options.asDataItem(data[i]);
			}
			return obj;
		},
		// КОНВЕРТИРУЕТ элемент РЕЗУЛЬТАТА команды data_command в стандартный источник для ячейки
		asDataItem : function(obj){
			obj.v = obj.v || null,
			$.extend(obj,{
				// признак того, что ячейка является прямым отображением ячейки в БД
				getIsCell : function(){return this.getCellId()!=0;},
				// идентификатор ячейки в БД (или 0)
				getCellId :  function(){return this.c || 0;},
				// идентификатор ячейки в таблице В ВИДЕ "ROWIDX:COLIDX"
				getId : function() {return this.i;},
				// признак наличия данных
				hasValue : function(){return null!=this.v;},
				// собственно значение
				getValue : function(){return this.v;}
			});
			// нормализуем ID ячейки в индексы строки и колонки
			var rc = obj.getId().split(":");
			// индекс строки
			obj.row = rc[0];
			// индекс колонки
			obj.col = rc[1];
			return obj;
		},
		//возвращает нормализованный список дивизионов и объектов, вызывая на них соответственно asDiv, asObject
		asObjectList : function (obj ){
			// в ходе выполнения формирует двустороннюю
			// связь между дивизионом и объектами
			// то есть можно делать так asObjectList(obj).getDivs().each(function(d){ d.getObjects().each(...) }
			// равно можно от объекта получить дивизион asObject(obj).getDiv();
			// при этом оба списка можно смотреть и просто как массивы
			$.each(obj.divs, function(k,v){			
				this.asDiv(v);
				obj.divs[v.getCode()] = v;
				v.objects =[];
			});
			$.each(obj.objs, function(k,v){
				this.asObject(v);				
				v.resolveddiv = obj.divs[v.div];
				v.resolveddiv.objects.push(v);
			});
			$.extend(obj,{
				getDivs : function(){return this.divs;},
				getObjects : function(){return this.objs;},
			});
			return obj;
		},
		// обертка для записи о дивизионе
		asDiv : function(obj){
			$.extend(obj,{
				// уникальный код дивизиона
				getCode : function(){return this.code;},
				// имя дивизиона
				getName : function(){return this.name;},
				// индекс порядка дивизиона
				getIdx : function(){return this.idx;},
				// возвращает массив объектов дивизиона
				getObjects : function(){return this.objects;}
			});
			return obj;
		},
		// обертка для объекта 
		asObject : function(obj){
			$.extend(obj,{
				// идентификатор объекта
				getId : function(){return this.id;},
				// имя объекта
				getName : function(){return this.name;},
				// индекс объекта
				getIdx : function(){return this.idx;},
				// ссылка на дивизион
				getDiv : function(){return this.resolveddiv;},
				// исходный код дивизиона
				getDivCode : function(){return this.div;}
			});
			return obj;
		},
	}
	
})())
})();
