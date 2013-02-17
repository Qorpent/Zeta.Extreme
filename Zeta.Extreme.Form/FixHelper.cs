#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FixHelper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	Вспомогательный класс расчета фиксации ячейки
	/// </summary>
	public static class FixHelper {
		/// <summary>
		/// 	Фаункция SQL проверки статуса
		/// </summary>
		public const string ExistedFixFunction = "usm.GetFixStatus(@id int)";

		/// <summary>
		/// 	Функция SQL для расчета предполагаемого статуса
		/// </summary>
		public const string LookupFixFunction = "usm.LookupFixStatus(@year,@period,@row,@value,@org)";

		/// <summary>
		/// 	Синхронизатор
		/// </summary>
		public static object sync = new object();

		private static IInversionContainer _container;

		/// <summary>
		/// 	Ссылка на контейнер
		/// </summary>
		public static IInversionContainer Container {
			get {
				if (_container.invalid()) {
					lock (typeof (FixHelper)) {
						if (_container.invalid()) {
							Container = myapp.ioc;
						}
					}
				}
				return _container;
			}
			set { _container = value; }
		}

		/// <summary>
		/// 	Выполняет расчет фиксации
		/// </summary>
		/// <param name="cell"> </param>
		/// <returns> </returns>
		public static FixRuleResult EvalFix(this IZetaCell cell) {
			lock (sync) {
				var activecol = TagHelper.Value(cell.Row.Tag, "activecol");
				if (activecol.hasContent()) {
					var activecols = activecol.split().Select(x => x.ToUpper()).ToList();
					if (-1 == activecols.IndexOf(cell.Column.Code.ToUpper())) {
						return FixRuleResult.Prohibited;
					}
				}
				if (TagHelper.Value(cell.Row.Tag, "fixfrom").toInt() >= cell.Year) {
					return FixRuleResult.Fixed;
				}
				if (TagHelper.Value(cell.Row.Tag, "fromyear").toInt() > cell.Year) {
					return FixRuleResult.Open;
				}
				if (cell.Id == 0) {
					cell.Fixed = Lookup(cell.Year, cell.Period, cell.Row, cell.Column, cell.Object);
				}

				if (cell.Fixed != FixRuleResult.Open) {
					return cell.Fixed;
				}

				if (cell.Finished) {
					return FixRuleResult.Fixed;
				}
				if (cell.Row.IsSumAggregation()) {
					return FixRuleResult.Fixed;
				}
				if (cell.Row.IsFormula || cell.Column.IsFormula) {
					return FixRuleResult.Fixed;
				}
				if (cell.Row.RefTo != null) {
					return FixRuleResult.Fixed;
				}

				if (cell.Row.ExRefTo != null && cell.Column.IsMarkSeted("DOEXREF")) {
					return FixRuleResult.Fixed;
				}
				if (cell.Row.HasChildren()) {
					return FixRuleResult.Fixed;
				}
				return FixRuleResult.Open;
			}
		}

		/// <summary>
		/// 	Предварительно смотрит фиксацию для несуществующих ячеек
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="row"> </param>
		/// <param name="value"> </param>
		/// <param name="org"> </param>
		/// <returns> </returns>
		public static FixRuleResult Lookup(int? year, int? period, IZetaRow row, IZetaColumn value, IZetaMainObject org) {
			lock (sync) {
				var c = Container.getConnection();
				if (value == null) {
					return FixRuleResult.Fixed;
				}
				try {
					return c.ExecuteScalar<FixRuleResult>
						("select " + LookupFixFunction,
						 new Dictionary<string, object>
							 {
								 {
									 "@year",
									 year.HasValue
										 ? (int?) year.Value
										 : null
								 },
								 {
									 "@period",
									 period.HasValue
										 ? (int?) period.Value
										 : null
								 },
								 {
									 "@row",
									 row == null
										 ? (object) null
										 : row.Id
								 },
								 {
									 "@value",
									 value == null
										 ? (object) null
										 : value.Id
								 },
								 {
									 "@org",
									 org == null
										 ? (object) null
										 : org.Id
								 },
							 });
				}
				finally {
					c.Close();
				}
			}
		}
	}
}