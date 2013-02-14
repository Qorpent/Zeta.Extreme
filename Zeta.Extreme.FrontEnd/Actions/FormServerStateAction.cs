#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServerStateAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Data.Minimal;
using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Действие, возвращающее статус загрузки приложения
	/// </summary>
	[Action("zefs.server")]
	public class FormServerStateAction : ActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return new
				{
					hibernate = new
						{
							status = FormServer.Default.HibernateLoad.Status,
							error = FormServer.Default.HibernateLoad.Error.ToStr()
						},
					meta = new
						{
							status = FormServer.Default.MetaCacheLoad.Status,
							error = FormServer.Default.MetaCacheLoad.Error.ToStr(),
							rows = RowCache.byid.Count,
						},
					formulas = new
						{
							status = FormServer.Default.CompileFormulas.Status,
							taskerror = FormServer.Default.CompileFormulas.Error.ToStr(),
							compileerror =
								FormulaStorage.Default.LastCompileError == null ? "" : FormulaStorage.Default.LastCompileError.ToString(),
							formulacount = FormulaStorage.Default.Count,
						},
					themas = new
						{
							status = FormServer.Default.LoadThemas.Status,
							error = FormServer.Default.LoadThemas.Error.ToStr(),
						},
				};
		}
	}
}