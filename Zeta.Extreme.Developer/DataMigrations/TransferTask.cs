using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent.Applications;
using Qorpent.Serialization;
using Zeta.Extreme.Developer.MetaStorage.Tree;
using Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.DataMigrations {
	/// <summary>
	/// Задача по переносу значения строки
	/// </summary>
	[Serialize]
	public class TransferTask {
		private string _targetCode;

		/// <summary>
		/// Исходный код
		/// </summary>
		public string SourceCode { get; set; }
		/// <summary>
		/// Целевой код
		/// </summary>
		public string TargetCode {
			get { return string.IsNullOrWhiteSpace(_targetCode)?SourceCode:_targetCode; }
			set { _targetCode = value; }
		}
		/// <summary>
		/// Набор кодов колонок
		/// </summary>
		[SerializeNotNullOnly]
		public string[] ColumnCodes { get; set; }
		/// <summary>
		/// Явный набор годов
		/// </summary>
		[SerializeNotNullOnly]
		public int[] Years { get; set; }

		/// <summary>
		/// Явный набор периодов
		/// </summary>
		[SerializeNotNullOnly]
		public int[] Periods { get; set; }

		/// <summary>
		/// Явный набор объектов
		/// </summary>
		[SerializeNotNullOnly]
		public int[] ObjectIds { get; set; }
		/// <summary>
		/// Признак автоматического набора годов
		/// </summary>
		[SerializeNotNullOnly]
		public bool AutoYears { get { return null == Years || 0 == Years.Length; } }
		/// <summary>
		/// Признак автоматического набора периодов
		/// </summary>
		[SerializeNotNullOnly]
		public bool AutoPeriods { get { return null == Periods || 0 == Periods.Length; } }
		/// <summary>
		/// Признак автоматического набора объектов
		/// </summary>
		[SerializeNotNullOnly]
		public bool AutoObjects { get { return null == ObjectIds || 0 == ObjectIds.Length; } }

		IZetaRow _target;
		/// <summary>
		/// Целевая строка
		/// </summary>
		[IgnoreSerialize]
		public IZetaRow Target {
			get {
				if (null == _target) {
					_target = MetaCache.Default.Get<IZetaRow>(TargetCode);
				}
				return _target;
			}
		}
		IZetaRow _source;
		/// <summary>
		/// Целевая строка
		/// </summary>
		[IgnoreSerialize]
		public IZetaRow Source {
			get {
				if (null == _source) {
					_source = MetaCache.Default.Get<IZetaRow>(SourceCode);
				}
				return _source;
			}
		}
		IZetaRow[] _sourcePrimaries;
		/// <summary>
		/// Перечень исходных строк
		/// </summary>
		[IgnoreSerialize]
		public IZetaRow[] SourcePrimaries
		{
			get
			{
				if (null == _sourcePrimaries) {
					var graphtask = new DependencyGraphTask {StartRow = Source, Direction = DependencyDirection.Down};
					graphtask.Build();
					_sourcePrimaries = graphtask.ResultGraph.Nodes.Values
					                           .Where(_ => _.Type==DependencyNodeType.Primary)
					                           .Select(_ => _.Label)
					                           .Select(_ => MetaCache.Default.Get<IZetaRow>(_))
					                           .ToArray();
												  
				}

				return _sourcePrimaries;
			}
		}
		/// <summary>
		/// Коды исходных строк
		/// </summary>
		[SerializeNotNullOnly]
		public string[] SourcePrimaryCodes {
			get { return SourcePrimaries.Select(_ => _.Code).ToArray(); }
		}

		IZetaColumn[] _columns;
		/// <summary>
		/// Колонки в переносе
		/// </summary>
		[IgnoreSerialize]
		public IZetaColumn[] Columns {
			get{
				if(null==_columns){
					_columns = ColumnCodes.Select(_=>MetaCache.Default.Get<IZetaColumn>(_)).ToArray();
				}
				return _columns;
			}
		}

		IZetaMainObject[] _objects;
		/// <summary>
		/// Объекты в переносе
		/// </summary>
		[SerializeNotNullOnly]
		public IZetaMainObject[] Objects {
			get{
				if(AutoObjects)return null;
				if(null==_objects){
					_objects = ObjectIds.Select(_=>MetaCache.Default.Get<IZetaMainObject>(_)).ToArray();
				}
				return _objects;
			}
		}

		string _rowcondition;
		private string GetRowCondition()
		{
			if(null==_rowcondition){
				if(0!=SourcePrimaries.Length){
					_rowcondition = "(row in ("+string.Join(", ", SourcePrimaries.Select(_=>_.Id))+"))";
				}else{
					_rowcondition = "(1=1)";
				}
			}
			return _rowcondition;
			
		}

		string _yearcondition;
		private string GetYearCondition()
		{
			if(null==_yearcondition){
				if(!AutoYears){
					_yearcondition = "(year in ("+string.Join(", ", Years)+"))";
				}else{
					_yearcondition = "(1=1)";
				}
			}
			return _yearcondition;
		}

		string _peirodcondition;
		private string GetPeriodCondition()
		{
			if(null==_peirodcondition){
				if(!AutoPeriods){
					_peirodcondition = "(period in ("+string.Join(", ", Periods)+"))";
				}else{
					_peirodcondition = "(1=1)";
				}
			}
			return _peirodcondition;
		
		}

		string _objectcondition;
		private string GetObjectCondition()
		{
			if(null==_objectcondition){
				if(!AutoObjects){
					_objectcondition = "(obj in ("+string.Join(", ", ObjectIds)+"))";
				}else{
					_objectcondition = "(1=1)";
				}
			}
			return _objectcondition;
			
		}

		private string GetFullCondition(){
			return "("+ GetRowCondition()+" and " + GetYearCondition()+ " and " + GetPeriodCondition() + " and " + GetObjectCondition()+")";
		}

		private string FormatStartCountQuery(string fld)
		{
			return "select count (distinct " + fld + " ) from cell where decimalvalue!=0 and " + GetFullCondition();
		}

		int _yearscount= -1;
		/// <summary>
		/// Условный код переноса пользователя
		/// </summary>
		public string UserCode { get; set; }
		
		/// <summary>
		/// Возвращает полную конструкцию where для запросов
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public int YearsCount
		{
			get
			{
				if (_yearscount == -1)
				{
					_yearscount =  EvalCount(FormatStartCountQuery("year"));
				}
				return _yearscount;
			}
		}
		int _periodscount = -1;
		/// <summary>
		/// Возвращает полную конструкцию where для запросов
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public int PeriodCount
		{
			get
			{
				if (_periodscount == -1)
				{
					_periodscount = EvalCount(FormatStartCountQuery("period"));
				}
				return _periodscount;
			}
		}

		int _objectscount = -1;
		/// <summary>
		/// Возвращает полную конструкцию where для запросов
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public int ObjectCount
		{
			get
			{
				if (_objectscount == -1)
				{
					_objectscount = EvalCount(FormatStartCountQuery("obj"));
				}
				return _objectscount;
			}
		}

		int _yearperiodcount = -1;
		/// <summary>
		/// Возвращает полную конструкцию where для запросов
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public int YearPeriodCount
		{
			get
			{
				if (_yearperiodcount == -1)
				{
					_yearperiodcount = EvalCount(FormatStartCountQuery(" period*100 + (year - 2000)"));
				}
				return _yearperiodcount;
			}
		}

		int _yearperiodobjectcount = -1;
		/// <summary>
		/// Возвращает полную конструкцию where для запросов
		/// </summary>
		/// <returns></returns>
		[Serialize]
		public int YearPeriodObjectCount
		{
			get
			{
				if (_yearperiodobjectcount == -1)
				{
					_yearperiodobjectcount = EvalCount(FormatStartCountQuery(" cast(year as bigint)-2000+cast(period as bigint)*1000000+obj*100 "));
				}
				return _yearperiodobjectcount;
			}
		}

		/// <summary>
		/// Объем переноса
		/// </summary>
		[Serialize]
		public int EvaluationCount
		{
			get
			{
				return YearPeriodObjectCount * ColumnCodes.Length;
			}
		}
        /// <summary>
        /// Признак необходимости выполнения переноса
        /// </summary>
        [Serialize]
        public bool Execute { get; set; }
        /// <summary>
        /// Признак необходимости предварительной проверки данных на изменение
        /// </summary>
        [Serialize]
	    public bool ChangedOnly { get; set; }

        /// <summary>
        /// Указание сделать по итогу переноса исходную строку первичной
        /// </summary>
        [Serialize]
        public bool MakeSourcePrimary { get; set; }

        /// <summary>
        /// Указание на то, что исходную строку следует рассматривать как сумму
        /// </summary>
        [Serialize]
	    public bool TreatSourceAsSum { get; set; }
        /// <summary>
        /// Указание на то, что исходную строку следует рассматривать как формулу
        /// </summary>
        [Serialize]
	    public bool TreatSourceAsFormula { get; set; }

	    private int EvalCount(string q)
		{
			using (var c = Application.Current.DatabaseConnections.GetConnection("Default"))
			{
				c.Open();
				var com = c.CreateCommand();
				com.CommandText = q;
				var result = com.ExecuteScalar();
				return Convert.ToInt32(result);
			}
		}

		/// <summary>
		/// Формирует набор целевых объектов для переноса
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TransferRecord> GetTransferRecords() {
			var result = new List<TransferRecord>();
			IDictionary<int, IZetaMainObject> _objcache = new Dictionary<int, IZetaMainObject>();
			Func<int, IZetaMainObject> _getobj = id => {
				if (!_objcache.ContainsKey(id)) {
					_objcache[id] = MetaCache.Default.Get<IZetaMainObject>(id);
				}
				return _objcache[id];
			};
			using (var c = Application.Current.DatabaseConnections.GetConnection("Default"))
			{
				c.Open();
				var com = c.CreateCommand();
				com.CommandText = "select distinct year,period,obj from cell where " + GetFullCondition();
				using (var reader = com.ExecuteReader()) {
					while (reader.Read()) {
						foreach (var col in Columns) {
							result.Add(
								new TransferRecord {
									SourceYear = (int) reader[0],
									TargetYear = (int)reader[0],
									SourcePeriod = (int) reader[1],
									TargetPeriod = (int)reader[1],
									SourceObject = (int) reader[2],
									TargetObject = (int)reader[2],
									SourceColumn = col.Code,
									TargetColumnId = col.Id,
									SourceRow = SourceCode,
									TargetRowId = Source.Id,
									UserCode = UserCode,
									TargetCurrency = EvalCurrency(Source, col, _getobj((int)reader[2]))
								});

						}
					}
				}
			}
			return result;
		}

		private string EvalCurrency(IZetaRow row, IZetaColumn col, IZetaMainObject obj) {
			var result = obj.Currency;
			if (string.IsNullOrWhiteSpace(result)) {
				result = "RUB";
			}
			if(!string.IsNullOrWhiteSpace(row.Currency) && row.Currency!="NONE") {
				result = row.Currency;
			}
			if(!string.IsNullOrWhiteSpace(col.Currency) && col.Currency!="NONE") {
				result = col.Currency;
			}
			return result;
		}
	}
}