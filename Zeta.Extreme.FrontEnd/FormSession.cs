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
using Qorpent.Applications;
using Qorpent.Log;
using Qorpent.Mvc;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form;
using Zeta.Extreme.Form.DbfsAttachmentSource;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.StateManagement;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Сессия работы с формой
	/// </summary>
	[Serialize]
	public class FormSession :
		IFormSession,
		IFormDataSynchronize,
		IFormSessionControlPointSource {
		/// <summary>
		/// 	Создает сессию формы
		/// </summary>
		/// <param name="form"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="obj"> </param>
		public FormSession(IInputTemplate form, int year, int period, IZetaMainObject obj) {
			Uid = Guid.NewGuid().ToString();
			Created = DateTime.Now;
			Template = form.PrepareForPeriod(year, period, new DateTime(1900, 1, 1), Object);
			Template.AttachedSession = this;
			Year = Template.Year;
			Period = Template.Period;
			Object = obj;
			Created = DateTime.Now;
			Usr = Application.Current.Principal.CurrentUser.Identity.Name;
			IsStarted = false;
			var reader = new NativeZetaReader();
			var currency = Object.Currency;
			if (string.IsNullOrWhiteSpace(currency)) {
				currency = "RUB";
			}
			decimal rate = 1;
			if (currency != "RUB") {
				rate = reader.GetCurrencyRate(Year, Period, currency, "RUB");
			}
			ObjInfo = new
				{
					Object.Id, 
					Object.Code, 
					Object.Name,
					Currency=currency,
					CurrencyRate = rate,
				};
			
			var holdlogin = GetHoldLogin(reader);
			FormInfo = new
				{
					Template.Code, 
					Template.Name, 
					ObjectResponsibility = reader.GetThemaResponsiveLogin(Template.Thema.Code, Object.Id), 
					HoldResponsibility = holdlogin ,
					Status = Template.Thema.GetParameter("status",""),
					FirstYear = Template.Thema.GetParameter("firstyear", ""),
					RolePrefix = Template.Thema.GetParameter("roleprefix", ""),
				};
			NeedMeasure = Template.ShowMeasureColumn;
			Activations = 1;
			Logger = Application.Current.LogManager.GetLog("form.log", this);


		}

		private string GetHoldLogin(NativeZetaReader reader) {
			int id = 0;
			var holdlogin = "";
			var holdobj = MetaCache.Default.Get<IZetaMainObject>("0CH");
			if (holdobj != null) {
				id = holdobj.Id;
				holdlogin = reader.GetThemaResponsiveLogin(Template.Thema.Code, id);
			}
			return holdlogin;
		}

		/// <summary>
		/// Журнал
		/// </summary>
		public IUserLog Logger { get; set; }


		/// <summary>
		/// 	Количество активаций (повторного использования сессий)
		/// </summary>
		public int Activations { get; set; }

		/// <summary>
		/// 	Признак требования показывать колонку с единицей измерения
		/// </summary>
		public bool NeedMeasure { get; set; }

		/// <summary>
		/// 	Признак, что сессия стартовала
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// 	Признак завершения обработки сессии
		/// </summary>
		public bool IsFinished {
			get { return PrepareDataTask.IsCompleted && PrepareStructureTask.IsCompleted; }
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
			PrepareDataTask.Wait();
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
		public int Period { get; private set; }

		/// <summary>
		/// 	Объект
		/// </summary>
		[IgnoreSerialize] public IZetaMainObject Object { get; private set; }

		/// <summary>
		/// 	Шаблон
		/// </summary>
		[IgnoreSerialize] public IInputTemplate Template { get; private set; }

		/// <summary>
		/// 	Пользователь
		/// </summary>
		public string Usr { get; private set; }

		/// <summary>
		/// 	Хранит уже подготовленные данные
		/// </summary>
		[IgnoreSerialize] public List<OutCell> Data {
			get { return _data ?? (_data = new List<OutCell>()); }
		}

		/// <summary>
		/// 	Возвращает статусную информацию по форме с поддержкой признака "доступа" блокировки
		/// </summary>
		/// <returns> </returns>
		public LockStateInfo GetMinimalStateInfo() {
			var isopen = Template.IsOpen;
			Template.CleanupStates();
			var state = Template.GetState(Object, null);
			var cansave = state == "0ISOPEN";
			return new LockStateInfo
				{
					isopen = isopen,
					state = state,
					cansave = cansave,
				};
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
							data = Data.Skip(startidx).Take(cnt).ToArray()
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
				if (IsStarted) {
					return;
				}

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

		private void RetrieveStructure() {
			StructureInProcess = true;
			var sw = Stopwatch.StartNew();
			Structure =
				(from ri in rows
				 let r = ri._
				 select new StructureItem
					 {
						 type = "r",
						 code = r.Code,
						 name = r.Name,
						 idx = ri.i,
						 iscaption = r.IsMarkSeted("0CAPTION"),
						 isprimary = r.GetIsPrimary(),
						 level = ri.l,
						 number = r.OuterCode,
						 measure = NeedMeasure ? r.ResolveMeasure() : "",
						 controlpoint = r.IsMarkSeted("CONTROLPOINT"),
						 exref = null!=r.ExRefTo,
						 format = r.ResolveTag("numberformat")
					 })
					.Union(
						(from ci in cols
						 let c = ci._
						 select new StructureItem
							 {
								 type = "c",
								 code = c.Code,
								 name = c.Title,
								 idx = ci.i,
								 isprimary = c.Editable && !c.IsFormula && !c.IsAuto,
								 year = c.Year,
								 period = c.Period,
								 controlpoint = c.ControlPoint,
								 exref = null!=c.Target && c.Target.IsMarkSeted("DOEXREF"),
								 format = c.NumberFormat
							 })
					).ToArray();
			sw.Stop();
			TimeToStructure = sw.Elapsed;
			StructureInProcess = false;
		}


		private void RetrieveData() {
			_controlpoints.Clear();
			Data.Clear();
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
					controlPointResult.Query = null;
				}
			}
			InitSaveMode = false;
			Logger.Info("data loaded");
		}

		private void LoadNoPrimary(IDictionary<string, IQuery> queries) {
			foreach (var c in cols) {
				foreach (var r in rows) {
					var key = r.i + ":" + c.i;
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
					var q = ExtremeFactory.CreateQuery( new QuerySetupInfo
						{
							Row = {Native = r._},
							Col = ch,
							Obj = {Native = Object},
							Time = {Year = c._.Year, Period = c._.Period}
						});
					q = DataSession.Register(q, key);

					if (null != q) {
						if (c._.ControlPoint && r._.IsMarkSeted("CONTROLPOINT")) {
							_controlpoints.Add(new ControlPointResult {Col = c._, Row = r._, Query = q});
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
							Row = {Native = primaryrow._},
							Col = {Native = primarycol._.Target},
							Obj = {Native = Object},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period}
						});
					var key = primaryrow.i + ":" + primarycol.i;
					queries[key] = (IQuery) DataSession.Register(q, key);
				}
			}
		}

		private void BuildNonEditablePrimarySet(IDictionary<string, IQuery> queries) {
			foreach (var primaryrow in primaryrows) {
				foreach (var primarycol in neditprimarycols) {
					var q =  ExtremeFactory.CreateQuery( new QuerySetupInfo
						{
							Row = {Native = primaryrow._},
							Col = {Native = primarycol._.Target},
							Obj = {Native = Object},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period}
						});
					var key = primaryrow.i + ":" + primarycol.i;
					queries[key] = DataSession.Register(q, key);
				}
			}
		}

		private void ProcessValues(IDictionary<string, IQuery> queries, bool canbefilled) {
			foreach (var q_ in queries.Where(_ => null != _.Value)) {
				if (_processed.ContainsKey(q_.Key)) {
					continue;
				}
				_processed[q_.Key] = q_.Value;
				var val = "";
				var cellid = 0;
				if (null != q_.Value && null != q_.Value.Result) {
					val = q_.Value.Result.NumericResult.ToString("0.#####", CultureInfo.InvariantCulture);
					
					cellid = q_.Value.Result.CellId;
				}
				var realkey = "";
				if (canbefilled) {
					realkey = q_.Value.Row.Code + "_" + q_.Value.Col.Code + "_" + q_.Value.Time.Year + "_" + q_.Value.Time.Period;
				}
				var cell = new OutCell {i = q_.Key, c = cellid, v = val, canbefilled = canbefilled, query = q_.Value, ri = realkey};
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
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		public object CollectDebugInfo() {
			return new {stats=DataStatistics, sql = SqlLog, colset = Colset};
		}

		private void PrepareMetaSets() {
			PrepareRows();
			InitializeColset();
			primarycols = cols.Where(_ => _._.Editable && !_._.IsFormula).ToArray();
			neditprimarycols = cols.Where(_ => !_._.Editable && !_._.IsFormula).ToArray();
			primaryrows = rows.Where(_ => !_._.IsFormula && 0 == _._.Children.Count && !_._.IsMarkSeted("0ISCAPTION")).ToArray();
		}

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
			cols = (
				       from col in cols
				       let firstyear = TagHelper.Value(col._.Tag, "firstyear").ToInt()
				       let ishistory = col._.Group == "HISTORY"
				       let include = (firstyear <= Year && !ishistory) || (firstyear > Year && ishistory)
				       where include
				       select col
			       ).ToArray();
			Colset = cols.Select(_ => _._).ToArray();
		}

		private void PrepareRows() {
			_ridx = 0;
			IList<IdxRow> result = new List<IdxRow>();
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

		private void AddRow(IList<IdxRow> result, IZetaRow row, int level, bool markreadonly= false) {
			_ridx++;
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
			result.Add(new IdxRow {i = _ridx, l = level, _ = myrow});
			var children = myrow.Children.OrderBy(_ => _.GetSortKey()).ToArray();
			foreach (var c in children) {
				if (IsRowMatch(c)) {
					AddRow(result, c, level + 1,mymarkreadonly);
				}
			}
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
			if (null != row.Object && row.Object.Id != Object.Id) {
#pragma warning restore 612,618
				return false;
			}
			if (row.IsMarkSeted("0NOINPUT")) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Возвращает статусную информацию по форме с поддержкой признака "доступа" блокировки
		/// </summary>
		/// <returns> </returns>
		public LockStateInfo GetStateInfo() {
			var isopen = Template.IsOpen;
			Template.CleanupStates();
			var state = Template.GetState(Object, null);
			if (string.IsNullOrWhiteSpace(state)) {
				state = "0ISOPEN";
			}
			var cansave = state == "0ISOPEN";
			var message = Template.CanSetState(Object, null, "0ISBLOCK");
			var principal = Usr.toPrincipal();
			
			var haslockrole = Application.Current.Roles.IsInRole(principal, Template.UnderwriteRole);
			var hasholdlockrole = Application.Current.Roles.IsInRole(principal, "HOLDUNDERWRITER");
			var hasnocontrolpoointsrole = Application.Current.Roles.IsInRole(principal,"SYS_NOCONTROLPOINTS",true);
			var canblock = state == "0ISOPEN" && (string.IsNullOrWhiteSpace(message)||(message=="cpavoid"&&hasnocontrolpoointsrole)) && haslockrole;
			var canopen = state != "0ISOPEN" && haslockrole && hasholdlockrole;
			var cancheck = state == "0ISBLOCK" && haslockrole &&  hasholdlockrole;
			var cansaveoverblock = Application.Current.Roles.IsInRole(principal, "NOBLOCK",true);
			cansave = cansave || cansaveoverblock;
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
					hasbadcontrolpoints = message=="cpavoid" || message.Contains("точк")
				};
		}

		/// <summary>
		/// 	Метод вызова начала сохранения данных
		/// </summary>
		/// <param name="xmldata"> </param>
		/// <returns> </returns>
		public bool BeginSaveData(XElement xmldata) {
			lock (this) {
				
				if (null != _currentSaveTask) {
					if (!_currentSaveTask.IsFaulted) {
						_currentSaveTask.Wait();
					}
				}
				CurrentSaver = CurrentSaver ?? (null == FormServer ? null : FormServer.GetSaver()) ?? new DefaultSessionDataSaver();
				_currentSaveTask = CurrentSaver.BeginSave(this, xmldata, Application.Current.Principal.CurrentUser);
				Logger.Info("save started");
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
				WaitData();
				PrepareDataTask = null;
				StartCollectData();
				return true;
			}
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
			var currentstate = GetStateInfo();
			if (currentstate.canblock) {
				Template.SetState(Object, null, "0ISBLOCK");
				return true;
			}
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
			var storage = GetFormAttachStorage();
			var realfilename = filename;
			if (string.IsNullOrWhiteSpace(realfilename)) {
				realfilename = string.Format("{0}_{1}_{2}_{3}", type, Object.Name.Replace("\"", "_"), Year,
				                             Periods.Get(Period).Name.Replace(".", ""));
			}
			var result = storage.AttachHttpFile(this, datafile, realfilename, type, uid);
			return result;
		}

		private static IFormAttachmentStorage GetFormAttachStorage() {
			var result = Application.Current.Container.Get<IFormAttachmentStorage>();
			if (null == result) {
				var basestorage = Application.Current.Container.Get<IAttachmentStorage>("attachment.source") ?? new DbfsAttachmentStorage();
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
			DataSession = DataSession ?? ExtremeFactory.CreateSession(new SessionSetupInfo {CollectStatistics = true});
		}

		#region Nested type: IdxCol

		private class IdxCol {
			public ColumnDesc _;
			public int i;
		}

		#endregion

		#region Nested type: IdxRow

		private class IdxRow {
			public IZetaRow _;
			public int i;
			public int l;
		}

		#endregion

		private readonly IList<ControlPointResult> _controlpoints = new List<ControlPointResult>();

		private readonly IDictionary<string, IQuery> _processed = new Dictionary<string, IQuery>();
		private IFormSessionDataSaver CurrentSaver;
		private Task<SaveResult> _currentSaveTask;

		private List<OutCell> _data;
		private int _ridx;
		private IEnumerable<IdxCol> cols;
		private IdxCol[] neditprimarycols;
		private IdxCol[] primarycols;
		private IdxRow[] primaryrows;
		private IdxRow[] rows;

	}
}