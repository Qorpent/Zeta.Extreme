#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : InputRowHelper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Meta;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Помогает при работе с ячейками ввода
	/// </summary>
	public class InputRowHelper {
		/// <summary>
		/// 	Признак игнорирования формул
		/// </summary>
		public bool IgnoreFormula { get; set; }

		/// <summary>
		/// 	Получить экземпляр ячейки
		/// </summary>
		/// <param name="org"> </param>
		/// <param name="subpart"> </param>
		/// <param name="tree"> </param>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public IZetaCell Get(IZetaMainObject org, IZetaDetailObject subpart, IZetaRow tree, ColumnDesc value) {
			return Get(org, subpart, tree, value, true);
		}


		/// <summary>
		/// 	Получить экземпляр ячейки
		/// </summary>
		/// <param name="org"> </param>
		/// <param name="subpart"> </param>
		/// <param name="tree"> </param>
		/// <param name="column"> </param>
		/// <param name="createNew"> </param>
		/// <returns> </returns>
		public IZetaCell Get(IZetaMainObject org, IZetaDetailObject subpart, IZetaRow tree, ColumnDesc column, bool createNew) {
			IZetaCell result = null;

			if (IgnoreFormula || ((!tree.IsFormula || tree.LocalProperties.get("ignoreformula", false)) && !column.IsFormula)) {
				var rs = new DataRowSet();

				rs
					.Row.SetId(tree.Id)
					.Column.SetCode(column.EffectiveCode)
					.Periods.Set(column);
				if (null != subpart) {
					rs.DetailObject.SetId(subpart.Id);
				}
				else {
					rs.Object.SetId(org.Id);
				}
				result = rs.ExecuteFirst();
			}


			if (null == result) {
				if (createNew) {
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
	}
}