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
namespace zeta.old.periods.export
",DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
			
			foreach (var g in pgroups) {
				sb.AppendFormat(@"
	class periods_{0} category='{0}'
",  g.Key);
				foreach (var gp in g.OrderBy(_=>_.BizId)) {
					Output(gp,sb);
				}
			}
			sb.AppendLine("\tclass formulas");
			foreach (var p in formulas.OrderBy(_=>_.BizId))
			{
				Output(p, sb);
			}
			return sb.ToString();
		}

		private void Output(Period period,StringBuilder sb) {
			sb.AppendFormat("\t\tperiod {0} '{1}'{2}{3}{4}{5}\r\n",
			                period.BizId, 
							period.Name, 
							string.IsNullOrWhiteSpace(period.ShortName)?"":" short='"+ period.ShortName+"'",
							
							0==period.MonthCount?"":" months="+ period.MonthCount,
							period.StartDate.Year==1900?"": " start='"+ period.StartDate.ToString("yyyy-MM-dd")+"'",
							period.EndDate.Year == 1900 ? "" : " finish='" + period.EndDate.ToString("yyyy-MM-dd") + "'");
			if (!string.IsNullOrWhiteSpace(period.Formula) && period.IsFormula) {
				sb.AppendLine("\t\t\tformula = '" + period.Formula + "'");
			}
		}
	}
}
