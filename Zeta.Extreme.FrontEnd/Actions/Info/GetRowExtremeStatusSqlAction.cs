using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	Возвращает информацию о сессии
	/// </summary>
	[Action("zefs.getrowextremestatus", Role = "ADMIN")]
	public class GetRowExtremeStatusSqlAction : FormServerActionBase
	{
		/// <summary>
		/// Код формы
		/// </summary>
		[Bind]
		public string Form { get; set; }
		StrongSumProvider _sumh = new StrongSumProvider();
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return "update row set ExtremeFormulaMode = 0 where IsFormula = 0 and isnull(ExtremeFormulaMode,0)!=0 \r\n" + string.Join(Environment.NewLine, RowCache.Bycode.Values.Where(
				_ => _.Id != 0 && _.IsFormula && (_.FormulaType == "boo" || _.FormulaType == "cs"))
			                                                .Select(ToExtremeStatusSql));
		}

		private object ToExtremeStatusSql(IZetaRow r)
		{
			int status = 0;
			if (r.ResolveTag("extreme") == "1") {
				status = 1;
			}
			if (_sumh.IsSum(r)) {
				status = 2;
			}

			return string.Format("update row set ExtremeFormulaMode = {0} where Code = '{1}' and isnull(ExtremeFormulaMode,0)!={0}", status, r.Code);
		}
	}

}