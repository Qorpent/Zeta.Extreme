#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/FormSession.cs
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Qorpent;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Log;
using Qorpent.Mvc;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.DbfsAttachmentSource;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.StateManagement;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.SqlSupport;
using FormState = Zeta.Extreme.Model.FormState;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Сессия работы с формой
	/// </summary>
	[Serialize]
	public class FormSession :
		ServiceBase,
		IFormDataSynchronize,
		IFormSessionControlPointSource,
        IFormSession {
		/// <summary>
		/// Empty ctor for IoC match
		/// </summary>
		public FormSession() {
			Parameters = new Dictionary<string, object>();
			_controlpoints = new List<ControlPointResult>();
			Application =Qorpent.Applications.Application.Current;
		}

		/// <summary>
		/// Расширенные параметры сессии
		/// </summary>
		public IDictionary<string,object> Parameters { get;  private  set; }

		/// <summary>
		/// 	Создает сессию формы
		/// </summary>
		/// <param name="form"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="obj"> </param>
		/// <param name="subobj"></param>
		public FormSession(IInputTemplate form, int year, int period, IZetaMainObject obj,IZetaMainObject subobj = null) {
			Application = Qorpent.Applications.Application.Current;
			_controlpoints = new List<ControlPointResult>();
			Parameters = new Dictionary<string, object>();
			Uid = Guid.NewGuid().ToString();
			Object = obj;
			if (null != subobj) {
				SubObject = subobj;
			}
			
			Created = DateTime.Now;
			Template = form.PrepareForPeriod(year, period, new DateTime(1900, 1, 1), Object);
			Template.AttachedSession = this;
			Year = Template.Year;
			Period = Template.Period;
			Created = DateTime.Now;
			
			IsStarted = false;
			var reader = new NativeZetaReader();
			var currency = Object.Currency;
			if (string.IsNullOrWhiteSpace(currency)) {
				currency = "RUB";
			}
			decimal rate = 1;
			if (currency != "RUB") {
				rate = reader.GetCurrencyRate(Year, Period, currency);
			}
			if (null!=form.Thema && !string.IsNullOrWhiteSpace(form.Thema.GetParameter("splittoobj", "")))
			{
				PrepareSubObjects();
			}

            if (Qorpent.Applications.Application.Current.Roles.IsInRole(Usr, "BUDGET"))
            {
                if (null != form.Thema && !string.IsNullOrWhiteSpace(form.Thema.GetParameter("forgroup", "")) && form.Thema.GetParameter("showprdgroup", "").ToBool())
                {
                    OverrideObjects = PrepareOverrideObjects();
                }
            }
			ObjInfo = new
				{
					Object.Id, 
					Object.Code, 
					Object.Name,
					Currency=currency,
					CurrencyRate = rate,
				};

			if (null != Template.Thema) {
				var holdlogin = Template.Thema.GetParameter("hold.responsibility");
				FormInfo = new
					{
						Template.Code,
						Template.Name,
						ObjectResponsibility = reader.GetThemaResponsiveLogin(Template.Thema.Code, Object.Id),
						HoldResponsibility = holdlogin,
						Status = Template.Thema.GetParameter("status", ""),
						FirstYear = Template.Thema.GetParameter("firstyear", ""),
						RolePrefix = Template.Thema.GetParameter("roleprefix", ""),
						PeriodState = ResolveService<IPeriodStateManager>().Get(Template.Year, Template.Period, Template.Thema.GetParameter("deadlinetype","")),
					};
			}


			if (RequireSubObjects) {
				if(0==splitobjects.Length)throw new Exception("Данная форма не может быть открыта для данного предприятия, так как нет соответствующих подобъектов");
				SubObjInfo = new
				{
					SubObject.Id,
					SubObject.Code,
					SubObject.Name,
					SubCurrency = currency,
					SubCurrencyRate = rate,
				};
			}

			NeedMeasure = Template.ShowMeasureColumn;

			

			Activations = 1;
			

		}
        /// <summary>
        /// Перекрывает "свои предприятия"
        /// </summary>
        [SerializeNotNullOnly]
	    public object OverrideObjects { get; set; }

	    private object PrepareOverrideObjects() {
	        var group = Template.Thema.GetParameter("forgroup");
	        var objects = new NativeZetaReader().ReadObjects(" groupcache like '%/" + group + "/%'");
            return objects.Select(_=>new{_.Id,Name=string.IsNullOrWhiteSpace(_.ShortName)?_.Name:_.ShortName}).ToArray();
	    }

	    private void PrepareSubObjects() {
			var zr = new NativeZetaReader();
			this.RequireSubObjects = true;
			var splittoobj = Template.Thema.GetParameter("splittoobj", "");
			var inconditions = string.Join(",", splittoobj.SmartSplit().Select(_ => "'" + _ + "'"));
			splitobjects = zr.ReadObjectsWithTypes(string.Format("o.Id != {0} and Path like  '%/{0}/%' and (t.Code in ({1}) or c.Code in ({1}) ) ",  Object.Id,inconditions)).ToArray();
			SplitObjInfo = splitobjects.Select(_ =>
#pragma warning disable 612,618
			                                        new
				                                        {
					                                        _.Id,
					                                        _.Name,
					                                        TypeCode = _.ObjType.Code,
					                                        TypeName = _.ObjType.Name,
					                                        ClassCode = _.ObjType.Class.Code,
					                                        ClassName = _.ObjType.Class.Name
				                                        }
#pragma warning restore 612,618
				).ToArray();
			if (null == SubObject || splitobjects.All(_=>_.Id!=SubObject.Id)) {
				SubObject = splitobjects.FirstOrDefault();
			}
		}
		/// <summary>
		/// Признак требования обязательного наличия подобъектов
		/// </summary>
		[SerializeNotNullOnly]
		public bool RequireSubObjects { get; set; }

		/// <summary>
		/// Внешняя информация по объектам сплита
		/// </summary>
		public object[] SplitObjInfo { get; set; }


		/// <summary>
		/// Журнал
		/// </summary>
		public IUserLog Logger {
			get {
				if (null == _logger) {
					_logger = Application.LogManager.GetLog("form.log", this);		
				}
				return _logger;
			}
			set { _logger = value; }
		}


		/// <summary>
		/// 	Количество активаций (повторного использования сессий)
		/// </summary>
		public int Activations { get; set; }

		/// <summary>
		/// 	Признак требования показывать колонку с единицей измерения
		/// </summary>
		public bool NeedMeasure { get; set; }


		/// <summary>
		/// Получить перечень сообщений в чате
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public FormChatItem[] GetChatList() {
			var provider = Container.Get<IFormChatProvider>();
			if (null == provider) {
				throw new Exception("no form chat configured");
			}
			return provider.GetSessionItems(this).ToArray();
		}

		/// <summary>
		/// Добавить сообщение в чат
		/// </summary>
		/// <returns></returns>
		public FormChatItem AddChatMessage(string message, string type) {
			var provider = Container.Get<IFormChatProvider>();
			if (null == provider)
			{
				throw new Exception("no form chat configured");
			}
			return provider.AddMessage(this, message,type);
		}

		/// <summary>
		/// 	Признак, что сессия стартовала
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// 	Признак завершения обработки сессии
		/// </summary>
		public bool IsFinished {
			get { return (null!=PrepareStructureTask && null!=PrepareDataTask) && (PrepareDataTask.IsCompleted && PrepareStructureTask.IsCompleted); }
		}

		/// <summary>
		/// 	Ошибка сессии
		/// </summary>
		public Exception Error { get; set; }

		/// <summary>
		/// 	Сообщение об ошибке для сериализации
		/// </summary>
		[SerializeNotNullOnly] public string ErrorMessage {
			get {
				if (null != Error) {
					return Error.ToString();
				}
				return null;
			}
		}

		/// <summary>
		/// 	Время создания
		/// </summary>
		public DateTime Created { get; private set; }

		/// <summary>
		/// 	Сессия работы с данными
		/// </summary>
		[IgnoreSerialize] public ISession DataSession { get; private set; }
		

		/// <summary>
		/// 	Задача формирования структуры
		/// </summary>
		[IgnoreSerialize] public TaskWrapper PrepareStructureTask { get; private set; }

		/// <summary>
		/// 	Задача формирования данных
		/// </summary>
		[IgnoreSerialize] public TaskWrapper PrepareDataTask { get; private set; }

		/// <summary>
		/// 	Хранит структуру формы
		/// </summary>
		[IgnoreSerialize] public StructureItem[] Structure { get; private set; }

		/// <summary>
		/// 	Признак процесса формирования структуры
		/// </summary>
		protected bool StructureInProcess { get; set; }

		/// <summary>
		/// 	Информация об объекте
		/// </summary>
		[Serialize] public object ObjInfo { get; private set; }

		/// <summary>
		/// 	Информация о подобъекте
		/// </summary>
		[Serialize] public object SubObjInfo { get; private set; }


		/// <summary>
		/// 	Информация о форме ввода
		/// </summary>
		[Serialize] public object FormInfo { get; private set; }

		/// <summary>
		/// 	Время подготовки
		/// </summary>
		[Serialize] public TimeSpan TimeToPrepare { get; set; }

		/// <summary>
		/// 	Журнал выполненных SQL
		/// </summary>
		[IgnoreSerialize] public string[] SqlLog { get; set; }

		/// <summary>
		/// 	Время генерации структуры
		/// </summary>
		[Serialize] public TimeSpan TimeToStructure { get; set; }

		/// <summary>
		/// 	Время генерации первичных ячеек
		/// </summary>
		[Serialize] public TimeSpan TimeToPrimary { get; set; }

		/// <summary>
		/// 	Время генерации первичных ячеек
		/// </summary>
		[Serialize] public TimeSpan LastDataTime { get; set; }

		/// <summary>
		/// 	Статистика сессии данных
		/// </summary>
		[IgnoreSerialize] public SessionStatistics DataStatistics { get; set; }

		/// <summary>
		/// 	Общее количество запросов в обработке
		/// </summary>
		public int QueriesCount { get; set; }

		/// <summary>
		/// 	Общее количество ячеек
		/// </summary>
		public int DataCount { get; set; }

		/// <summary>
		/// 	Количество первичных ячеек
		/// </summary>
		public int PrimaryCount { get; set; }

		/// <summary>
		/// 	Количество перезапрошенных сессий
		/// </summary>
		public int DataCollectionRequests { get; set; }

		/// <summary>
		/// 	Общеее время получения данных
		/// </summary>
		public TimeSpan OverallDataTime { get; set; }

		/// <summary>
		/// 	Описатель реального колсета
		/// </summary>
		[IgnoreSerialize] public ColumnDesc[] Colset { get; set; }

		/// <summary>
		/// 	Обратная ссылка на сервер форм
		/// </summary>
		public FormServer FormServer { get; set; }

		/// <summary>
		/// 	Режим подготовки к сохранению
		/// </summary>
		public bool InitSaveMode { get; set; }

		/// <summary>
		/// 	Метод для ожидания окончания данных
		/// </summary>
		public void WaitData() {
			if(null!=PrepareDataTask)PrepareDataTask.Wait();
		}

		/// <summary>
		/// 	Идентификатор сессии
		/// </summary>
		public string Uid { get; private set; }

		/// <summary>
		/// 	Год
		/// </summary>
		public int Year { get; private set; }

		/// <summary>
		/// 	Период
		/// </summary>
		public int Period { get;  set; }

		/// <summary>
		/// 	Объект
		/// </summary>
		[IgnoreSerialize] public IZetaMainObject Object { get; private set; }


		/// <summary>
		/// Подъобъект для составных форм
		/// </summary>
		[IgnoreSerialize] public IZetaMainObject SubObject { get; private set; }

		/// <summary>
		/// 	Шаблон
		/// </summary>
		[IgnoreSerialize] public IInputTemplate Template { get; private set; }

		/// <summary>
		/// 	Пользователь
		/// </summary>
		public string Usr {
			get {
				if (string.IsNullOrWhiteSpace(_usr)) {
					_usr = (Application ??Qorpent.Applications.Application.Current).Principal.CurrentUser.Identity.Name;
				}
				return _usr;
			}
			set { _usr = value; }
		}

		/// <summary>
		/// 	Хранит уже подготовленные данные
		/// </summary>
		[IgnoreSerialize] public List<OutCell> Data {
			get { return _data ?? (_data = new List<OutCell>()); }
		}

		/// <summary>
		/// 	Коллекция контрольных точек
		/// </summary>
		[IgnoreSerialize] public ControlPointResult[] ControlPoints {
			get {
				WaitData();
				return _controlpoints.ToArray();
			}
		}

		/// <summary>
		/// 	Возвращает следующий пакет данных
		/// </summary>
		/// <param name="startidx"> </param>
		/// <returns> </returns>
		public DataChunk GetNextChunk(int startidx) {
			lock (Data) {
				var state = IsFinished ? "f" : "w";
				if (!string.IsNullOrWhiteSpace(ErrorMessage)) {
					state = "e";
				}
				var max = Data.Count - 1;
				if (Data.Count <= startidx) {
					return new DataChunk {state = state, ei = max};
				}

				var cnt = max - startidx + 1;

				return
					new DataChunk
						{
							si = startidx,
							ei = max,
							state = state,
							e = ErrorMessage,
							data = Data.Skip(startidx).Take(cnt).Where(_=>!_.IsZero || _.c!=0 || _.iserror).ToArray()
						};
			}
		}

		/// <summary>
		/// 	Синхронизированный метод доступа к структуре
		/// </summary>
		/// <returns> </returns>
		public StructureItem[] GetStructure() {
			if (null != Structure && !StructureInProcess) {
				return Structure;
			}
			if (null != PrepareStructureTask) {
				PrepareStructureTask.Wait();
			}
			return Structure;
		}

		/// <summary>
		/// 	Стартует сессию
		/// </summary>
		public void Start() {
			lock (this) {
                FormSessionsState.CurrentSessionsIncrease();
                FormServersState.TotalSessionsHandledIncrease();

			    var logWriter = Application.LogManager.GetLog("form.server",this);
                logWriter.Trace("Session started");

				if (IsStarted) {
					return;
				}
				Activations++;
				var sw = Stopwatch.StartNew();
				PrepareMetaSets();
				sw.Stop();
				TimeToPrepare = sw.Elapsed;
				PrepareStructureTask = new TaskWrapper(
					Task.Run(() => { RetrieveStructure(); })
					);
				StartCollectData();

				IsStarted = true;
			}
		}

		/// <summary>
		/// 	Метод прямого вызова повторного сбора данных
		/// </summary>
		protected internal void StartCollectData() {
            
			lock (this) {
				if (null != PrepareDataTask) {
					return;
				}
				_processed.Clear();
				Data.Clear();
				DataCollectionRequests++;
				EnsureDataSession();
				PrepareDataTask = new TaskWrapper(
					Task.Run(() =>
						{
							try {
								RetrieveData();
							}
							catch (Exception ex) {
								Error = ex;
							}
						})
					) {SelfWait = 30000};
				while (PrepareDataTask.Status == TaskStatus.Created) {
					Thread.Sleep(10);
				}
			}
		}

		/// <summary>
		/// Признак предприятия, пригодного для правки
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public bool ObjectIsEditable {
			get { return WorkingObject.Start.Year <= Year && WorkingObject.Finish.Year > Year && !WorkingObject.IsFormula; }
		}
		/// <summary>
		/// Служба управления статусами
		/// </summary>
		[Inject]
		public IFormStateManager FormStateManager { get; set; }
		/// <summary>
		/// Режим для смены статусов
		/// </summary>
		public bool InitStateMode { get; set; }

		private void 
		RetrieveStructure() {
            FormSessionsState.CurrentFormRenderingOperationsIncrease();
			StructureInProcess = true;
			var sw = Stopwatch.StartNew();
			Structure =
				(from ri in rows
				 let r = ri.Native
				 select new StructureItem
					 {
						 type = "r",
						 code = r.Code,
						 name = r.Name,
						 idx = ri.Idx,
						 iscaption = r.IsMarkSeted("0CAPTION"),
						 isprimary = ri.GetIsPrimary() && ObjectIsEditable,
						 level = ri.Level,
						 number = r.OuterCode,
						 measure = NeedMeasure ? r.ResolveMeasure() : "",
						 controlpoint = r.IsMarkSeted("CONTROLPOINT"),
						 exref = null!=r.ExRefTo,
						 format = r.ResolveTag("numberformat"),
						 activecols = r.ResolveTag("activecol").SmartSplit().ToArray(),
						 comment = r.Comment,
						 isformula = r.IsFormula || (null!=r.RefTo) || (null!=r.ExRefTo),
						 
					 })
					.Union(
						(from ci in cols
						where CheckIsOuterVisible(ci)
						 let c = ci._
						 select new StructureItem
							 {
								 type = "c",
								 code = c.Code,
								 name = c.Title,
								 idx = ci.i,
								 isprimary = c.Editable && !c.IsFormula && !c.IsAuto && ObjectIsEditable,
								 year = c.Year,
								 period = c.Period,
								 controlpoint = c.ControlPoint,
								 exref = null!=c.Target && c.Target.IsMarkSeted("DOEXREF"),
								 format = c.NumberFormat,
								 rules = c.RowCheckConditions,
								 validate = c.Validation,
								 comment =  c.Comment
							 })
					).ToArray();
			sw.Stop();
			TimeToStructure = sw.Elapsed;
			StructureInProcess = false;
            FormSessionsState.CurrentFormRenderingOperationsDecrease();
		}

		private bool CheckIsOuterVisible(IdxCol ci) {
			if (ci._.Title == "hide") return false;
			return true;

		}


		private void RetrieveData() {
		    var startTime = DateTime.Now;

            FormSessionsState.CurrentFormLoadingOperationsIncrease();
			_controlpoints.Clear();
			Data.Clear();
			var customDataRetriever = 
				Container.Get<IFormDataRetriever>(Template.Thema.Code + ".data.retriever")
				??
				Container.Get<IFormDataRetriever>(Template.Thema.GetParameter("extension", "") + ".data.retriever");
			if (null != customDataRetriever) {
				customDataRetriever.RetrieveData(this);
			}
			else {
				DefaultRetrieveData();
			}
			Logger.Info("data loaded");
            FormSessionsState.CurrentFormLoadingOperationsDecrease();
            FormServersState.TotalTimeToLoadDataIncrease(DateTime.Now - startTime);
		}

		private void DefaultRetrieveData() {
			var sw = Stopwatch.StartNew();
			IDictionary<string, IQuery> queries = new Dictionary<string, IQuery>();
			LoadEditablePrimaryData(queries);
			if (!InitSaveMode) {
				LoadNonEditablePrimaryData(queries);
			}
			TimeToPrimary = sw.Elapsed;
			PrimaryCount = Data.Count;

			if (!InitSaveMode) {
				LoadNoPrimary(queries);

				QueriesCount = queries.Count;
				DataStatistics = DataSession.GetStatistics();
				SqlLog = DataSession.GetPrimarySource().QueryLog.ToArray();
				DataSession = null;
				DataCount = Data.Count;
				LastDataTime = sw.Elapsed;
				OverallDataTime = OverallDataTime + sw.Elapsed;
				foreach (var controlPointResult in _controlpoints) {
					controlPointResult.Value = controlPointResult.Query.Result.NumericResult;
					//	controlPointResult.Query = null;
				}
			}
			InitSaveMode = false;
		}

		private void LoadNoPrimary(IDictionary<string, IQuery> queries) {
			foreach (var c in cols) {
				foreach (var r in rows) {
					var key = r.Idx + ":" + c.i;
					if (queries.ContainsKey(key)) {
						continue;
					}
					var ch = ExtremeFactory.CreateColumnHandler();
					ch.Native = c._.Target;
					if (null == ch.Native) {
						ch.Code = c._.Code;
						ch.IsFormula = c._.IsFormula;
						ch.Formula = c._.Formula;
						ch.FormulaType = c._.FormulaType;
					}
					if (c._.ControlPoint) {
						ch.LocalProperties["controlpoint"] = true;
					}
					var q = ExtremeFactory.CreateQuery( new QuerySetupInfo
						{
							Row = {Native = r.Native},
							Col = ch,
							Obj = { Native = r.AttachedObject ?? WorkingObject, DetailMode = r.SumObj ? DetailMode.SumObject : DetailMode.None },
							Time = {Year = c._.Year, Period = c._.Period},
							Reference = { Contragents = r.AltObjFilter, Types = r.ContoFilter }
						});
					q = DataSession.Register(q, key);

					if (null != q) {
						if (c._.ControlPoint && r.Native.IsMarkSeted("CONTROLPOINT")) {
							if (!(r.Native.Tag??"").Contains("/noblocker:1/")) {
								_controlpoints.Add(new ControlPointResult {Col = c._, Row = r.Native, Query = q});
							}
						}
						queries[key] = q;
					}
				}
				DataSession.Execute(500);
				ProcessValues(queries, false);
			}
		}

		private void LoadEditablePrimaryData(IDictionary<string, IQuery> queries) {
			BuildEditablePrimarySet(queries);
			DataSession.Execute(500);
			ProcessValues(queries, true);
		}

		private void LoadNonEditablePrimaryData(IDictionary<string, IQuery> queries) {
			BuildNonEditablePrimarySet(queries);
			DataSession.Execute(500);
			ProcessValues(queries, false);
		}

		private void BuildEditablePrimarySet(IDictionary<string, IQuery> queries) {
			foreach (var primaryrow in primaryrows) {
				foreach (var primarycol in primarycols) {
					var q = ExtremeFactory.CreateQuery(  new QuerySetupInfo
						{
							Row = {Native = primaryrow.Native},
							Col = {Native = primarycol._.Target},
							Obj = { Native = primaryrow.AttachedObject ?? WorkingObject, DetailMode = primaryrow.SumObj?DetailMode.SumObject : DetailMode.None},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period},
							Reference = {Contragents = primaryrow.AltObjFilter, Types = primaryrow.ContoFilter}
							
						});
					var key = primaryrow.Idx + ":" + primarycol.i;
					queries[key] = DataSession.Register(q, key);
				}
			}
		}

		private void BuildNonEditablePrimarySet(IDictionary<string, IQuery> queries) {
			foreach (var primaryrow in primaryrows) {
				foreach (var primarycol in neditprimarycols) {
					var q =  ExtremeFactory.CreateQuery( new QuerySetupInfo
						{
							Row = {Native = primaryrow.Native},
							Col = {Native = primarycol._.Target},
							Obj = { Native = primaryrow.AttachedObject ?? WorkingObject, DetailMode = primaryrow.SumObj?DetailMode.SumObject : DetailMode.None},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period},
							Reference = { Contragents = primaryrow.AltObjFilter, Types = primaryrow.ContoFilter }
						});
					var key = primaryrow.Idx + ":" + primarycol.i;
					queries[key] = DataSession.Register(q, key);
				}
			}
		}

		private void ProcessValues(IDictionary<string, IQuery> queries, bool canbefilled) {
			
			foreach (var q_ in queries.Where(_ => null != _.Value)) {
				var reallycanbefilled = canbefilled;
				if (_processed.ContainsKey(q_.Key)) {
					continue;
				}
				if (reallycanbefilled) {
					reallycanbefilled = reallycanbefilled && ObjectIsEditable;
				}
				if (reallycanbefilled) {
					var r = q_.Value.Row.Native;
					var c = q_.Value.Col;
					var activecols = r.ResolveTag("activecol").SmartSplit().ToArray();
					if (0 != activecols.Length && -1 == Array.IndexOf(activecols, c.Code)) {
						reallycanbefilled = false;
					}

				}
				_processed[q_.Key] = q_.Value;
				var val = "";
				var cellid = 0;
				if (null != q_.Value && null != q_.Value.Result) {
					val = q_.Value.Result.NumericResult.ToString("0.#####", CultureInfo.InvariantCulture);
					
					cellid = q_.Value.Result.CellId;
				}
				var realkey = "";
				if (reallycanbefilled)
				{
					realkey = q_.Value.Row.Code + "_" + q_.Value.Col.Code + "_" + q_.Value.Time.Year + "_" + q_.Value.Time.Period;
					if (q_.Value.Obj.Native != WorkingObject) {
						realkey += q_.Value.Obj.Type + "_" + q_.Value.Obj.Id;
					}
				}
				var cell = new OutCell { i = q_.Key, c = cellid, v = val, canbefilled = reallycanbefilled, query = q_.Value, ri = realkey };
				if (q_.Value.Result.Error != null) {
					cell.iserror = true;
					cell.error = q_.Value.Result.Error;
				}

				lock (Data) {
					Data.Add(cell);
				}
			}
		}

		
		/// <summary>
		/// Текущий рабочий объект
		/// </summary>
		public IZetaMainObject WorkingObject {
			get { return SubObject ?? Object; }
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		public object CollectDebugInfo() {
			return new {stats=DataStatistics, sql = SqlLog, colset = Colset};
		}

		private void PrepareMetaSets() {
			ExecutePreData();
			PrepareRows();
			InitializeColset();
			if (InitStateMode)
			{
				rows = rows.Where(_ => _.Native != null && _.Native.IsMarkSeted("CONTROLPOINT")).ToArray();
				cols = cols.Where(_ => _._.ControlPoint).ToArray();
			}
			primarycols = cols.Where(_ => _._.Editable && !_._.IsFormula).ToArray();
			neditprimarycols = cols.Where(_ => !_._.Editable && !_._.IsFormula).ToArray();
			primaryrows = rows.Where(_ => _.GetIsPrimary()).ToArray();
			
		}

		private void ExecutePreData() {
			var customPreData =
				Container.Get<ISessionPreDataExtension>(Template.Thema.Code + ".pre.data")
				??
				Container.Get<ISessionPreDataExtension>(Template.Thema.GetParameter("extension", "") + ".pre.data");
			if (null != customPreData) {
				customPreData.Prepare(this);
			}
			var commonpreparators = Container.All<ISessionCommonPreDataExtension>();
			foreach (var sessionCommonPreDataExtension in commonpreparators) {
				sessionCommonPreDataExtension.Prepare(this);
			}

		}
		/// <summary>
		/// Позволяет установить фильтр для идентификаторов входны строк для структуры по умолчанию
		/// </summary>
		public string[] RowCodeFilter { get; set; }

		private void InitializeColset() {
			EnsureDataSession();
			PrepareVisibleColumns();
			SetupNativeColumns();
		}

		private void SetupNativeColumns() {
			foreach (var columnDesc in cols) {
				PrepareNativeColumnFromUsualCode(columnDesc);
				CheckCustomCodedColumn(columnDesc);
			}
		}

		private static void PrepareNativeColumnFromUsualCode(IdxCol columnDesc) {
			if (null == columnDesc._.Target) {
				columnDesc._.Target = MetaCache.Default.Get<IZetaColumn>(columnDesc._.Code);
			}
		}

		private void CheckCustomCodedColumn(IdxCol columnDesc) {
			if (string.IsNullOrWhiteSpace(columnDesc._.CustomCode)) {
				return;
			}
			var src = columnDesc._;
			DataSession.GetMetaCache().Set(
				new Column
					{
						Code = src.CustomCode,
						ForeignCode = src.InitialCode,
						Year = src.Year,
						Period = src.Period,
						Formula = src.Formula,
						FormulaType = src.FormulaType,
						IsFormula = src.IsFormula
					}
				);
		}

		private void PrepareVisibleColumns() {
			cols = Template.GetAllColumns().Where(
				_ => _.GetIsVisible(Object)).Select((_, i) => new IdxCol {i = i, _ = _}
				);
			var firstyear = Template.Thema.GetParameter("firstyear", 0);
			cols = (
				       from col in cols
					   let ishistory = col._.Group == "HISTORY"
				       let include = (firstyear <= Year && !ishistory) || (firstyear > Year && ishistory)
				       where include
				       select col
			       ).ToArray();
			Colset = cols.Select(_ => _._).ToArray();
		}

		private void PrepareRows() {
			var customRowPreparator = 
				Container.Get<IFormRowProvider>(Template.Thema.Code+".row.preparator")
				??
				Container.Get<IFormRowProvider>(Template.Thema.GetParameter("extension","") + ".row.preparator");
			if (null != customRowPreparator) {
				rows = customRowPreparator.GetRows(this);
				
			}
			else {
				DefaultPrepareRows();
			}
			
		}

		private void DefaultPrepareRows() {
			_ridx = 0;
			IList<FormStructureRow> result = new List<FormStructureRow>();
			foreach (var r in Template.Rows) {
				if (null == r.Target) {
					r.Target = MetaCache.Default.Get<IZetaRow>(r.Code);
				}
			}
			foreach (var row in Template.Rows.Select(_ => _.Target)) {
				if (IsRowMatch(row)) {
					
					AddRow(result, row, 0);
				}
			}
			
			rows = result.ToArray();
			
		}

		private void AddRow(IList<FormStructureRow> result, IZetaRow row, int level, bool markreadonly= false) {
			_ridx++;
			if (PrepareByCustomGeneratorIfNeeded(result, row, level)) return;


			bool mymarkreadonly = markreadonly;
			var myrow = row;
			if (myrow.RefTo != null) {
				myrow = (IZetaRow) myrow.RefTo.GetCopyOfHierarchy();
				myrow.Name = row.Name;
				mymarkreadonly = true;
			}
			if (mymarkreadonly) {
				myrow.LocalProperties["readonly"] = true;
			}
			result.Add(new FormStructureRow {Idx = _ridx, Level = level, Native = myrow});
			var children = myrow.Children.OrderBy(_ => _.GetSortKey()).ToArray();
			foreach (var c in children) {
				if (IsRowMatch(c)) {
					AddRow(result, c, level + 1,mymarkreadonly);
				}
			}
		}

		private bool PrepareByCustomGeneratorIfNeeded(IList<FormStructureRow> result, IZetaRow row, int level) {
			var specialView = TagHelper.Value(row.Tag, "specialformview");
			if (!string.IsNullOrWhiteSpace(specialView)) {
				var customRowPreparator =
					Container.Get<IFormRowProvider>(specialView + ".special.row.preparator");
				if (null == customRowPreparator) {
					var stub = new Row
						{
							Code = "stub_" + row.Code,
							Name = row.Name + " (строка не поддерживается - отсутствует расширение " + specialView + ")",
							MarkCache = "/0CAPTION/"
						};
					result.Add(new FormStructureRow {Idx = _ridx, Level = level, Native = stub});
				}
				else {
					foreach (var formStructureRow in customRowPreparator.GetRows(this, row,level)) {
						formStructureRow.Idx = _ridx++;
						result.Add(formStructureRow);
					}
				}
				return true;
			}
			return false;
		}

		private bool IsRowMatch(IZetaRow row) {
			if (null == row) {
				return false;
			}
#pragma warning disable 612,618
			if (row.IsObsolete(Year)) {
#pragma warning restore 612,618
				return false;
			}
#pragma warning disable 612,618
			if (null != row.Object && row.Object.Id != WorkingObject.Id) {
#pragma warning restore 612,618
				return false;
			}
			if (row.IsMarkSeted("0NOINPUT")) {
				return false;
			}
			var viewforgroup = row.ResolveTag("viewforgroup");
			if (!string.IsNullOrWhiteSpace(viewforgroup)) {
				if (!Object.IsMatchAliases(viewforgroup)) {
					return false;
				}
			}
			if (null != RowCodeFilter) {
				if (-1 == Array.IndexOf(RowCodeFilter, row.Code)) {
					return false;
				}
			}
			return true;
		}
		/// <summary>
		///     Каталог периодов для форм с требованием на файлы
		/// </summary>
		private static readonly IDictionary<int, int> FileBasedPeriods = new Dictionary<int, int>
			{
				{13, 1},
				{1, 13},
				{16, 2},
				{2, 16},
				{19, 3},
				{3, 19},
				{112, 4},
				{4, 112}
			};

		/// <summary>
		/// Возвращает просто статус формы, без расширенных данных
		/// </summary>
		/// <param name="objid"></param>
		/// <returns></returns>
		public string GetSimpleLockState(int objid = 0) {
			lock (this) {
				if (0 == objid) {
					objid = this.Object.Id;
				}
				var obj = MetaCache.Default.Get<IZetaMainObject>(objid);
				return Template.GetState(obj, null);
			}
		}

		/// <summary>
		/// 	Возвращает статусную информацию по форме с поддержкой признака "доступа" блокировки
		/// </summary>
		/// <returns> </returns>
		public LockStateInfo GetStateInfo() {
			WaitData();
			var roles = Application.Roles;
			var isopen = Template.IsOpen;
			Template.CleanupStates();
			var state = Template.GetState(Object, null);

			if (string.IsNullOrWhiteSpace(state)) {
				state = "0ISOPEN";
			}

			var cansave = state == "0ISOPEN";
			var message = Template.CanSetState(Object, null, "0ISBLOCK");
			var principal = Usr.toPrincipal();
			
			var haslockrole = roles.IsInRole(principal, Template.UnderwriteRole);
			var hasholdlockrole = roles.IsInRole(principal, "HOLDUNDERWRITER",true);
			var hasnocontrolpoointsrole = roles.IsInRole(principal,"SYS_NOCONTROLPOINTS",true);
			var canblock = state == "0ISOPEN" && (string.IsNullOrWhiteSpace(message)||(message=="cpavoid"&&hasnocontrolpoointsrole)) && haslockrole;
			var canopen = state != "0ISOPEN" && haslockrole && hasholdlockrole;
			var cancheck = state == "0ISBLOCK" && haslockrole &&  hasholdlockrole;
			var cansaveoverblock = roles.IsInRole(principal, "NOBLOCK",true);
			var periodstateoverride = false;
			var noattachedfiles = false;
			cansave = cansave || cansaveoverblock;
			if (cansave) {
				var periodStateManager = new PeriodStateManager();
				var periodState = periodStateManager.Get(Year, Period,Template.Thema.GetParameter("deadlinetype",""));
				if (!periodState.State) {
					cansave = Template.IgnorePeriodState || roles.IsInRole(principal,"IGNOREPERIODSTATE",true);
					periodstateoverride = cansave;
				}
			}

			var newstates = Template.Thema.GetParameter("bizporcess.isprimary").ToBool();
			FormStateOperationResult canblockresult = null;
			FormStateOperationResult cancheckresult = null;
			FormStateOperationResult canopenresult = null;
			bool cpavoid = false;
			if (newstates) {
				canblockresult = FormStateManager.GetCanSet(this, FormStateType.Closed);
				cancheckresult = FormStateManager.GetCanSet(this, FormStateType.Checked);
				canopenresult = FormStateManager.GetCanSet(this, FormStateType.Open);
				if (!string.IsNullOrWhiteSpace(canblockresult.DefaultMessageForState)) {
					cpavoid = canblockresult.DefaultMessageForState.Contains("cpavoid");
				}
				canopen = canopenresult.Allow;
				canblock = canblockresult.Allow;
				cancheck = cancheckresult.Allow;
			}

			if (!newstates && canblock) {
				if (FileBasedPeriods.ContainsKey(Period)) {
					if (!string.IsNullOrWhiteSpace(Template.NeedFiles)) {
						var types = Template.NeedFiles.SmartSplit().ToArray();
						var attachs = GetAttachedFiles().Where(_=>_.Period==Period||_.Period==FileBasedPeriods[Period]).ToArray();
						foreach (var type in types) {							
							if (!attachs.Any(_ => _.Type == type)) {
								canblock = false;
								noattachedfiles = true;
								break;
							}
						}
					}
				}
			}

			return new LockStateInfo
				{
					isopen = isopen,
					state = state,
					cansave = cansave,
					canblock = canblock,
					message = message,
					haslockrole = haslockrole,
					hasholdlockrole = hasholdlockrole,
					hasnocontrolpoointsrole = hasnocontrolpoointsrole,
					canopen = canopen,
					cancheck  = cancheck,
					cansaveoverblock = cansaveoverblock,
					hasbadcontrolpoints = message=="cpavoid" || message.Contains("точк"),
					periodstateoverride = periodstateoverride,
					newstates = newstates,
					canblockresult = canblockresult,
					cancheckresult = cancheckresult,
					canopenresult = canopenresult,
					noattachedfiles = noattachedfiles,
					cpavoid = cpavoid,
				};
		}

		/// <summary>
		/// 	Метод вызова начала сохранения данных
		/// </summary>
		/// <param name="xmldata"> </param>
		/// <returns> </returns>
		public bool BeginSaveData(XElement xmldata) {
			lock (this) {
                FormSessionsState.DataSaveOperationsIncrease();
				if (null != _currentSaveTask) {
					if (!_currentSaveTask.IsFaulted) {
						_currentSaveTask.Wait();
					}
				}
				CurrentSaver = CurrentSaver ?? (null == FormServer ? null : FormServer.GetSaver()) ?? new DefaultSessionDataSaver();
				_currentSaveTask = CurrentSaver.BeginSave(this, xmldata, Application.Principal.CurrentUser);
				Logger.Info("save started");

                FormSessionsState.DataSaveOperationsDecrease();

				return true;
			}
		}

		/// <summary>
		/// 	Возвращает текущий объект сохранения
		/// </summary>
		/// <returns> </returns>
		public object GetSaveState() {
			lock (this) {
				if (null == CurrentSaver) {
					return new {stage = SaveStage.None, error = null as Exception, result = null as SaveResult};
				}
				if (_currentSaveTask != null && _currentSaveTask.IsFaulted) {
					return new {stage = CurrentSaver.Stage, error = CurrentSaver.Error, result = null as SaveResult};
				}
				if (_currentSaveTask != null && _currentSaveTask.IsCompleted) {
					return new {stage = CurrentSaver.Stage, error = CurrentSaver.Error, result = _currentSaveTask.Result};
				}
				return new {stage = CurrentSaver.Stage, error = CurrentSaver.Error};
			}
		}

		/// <summary>
		/// 	Рестартует сбор данных
		/// </summary>
		/// <returns> </returns>
		public bool RestartData() {
			
			lock (this) {
				Activations++;
				WaitData();
				PrepareDataTask = null;
				DataSession = null;
				Data.Clear();
				if (NeedForceRestart()) {
					return ForceRestart();
				}				
				StartCollectData();
				return true;
			}
		}

		private bool NeedForceRestart() {
			return !string.IsNullOrWhiteSpace(Template.Thema.GetParameter("extension", ""));
		}

		private bool ForceRestart() {
			IsStarted = false;
			Start();
			PrepareStructureTask.Wait();
			return true;
		}

		/// <summary>
		/// 	Подчистка после загрузки данных
		/// </summary>
		/// <returns> </returns>
		public object CleanupAfterDataLoaded() {
			//Structure = null;
			var npcells = Data.Where(_ => !_.canbefilled).ToArray();
			foreach (var outCell in npcells) {
				Data.Remove(outCell);
			}
			return true;
		}

		/// <summary>
		/// 	Вызов блокировки формы
		/// </summary>
		/// <returns> </returns>
		public bool DoLockForm() {
		    FormSessionsState.LockFormOperationsIncrease();

			var currentstate = GetStateInfo();
			if (currentstate.canblock) {
				Template.SetState(Object, null, "0ISBLOCK");
                FormSessionsState.LockFormOperationsDecrease();
				return true;
			}

            FormSessionsState.LockFormOperationsDecrease();
			throw new SecurityException("try lock form without valid state or permission");
		}

		/// <summary>
		/// 	Возвращает историю блокировок
		/// </summary>
		/// <returns> </returns>
		public FormState[] GetLockHistory() {
			var states =
				new NativeZetaReader().ReadFormStates(
					string.Format("Year = {0} and Period = {1} and LockCode='{2}' and Object = {3} order by Version desc"
					              , Year, Period, Template.UnderwriteCode, Object.Id)).ToArray();
			return states;
		}

		/// <summary>
		/// 	Присоединяет файл к сессии и возвращает итог сохранения
		/// </summary>
		/// <param name="datafile"> </param>
		/// <param name="filename"> </param>
		/// <param name="type"> </param>
		/// <param name="uid"> </param>
		/// <returns> </returns>
		public FormAttachment AttachFile(HttpPostedFileBase datafile, string filename, string type, string uid) {
            FormSessionsState.FileAttachOperationsIncrease();

			var storage = GetFormAttachStorage();
			var realfilename = filename;
			if (string.IsNullOrWhiteSpace(realfilename)) {
				realfilename = string.Format("{0}_{1}_{2}_{3}", type, Object.Name.Replace("\"", "_"), Year,
				                             Periods.Get(Period).Name.Replace(".", ""));
			}
			var result = storage.AttachHttpFile(this, datafile, realfilename, type, uid);
            FormSessionsState.FileAttachOperationsDecrease();

			return result;
		}

		private  IFormAttachmentStorage GetFormAttachStorage() {
			var result = Container.Get<IFormAttachmentStorage>();
			if (null == result) {
				var basestorage = Container.Get<IAttachmentStorage>("attachment.source") ?? new DbfsAttachmentStorage();
				result = new FormAttachmentSource();
				((FormAttachmentSource) result).SetStorage(basestorage);
			}
			return result;
		}

		/// <summary>
		/// 	Получает список связанных с сессией файлов
		/// </summary>
		/// <returns> </returns>
		public FormAttachment[] GetAttachedFiles() {
			var storage = GetFormAttachStorage();
			return storage.GetAttachments(this).ToArray();
		}

		/// <summary>
		/// 	Удалить присоединенный файл
		/// </summary>
		/// <param name="uid"> уникальный код аттача </param>
		public void DeleteAttach(string uid) {
			var attach = GetAttachedFiles().FirstOrDefault(_ => _.Uid == uid);
			GetFormAttachStorage().Delete(attach);
		}

		/// <summary>
		/// </summary>
		/// <param name="uid"> </param>
		/// <returns> </returns>
		public IFileDescriptor GetDownloadAbleFileDescriptor(string uid) {
			var attach = GetAttachedFiles().FirstOrDefault(_ => _.Uid == uid);
			var filedesc = new FormAttachmentFileDescriptor(attach, GetFormAttachStorage());
			return filedesc;
		}

		/// <summary>
		/// 	Возвращает допустимые типы файлов для сессии
		/// </summary>
		/// <returns> </returns>
		public FileTypeRecord[] GetAllowedFileTypes() {
			EnsureDataSession();
			var filetypes = DataSession.GetMetaCache().Get<IZetaRow>("DIR_FILE_TYPES").Children.ToArray();
			return (
				       from filetypedesc in filetypes
				       from formcode in TagHelper.Value(filetypedesc.Tag, "form").Split(',')
				       where "any" == formcode || Template.Thema.Code == formcode
				       orderby filetypedesc.Index
				       select new FileTypeRecord {code = filetypedesc.OuterCode, name = filetypedesc.Name}
			       ).ToArray();
		}

		private void EnsureDataSession() {
			DataSession = DataSession ?? PrepareDataSession();
			//DataSession.UseSyncPreparation = true;
		}

		private ISession PrepareDataSession() {
			return ExtremeFactory.CreateSession(new SessionSetupInfo {CollectStatistics = true, PropertySource = new FormSessionDataSessionPropertySoruce(this)});
		}

		#region Nested type: IdxCol

		private class IdxCol {
			public ColumnDesc _;
			public int i;
		}

		#endregion

		#region Nested type: IdxRow

		#endregion
		/// <summary>
		/// Хранилище контрольных точек
		/// </summary>
		internal protected readonly IList<ControlPointResult> _controlpoints = new List<ControlPointResult>();

		private readonly IDictionary<string, IQuery> _processed = new Dictionary<string, IQuery>();
		private IFormSessionDataSaver CurrentSaver;
		private Task<SaveResult> _currentSaveTask;

		private List<OutCell> _data;
		private int _ridx;
		private IEnumerable<IdxCol> cols;
		private IdxCol[] neditprimarycols;
		private IdxCol[] primarycols;
		private FormStructureRow[] primaryrows;
		private FormStructureRow[] rows;
		private string _usr;
		private IUserLog _logger;
		private IZetaMainObject[] splitobjects;

		/// <summary>
		/// Установить статус текущей сессии
		/// </summary>
		/// <param name="state"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		public FormStateOperationResult SetState(FormStateType state, string comment) {
			return FormStateManager.SetState(this, state,comment);
		}
	}
}