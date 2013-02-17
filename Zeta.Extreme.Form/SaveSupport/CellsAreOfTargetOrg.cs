#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CellsAreOfTargetOrg.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Extensions;
using Comdiv.MVC;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	Проверяет, что используется правильный целевой объект
	/// </summary>
	public class CellsAreOfTargetOrg : ICheckRequestObject {
		/// <summary>
		/// 	Выполняет проверку целевых ячеек, возвращает результат проверки
		/// </summary>
		/// <param name="targetResult"> </param>
		/// <param name="request"> </param>
		/// <param name="cells"> </param>
		/// <returns> </returns>
		public ValidationResult Validate(ValidationResult targetResult, InputTemplateRequest request,
		                                 IEnumerable<IZetaCell> cells) {
			try {
				if (request.Template.Parameters.get("anyobject", "false").toBool()) {
					return targetResult;
				}
				var orgToCheck = request.Template.FixedObject ?? request.Object;
				//если запрос прошел проверки и при этом не связан с объектом, ячейки могут быть из разных объектов
				if (null == orgToCheck) {
					return targetResult;
				}
				foreach (var cell in cells) {
					if (!cell.Object.Path.Contains("/" + orgToCheck.Id + "/")) {
						targetResult.IsValid = false;
						targetResult.Messages.Add(
							string.Format(
								"cell with Id {0} has not allowed object {1} (requested {2})",
								cell.Id,
								cell.Object.Name, orgToCheck.Name));
					}
					if (cell.DetailObject != null) {
						if (!cell.DetailObject.Object.Path.Contains("/" + orgToCheck.Id + "/")) {
							targetResult.IsValid = false;
							string.Format(
								"cell with Id {0} has not allowed detail object {1} of {2} (requested {3})",
								cell.Id,
								cell.DetailObject.Name, cell.Object.Name,
								orgToCheck.Name);
						}
					}
				}
				return targetResult;
			}
			catch (Exception ex) {
				targetResult.Error = ex;
				return targetResult;
			}
		}
	}
}