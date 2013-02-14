using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Comdiv.Zeta.Model;
using Qorpent.Applications;
using Qorpent.Serialization;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Сессия работы с формой
	/// </summary>
	[Serialize]
	public class FormSession {
		private List<OutCell> _data;

		/// <summary>
		/// Создает сессию формы
		/// </summary>
		/// <param name="form"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="obj"></param>
		public FormSession(IInputTemplate form, int year, int period, IZetaMainObject obj) {
			Uid = Guid.NewGuid().ToString();
			DataSession = new Session();
			Serial = DataSession.AsSerial();
			Created = DateTime.Now;
			Template = form.PrepareForPeriod(year,period,DateTime.Now,Object);
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
		/// Признак, что сессия стартовала
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// Признак завершения обработки сессии
		/// </summary>
		public bool IsFinished {
			get { return PrepareDataTask.IsCompleted && PrepareStructureTask.IsCompleted; }
		}

		/// <summary>
		/// Стартует сессию
		/// </summary>
		public void Start() {
			PrepareStructureTask = new TaskWrapper(
				Task.Run(()=>
					{
						Structure = new XElement("form");
					})
			);
			PrepareDataTask = new TaskWrapper(	
				Task.Run(()=>
					{
						try {
							RetrieveData();
						}catch(Exception ex) {
							this.Error = ex;
						}
					})
			);
			IsStarted = true;
		}
		/// <summary>
		/// Ошибка сессии
		/// </summary>
		protected Exception Error { get; set; }

		/// <summary>
		/// Сообщение об ошибке для сериализации
		/// </summary>
		[SerializeNotNullOnly]
		public string ErrorMessage {get {
			if(null!=Error) {
				return Error.ToString();
			}
			return null;
		}}

		private void RetrieveData() {
			var rootrow = MetaCache.Default.Get<IZetaRow>(Template.Form.Code);
			var rows = rootrow.AllChildren.OrderBy(_=>_.Path).Select((_,i)=>new{i,_}).ToArray();
			var cols = Template.GetAllColumns().Where(_ => _.GetIsVisible(Object)).Select((_,i)=>new{i,_});
			foreach (var columnDesc in cols) {
				if(null==columnDesc._.Target) {
					columnDesc._.Target = MetaCache.Default.Get<IZetaColumn>(columnDesc._.Code);
				}
			}
			var primarycols = cols.Where(_ => _._.Editable && !_._.IsFormula).ToArray();
			var primaryrows = rows.Where(_ => !_._.IsFormula && 0 == _._.Children.Count && !_._.IsMarkSeted("0ISCAPTION")).ToArray();
			IDictionary<string,Query> queries = new Dictionary<string,Query>();
			foreach (var primaryrow in primaryrows) {
				foreach (var primarycol in primarycols) {
					var q = new Query
						{
							Row = {Native = primaryrow._},
							Col = {Native = primarycol._.Target},
							Obj = {Native = Object},
							Time = {Year = primarycol._.Year, Period = primarycol._.Period}
						};
					var key = primaryrow.i+":"+primarycol.i;
					queries[key]=DataSession.Register(q,key);
				}
			}
			DataSession.Execute();
			foreach (var q in queries) {
				string val = "";
				var cellid = 0;
				if(null!=q.Value && null!=q.Value.Result) {


					val = q.Value.Result.NumericResult.ToString("#.#,##");
					if (q.Value.Result.Error != null) {
						val = q.Value.Result.Error.Message;
					}
					cellid = q.Value.Result.CellId;
				}
		
					lock (Data) {
						Data.Add(new OutCell {i = q.Key, c = cellid, v = val});
					}
				
			}

			foreach (var r in rows) {
				foreach (var c in cols) {
					var key =r.i+":"+c.i;
					if(queries.ContainsKey(key))continue;
					var q = new Query
						{
							Row = {Native = r._},
							Col = {Native = c._.Target},
							Obj = {Native = Object},
							Time = {Year = c._.Year, Period = c._.Period}
						};
					queries[key] = q = DataSession.Register(q,key);
					var qr = Serial.Eval(q);
					var val = qr.NumericResult.ToString("#.#,##");
					if(qr.Error!=null) {
						val = qr.Error.Message;
					}
					
						lock (Data) {
							Data.Add(new OutCell {i = key, c = 0, v = val});
						}
					
				}
			}

		}

		/// <summary>
		/// Идентификатор сессии
		/// </summary>
		public string Uid { get; private set; }
		/// <summary>
		/// Время создания
		/// </summary>
		public DateTime Created{ get; private set; }
		/// <summary>
		/// Год
		/// </summary>
		public int Year { get; private set; }
		/// <summary>
		/// Период
		/// </summary>
		public int Period { get; private set; }
		/// <summary>
		/// Объект
		/// </summary>
		[IgnoreSerialize]
		public IZetaMainObject Object { get; private set; }
		/// <summary>
		/// Шаблон
		/// </summary>
		[IgnoreSerialize]
		public IInputTemplate Template { get; private set; }
		/// <summary>
		/// Пользователь
		/// </summary>
		public string Usr { get; private set; }
		/// <summary>
		/// Сессия работы с данными
		/// </summary>
		[IgnoreSerialize]
		public Session DataSession { get; private set; }

		/// <summary>
		/// Задача формирования структуры
		/// </summary>
		[IgnoreSerialize]
		public TaskWrapper PrepareStructureTask { get; private set; }

		/// <summary>
		/// Задача формирования данных
		/// </summary>
		[IgnoreSerialize]
		public TaskWrapper PrepareDataTask { get; private set; }
		/// <summary>
		/// Хранит уже подготовленные данные
		/// </summary>
		[IgnoreSerialize]
		public List<OutCell> Data {
			get { return _data ?? (_data = new List<OutCell>()); }
			
		}
		/// <summary>
		/// Хранит структуру формы
		/// </summary>
		[IgnoreSerialize]
		public XElement Structure { get; private set; }

		/// <summary>
		/// Информация об объекте
		/// </summary>
		[Serialize] public object ObjInfo { get; private set; }

		/// <summary>
		/// Информация о форме ввода
		/// </summary>
		[Serialize] public object FormInfo { get; private set; }

	}
}