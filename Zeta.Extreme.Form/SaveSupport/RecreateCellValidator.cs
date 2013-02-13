#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : RecreateCellValidator.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.MVC;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Проверяет пересоздаение ячейки
	/// </summary>
	public class RecreateCellValidator : IRecreateCellValidator {
		/// <summary>
		/// 	Выполняет проверку целевых ячеек, возвращает результат проверки
		/// </summary>
		/// <param name="targetResult"> </param>
		/// <param name="request"> </param>
		/// <param name="cells"> </param>
		/// <returns> </returns>
		public ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
		                                 IEnumerable<IZetaCell> cells) {
			foreach (var cell in cells) {
				if (cell.Id == 0) {
					var checkExisted = new InputRowHelper().Get(cell.Object, cell.DetailObject, cell.Row,
					                                            new ColumnDesc(cell.Column.Code, cell.Year, cell.Period),
					                                            // { DirectDate = cell.DirectDate },
					                                            false);
					if (checkExisted != null) {
						if (cell.Value != checkExisted.Value) {
							//redirect
							cell.Tag = checkExisted;
						}
						else {
							cell.Tag = "ignore";
						}
						//targetResult.Fail(string.Format("try to recreate cell {0}", cell));
					}
				}
			}
			return targetResult;
		}
	}
}