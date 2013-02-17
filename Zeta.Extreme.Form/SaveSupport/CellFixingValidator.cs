#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CellFixingValidator.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.MVC;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Проверяет данные на предмет фиксированности
	/// </summary>
	public class CellFixingValidator : ICellFixingValidator {
		/// <summary>
		/// 	Выполняет проверку того, что ячейки в которые осуществляется
		/// 	ввод не фиксированы
		/// </summary>
		/// <param name="targetResult"> </param>
		/// <param name="request"> </param>
		/// <param name="cells"> </param>
		/// <returns> </returns>
		public ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
		                                 IEnumerable<IZetaCell> cells) {
			foreach (var cell in cells) {
				if (cell.Finished) {
					targetResult.IsValid = false;
					targetResult.Messages.Add(string.Format("cell {0} was FINISHED", cell.Id));
					continue;
				}
				var fixresult = FixRuleResult.Open;
				if (!((fixresult = cell.EvalFix()) == FixRuleResult.Open)) {
					targetResult.IsValid = false;
					targetResult.Messages.Add(string.Format("cell {0} was FIXED {1}", cell.Id, fixresult));
				}
			}
			return targetResult;
		}
	}
}