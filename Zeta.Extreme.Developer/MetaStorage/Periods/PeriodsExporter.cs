using System;
using System.Linq;
using System.Text;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage.Periods
{
	/// <summary>
	/// Экспортер периодов
	/// </summary>
	public class PeriodsExporter
	{
		/// <summary>
		/// 
		/// </summary>
		public PeriodsExporter() {
			Namespace = "import";
			ClassName = "periods";
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GeneratePeriods() {
			var sb = new StringBuilder();
			var srcperiods = new NativeZetaReader().ReadPeriods().ToArray();
			var primary = srcperiods.Where(_ =>! _.IsFormula).ToArray();
			var formulas = srcperiods.Where(_ => _.IsFormula).ToArray();
			var pgroups = primary.GroupBy(_ => _.Category);
		
			sb.AppendFormat(@"
#################################################################################
##     ЭКСПОРТ ПЕРИОДОВ ИЗ БД ECO ПО СОСТОЯНИЮ НА  {0}       
#################################################################################
namespace {1}
	class {2}
",srcperiods.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss"), Namespace,ClassName);
			
			foreach (var g in pgroups) {
				sb.AppendFormat(@"
		set category='{0}'
",  g.Key);
				foreach (var gp in g.OrderBy(_=>_.BizId)) {
					Output(gp,sb);
				}
			}
			sb.AppendLine("\t\tset isformula=true");
			foreach (var p in formulas.OrderBy(_=>_.BizId))
			{
				Output(p, sb);
			}
			return sb.ToString();
		}
		/// <summary>
		/// Имя класса
		/// </summary>
		public string ClassName { get; set; }
		/// <summary>
		/// Пространство имен
		/// </summary>
		public string Namespace { get; set; }

		private void Output(Period period,StringBuilder sb) {
			sb.AppendFormat("\t\t\tperiod {0} '{1}'{2}{3}{4}{5}\r\n",
			                period.BizId, 
							period.Name, 
							string.IsNullOrWhiteSpace(period.ShortName)?"":" short='"+ period.ShortName+"'",
							
							0==period.MonthCount?"":" months="+ period.MonthCount,
							period.StartDate.Year==1900?"": " start='"+ period.StartDate.ToString("yyyy-MM-dd")+"'",
							period.EndDate.Year == 1900 ? "" : " finish='" + period.EndDate.ToString("yyyy-MM-dd") + "'");
			if (!string.IsNullOrWhiteSpace(period.Formula) && period.IsFormula) {
				sb.AppendLine("\t\t\t\tformula = '" + period.Formula + "'");
			}
		}
	}
}
