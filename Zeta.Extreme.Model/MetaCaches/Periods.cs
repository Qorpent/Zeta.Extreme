#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : Periods.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.SqlSupport;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.MetaCaches {
	/// <summary>
	/// 	Обслуживание логики периодов
	/// </summary>
	public static class Periods {
		private static IList<IPeriod> _cached;

		private static readonly IDictionary<string, IDictionary<int, string>> cache =
			new Dictionary<string, IDictionary<int, string>>();

		/// <summary>
		/// 	Загруженные периоды
		/// </summary>
		public static IEnumerable<IPeriod> All {
			get { return _cached ?? (_cached = new NativeZetaReader().ReadPeriods().OfType<IPeriod>().ToList()); }
		}

		/// <summary>
		/// 	Вычисление формульного периода
		/// </summary>
		/// <param name="sourceyear"> </param>
		/// <param name="sourceperiod"> </param>
		/// <param name="formulaperiod"> </param>
		/// <returns> </returns>
		public static PeriodDefinition Eval(int sourceyear, int sourceperiod, int formulaperiod) {
			return Get(formulaperiod).Evaluate(sourceyear, QorpentConst.Date.Begin, sourceperiod);
		}

		/// <summary>
		/// 	Перезагрузка кэша
		/// </summary>
		public static void Reload() {
			_cached = null;
		}

		/// <summary>
		/// 	Получение периода по ИД
		/// </summary>
		/// <param name="classicId"> </param>
		/// <returns> </returns>
		public static IPeriod Get(int classicId) {
			var result = All.FirstOrDefault(x => x.ClassicId == classicId);
			if (null == result) {
				result = new period();
				result.ClassicId = classicId;
				result.StartDate = QorpentConst.Date.Begin;
				result.EndDate = QorpentConst.Date.End;
				if (classicId < 0) {
					result.IsFormula = true;
				}
			}
			return result;
		}

		/// <summary>
		/// 	Вычисление формулы
		/// </summary>
		/// <param name="period"> </param>
		/// <param name="otherperiodId"> </param>
		/// <returns> </returns>
		public static int Evaluate(this IPeriod period, int otherperiodId) {
			return period.Evaluate(DateTime.Today.Year, QorpentConst.Date.Begin, otherperiodId).Periods[0];
		}

		/// <summary>
		/// 	Вычисление смешенного периода
		/// </summary>
		/// <param name="period"> </param>
		/// <param name="otherperiodId"> </param>
		/// <returns> </returns>
		public static PeriodDefinition EvaluateDef(this IPeriod period, int otherperiodId) {
			return period.Evaluate(DateTime.Today.Year, QorpentConst.Date.Begin, otherperiodId);
		}

		/// <summary>
		/// 	Вычисление полной формулы
		/// </summary>
		/// <param name="period"> </param>
		/// <param name="year"> </param>
		/// <param name="date"> </param>
		/// <param name="otherperiodId"> </param>
		/// <returns> </returns>
		public static PeriodDefinition Evaluate(this IPeriod period, int year, DateTime date, int otherperiodId) {
			var result = new PeriodDefinition(year, period.ClassicId);
			if (period.IsFormula) {
				result.Periods = new[] {otherperiodId};
				EvaluateFormula(result, period.Formula);
			}
			return result;
		}

		private static void EvaluateFormula(PeriodDefinition definition, string formula) {
			var dict = PrepareFormulaDictionary(formula);
			var rule = PrepareRule(definition, dict);
			if (rule.IsNotEmpty()) {
				ApplyRule(definition, rule);
			}
		}

		private static void ApplyRule(PeriodDefinition definition, string rule) {
			var pt = rule.SmartSplit(false, true, '!');
			if (pt.Count > 1) {
				var titlesoruce = pt[1].ToInt();
				definition.PeriodName = Get(titlesoruce).Name;
			}
			var py = pt[0].SmartSplit(false, true, ':');
			var ps = py[0].SmartSplit(false, true, '+').Select(x => x.ToInt()).ToArray();
			var y = 0;
			if (py.Count > 1) {
				y = py[1].ToInt();
			}
			definition.Year = definition.Year + y;
			definition.Periods = ps;
		}

		private static string PrepareRule(PeriodDefinition definition, IDictionary<int, string> dict) {
			var rule = "";
			if (dict.ContainsKey(definition.Periods[0])) {
				rule = dict[definition.Periods[0]];
			}
			else if (dict.ContainsKey(0)) {
				rule = dict[0];
			}
			return rule;
		}

		private static IDictionary<int, string> PrepareFormulaDictionary(string formula) {
			IDictionary<int, string> dict = null;
			if (cache.ContainsKey(formula)) {
				dict = cache[formula];
			}
			else {
				cache[formula] = dict = GetFormulaDictionary(formula);
			}
			return dict;
		}

		private static Dictionary<int, string> GetFormulaDictionary(string formula) {
			var res = new Dictionary<int, string>();
			var rules = formula.SmartSplit();
			foreach (var r in rules) {
				var parts = r.SmartSplit(false, true, '=');
				res[parts[0].ToInt()] = parts[1];
			}
			return res;
		}
	}
}