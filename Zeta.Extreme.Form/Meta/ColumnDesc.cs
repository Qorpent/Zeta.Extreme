// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE

#region

#if NEWMODEL
#endif

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Logging;
using Comdiv.Olap.Model;
using Comdiv.Security;
using Comdiv.Zeta.Model;
using Qorpent.Serialization;

#endregion

namespace Zeta.Extreme.Meta{

    #region

    #endregion
	/// <summary>
	/// Описатель колонки
	/// </summary>
	[Serialize]
    public class ColumnDesc : DimensionDescriptor<IZetaColumn, ColumnDesc> {
        private readonly ILog log = logger.get("zinput");
        private readonly IDictionary<int, int> periodMap = new Dictionary<int, int>();
        /// <summary>
        /// 
        /// </summary>
        [SerializeNotNullOnly][XmlAttribute] public string CacheKey;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public bool Dynamic;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public bool Editable;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public string FilterName;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public int Period;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public string StringFormat;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public string Title;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public string ColGroup;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		[XmlAttribute]
		public string MatrixTotalFormula;
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
		[XmlAttribute]
		public bool UsePersistentCache = true;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public int CalcIdx { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public bool UseObj { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
    	public string EffectiveCode {
    		get { return null == Target ? Code : Target.Code; }
    	}
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public bool AutoCalc { get; set; }

		/// <summary>
		/// Клонирование
		/// </summary>
		/// <returns></returns>
		public override ColumnDesc Clone()
        {
            var result =  base.Clone();
        	result.CacheKey = null;
            result.RowCheckConditions = RowCheckConditions;
            _gmcache  = new Dictionary<string, ColumnRowCheckCondition[]>();
            return result;
        }
        IDictionary<string, ColumnRowCheckCondition[]> _gmcache = new Dictionary<string, ColumnRowCheckCondition[]>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ColumnRowCheckCondition[] GetMatched(IZetaRow row, decimal  value) {
            var key = row.Code + value;
            return _gmcache.get(key, () =>
                                         {
                                             List<ColumnRowCheckCondition> result = new List<ColumnRowCheckCondition>();
                                             foreach (var rule in RowCheckConditions) {
                                                 if (rule.RowTag.hasContent()) {
                                                     if (!row.Tag.like(rule.RowTag)) continue;
                                                 }
                                                 if (value == Decimal.MinValue || isvaluematch(rule, value)) {
                                                     result.Add(rule);
                                                 }
                                             }
                                             return result.ToArray();
                                         });
        }

        private bool isvaluematch(ColumnRowCheckCondition rule, decimal value) {
            switch (rule.Action)
            {
                case "<>": goto case "!=";
                case "!=":
                    return value != rule.Value;
                case "=":
                    goto case "==";
                case "==":
                    return value == rule.Value;

                case "|<>|": goto case "|!=|";
                case "|!=|":
                    return Math.Abs(value) != Math.Abs(rule.Value);
                case "|=|":
                    goto case "|==|";
                case "|==|":
                    return Math.Abs(value) == Math.Abs(rule.Value);

                case "~<>": goto case "~!=";
                case "~!=":
                    return Math.Abs(( ((rule.Value - value)/rule.Value))*100) > 5; 
                case "~=":
                    goto case "~==";
                case "~==":
                    return Math.Abs(( ((rule.Value - value) / rule.Value)) * 100) <= 5; 

                case "~|<>|": goto case "~|!=|";
                case "~|!=|":
                    return Math.Abs(( ((Math.Abs(rule.Value) - Math.Abs(value)) / Math.Abs(rule.Value))) * 100) > 5; 
                case "~|=|":
                    goto case "~|==|";
                case "~|==|":
                    return Math.Abs(( ((Math.Abs(rule.Value) - Math.Abs(value)) / Math.Abs(rule.Value))) * 100) <= 5;


                case ">=":
                    return value >= rule.Value;
                case ">":
                    return value > rule.Value;
                case "<=":
                    return value <= rule.Value;
                case "<":
                    return value < rule.Value;

                case "|>=|":
                    return Math.Abs(value) >= Math.Abs(rule.Value);
                case "|>|":
                    return Math.Abs(value) > Math.Abs(rule.Value);
                case "|<|":
                    return Math.Abs(value) < Math.Abs(rule.Value);
                case "|<=|":
                    return Math.Abs(value) <= Math.Abs(rule.Value);


                case "<->":
                    return value > rule.Value && value < rule.Value2;
                case "<=->":
                    return value >= rule.Value && value < rule.Value2;
                case "<=-=>":
                    return value >= rule.Value && value <= rule.Value2;
                case "<-=>":
                    return value > rule.Value && value <= rule.Value2;

                case "|<->|":
                    return Math.Abs(value) > Math.Abs(rule.Value) && Math.Abs(value) < Math.Abs(rule.Value2);
                case "|<=->|":
                    return Math.Abs(value) >= Math.Abs(rule.Value) && Math.Abs(value) < Math.Abs(rule.Value2);
                case "|<=-=>|":
                    return Math.Abs(value) >= Math.Abs(rule.Value) && Math.Abs(value) <= Math.Abs(rule.Value2);
                case "|<-=>|":
                    return Math.Abs(value) > Math.Abs(rule.Value) && Math.Abs(value) <= Math.Abs(rule.Value2);
            }
            return false;
        }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string Valuta { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string MatrixId { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string MatrixFormula { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public string MatrixFormulaType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string MatrixForRows { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string WAvg { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        [XmlAttribute] public int Year;
        private IDictionary<IZetaRow, IZetaRow> _rowcache;
        private IDictionary<string, string> _transmap;
        private ValueDataType dataType = ValueDataType.Undefined;
        private DateTime directDate;
        private string srctitle;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public int[] ResolvedPeriods { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ColumnDesc()
        {
            Tag = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        public ColumnDesc(string code, int year, int period) : this(){
            SetCode(code);
            NeedPeriodPreparation = true;
            ApplyPeriod(year, period, DateExtensions.Begin);
            NeedPeriodPreparation = false;
            Target = ColumnCache.get(code);
        }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string CustomView { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public bool ValueToCssClass { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string TranslateRows { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string Periods { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public bool IsAuto { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        [XmlAttribute]
        public DateTime DirectDate{
            get { return directDate; }
            set{
                directDate = value;
                StartDate = value;
                EndDate = value;
            }
        }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        [XmlAttribute]
        public DateTime StartDate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        [XmlAttribute]
        public DateTime EndDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        [XmlAttribute]
        public string ValueTypeCode{
            get { return Target.Code; }
            set{
                if (null == value){
                    Target = null;
                }

                else{
                    Target = ColumnCache.get(value);
                }
            }
        }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public bool ControlPoint { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public bool NeedYearPreparation { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public bool NeedPeriodPreparation { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public ValueDataType DataType{
            get{
                if (dataType == ValueDataType.Undefined && Target.yes()){
                    return Target.DataType;
                }
                return dataType;
            }
            set { dataType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<int, int> PeriodMap{
            get { return periodMap; }
        }

        private bool _lockPeriod;
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public bool LockPeriod
        {
            get { return _lockPeriod; }
            set
            {
                _lockPeriod = value;
            }
        }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public bool LockYear { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public int InitialYear { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public int[] ForPeriods { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public DateTime InitialDirectDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public int InitialPeriod { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public string Lookup { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
        public string LookupFilter { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string Validation { get; set; }

	    /// <summary>
	    /// 
	    /// </summary>
	    [SerializeNotNullOnly] public bool Visible { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[IgnoreSerialize]
	    public string ValueReplacer { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string EditForRole { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string ForGroup { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string ForRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IZetaRow Resolve(IZetaRow row){
            if (TranslateRows.noContent()){
                return row;
            }
            if (_transmap == null){
                _transmap = new Dictionary<string, string>();
                _rowcache = new Dictionary<IZetaRow, IZetaRow>();
                var rules = TranslateRows.split();
                foreach (var rule in rules){
                    var rule_ = rule.split(false, true, '=');
                    _transmap[rule_[0]] = rule_[1];
                }
            }
            return _rowcache.get(row, () =>{
                                          var newcode = _transmap.get(row.Code, "");
                                          if (newcode.noContent()){
                                              return row;
                                          }
                                          return RowCache.get(newcode);
                                      });
        }


		/// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetIsVisible(IZetaMainObject obj){
            if (!GetIsVisible()){
                return false;
            }

        	return GroupFilterHelper.IsMatch(obj, ForGroup);
           
        }

		/// <summary>
		/// Определение видимости элемента
		/// </summary>
		/// <returns></returns>
		public override bool GetIsVisible(){

			if(!Visible) return false;
	        
            var result = base.GetIsVisible();
            if (!result){
                return false;
            }

            if (ForRole.hasContent() && !myapp.roles.IsAdmin()){
                var roles = ForRole.split();
                var has = false;
                foreach (var r in roles){
                    if (myapp.roles.IsInRole(r)){
                        has = true;
                        break;
                    }
                }
                if (!has){
                    return false;
                }
            }
			if (Condition.hasContent())
			{
				if (null == this.ConditionMatcher)
				{
					return false;
				}
				if (!ConditionMatcher.IsConditionMatch(this.Condition))
				{
					return false;
				}

			}

            if (ForPeriods == null || ForPeriods.Length == 0){
                return true;
            }
            if (-1 == Array.IndexOf(ForPeriods, InitialPeriod)){
                return false;
            }

           

            return true;
        }
		/// <summary>
		/// Зона проверки условий
		/// </summary>
        public IConditionMatcher ConditionMatcher { get; set; }
		/// <summary>
		/// Применение периода
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <param name="directDate"></param>
		/// <returns></returns>
		public ColumnDesc ApplyPeriod(int year, int period, DateTime directDate){
            log.debug(
                () =>
                Code + "(" + ToString() + ") - start apply period " + year + " " + period + " to " + Year + " " + Period);
            InitialPeriod = period;
            InitialYear = year;
            InitialDirectDate = directDate;

            DirectDate = directDate;

            if (!LockYear){
                if (NeedYearPreparation && ((year + Year) < 3000)){
                    Year += year;
                }
                else{
                    if (0 != year){
                        Year = year;
                    }
                }
            }
            if (!LockPeriod && NeedPeriodPreparation){
                this.NeedPeriodPreparation = false;
                this.LockPeriod = true;
                if (PeriodMap.ContainsKey(period)){
                    Period = PeriodMap[period];
                }
                else{
                    var deltp = 0;
                    var mainp = 0;
                    if (Period <= 0){
                        deltp = Period;
                        mainp = period;
                    }
                    else{
                        deltp = period;
                        mainp = Period;
                    }
                    if (deltp.between(-130, -101)){
                        //прошлый месяц

                        Period = getPrevPeriod(mainp, deltp);
                    }
                    else{
                        var dp = Meta.Periods.Get(deltp);
                        
                        if (dp.IsFormula){
                            var x = dp.Evaluate(Year, DirectDate, mainp);
                            Year = x.Year;
                            this.ResolvedPeriods = x.Periods;
                            this.ResolvedPeriodName = x.PeriodName;
                            Period = x.Periods[0];
                        }


                        else if (-1 != deltp){
                            Period = mainp;
                        }
                        else{
                            Period = deltp;
                        }
                    }
                }
            }
            log.debug(
                () => Code + " result " + Year + " " + Period);
            SetTitle(Title);

            log.debug(
                () => Code + " title " + Title);
            return this;
        }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string ResolvedPeriodName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string Group { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        public string Uid { get; set; }

        private ColumnRowCheckCondition[] _rowCheckConditions;
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        
        public ColumnRowCheckCondition[] RowCheckConditions {
            get { return _rowCheckConditions??(_rowCheckConditions = new ColumnRowCheckCondition[]{}); }
            set { _rowCheckConditions = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        
    	public bool UseThema { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        
    	public string RedirectRowCode { get; set; }

		private int getPrevPeriod(int from, int to){
            var delta = -(to + 100);

            var current = from;
            for (var i = 0; i < delta; i++){
                current = getPrev(current);
            }
            return current;
        }

        private int getPrev(int current){
            if (current == 11){
                Year = Year - 1;
                return 112;
            }
            if (current == 110){
                return 19;
            }
            if (current == 1){
                Year = Year - 1;
                return 4;
            }
            if (current == 2){
                return 1;
            }
            if (current == 3){
                return 2;
            }
            if (current == 4){
                return 3;
            }
            return current - 1;
        }

		/// <summary>
		/// Настройка заголовка
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
        public ColumnDesc SetTitle(string title){
			try {
				if (null == title) {
					title = "{1} {0}";
				}
				if (srctitle.noContent()) {
					srctitle = title;
				}
				if (DirectDate != DateExtensions.Begin) {
					Title = String.Format(srctitle, "", "", "", DirectDate);
				}
				else {
					var p = Meta.Periods.Get(Period);
					DateTime st = DateExtensions.Begin;
					DateTime et = DateExtensions.End;
					if (p != null) {
						st = p.StartDate.accomodateToYear(Year);
					}
					if (p != null) {
						et = p.EndDate.accomodateToYear(Year);
					}
					Title = String.Format(srctitle,
					                      Year,
					                      Period,
					                      ResolvedPeriodName.hasContent() ? ResolvedPeriodName: Meta.Periods.Get(Period).Name,
					                      Year - 1,
					                      st.ToString("dd.MM.yyyy"),
					                      st.AddDays(-1).ToString("dd.MM.yyyy"),
					                      et.ToString("dd.MM.yyyy"),
					                      et.AddDays(1).ToString("dd.MM.yyyy"));
				}
			}catch(Exception) {
				this.Title = String.Format("error");
			}
        	return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueTypeCode"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static ColumnDesc Create(string valueTypeCode, int year, int period){
            return new ColumnDesc{
                                     Target = ColumnCache.get(valueTypeCode),
                                     Year = year,
                                     Period = period
                                 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ColumnDesc SetCode(string code){
            Code = code;
            return this;
        }


		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        
		public string RedirectBasis { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public string CustomCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
        
	    public string InitialCode { get; set; }
    }
}