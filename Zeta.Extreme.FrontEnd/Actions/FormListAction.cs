#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormListAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using Qorpent.Mvc;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Выдает список загруженных форм
	/// </summary>
	[Action("zefs.formlist")]
	public class FormListAction : ActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			FormServer.Default.LoadThemas.Wait();
			return ((ExtremeFormProvider) FormServer.Default.FormProvider).Factory
				.GetAll().Where(_ => !_.Code.Contains("lib")).SelectMany(_ => _.GetAllForms())
				.Select(_ => new {code = _.Code, name = _.Name}).ToArray();
		}
	}
}