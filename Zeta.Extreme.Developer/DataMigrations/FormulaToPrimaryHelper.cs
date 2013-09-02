using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Applications;
using Zeta.Extreme.Developer.CodeMetrics;
using Zeta.Extreme.Developer.MetaStorage.Tree;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.DataMigrations
{
	/// <summary>
	/// Вспомогательный класс для генерации скриптов на превращение формулы в первичную
	/// строку
	/// </summary>
	public class FormulaToPrimaryHelper
	{
		class pyo {
			public int Year;
			public int Period;
			public int Obj;
		}
		/// <summary>
		/// Создает скрипт для переноса строки
		/// </summary>
		/// <returns></returns>
		public string GenerateScript(IZetaRow row, string[] columns) {
			var primaryCodesSrc = GetPrimarySet(row);
			var primaryCond = string.Join(", ", primaryCodesSrc);
			IList<pyo> sets = new List<pyo>();
			using (var c = Application.Current.DatabaseConnections.GetConnection("Default"))
			{
				c.Open();
				var com = c.CreateCommand();
				com.CommandText = "select distinct year,period,obj from cell where row in ("+primaryCond+")";
				using (var r = com.ExecuteReader()) {
					while (r.Read()) {
						sets.Add(new pyo {Year = (int) r[0], Period = (int) r[1], Obj = (int) r[2]});
					}
					r.Close();
				}
				
			}
			var sb = new StringBuilder();
			int cnt = 0;
			var session = new Session();
			IList<Query> _agenda = new List<Query>();
			foreach (var s in sets) {
				foreach (var c in columns) {
					cnt++;
					var q = new Query(row.Code, c, s.Obj, s.Year, s.Period);
					_agenda.Add((Query) session.Register(q));

					if (cnt >= 200) {
						cnt = 0;
						session.WaitPreparation();
						session.WaitEvaluation();
						foreach (var rq in _agenda) {
							if (rq.Result.NumericResult != 0) {
								if (null == rq.Obj.Native) continue;
								var valuta = ((IZetaMainObject) rq.Obj.Native).Currency;
								if (string.IsNullOrWhiteSpace(valuta)) {
									valuta = "RUB";
								}
								var sql =
									string.Format(@"insert usm.insertdata ( year, period, row, col, obj, decimalvalue, usr, valuta, op) values ({0},{1},{2},{3},{4},{5},'{6}','{7}','=')",
									rq.Time.Year,rq.Time.Period,rq.Row.Id,rq.Col.Id,rq.Obj.Id,rq.Result.NumericResult.ToString("0.######",CultureInfo.InvariantCulture),"autofp",valuta);
								sb.AppendLine(sql);
							}
						}
						_agenda.Clear();
						session = new Session();
					}
				}
			}
			return sb.ToString();
		}
		/// <summary>
		/// Рассчитывает метрики переноса формулы в первичнку
		/// </summary>
		/// <param name="row"></param>
		/// <param name="columns"></param>
		/// <returns></returns>
		public IEnumerable<MetricResult> CalculateMetrics(IZetaRow row, string[] columns) {
			var primaryCodesSrc = GetPrimarySet(row);
			var primaryCodes = string.Join(", ", primaryCodesSrc);
			yield return new MetricResult { ItemName = "rowsCount", Value = primaryCodesSrc.Length };
			int periodCount = Eval("select count(distinct period) from cell where decimalvalue!=0 and row in (" + primaryCodes+")");
			yield return new MetricResult {ItemName = "periodCount", Value = periodCount};
			int yearCount = Eval("select count(distinct year) from cell where decimalvalue!=0 and row  in (" + primaryCodes + ")");
			yield return new MetricResult {ItemName = "yearCount", Value = yearCount};
			int yearPeriodCount = Eval("select count(distinct year*100000+period) from cell where decimalvalue!=0 and row in (" + primaryCodes + ")");
			yield return new MetricResult {ItemName = "yearPeriodCount", Value = yearPeriodCount};
			int objCount = Eval("select count(distinct obj) from cell where decimalvalue!=0 and row in (" + primaryCodes + ")");
			yield return new MetricResult {ItemName = "objCount", Value = objCount};
			int objYearPeriodCount =
				Eval("select count(distinct cast(year as bigint)-2000+cast(period as bigint)*1000000+obj*100) from cell where decimalvalue!=0 and row in (" + primaryCodes + ")");
			yield return new MetricResult { ItemName = "objYearPeriodCount", Value = objYearPeriodCount };
			yield return new MetricResult { ItemName = "colsTransferCount", Value = columns.Length };
			int estimatedP_Y_O = periodCount*yearCount*objCount*columns.Length;
			yield return new MetricResult { ItemName = "estimatedP_Y_O", Value = estimatedP_Y_O };
			int estimatedPY_O = yearPeriodCount * objCount * columns.Length;
			yield return new MetricResult { ItemName = "estimatedPY_O", Value = estimatedPY_O };
			int estimatedPYO = objYearPeriodCount * columns.Length;
			yield return new MetricResult { ItemName = "estimatedPYO", Value = estimatedPYO };
		}

		private static int[] GetPrimarySet(IZetaRow row) {
			var depGraph = FormDependencyHelper.GetFormulaDependencyGraph(row);
			var primaryCodesSrc = depGraph.Nodes
			                              .Where(_ => _.StartsWith("p_"))
			                              .Select(_ => _.Substring(2))
			                              .Select(_ => MetaCache.Default.Get<IZetaRow>(_))
			                              .Select(_ => _.Id)
			                              .ToArray();
			return primaryCodesSrc;
		}

		private int Eval(string q) {
			using (var c = Application.Current.DatabaseConnections.GetConnection("Default")) {
				c.Open();
				var com = c.CreateCommand();
				com.CommandText = q;
				var result = com.ExecuteScalar();
				return Convert.ToInt32(result);
			}
		}
	}
}
