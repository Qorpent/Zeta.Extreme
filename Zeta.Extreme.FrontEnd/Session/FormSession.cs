#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormSession.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Qorpent.Applications;
using Qorpent.Serialization;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.FrontEnd.Session {
	/// <summary>
	/// 	Сессия работы с формой
	/// </summary>
	[Serialize]
	public class FormSession {
		/// <summary>
		/// 	Создает сессию формы
		/// </summary>
		/// <param name="form"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="obj"> </param>
		public FormSession(IInputTemplate form, int year, int period, IZetaMainObject obj) {
			Uid = Guid.NewGuid().ToString();
			DataSession = new Extreme.Session();
			Serial = DataSession.AsSerial();
			Created = DateTime.Now;
			Template = form.PrepareForPeriod(year, period, DateTime.Now, Object);
			Year = Template.Year;
			Period = Template.Period;
			Object = obj;
			Created = DateTime.Now;
			Usr = Application.Current.Principal.CurrentUser.Identity.Name;
			IsStarted = false;
			ObjInfo = new {Object.Id, Object.Code, Object.Name};
			FormInfo = new {Template.Code, Template.Name};
		}

		private ISerialSession Serial { get; set; }

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
		protected Exception Error { get; set; }

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
		/// 	Идентификатор сессии
		/// </summary>
		public string Uid { get; private set; }

		/// <summary>
		/// 	Время создания
		/// </summary>
		public DateTime Created { get; private set; }

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
		/// 	Сессия работы с данными
		/// </summary>
		[IgnoreSerialize] public Extreme.Session DataSession { get; private set; }

		/// <summary>
		/// 	Задача формирования структуры
		/// </summary>
		[IgnoreSerialize] public TaskWrapper PrepareStructureTask { get; private set; }

		/// <summary>
		/// 	Задача формирования данных
		/// </summary>
		[IgnoreSerialize] public TaskWrapper PrepareDataTask { get; private set; }

		/// <summary>
		/// 	Хранит уже подготовленные данные
		/// </summary>
		[IgnoreSerialize] public List<OutCell> Data {
			get { return _data ?? (_data = new List<OutCell>()); }
		}

		/// <summary>
		/// 	Хранит структуру формы
		/// </summary>
		[IgnoreSerialize] public StructureItem[] Structure { get; private set; }

		/// <summary>
		/// 	Информация об объекте
		/// </summary>
		[Serialize] public object ObjInfo { get; private set; }

		/// <summary>
		/// 	Информация о форме ввода
		/// </summary>
		[Serialize] public object FormInfo { get; private set; }

		/// <summary>
		/// 	Стартует сессию
		/// </summary>
		public void Start() {
			var sw = Stopwatch.StartNew();
			PrepareMetaSets();
			sw.Stop();
			TimeToPrepare = sw.Elapsed;
			PrepareStructureTask = new TaskWrapper(
				Task.Run(() => { RetrieveStructura(); })
				);
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
				);
			IsStarted = true;
		}
		/// <summary>
		/// Время подготовки
		/// </summary>
		[Serialize]
		public TimeSpan TimeToPrepare { get; set; }

		private void RetrieveStructura() {
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
						 isprimary = !r.IsFormula && !r.IsMarkSeted("0SA") && 0 == r.Children.Count,
						 level = r.Level
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
								 isprimary = c.Editable && !c.IsFormula,
								 year = c.Year,
								 period = c.Period,
							 })
					).ToArray();
			sw.Stop();
			TimeToStructure = sw.Elapsed;
		}
		/// <summary>
		/// Время генерации структуры
		/// </summary>
		[Serialize]
		public TimeSpan TimeToStructure { get; set; }

		/// <summary>
		/// Время генерации первичных ячеек
		/// </summary>
		[Serialize]
		public TimeSpan TimeToPrimary { get; set; }

		/// <summary>
		/// Время генерации первичных ячеек
		/// </summary>
		[Serialize]
		public TimeSpan TimeToGetData { get; set; }


		private void RetrieveData() {
			var sw = Stopwatch.StartNew();
			IDictionary<string, Query> queries = new Dictionary<string, Query>();
			foreach (var primaryrow in primaryrows) {
				foreach (var primarycol in primarycols) {
					var q = new Query
						{
							Row = {Native = primaryrow._},
							Col = {Native = primarycol._.Target},
							Obj = {Native = Object},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period}
						};
					var key = primaryrow.i + ":" + primarycol.i;
					queries[key] = DataSession.Register(q, key);
				}
			}
			DataSession.Execute(500);
			ProcessValues(queries);
			TimeToPrimary = sw.Elapsed;
			PrimaryCount = Data.Count;
			//var cnt = 0;
			foreach (var r in rows) {
				foreach (var c in cols) {

					var key = r.i + ":" + c.i;
					if (queries.ContainsKey(key)) {
						continue;
					}
					var q = new Query
						{
							Row = {Native = r._},
							Col = {Native = c._.Target},
							Obj = {Native = Object},
							Time = {Year = c._.Year, Period = c._.Period}
						};
					 q = DataSession.Register(q, key);
					 if(null!=q) {
						 queries[key] = q;
					 }
					//cnt++;
					//if(cnt>=300) {
					//	cnt = 0;
					//	DataSession.Execute(500);
					//	ProcessValues(queries);
					//}
				}
			}
			DataSession.Execute(500);
			ProcessValues(queries);
			QueriesCount = queries.Count;
			DataSession = null;
			DataCount = Data.Count;
			TimeToGetData = sw.Elapsed;
		}
		/// <summary>
		/// Общее количество запросов в обработке
		/// </summary>
		public int QueriesCount { get; set; }

		private void ProcessValues(IDictionary<string, Query> queries) {
			foreach (var q_ in queries.Where(_ =>null!=_.Value && _.Value.Processed == false)) {
				q_.Value.Processed = true;
				var val = "";
				var cellid = 0;
				if (null != q_.Value && null != q_.Value.Result) {
					val = q_.Value.Result.NumericResult.ToString("#.#,##");
					if (q_.Value.Result.Error != null) {
						val = q_.Value.Result.Error.Message;
					}
					cellid = q_.Value.Result.CellId;
				}

				lock (Data) {
					Data.Add(new OutCell {i = q_.Key, c = cellid, v = val});
				}
			}
		}

		/// <summary>
		/// Общее количество ячеек
		/// </summary>
		public int DataCount { get; set; }

		/// <summary>
		/// Количество первичных ячеек
		/// </summary>
		
		public int PrimaryCount { get; set; }

		private void PrepareMetaSets() {
			rootrow = MetaCache.Default.Get<IZetaRow>(Template.Form.Code);
			rows = rootrow.AllChildren.OrderBy(_ => _.Path).Select((_, i) => new IdxRow {i = i, _ = _}).ToArray();
			cols = Template.GetAllColumns().Where(_ => _.GetIsVisible(Object)).Select((_, i) => new IdxCol {i = i, _ = _});
			foreach (var columnDesc in cols) {
				if (null == columnDesc._.Target) {
					columnDesc._.Target = MetaCache.Default.Get<IZetaColumn>(columnDesc._.Code);
				}
			}
			cols = cols.Where(_ => _._.Target != null).ToArray(); //пока только хранимые колонки поддерживаем
			primarycols = cols.Where(_ => _._.Editable && !_._.IsFormula).ToArray();
			primaryrows = rows.Where(_ => !_._.IsFormula && 0 == _._.Children.Count && !_._.IsMarkSeted("0ISCAPTION")).ToArray();
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
		}

		#endregion

		private List<OutCell> _data;
		private IEnumerable<IdxCol> cols;
		private IdxCol[] primarycols;
		private IdxRow[] primaryrows;
		private IZetaRow rootrow;
		private IdxRow[] rows;
	}
}