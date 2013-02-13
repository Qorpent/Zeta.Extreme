using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Zeta.Forms;

namespace Comdiv.Zeta.Web {
	/// <summary>
	/// Помогает при работе с ячейками ввода
	/// </summary>
	public class InputRowHelper
	{
		/// <summary>
		/// Получить экземпляр ячейки
		/// </summary>
		/// <param name="org"></param>
		/// <param name="subpart"></param>
		/// <param name="tree"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public IZetaCell Get(IZetaMainObject org, IZetaDetailObject subpart, IZetaRow tree, ColumnDesc value)
		{
			return Get(org, subpart, tree, value, true);
		}

		/// <summary>
		/// Получить экземпляр ячейки
		/// </summary>
		/// <param name="org"></param>
		/// <param name="subpart"></param>
		/// <param name="tree"></param>
		/// <param name="value"></param>
		/// <param name="createNew"></param>
		/// <returns></returns>
		public IZetaCell Get(IZetaMainObject org, IZetaDetailObject subpart, IZetaRow tree, ColumnDesc value,
		                     bool createNew)
		{
			return Get(org, subpart, tree, value, null, createNew);
		}

		/// <summary>
		/// Получить экземпляр ячейки
		/// </summary>
		/// <param name="org"></param>
		/// <param name="subpart"></param>
		/// <param name="tree"></param>
		/// <param name="column"></param>
		/// <param name="form"></param>
		/// <param name="createNew"></param>
		/// <returns></returns>
		public IZetaCell Get(IZetaMainObject org, IZetaDetailObject subpart, IZetaRow tree, ColumnDesc column, Form form, bool createNew)
		{
			IZetaCell result = null;
			/* проверяет, установлена ли данная ячейка в матрице */
			bool ischeked = false;
			/* Id из матрицы */
			int matrixid = 0;
			Cell cell = null;
			if (form != null)
			{
				var row = tree.ResolveToReal(column);
				cell = form.GetCell(row.Code, column.EffectiveCode, column.Year, column.Period);
				if (null != cell)
				{
					ischeked = true;
					matrixid = cell.CellId;
				}
			}

			if (ischeked)
			{
				result = myapp.storage.Get<IZetaCell>().New();
				result.Id = matrixid;
				result.Object = subpart == null ? org : subpart.Object;
				result.DetailObject = subpart;
				result.Row = tree;
				result.DirectDate = column.DirectDate;
				result.Column = column.Target;
				result.Year = column.Year;
				result.Period = column.Period;
				result.FixStatus = (FixRuleResult)cell.Fix;
				result.Tag = cell.Error;
				//  result.Value = cell.Value;
			}
			else
			{

				if (IgnoreFormula || ((!tree.IsFormula || tree.LocalProperties.get("ignoreformula", false)) && !column.IsFormula))
				{

					var rs = new DataRowSet();

					rs
						.Row.SetId(tree.Id)
						.Column.SetCode(column.EffectiveCode)
						.Periods.Set(column);
					if (null != subpart)
					{
						rs.DetailObject.SetId(subpart.Id);
					}
					else
					{
						rs.Object.SetId(org.Id);
					}
					result = rs.ExecuteFirst();
				}
			}


			if (null == result)
			{
				if (createNew)
				{
					result = myapp.storage.Get<IZetaCell>().New();

					result.Object = subpart == null ? org : subpart.Object;
					result.DetailObject = subpart;
					result.Row = tree;
					result.DirectDate = column.DirectDate;
					result.Column = column.Target;
					result.Year = column.Year;
					result.Period = column.Period;
				}
			}
			return result;
		}

		/// <summary>
		/// Признак игнорирования формул
		/// </summary>
		public bool IgnoreFormula { get; set; }
	}
}