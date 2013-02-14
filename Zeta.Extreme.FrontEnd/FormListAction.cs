using System.Linq;
using Qorpent.Mvc;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Выдает список загруженных форм
	/// </summary>
	[Action("exf.formlist")]
	public class FormListAction : ActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			FormServer.Default.LoadThemas.Wait();
			return ((ExtremeFormProvider) FormServer.Default.FormProvider).Factory
				.GetAll().Where(_ => !_.Code.Contains("lib")).SelectMany(_ => _.GetAllForms())
				.Select(_ => new{code=_.Code,name=_.Name}).ToArray();
		}
	}
}